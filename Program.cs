using System;
using System.IO;
using CommandLine;
using Iyokan_L1.Converter;
using Iyokan_L1.Models;


namespace Iyokan_L1
{
    class Program
    {
        static void Main(string[] args)
        {
            var convertRam = args.Length == 3 && args[2] == "genRAM";
            YosysConverter conv1 = new YosysConverter(args[0]);
            conv1.Convert(convertRam);
            var netList = conv1.netList;
            var res = netList.Serialize();
            FileStream fs = new FileStream(args[1], FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(res);
            sw.Close();
            fs.Close();
        }
    }
}