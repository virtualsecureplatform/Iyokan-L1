using System.Collections.Generic;
using Newtonsoft.Json;

namespace Iyokan_L1.Models
{
    public class LogicNetList
    {
        public List<LogicCell> cells { get; }
        public List<LogicPort> ports { get; }

        public LogicNetList()
        {
            cells = new List<LogicCell>();
            ports = new List<LogicPort>();
        }

        public void Add(LogicCell cell)
        { 
            cell.id = ports.Count+cells.Count;
            cells.Add(cell);
        }

        public void Add(LogicPort port)
        {
                port.id = ports.Count+cells.Count;
                ports.Add(port);
        }

        public string Serialize()
        {
            foreach (var item in cells)
            {
               item.Serialize(); 
            }

            foreach (var item in ports)
            {
                item.Serialize();
            }
            
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}