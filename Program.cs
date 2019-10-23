using System;
using System.Collections.Generic;
using System.IO;
using Iyokan_L1.Models;
using Newtonsoft.Json;

namespace Iyokan_L1
{
    class Program
    {
        static void Main(string[] args)
        {
            var text = File.ReadAllText("/home/naoki/Iyokan-L1/test/circuit.json");
            var yosys = JsonConvert.DeserializeObject<YosysNetList>(text);
            Console.WriteLine(yosys.modules["YosysTest"].attributes.src);
            var module = yosys.modules["YosysTest"];
            foreach (KeyValuePair<string, YosysPort> port in module.ports)
            {
               Console.WriteLine(port.Key); 
               Console.WriteLine(port.Value.direction);
            }
            foreach (KeyValuePair<string, YosysCell> cell in module.cells)
            {
                Console.WriteLine(cell.Value.type);
            }
            foreach (KeyValuePair<string, YosysNetname> netname in module.netnames)
            {
                Console.WriteLine(netname.Key);
            }
        }
    }
}