using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Iyokan_L1.Models;
using Newtonsoft.Json;

namespace Iyokan_L1.Converter
{
    public class RomBuilder
    {
        public RomBuilder()
        {
        }

        public LogicNetList Build(int wordWidth, int wordNum)
        {
            LogicNetList netList = new LogicNetList();
            var (romCell, romPort) = BuildTwoWordCell(0, wordWidth);
            for (int i = 0; i < wordWidth; i++)
            {
                var output = new LogicPortOutput("RomData", i, "RomData", i);
                ((LogicCellMUX) romCell[wordWidth * 2 + i]).output.cellY.Add(output);
                output.cellBits.Add(romCell[wordWidth * 2 + i]);
                romPort.Add(output);
            }

            foreach (var port in romPort)
            {
                netList.Add(port);
            }

            foreach (var cell in romCell)
            {
                netList.Add(cell);
            }

            Console.WriteLine(netList.Serialize());
            return netList;
        }

        public class TwoWordRom
        {
            public List<LogicCell> romCell { get; } = new List<LogicCell>();
            public List<LogicCellMUX> outMUX { get; } = new List<LogicCellMUX>();

            public TwoWordRom(int addr, int width, LogicPortInput addrInput)
            {
                romCell.AddRange(BuildRomCell(addr, width));
                romCell.AddRange(BuildRomCell(addr + 1, width));
                for (int i = 0; i < width; i++)
                {
                    var mux = new LogicCellMUX((LogicCellROM) romCell[i], (LogicCellROM) romCell[width + i],
                        addrInput.portBit);
                    romCell.Add(mux);
                    outMUX.Add(mux);
                }

                for (int i = 0; i < width; i++)
                {
                    ((LogicCellMUX) romCell[width * 2 + i]).input.cellS = addrInput;
                    addrInput.cellBits.Add(romCell[width * 2 + i]);
                }
            }
        }

        public class MuxBlock
        {
            public List<LogicCell> romCell { get; } = new List<LogicCell>();
            public List<LogicCellMUX> outMUX { get; } = new List<LogicCellMUX>();

            public MuxBlock(TwoWordRom rom1, TwoWordRom rom2, LogicPortInput addrInput)
            {
                if (rom1.outMUX.Count != rom2.outMUX.Count)
                {
                    throw new Exception("Unmatced RomPort width");
                }

                int width = rom1.outMUX.Count;
                romCell.AddRange(rom1.romCell);
                romCell.AddRange(rom2.romCell);
                for (int i = 0; i < width; i++)
                {
                    var mux = new LogicCellMUX(rom1.outMUX[i], rom2.outMUX[i], addrInput.portBit);
                    mux.input.cellS = addrInput;
                    addrInput.cellBits.Add(mux);
                    romCell.Add(mux);
                    outMUX.Add(mux);
                }
            }

            public MuxBlock(MuxBlock mux1, MuxBlock mux2, LogicPortInput addrInput)
            {
                if (mux1.outMUX.Count != mux1.outMUX.Count)
                {
                    throw new Exception("Unmatched MUX output width");
                }

                int width = mux1.outMUX.Count;
                romCell.AddRange(mux1.romCell);
                romCell.AddRange(mux2.romCell);
                for (int i = 0; i < width; i++)
                {
                    var mux = new LogicCellMUX(mux1.outMUX[i], mux2.outMUX[i], addrInput.portBit);
                    mux.input.cellS = addrInput;
                    addrInput.cellBits.Add(mux);
                    romCell.Add(mux);
                    outMUX.Add(mux);
                }
            }
        }

