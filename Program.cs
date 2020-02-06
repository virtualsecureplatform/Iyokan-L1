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
            var parseResult = Parser.Default.ParseArguments<Option>(args);
            Option opt = null;
            switch (parseResult.Tag)
            {
                case ParserResultType.Parsed:
                    var parsed = parseResult as Parsed<Option>;
                    opt = parsed.Value;
                    Console.WriteLine($"InputFile: {opt.input}");
                    Console.WriteLine($"OutputFile: {opt.output}");
                    Console.WriteLine($"WithROM: {opt.WithRom}");
                    
                    YosysConverter conv1 = new YosysConverter(opt.input);
                    conv1.Convert(false, false);
                    LogicNetList netList = null;
                    if (opt.WithRom)
                    {
                        var romNetList = RomBuilder.Build128WordCell(32);
                        var integrator1 = new NetListIntegrator();
                        integrator1.AddNetList(conv1.netList);
                        integrator1.AddNetList(romNetList);
                        integrator1.Combine("io_romAddr", "RomAddr");
                        integrator1.Combine("RomData", "io_romData");
                        netList = integrator1.Integrate();
                    }
                    else
                    {
                        netList = conv1.netList;
                    }
                    netList.UpdatePriority();
                    var res = netList.Serialize();
                    netList.Validation();
                    FileStream fs = new FileStream(opt.output, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine(res);
                    sw.Close();
                    fs.Close();
                    break;
                case ParserResultType.NotParsed:
                    break;
            }
        }
    }
}