using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Iyokan_L1.Models;
using Iyokan_L1.Utils;
using Newtonsoft.Json;

namespace Iyokan_L1.Converter
{
    public class YosysConverter
    {
        private string jsonPath;
        public LogicNetList netList { get; }
        
        public Dictionary<int, List<Logic>> LogicInputNetList = new Dictionary<int, List<Logic>>();
        
        public Dictionary<int, Logic> LogicOutputNetList = new Dictionary<int, Logic>();
        
        private List<Logic> prioritySolverStartingLogics = new List<Logic>();
        
        public YosysConverter(string jsonPath)
        {
            this.jsonPath = jsonPath;
            netList = new LogicNetList();
        }

        public string Convert(bool convertRam)
        {
            var yosysNetList = Deserialize();
            var yosysModule = yosysNetList.modules.First();
            var yosysPorts = yosysModule.Value.ports;
            var yosysCells = yosysModule.Value.cells;
            var yosysNets = yosysModule.Value.netnames;

            foreach (var yosysPort in yosysPorts)
            {
                if (yosysPort.Key == "clock")
                {
                    continue;
                }

                if (yosysPort.Key == "reset" && yosysPort.Value.bits.Count == 0)
                {
                    continue;
                }

                for (int i = 0; i < yosysPort.Value.bits.Count; i++)
                {
                    var port = ConvertYosysPort(yosysPort.Value.direction, yosysPort.Key, i);
                    port.bitsCell.Add(yosysPort.Value.bits[i]);
                    
                    if (yosysPort.Value.direction == "input")
                    {
                        LogicOutputNetList[yosysPort.Value.bits[i]] = port;
                        prioritySolverStartingLogics.Add(port);
                    }
                    else if (yosysPort.Value.direction == "output")
                    {
                        if (!LogicInputNetList.ContainsKey(yosysPort.Value.bits[i]))
                        {
                            LogicInputNetList[yosysPort.Value.bits[i]] = new List<Logic>();
                        }
                        LogicInputNetList[yosysPort.Value.bits[i]].Add(port);
                    }
                    netList.Add(port);
                }
            }

            foreach (var yosysCell in yosysCells)
            {
                var cell = ConvertYosysCell(yosysCell);
                if (cell == null)
                {
                    // Skip metadata cells like $scopeinfo
                    continue;
                }
                if (cell.type == "DFFP")
                {
                    prioritySolverStartingLogics.Add(cell);
                }
                var connections = yosysCell.Value.connections;

                foreach (var connectionPair in connections)
                {
                    var portName = connectionPair.Key;
                    var bits = connectionPair.Value;
                    var direction = yosysCell.Value.port_directions[portName];

                    //Reject DFF_P Clock Port
                    if ((cell.type == "DFFP"||cell.type == "RAM") && portName == "C")
                    {
                        continue;
                    }
                    
                    if (direction == "input")
                    {
                        cell.inputCell[portName] = bits.First();
                        foreach (var bit in bits)
                        {
                            if (!LogicInputNetList.ContainsKey(bit))
                            {
                                LogicInputNetList[bit] = new List<Logic>();
                            }
                            LogicInputNetList[bit].Add(cell);
                        }
                    }
                    else if (direction == "output")
                    {
                        cell.outputCell[portName] = bits;
                        if (LogicOutputNetList.ContainsKey(bits.First()))
                        {
                            throw new Exception("Invalid output");
                        }
                        LogicOutputNetList[bits.First()] = cell;
                    }
                }
                netList.Add(cell);
            }

            foreach (var port in netList.ports)
            {
                foreach (var bitCell in port.bitsCell)
                {
                    if (port.type == "input")
                    {
                        if (LogicInputNetList.ContainsKey(bitCell))
                        {
                            foreach (var logic in LogicInputNetList[bitCell])
                            {
                                port.bits.Add(logic.id); 
                                port.outputLink.Add(logic.id, logic);
                            }
                        }
                    }
                    else if (port.type == "output")
                    {
                        if (LogicOutputNetList.ContainsKey(bitCell))
                        {
                            var logic = LogicOutputNetList[bitCell];
                            port.bits.Add(logic.id);
                            port.inputLink.Add(logic.id, logic);
                            port.tmpInputLink.Add(logic.id, logic);
                        }
                    }
                }
            }

            foreach (var cell in netList.cells)
            {
                foreach (var portPair in cell.inputCell)
                {
                    if (LogicOutputNetList.ContainsKey(portPair.Value))
                    {
                        var logic = LogicOutputNetList[portPair.Value];
                        cell.inputLink.Add(logic.id, logic);
                        cell.tmpInputLink.Add(logic.id, logic);
                        cell.input[portPair.Key] = logic.id;
                    }
                }

                foreach (var portPair in cell.outputCell)
                {
                    foreach (var logicCell in portPair.Value)
                    {
                        if (LogicInputNetList.ContainsKey(logicCell))
                        {
                            foreach (var logic in LogicInputNetList[logicCell])
                            {
                                if (!cell.output.ContainsKey(portPair.Key))
                                {
                                    cell.output[portPair.Key] = new List<int>();
                                }
                                cell.output[portPair.Key].Add(logic.id);
                                cell.outputLink.Add(logic.id, logic);
                            }
                        }
                    }
                }
            }

            if (convertRam)
            {
                foreach (var port in netList.ports)
                {
                    if (!port.portName.Contains("io_debug") && port.type != "output")
                    {
                        continue;
                    }

                    foreach (var cellPair in port.inputLink)
                    {
                        var cell = (LogicCell) cellPair.Value;
                        if (cell.type == "DFFP")
                        {
                            cell.ramBit = port.portBit;
                            string match = Regex.Replace(port.portName, "[^0-9]", "");
                            cell.ramAddress = Int32.Parse(match);
                            cell.type = "RAM";
                        }
                    }
                }
            }

            return netList.Serialize();
        }

