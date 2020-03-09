using System;
using System.Collections.Generic;
using Iyokan_L1.Utils;
using Newtonsoft.Json;

namespace Iyokan_L1.Models
{
    public class LogicPort : Logic
    {
        private string name;
        
        [JsonIgnore] public List<int> bitsCell = new List<int>();
        
        public List<int> bits = new List<int>();

        public string portName { get; }

        public int portBit { get; }
        public LogicPort(string type, string name, string portName, int portBit)
        {
            this.name = name;
            this.type = type;
            this.portName = portName;
            this.portBit = portBit;
        }

        public override void UpdatePriority(int pri)
        {
            if (pri > priority)
            {
                priority = pri;
            }
        }

        public override string ToString()
        {
            return $"[Port {type}] name:{this.name} id:{this.id} type:{this.type} to:{bits.ToString<int>()}";
        }
    }
}