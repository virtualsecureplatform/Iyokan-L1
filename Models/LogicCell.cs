using System.Collections.Generic;
using Iyokan_L1.Converter;
using Newtonsoft.Json;

namespace Iyokan_L1.Models
{
    public abstract class LogicCell : Logic
    {
        [JsonIgnore]
        public Dictionary<string, List<int>> yosysConnections { get; set; }
        [JsonIgnore]
        public Dictionary<string, string> yosysPortDirections { get; set; }


        public void AttachYosysCell(YosysCell yosysCell)
        {
            yosysConnections = yosysCell.connections;
            yosysPortDirections = yosysCell.port_directions;
        }

        private List<string> GetOutputPort()
        {
            var port = new List<string>();
            foreach (var item in yosysPortDirections)
            {
                if (item.Value == "output")
                {
                    port.Add(item.Key);
                }
            }

            return port;
        }
        private List<string> GetInputPort()
        {
            var port = new List<string>();
            foreach (var item in yosysPortDirections)
            {
                if (item.Value == "input")
                {
                    port.Add(item.Key);
                }
            }

            return port;
        }
        
        public bool ContainOutputNet(int netID)
        {
            var outputPort = GetOutputPort();
            foreach(var port in outputPort)
            {
                if (yosysConnections[port].Contains(netID))
                {
                    return true;
                }
            }
            return false;
        }
        
        public bool ContainInputNet(int netID)
        {
            var inputPort = GetInputPort();
            foreach(var port in inputPort)
            {
                if (yosysConnections[port].Contains(netID))
                {
                    return true;
                }
            }
            return false;
        }

        public override abstract string ToString();

        public abstract void ResolveNetList(YosysConverter converter);
    }
}