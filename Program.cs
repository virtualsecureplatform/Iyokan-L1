using System;
using System.Collections.Generic;
using System.IO;
using Iyokan_L1.Converter;
using Iyokan_L1.Models;
using Newtonsoft.Json;


namespace Iyokan_L1
{
    class Program
    {
        static void Main(string[] args)
        {
            YosysConverter conv1 = new YosysConverter("/home/naoki/Iyokan-L1/test/circuit.json");
            conv1.Convert();
            var romNetList = RomBuilder.Build128WordCell(32);
            var integrator = new NetListIntegrator();
            integrator.AddNetList(conv1.netList);
            integrator.AddNetList(romNetList);
            integrator.Combine("io_romAddr", "RomAddr");
            integrator.Combine("RomData", "io_romData");
            var netList = integrator.Integrate();
            netList.UpdatePriority();
            var res = netList.Serialize();
            netList.Validation();
            //Console.WriteLine(res);
            FileStream fs = new FileStream("/home/naoki/Iyokan-L2/test-pri.json", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(res);
            sw.Close();
            fs.Close();
        }
    }
}