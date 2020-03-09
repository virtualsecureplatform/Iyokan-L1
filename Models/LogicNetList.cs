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

        public Dictionary<string, Dictionary<int, LogicPort>> inputs { get; } =
            new Dictionary<string, Dictionary<int, LogicPort>>();

        public Dictionary<string, Dictionary<int, LogicPort>> outputs { get; } =
            new Dictionary<string, Dictionary<int, LogicPort>>();

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
            ports.Add(port);
            if (port.type == "input")
            {
                if (!inputs.ContainsKey(port.portName))
                {
                    inputs[port.portName] = new Dictionary<int, LogicPort>();
                }

                inputs[port.portName][port.portBit] = port;
            }
            else if (port.type == "output")
            {
                if (!outputs.ContainsKey(port.portName))
                {
                    outputs[port.portName] = new Dictionary<int, LogicPort>();
                }

                outputs[port.portName][port.portBit] = port;
            }
            else
            {
                throw new Exception("Invalid port type token:" + port.type);
            }
        }
        
        public string Serialize()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        [JsonIgnore] private Dictionary<int, int> pris = new Dictionary<int, int>();

        public void UpdatePriority()
        {
            //Aggregate Priority Stats
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
    }
}