using System.Collections.Generic;
using Iyokan_L1.Converter;
using Newtonsoft.Json;

namespace Iyokan_L1.Models
{
    public class LogicCell : Logic
    {

        [JsonIgnore] public Dictionary<string, int> inputCell = new Dictionary<string, int>();
        [JsonIgnore] public Dictionary<string, List<int>> outputCell = new Dictionary<string, List<int>>();
        
        public Dictionary<string, int> input = new Dictionary<string, int>();
        public Dictionary<string, List<int>> output = new Dictionary<string, List<int>>();

        public LogicCell(string type)
        {
            this.type = type;
        }
        
        public LogicCell(){}

        public override string ToString()
        {
            return
                $"[Cell] id:{this.id} type:{this.type} input: output:";
        }
    }
}