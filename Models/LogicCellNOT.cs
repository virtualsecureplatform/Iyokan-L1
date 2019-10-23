using System;
using System.Runtime.Serialization;
using System.Threading;
using Newtonsoft.Json;

namespace Iyokan_L1.Models
{
    public class LogicCellNOT : LogicCell
    {
        public class Input
        {
            [JsonIgnore]
            public Logic cellA { get; set; }
            
            public int A { get; set; }
        }

        public class Output
        {
            [JsonIgnore]
            public Logic cellY { get; set; }            
            
            public int Y { get; set; }
        }

        public Input input { get; }
        public Output output { get; }
        
        public LogicCellNOT(YosysCell yosysCell)
        {
            AttachYosysCell(yosysCell);
            type = "NOT";
            input = new Input();
            output = new Output();
        }

        public override void Serialize()
        {
            input.A = input.cellA.id;
            output.Y = output.cellY.id;
        }
    }
}