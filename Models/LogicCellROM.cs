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
        public int romAddress { get; }

        public int romBit { get; }

        public LogicCellROM(int bit, int address)
        {
            type = "ROM";
            romBit = bit;
            romAddress = address;
        }

        public override string ToString()
        {
            return
                $"[Cell] id:{this.id} type:{this.type} outputQ:";
        }
    }
}