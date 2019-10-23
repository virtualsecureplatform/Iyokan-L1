using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Microsoft.VisualBasic;

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

        public void Assign(int bit, LogicCell cell)
        {
            cellBits[bit] = cell;
        }
    }
}