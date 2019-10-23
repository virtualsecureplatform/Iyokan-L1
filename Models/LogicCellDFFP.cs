using System;
using System.Runtime.Serialization;
using System.Threading;
using Newtonsoft.Json;

namespace Iyokan_L1.Models
{
    public class LogicCellDFFP : LogicCell
    {
        public class Input
        {
            [JsonIgnore]
            public Logic cellC { get; set; }
            [JsonIgnore]
            public Logic cellD { get; set; }
            
            public int C { get; set; }
            public int D { get; set; }
        }

        public class Output
        {
            [JsonIgnore]
            public Logic cellQ { get; set; }
            
            public int Q { get; set; }
        }

        public Input input { get; }
        public Output output { get; }
        
        public LogicCellDFFP(YosysCell yosysCell)
        {
            AttachYosysCell(yosysCell);
            type = "DFFP";
            input = new Input();
            output = new Output();
        }

        public override void Serialize()
        {
            input.C = input.cellC.id;
            input.D = input.cellD.id;
            output.Q = output.cellQ.id;
        }
    }
}