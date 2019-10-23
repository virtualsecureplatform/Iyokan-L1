using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Iyokan_L1.Models
{
    public class LogicPortOutput : LogicPort
    {
        public LogicPortOutput(string name, int yosysBit)
        {
            this.name = name;
            type = "output";
            cellBits = new List<Logic>();
            bits = new List<int>();
            this.yosysBit = yosysBit;
        }

        public void Assign(int bit, LogicCell cell)
        {
            cellBits[bit] = cell;
        }
        
    }
}