using System.Collections.Generic;
using Iyokan_L1.Converter;

namespace Iyokan_L1.Models
{
    public abstract class Logic
    {
        public int id { get; set; }
        public string type { get; set; }
        public int priority { get; set; } = 0;

        public abstract void RemoveAttachOutputLogic(Logic removeLogic, Logic attachLogic);
        public abstract void RemoveAttachInputLogic(Logic removeLogic, Logic attachLogic);
        public abstract void Serialize();
        public abstract void ResolveNetList(YosysConverter converter);

        public abstract void UpdatePriority(int pri);
        public abstract List<Logic> GetOutput();
        public abstract List<Logic> GetInput();
    }
}