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

        public YosysConverter(string jsonPath)
        {
            this.jsonPath = jsonPath;
            netList = new LogicNetList();
        }

        public string Convert(bool odd, bool even)
        {
            var yosysNetList = Deserialize();
            var yosysModule = yosysNetList.modules.First();
            var yosysPorts = yosysModule.Value.ports;
            var yosysCells = yosysModule.Value.cells;
            var yosysNets = yosysModule.Value.netnames;
            var yosysRAM = new Dictionary<int, List<int>>();
            
            foreach (var yosysNet in yosysNets)
            {
                if (yosysNet.Key.Contains("memA.mem["))
                {
                    Console.WriteLine(yosysNet.Key);
                    string match = Regex.Replace(yosysNet.Key, "[^0-9]", "");
                    yosysRAM[Int32.Parse(match)*2+1] = yosysNet.Value.bits;
                }
                else if (yosysNet.Key.Contains("memB.mem["))
                {
                    Console.WriteLine(yosysNet.Key);
                    string match = Regex.Replace(yosysNet.Key, "[^0-9]", "");
                    yosysRAM[Int32.Parse(match)*2] = yosysNet.Value.bits;
                }
            }

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
                if (cell.type == "DFFP")
                {
                    var tmp = FindCellRAM(yosysCell.Value, yosysRAM);
                    if (tmp != null)
                    {
                        cell = tmp;
                    }
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
                            }
                        }
                    }
                    else if (port.type == "output")
                    {
                        if (LogicOutputNetList.ContainsKey(bitCell))
                        {
                            var logic = LogicOutputNetList[bitCell];
                            port.bits.Add(logic.id);
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
                        cell.input[portPair.Key] = LogicOutputNetList[portPair.Value].id;
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
                            }
                        }
                    }
                }
            }
            
            return netList.Serialize();
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