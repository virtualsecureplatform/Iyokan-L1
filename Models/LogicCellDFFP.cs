using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;
using Iyokan_L1.Converter;
using Newtonsoft.Json;
using Iyokan_L1.Utils;

namespace Iyokan_L1.Models
{
    public class LogicCellDFFP : LogicCell
    {
        public class Input
        {
            [JsonIgnore] public Logic cellD { get; set; }

            public int D { get; set; }
        }

        public class Output
        {
            [JsonIgnore] public List<Logic> cellQ { get; set; }

            public List<int> Q { get; set; }
        }

        public Input input { get; }
        public Output output { get; }

        public LogicCellDFFP(YosysCell yosysCell)
        {
            AttachYosysCell(yosysCell);
            type = "DFFP";
            input = new Input();
            output = new Output();
            output.cellQ = new List<Logic>();
            output.Q = new List<int>();
        }

        public override void Serialize()
        {
            input.D = input.cellD.id;
            output.Q.RemoveAll(p => true);
            foreach (var cell in output.cellQ)
            {
                output.Q.Add(cell.id);
            }
        }

        public override string ToString()
        {
            return
                $"[Cell] id:{this.id} type:{this.type} inputD:{this.input.D} outputQ:{this.output.Q.ToString<int>()}";
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

            input.cellD = cellDConnection[0];
            var cellQyosysBits = yosysConnections["Q"];
            foreach (var bit in cellQyosysBits)
            {
                output.cellQ.AddRange(converter.FindIncomingNetContainsLogic(bit));
            }
        }
        public override void RemoveAttachOutputLogic(Logic removeLogic, Logic attachLogic)
        {
            output.cellQ.Remove(removeLogic);
            output.cellQ.Add(attachLogic);
        }
        public override void RemoveAttachInputLogic(Logic removeLogic, Logic attachLogic)
        {
            if (input.cellD == removeLogic)
            {
                input.cellD = attachLogic;
            }
        }
        
        public override List<Logic> GetOutput()
        {
            return output.cellQ;
        }
        
        public override void UpdatePriority(int pri)
        {
            if (pri > priority)
            {
                priority = pri;
            }
        }

        public override List<Logic> GetInput()
        {
            var res = new List<Logic>();
            res.Add(input.cellD);
            return res;
        }
    }
}