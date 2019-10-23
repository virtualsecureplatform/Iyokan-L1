using System;
using System.Runtime.Serialization;
using System.Threading;
using Newtonsoft.Json;

namespace Iyokan_L1.Models
{
    public class LogicCellXNOR : LogicCell
    {
        public class Input
        {
            [JsonIgnore]
            public Logic cellA { get; set; }
            [JsonIgnore]
            public Logic cellB { get; set; }
            
            public int A { get; set; }
            public int B { get; set; }
        }

        public class Output
        {
            [JsonIgnore]
            public Logic cellY { get; set; }            
            
            public int Y { get; set; }
        }

        public Input input { get; }
        public Output output { get; }
        
        public LogicCellXNOR(YosysCell yosysCell)
        {
            AttachYosysCell(yosysCell);
            type = "XNOR";
            input = new Input();
            output = new Output();
        }

        public override void Serialize()
        {
            input.A = input.cellA.id;
            input.B = input.cellB.id;
            output.Y = output.cellY.id;
        }
    }
}