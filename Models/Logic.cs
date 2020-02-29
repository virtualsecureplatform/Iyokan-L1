using System.Collections.Generic;
using Iyokan_L1.Converter;
using Newtonsoft.Json;

namespace Iyokan_L1.Models
{
    public abstract class Logic
    {
        public int id { get; set; }
        public string type { get; set; }
        public int priority { get; set; } = 0;
        
        // This link connects to input logics
        [JsonIgnore] public Dictionary<int, Logic> inputLink = new Dictionary<int, Logic>();
        [JsonIgnore] public Dictionary<int, Logic> tmpInputLink = new Dictionary<int, Logic>();

        // This link  onnects to output logics
        [JsonIgnore] public Dictionary<int, Logic> outputLink = new Dictionary<int, Logic>();

        public abstract void UpdatePriority(int pri);

    }
}