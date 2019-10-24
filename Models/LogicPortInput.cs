using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Iyokan_L1.Converter;
using Microsoft.VisualBasic;
using Iyokan_L1.Utils;

namespace Iyokan_L1.Models
{
    public class LogicPortInput : LogicPort
    {
        public LogicPortInput(string name, int yosysBit)
        {
            this.name = name;
            type = "input";
            cellBits = new List<Logic>();
            bits = new List<int>();
            this.yosysBit = yosysBit;
        }

        public override void ResolveNetList(YosysConverter converter)
        {
            cellBits = converter.FindIncomingNetContainsLogic(yosysBit);
            if (cellBits.Count == 0)
            {
                Console.WriteLine($"Port {name} is not used");
            }
        }
        
        public override string ToString()
        {
            return $"[Port] name:{this.name} id:{this.id} type:{this.type} to:{bits.ToString<int>()}";
        }
    }
}