        public static LogicNetList Build128WordCell(int width)
        {
            List<LogicPortInput> addr = new List<LogicPortInput>();
            LogicNetList netList = new LogicNetList();
            for (int i = 0; i < 7; i++)
            {
                var addrPort = new LogicPortInput($"RomAddr[{i}]", i, "RomAddr", i);
                addr.Add(addrPort);
                netList.Add(addrPort);
            }

            List<TwoWordRom> roms1 = new List<TwoWordRom>();
            for (int i = 0; i < 64; i++)
            {
                roms1.Add(new TwoWordRom(i * 2, width, addr[0]));
            }

            List<MuxBlock> roms2 = new List<MuxBlock>();
            for (int i = 0; i < 32; i++)
            {
                roms2.Add(new MuxBlock(roms1[i * 2], roms1[i * 2 + 1], addr[1]));
            }

            List<MuxBlock> roms3 = new List<MuxBlock>();
            for (int i = 0; i < 16; i++)
            {
                roms3.Add(new MuxBlock(roms2[i * 2], roms2[i * 2 + 1], addr[2]));
            }

            List<MuxBlock> roms4 = new List<MuxBlock>();
            for (int i = 0; i < 8; i++)
            {
                roms4.Add(new MuxBlock(roms3[i * 2], roms3[i * 2 + 1], addr[3]));
            }

            List<MuxBlock> roms5 = new List<MuxBlock>();
            for (int i = 0; i < 4; i++)
            {
                roms5.Add(new MuxBlock(roms4[i * 2], roms4[i * 2 + 1], addr[4]));
            }

            List<MuxBlock> roms6 = new List<MuxBlock>();
            for (int i = 0; i < 2; i++)
            {
                roms6.Add(new MuxBlock(roms5[i * 2], roms5[i * 2 + 1], addr[5]));
            }

            MuxBlock rom = new MuxBlock(roms6[0], roms6[1], addr[6]);
            for (int i = 0; i < width; i++)
            {
                LogicPortOutput outport = new LogicPortOutput($"RomData[{i}]", i, "RomData", i);
                outport.cellBits.Add(rom.outMUX[i]);
                rom.outMUX[i].output.cellY.Add(outport);
                netList.Add(outport);
            }

            foreach (var cell in rom.romCell)
            {
                netList.Add(cell);
            }

            return netList;
        }

        public static LogicNetList Build4WordCell(int width)
        {
            LogicNetList netList = new LogicNetList();
            LogicPortInput addr0 = new LogicPortInput($"RomAddr[{0}]", 0, "RomAddr", 0);
            LogicPortInput addr1 = new LogicPortInput($"RomAddr[{1}]", 1, "RomAddr", 1);
            TwoWordRom rom0 = new TwoWordRom(0, width, addr0);
            TwoWordRom rom1 = new TwoWordRom(2, width, addr0);
            MuxBlock rom = new MuxBlock(rom0, rom1, addr1);

            netList.Add(addr0);
            netList.Add(addr1);
            for (int i = 0; i < width; i++)
            {
                LogicPortOutput outport = new LogicPortOutput($"RomData[{i}]", i, "RomData", i);
                outport.cellBits.Add(rom.outMUX[i]);
                rom.outMUX[i].output.cellY.Add(outport);
                netList.Add(outport);
            }

            foreach (var cell in rom.romCell)
            {
                netList.Add(cell);
            }

            return netList;
        }

        public static LogicNetList Build2WordCell(int width)
        {
            LogicNetList netList = new LogicNetList();
            LogicPortInput addr0 = new LogicPortInput($"RomAddr[{0}]", 0, "RomAddr", 0);
            TwoWordRom rom = new TwoWordRom(0, width, addr0);

            if (width != rom.outMUX.Count)
            {
                throw new Exception("Unmatched DataPort width");
            }

            for (int i = 0; i < width; i++)
            {
                LogicPortOutput outport = new LogicPortOutput($"RomData[{i}]", i, "RomData", i);
                outport.cellBits.Add(rom.outMUX[i]);
                rom.outMUX[i].output.cellY.Add(outport);
                netList.Add(outport);
            }

            netList.Add(addr0);
            foreach (var cell in rom.romCell)
            {
                netList.Add(cell);
            }

            return netList;
        }

        private (List<LogicCell>, List<LogicPort>) BuildTwoWordCell(int addr, int width)
        {
            List<LogicCell> romCell = BuildRomCell(addr, width);
            romCell.AddRange(BuildRomCell(addr + 1, width));
            for (int i = 0; i < width; i++)
            {
                romCell.Add(new LogicCellMUX((LogicCellROM) romCell[i], (LogicCellROM) romCell[width + i], 0));
            }

            var addrInput = new LogicPortInput("RomAddr", 0, "RomAddr", 0);
            for (int i = 0; i < width; i++)
            {
                ((LogicCellMUX) romCell[width * 2 + i]).input.cellS = addrInput;
                addrInput.cellBits.Add(romCell[width * 2 + i]);
            }

            var romPort = new List<LogicPort>();
            romPort.Add(addrInput);
            return (romCell, romPort);
        }

        public static List<LogicCell> BuildRomCell(int addr, int width)
        {
            var res = new List<LogicCell>();
            for (int i = 0; i < width; i++)
            {
                res.Add(new LogicCellROM(i, addr));
            }

            return res;
        }
    }
}