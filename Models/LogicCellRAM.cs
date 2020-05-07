using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;
using Iyokan_L1.Converter;
using Newtonsoft.Json;
using Iyokan_L1.Utils;

namespace Iyokan_L1.Models
{
    public class LogicCellRAM : LogicCell
    {

        public LogicCellRAM(int ramAddress, int ramBit)
        {
            type = "RAM";
            this.ramAddress = ramAddress;
            this.ramBit = ramBit;
        }

        public override string ToString()
        {
            return
                $"[Cell] id:{this.id} type:{this.type} addr:{this.ramAddress} bit:{this.ramBit} inputD: outputQ:";
        }
    }
}