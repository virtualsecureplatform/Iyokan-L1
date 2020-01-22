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
            /*
            YosysConverter conv1 = new YosysConverter(args[0]);
            conv1.Convert(false, false);
            var romNetList = RomBuilder.Build128WordCell(32);
            var integrator1 = new NetListIntegrator();
            integrator1.AddNetList(conv1.netList);
            integrator1.AddNetList(romNetList);
            integrator1.Combine("io_romAddr", "RomAddr");
            integrator1.Combine("RomData", "io_romData");
            var netList = integrator1.Integrate();
            var netList = conv1.netList;
            netList.UpdatePriority();
            var res = netList.Serialize();
            netList.Validation();
            FileStream fs = new FileStream(args[1], FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(res);
            sw.Close();
            fs.Close();
            */
            YosysConverter conv1 = new YosysConverter(args[0]);
            conv1.Convert(false, false);
            var romNetList = RomBuilder.Build64WordCell(64);
            var integrator1 = new NetListIntegrator();
            integrator1.AddNetList(conv1.netList);
            integrator1.AddNetList(romNetList);
            //integrator1.Combine("io_romAddr", "RomAddr");
            //integrator1.Combine("RomData", "io_romData");
            //var netList = integrator1.Integrate();
            var netList = conv1.netList;
            netList.UpdatePriority();
            var res = netList.Serialize();
            netList.Validation();
            FileStream fs = new FileStream(args[1], FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(res);
            sw.Close();
            fs.Close();
        }
    }
}