        /*
         L ← トポロジカルソートした結果を蓄積する空リスト
         S ← 入力辺を持たないすべてのノードの集合

         while S が空ではない do
            S からノード n を削除する
            L に n を追加する
            for each n の出力辺 e とその先のノード m do
                辺 e をグラフから削除する
                if m がその他の入力辺を持っていなければ then
                    m を S に追加する
         if グラフに辺が残っている then
            閉路があり DAG でないので中断
         */
        public List<Logic> UpdatePriority()
        {
            var res = new List<Logic>();
            var nodeWithoutInput = new List<Logic>(prioritySolverStartingLogics);
            while (nodeWithoutInput.Count > 0)
            {
                var node = nodeWithoutInput.First();
                nodeWithoutInput.Remove(node);

                if (node.type == "DFFP" || node.type == "RAM" || node.tmpInputLink.Count == 0)
                {
                    node.priority = 0;
                }
                else
                {
                    int max_priority = 0;
                    foreach (var input in node.tmpInputLink.Values)
                    {
                        if (input.priority > max_priority)
                        {
                            max_priority = input.priority;
                        }
                    }
                    node.priority = max_priority + 1;
                }
                res.Add(node);
                foreach (var m in node.outputLink.Values)
                {
                    // Ignore DFFP,RAM bacause these logics do not have input on DAG
                    if (m.type == "DFFP" || m.type == "RAM") continue;
                    
                    if (!m.inputLink.ContainsKey(node.id))
                    {
                        throw new Exception("Invalid DAG");
                    }
                    m.inputLink.Remove(node.id);
                    if (m.inputLink.Count() == 0)
                    {
                        nodeWithoutInput.Add(m);
                    }
                }
            }
            return res;
        }

        private LogicPort ConvertYosysPort(string direction, string portName, int bit)
        {
            if (direction == "input" || direction == "output")
            {
                return new LogicPort(direction, $"{portName}[{bit}]", portName, bit);
            }
            else
            {
                throw new Exception($"Invalid direction token: {direction}");
            }
        }
        

        private LogicCellRAM FindCellRAM(YosysCell cell, Dictionary<int, List<int>> ram)
        {
            List<int> Qbit = cell.connections["Q"];
            foreach (var ramCell in ram)
            {
                for (int i = 0; i < ramCell.Value.Count; i++)
                {
                    if (Qbit.Contains(ramCell.Value[i]))
                    {
                        return new LogicCellRAM(ramCell.Key, i);
                    }
                }
            }
            return null;
        }

        private LogicCell ConvertYosysCell(KeyValuePair<string, YosysCell> yosysCell)
        {
            var type = yosysCell.Value.type;
            LogicCell cell = null;
            switch (type)
            {
                case "$_NOT_":
                    cell = new LogicCell("NOT");
                    break;
                case "$_AND_":
                    cell = new LogicCell("AND");
                    break;
                case "$_ANDNOT_":
                    cell = new LogicCell("ANDNOT");
                    break;
                case "$_NAND_":
                    cell = new LogicCell("NAND");
                    break;
                case "$_OR_":
                    cell = new LogicCell("OR");
                    break;
                case "$_XOR_":
                    cell = new LogicCell("XOR");
                    break;
                case "$_XNOR_":
                    cell = new LogicCell("XNOR");
                    break;
                case "$_NOR_":
                    cell = new LogicCell("NOR");
                    break;
                case "$_ORNOT_":
                    cell = new LogicCell("ORNOT");
                    break;
                case "$_DFF_P_":
                    cell = new LogicCell("DFFP");
                    break;
                case "$_MUX_":
                    cell = new LogicCell("MUX");
                    break;
                case "$scopeinfo":
                    // Skip scopeinfo metadata cells
                    return null;
                default:
                    throw new Exception($"Invalid type token: {type}");
            }
            return cell;
        }

        private YosysNetList Deserialize()
        {
            var yosysJson = File.ReadAllText(jsonPath);
            return JsonConvert.DeserializeObject<YosysNetList>(yosysJson);
        }
    }
}