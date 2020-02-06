using System.Collections.Generic;
using CommandLine;

namespace Iyokan_L1.Models
{
    public class Option
    {
           [Option( "with-rom", Required = false, HelpText = "Integrate ROM")]
           public bool WithRom { get; set; }
           
           [Option('i', "input", Required = true, HelpText = "Input yosys format json file")]
           public string input { get; set; }
           
           [Option('o', "output", Required = true, HelpText = "Output iyokan format json file")]
           public string output { get; set; }
    }
}