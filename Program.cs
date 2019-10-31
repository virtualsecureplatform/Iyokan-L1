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
            YosysConverter converter = new YosysConverter("/home/naoki/Iyokan-L1/test/circuit.json");
            var res = converter.Convert();
            FileStream fs = new FileStream("/home/naoki/Iyokan-L2/test.json", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(res);
            sw.Close();
            fs.Close();
        }
    }
}