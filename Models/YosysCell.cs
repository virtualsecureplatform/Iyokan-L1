using System.Collections.Generic;

namespace Iyokan_L1.Models
{
    public class YosysCell
    {
        public int hide_name { get; set; }
        public string type { get; set; }
        public Dictionary<string, string> parameters { get; set; }
        public Dictionary<string, string> attributes { get; set; }
        public Dictionary<string, string> port_directions { get; set; }
        public Dictionary<string, List<int>> connections { get; set; }
    }
}