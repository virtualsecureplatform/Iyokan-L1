using System.Collections.Generic;
using Iyokan_L1.Converter;

namespace Iyokan_L1.Models
{
    public abstract class Logic
    {
        public int id { get; set; }
        public string type { get; set; }
        public int priority { get; set; } = 0;
        

    }
}