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
            YosysConverter conv1 = new YosysConverter(args[0]);
            conv1.Convert(false, false);
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