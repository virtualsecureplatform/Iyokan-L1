using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace Iyokan_L1.Models
{
    public class YosysNetList
    {
        public String creator { get; set; }

        public Dictionary<string, YosysModule> modules { get; set; }
    }
}