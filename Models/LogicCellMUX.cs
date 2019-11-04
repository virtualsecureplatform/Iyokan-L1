using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;
using Iyokan_L1.Converter;
using Iyokan_L1.Utils;
using Newtonsoft.Json;

namespace Iyokan_L1.Models
{
    public class LogicCellMUX : LogicCell
    {
        public class Input
        {
            [JsonIgnore] public Logic cellA { get; set; }
            [JsonIgnore] public Logic cellB { get; set; }
            
            [JsonIgnore] public Logic cellS { get; set; }

            public int A { get; set; }
            public int B { get; set; }
            public int S { get; set; }
        }

        public class Output
        {
            [JsonIgnore] public List<Logic> cellY { get; set; }

            public List<int> Y { get; set; }
        }

        public Input input { get; }
        public Output output { get; }

        public int romAddrBit { get; }
        public LogicCellMUX(YosysCell yosysCell)
        {
            AttachYosysCell(yosysCell);
            type = "MUX";
            input = new Input();
            output = new Output();
            output.cellY = new List<Logic>();
            output.Y = new List<int>();
        }

        public LogicCellMUX(LogicCellROM in0, LogicCellROM in1, int addrBit)
        {
            type = "MUX";
            input = new Input();
            output = new Output();
            output.cellY = new List<Logic>();
            output.Y = new List<int>();
            romAddrBit = addrBit;
            input.cellA = in0;
            input.cellB = in1;
            in0.AttachOutput(this);
            in1.AttachOutput(this);
        }

        public LogicCellMUX(LogicCellMUX in0, LogicCellMUX in1, int addrBit)
        {
            type = "MUX";
            input = new Input();
            output = new Output();
            output.cellY = new List<Logic>();
            output.Y = new List<int>();
            romAddrBit = addrBit;
            input.cellA = in0;
            input.cellB = in1;
            in0.AttachOutput(this);
            in1.AttachOutput(this);
        }
        
        public void AttachOutput(LogicCellMUX mux)
        {
            output.cellY.Add(mux);
        }
        
        public override void Serialize()
        {
            input.A = input.cellA.id;
            input.B = input.cellB.id;
            input.S = input.cellS.id;
            output.Y.RemoveAll(p => true);
            foreach (var cell in output.cellY)
            {
                output.Y.Add(cell.id);
            }
        }

        public override string ToString()
        {
            return
                $"[Cell] id:{this.id} type:{this.type} inputA:{this.input.A} inputB:{this.input.B} inputS:{this.input.S} outputY:{this.output.Y.ToString<int>()}";
        }

        public override void ResolveNetList(YosysConverter converter)
        {
            if (yosysConnections["A"].Count != 1)
            {
                throw new Exception("Invalid netList");
            }

            if (yosysConnections["B"].Count != 1)
            {
                throw new Exception("Invalid netList");
            }

            if (yosysConnections["S"].Count != 1)
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

            int cellByosysBit = yosysConnections["B"][0];
            List<Logic> cellBConnection = converter.FindOutgoingNetContainsLogic(cellByosysBit);
            if (cellBConnection.Count != 1)
            {
                throw new Exception("Invalid netList");
            }
            input.cellB = cellBConnection[0];
            
            int cellSyosysBit = yosysConnections["S"][0];
            List<Logic> cellSConnection = converter.FindOutgoingNetContainsLogic(cellSyosysBit);
            if (cellSConnection.Count != 1)
            {
                throw new Exception("Invalid netList");
            }
            input.cellS = cellSConnection[0];
            
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
            if (input.cellB == removeLogic)
            {
                input.cellB = attachLogic;
            }
            if (input.cellS == removeLogic)
            {
                input.cellS = attachLogic;
            }
        }
        
        public override List<Logic> GetOutput()
        {
            return output.cellY;
        }
        
        public override List<Logic> GetInput()
        {
            var res = new List<Logic>();
            res.Add(input.cellA);
            res.Add(input.cellB);
            res.Add(input.cellS);
            return res;
        }
    }
}