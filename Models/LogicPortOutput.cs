using System;
using System.Collections.Generic;
using System.Linq;
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
        public override void RemoveAttachOutputLogic(Logic removeLogic, Logic attachLogic)
        {
            throw new Exception("Invalid RemoveAttachOutputLogic");
        }
        public override void RemoveAttachInputLogic(Logic removeLogic, Logic attachLogic)
        {
            cellBits.Remove(removeLogic);
            cellBits.Add(attachLogic);
        }
        public override void UpdatePriority(int pri)
        {
            if (pri > priority)
            {
                priority = pri;
            }
            cellBits[0].UpdatePriority(priority+1);
        }

        public override List<Logic> GetOutput()
        {
            throw new Exception("Invalid Operation: GetOutput");
        }
        public override List<Logic> GetInput()
        {
            return cellBits;
        }
    }
}