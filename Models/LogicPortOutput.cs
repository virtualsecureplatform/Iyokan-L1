using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Iyokan_L1.Converter;
using Iyokan_L1.Utils;

namespace Iyokan_L1.Models
{
    public class LogicPortOutput : LogicPort
    {
        public LogicPortOutput(string name, int yosysBit, string portName, int portBit)
        {
            this.name = name;
            type = "output";
            cellBits = new List<Logic>();
            bits = new List<int>();
            this.yosysBit = yosysBit;
            this.portName = portName;
            this.portBit = portBit;
        }

        public override void ResolveNetList(YosysConverter converter)
        {
            cellBits = converter.FindOutgoingNetContainsLogic(yosysBit);
            if (cellBits.Count == 0)
            {
                Console.WriteLine($"Port {name} is not used");
            }
        }
        
        public override string ToString()
        {
            return $"[Port] name:{this.name} id:{this.id} type:{this.type} from:{bits.ToString<int>()}";
        }
    }
}