using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Iyokan_L1.Models
{
    public class LogicNetList
    {
        public List<LogicCell> cells { get; }
        public List<LogicPort> ports { get; }

        public Dictionary<string, Dictionary<int, LogicPortInput>> inputs { get; } =
            new Dictionary<string, Dictionary<int, LogicPortInput>>();

        public Dictionary<string, Dictionary<int, LogicPortOutput>> outputs { get; } =
            new Dictionary<string, Dictionary<int, LogicPortOutput>>();

        public LogicNetList()
        {
            cells = new List<LogicCell>();
            ports = new List<LogicPort>();
        }

        public void Add(LogicCell cell)
        {
            cell.id = ports.Count + cells.Count;
            cells.Add(cell);
        }

        public void Add(LogicPort port)
        {
            port.id = ports.Count + cells.Count;
            port.parentNetList = this;
            ports.Add(port);
            if (port.type == "input")
            {
                if (!inputs.ContainsKey(port.portName))
                {
                    inputs[port.portName] = new Dictionary<int, LogicPortInput>();
                }

                inputs[port.portName][port.portBit] = (LogicPortInput) port;
            }
            else if (port.type == "output")
            {
                if (!outputs.ContainsKey(port.portName))
                {
                    outputs[port.portName] = new Dictionary<int, LogicPortOutput>();
                }

                outputs[port.portName][port.portBit] = (LogicPortOutput) port;
            }
            else
            {
                throw new Exception("Invalid port type token:" + port.type);
            }
        }

        public string Serialize()
        {
            foreach (var item in cells)
            {
                item.Serialize();
            }

            foreach (var item in ports)
            {
                item.Serialize();
            }

            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        [JsonIgnore]
        private Dictionary<int,int> pris = new Dictionary<int, int>();
        public void UpdatePriority()
        {
            foreach (var port in ports)
            {
                if (port is LogicPortOutput)
                {
                    port.UpdatePriority(0);
                }
            }

            foreach (var cell in cells)
            {
                if (cell is LogicCellDFFP dff)
                {
                    dff.input.cellD.UpdatePriority(0);
                }
            }

            foreach (var port in ports)
            {
                if (!pris.ContainsKey(port.priority))
                {
                    pris[port.priority] = 0;
                }
                else
                {
                    pris[port.priority]++;
                }
            }

            foreach (var cell in cells)
            {
                if (!pris.ContainsKey(cell.priority))
                {
                    pris[cell.priority] = 0;
                }
                else
                {
                    pris[cell.priority]++;
                }
            }

            var priOrder = pris.OrderBy((x) => x.Key);
            foreach (var priNum in priOrder)
            {
                Console.WriteLine($"Priority {priNum.Key} : {priNum.Value}");
            }
        }
        public void Validation()
        {
            foreach (var cell in cells)
            {
                foreach (var child in cell.GetOutput())
                {
                    var childInput = child.GetInput();
                    if (!childInput.Contains(cell))
                    {
                        throw new Exception("Invalid NetList");
                    }
                }
            }

            foreach (var port in ports)
            {
                if (port is LogicPortOutput)
                {
                    continue;
                }
                foreach (var child in port.GetOutput())
                {
                    var childInput = child.GetInput();
                    if (!childInput.Contains(port))
                    {
                        throw new Exception("Invalid NetList");
                    }
                }
            }
        }
    }
}