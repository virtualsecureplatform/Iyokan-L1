using System.Collections.Generic;

namespace Iyokan_L1.Models
{
    public class YosysNetname
    {
       public string hide_name { get; set; }
       public List<int> bits { get; set; }
       public Dictionary<string, string> attributes { get; set; }
    }
}