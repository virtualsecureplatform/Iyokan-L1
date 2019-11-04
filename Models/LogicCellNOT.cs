using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;
using Iyokan_L1.Converter;
using Iyokan_L1.Utils;
using Newtonsoft.Json;

namespace Iyokan_L1.Models
{
    public class LogicCellNOT : LogicCell
    {
        public class Input
        {
            [JsonIgnore] public Logic cellA { get; set; }

            public int A { get; set; }
        }

        public class Output
        {
            [JsonIgnore] public List<Logic> cellY { get; set; }

            public List<int> Y { get; set; }
        }

        public Input input { get; }
        public Output output { get; }

        public LogicCellNOT(YosysCell yosysCell)
        {
            AttachYosysCell(yosysCell);
            type = "NOT";
            input = new Input();
            output = new Output();
            output.cellY = new List<Logic>();
            output.Y = new List<int>();
        }

        public override void Serialize()
        {
            input.A = input.cellA.id;
            output.Y.RemoveAll(p => true);
            foreach (var cell in output.cellY)
            {
                output.Y.Add(cell.id);
            }
        }

        public override string ToString()
        {
            return
                $"[Cell] id:{this.id} type:{this.type} inputA:{this.input.A} outputY:{this.output.Y.ToString<int>()}";
        }

        public override void ResolveNetList(YosysConverter converter)
        {
            if (yosysConnections["A"].Count != 1)
            {
                throw new Exception("Invalid netList");
            }

            int cellAyosysBit = yosysConnections["A"][0];
            List<Logic> cellAConnection = converter.FindOutgoingNetContainsLogic(cellAyosysBit);
            if (cellAConnection.Count != 1)
            {
                throw new Exception("Invalid netList");
            }

            input.cellA = cellAConnection[0];
            var cellYyosysBits = yosysConnections["Y"];
            foreach (var bit in cellYyosysBits)
            {
                output.cellY.AddRange(converter.FindIncomingNetContainsLogic(bit));
            }
        }
        public override void RemoveAttachOutputLogic(Logic removeLogic, Logic attachLogic)
        {
            output.cellY.Remove(removeLogic);
            output.cellY.Add(attachLogic);
        }
        public override void RemoveAttachInputLogic(Logic removeLogic, Logic attachLogic)
        {
            if (input.cellA == removeLogic)
            {
                input.cellA = attachLogic;
            }
        }
        public override List<Logic> GetOutput()
        {
            return output.cellY;
        }
        public override void UpdatePriority(int pri)
        {
            if (pri > priority)
            {
                priority = pri;
            }
            input.cellA.UpdatePriority(priority+1);
        }

        public override List<Logic> GetInput()
        {
            var res = new List<Logic>();
            res.Add(input.cellA);
            return res;
        }
    }
}