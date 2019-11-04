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
        public LogicPortInput(string name, int yosysBit, string portName, int portBit)
        {
            this.name = name;
            type = "input";
            cellBits = new List<Logic>();
            bits = new List<int>();
            this.yosysBit = yosysBit;
            this.portName = portName;
            this.portBit = portBit;
        }

        public LogicPortInput(int addrBit, LogicCell connCell)
        {
            this.portBit = addrBit;
            cellBits.Add(connCell);
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

        public override void RemoveAttachOutputLogic(Logic removeLogic, Logic attachLogic)
        {
            cellBits.Remove(removeLogic);
            cellBits.Add(attachLogic);
        }

        public override void RemoveAttachInputLogic(Logic removeLogic, Logic attachLogic)
        {
            throw new Exception("Invalid RemoveAttachInputLogic");
        }

        public override void UpdatePriority(int pri)
        {
            if (pri > priority)
            {
                priority = pri;
            }
        }

        public override List<Logic> GetOutput()
        {
            return cellBits;
        }

        public override List<Logic> GetInput()
        {
            throw new Exception("Invalid Operation: GetInput");
        }
    }
}