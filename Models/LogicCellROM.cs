using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;
using Iyokan_L1.Converter;
using Newtonsoft.Json;
using Iyokan_L1.Utils;

namespace Iyokan_L1.Models
{
    public class LogicCellROM : LogicCell
    {
        public class Input
        {
        }

        public class Output
        {
            [JsonIgnore] public List<Logic> cellQ { get; set; }

            public List<int> Q { get; set; }
        }

        public Input input { get; }
        public Output output { get; }

        public int romAddress { get; }
        
        public int romBit { get; }
        
        public LogicCellROM(int bit, int address)
        {
            type = "ROM";
            input = new Input();
            output = new Output();
            output.cellQ = new List<Logic>();
            output.Q = new List<int>();
            romBit = bit;
            romAddress = address;
        }

        public void AttachOutput(LogicCellMUX mux)
        {
            output.cellQ.Add(mux);
        }

        public override void Serialize()
        {
            output.Q.RemoveAll(p => true);
            foreach (var cell in output.cellQ)
            {
                output.Q.Add(cell.id);
            }
        }

        public override string ToString()
        {
            return
                $"[Cell] id:{this.id} type:{this.type} outputQ:{this.output.Q.ToString<int>()}";
        }

        public override void ResolveNetList(YosysConverter converter)
        {
            if (yosysConnections["D"].Count != 1)
            {
                throw new Exception("Invalid netList");
            }

            int cellDyosysBit = yosysConnections["D"][0];
            List<Logic> cellDConnection = converter.FindOutgoingNetContainsLogic(cellDyosysBit);
            if (cellDConnection.Count != 1)
            {
                throw new Exception("Invalid netList");
            }
        }
        public override void RemoveAttachOutputLogic(Logic removeLogic, Logic attachLogic)
        {
            output.cellQ.Remove(removeLogic);
            output.cellQ.Add(attachLogic);
        }
        public override void RemoveAttachInputLogic(Logic removeLogic, Logic attachLogic)
        {
        }
        public override List<Logic> GetOutput()
        {
            return output.cellQ;
        }
        public override List<Logic> GetInput()
        {
            throw new Exception("Invalid Operation: GetInput");
        }
    }
}