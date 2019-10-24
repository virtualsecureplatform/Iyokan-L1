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
            [JsonIgnore] public Logic cellC { get; set; }
            [JsonIgnore] public Logic cellD { get; set; }

            public int C { get; set; }
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
            input.C = input.cellC.id;
            input.D = input.cellD.id;
            foreach (var cell in output.cellQ)
            {
                output.Q.Add(cell.id);
            }
        }

        public override string ToString()
        {
            return
                $"[Cell] id:{this.id} type:{this.type} inputC:{this.input.C} inputD:{this.input.D} outputQ:{this.output.Q.ToString<int>()}";
        }

        public override void ResolveNetList(YosysConverter converter)
        {
            if (yosysConnections["C"].Count != 1)
            {
                throw new Exception("Invalid netList");
            }

            if (yosysConnections["D"].Count != 1)
            {
                throw new Exception("Invalid netList");
            }

            int cellCyosysBit = yosysConnections["C"][0];
            List<Logic> cellCConnection = converter.FindOutgoingNetContainsLogic(cellCyosysBit);
            if (cellCConnection.Count != 1)
            {
                throw new Exception("Invalid netList");
            }

            input.cellC = cellCConnection[0];

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
    }
}