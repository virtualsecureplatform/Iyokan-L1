using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Linq;
using Iyokan_L1.Models;
using Iyokan_L1.Utils;
using Newtonsoft.Json;

namespace Iyokan_L1.Converter
{
    public class YosysConverter
    {
        private string jsonPath;
        private LogicNetList netList;

        public YosysConverter(string jsonPath)
        {
            this.jsonPath = jsonPath;
            netList = new LogicNetList();
        }

        public string Convert()
        {
            var yosysNetList = Deserialize();
            var yosysModule = yosysNetList.modules.First();
            var yosysPorts = yosysModule.Value.ports;
            var yosysCells = yosysModule.Value.cells;

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
                var port = ConvertYosysPort(yosysPort);
                foreach (var item in port)
                {
                    netList.Add(item);
                }
            }

            foreach (var yosysCell in yosysCells)
            {
                var cell = ConvertYosysCell(yosysCell);
                netList.Add(cell);
            }

            NetListResolver();
            return netList.Serialize();
        }

        private List<LogicPort> ConvertYosysPort(KeyValuePair<string, YosysPort> port)
        {
            var direction = port.Value.direction;
            var bitWidth = port.Value.bits.Count;
            var bits = port.Value.bits;
            var ports = new List<LogicPort>();
            switch (direction)
            {
                case "input":
                    if (bitWidth == 1)
                    {
                        ports.Add(new LogicPortInput(port.Key, bits[0], port.Key, 0));
                    }
                    else
                    {
                        for (int i = 0; i < bitWidth; i++)
                        {
                            ports.Add(new LogicPortInput($"{port.Key}[{i}]", bits[i], port.Key, i));
                        }
                    }

                    return ports;
                case "output":
                    if (bitWidth == 1)
                    {
                        ports.Add(new LogicPortOutput(port.Key, bits[0], port.Key, 0));
                    }
                    else
                    {
                        for (int i = 0; i < bitWidth; i++)
                        {
                            ports.Add(new LogicPortOutput($"{port.Key}[{i}]", bits[i], port.Key, i));
                        }
                    }

                    return ports;
                default:
                    throw new Exception($"Invalid direction token: {direction}");
            }
        }

        private LogicCell ConvertYosysCell(KeyValuePair<string, YosysCell> cell)
        {
            var type = cell.Value.type;
            var connections = cell.Value.connections;
            switch (type)
            {
                case "$_NOT_":
                    return new LogicCellNOT(cell.Value);
                case "$_AND_":
                    return new LogicCellAND(cell.Value);
                case "$_ANDNOT_":
                    return new LogicCellANDNOT(cell.Value);
                case "$_NAND_":
                    return new LogicCellNAND(cell.Value);
                case "$_OR_":
                    return new LogicCellOR(cell.Value);
                case "$_XOR_":
                    return new LogicCellXOR(cell.Value);
                case "$_XNOR_":
                    return new LogicCellXNOR(cell.Value);
                case "$_NOR_":
                    return new LogicCellNOR(cell.Value);
                case "$_ORNOT_":
                    return new LogicCellORNOT(cell.Value);
                case "$_DFF_P_":
                    return new LogicCellDFFP(cell.Value);
                case "$_MUX_":
                    return new LogicCellMUX(cell.Value);
                default:
                    throw new Exception($"Invalid type token: {type}");
            }
        }

        public List<Logic> FindIncomingNetContainsLogic(int netID)
        {
            List<Logic> res = new List<Logic>();
            foreach (var port in netList.ports)
            {
                if (port.ContainOutputNet(netID))
                {
                    res.Add(port);
                }
            }

            foreach (var cell in netList.cells)
            {
                if (cell.ContainInputNet(netID))
                {
                    res.Add(cell);
                }
            }

            return res;
        }

        public List<Logic> FindOutgoingNetContainsLogic(int netID)
        {
            List<Logic> res = new List<Logic>();
            foreach (var port in netList.ports)
            {
                if (port.ContainInputNet(netID))
                {
                    res.Add(port);
                }
            }

            foreach (var cell in netList.cells)
            {
                if (cell.ContainOutputNet(netID))
                {
                    res.Add(cell);
                }
            }

            return res;
        }

        private void NetListResolver()
        {
            foreach (var port in netList.ports)
            {
                port.ResolveNetList(this);
                port.Serialize();
                Console.WriteLine(port);
            }

            foreach (var cell in netList.cells)
            {
                cell.ResolveNetList(this);
                cell.Serialize();
                Console.WriteLine(cell);
            }

            netList.ports.RemoveAll(p => p.cellBits.Count == 0 && p.type != "output");
        }

        private YosysNetList Deserialize()
        {
            var yosysJson = File.ReadAllText(jsonPath);
            return JsonConvert.DeserializeObject<YosysNetList>(yosysJson);
        }
    }
}