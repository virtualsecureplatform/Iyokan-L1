using System.Collections.Generic;

namespace Iyokan_L1.Models
{
    public class YosysModule
    {
        public YosysAttribute attributes { get; set; }
        public Dictionary<string, YosysPort> ports { get; set; }
        public Dictionary<string, YosysCell> cells { get; set; }
        public Dictionary<string, YosysNetname> netnames { get; set; }
    }
}