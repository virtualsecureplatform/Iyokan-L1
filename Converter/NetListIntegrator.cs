using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using Iyokan_L1.Models;

namespace Iyokan_L1.Converter
{
    public class NetListIntegrator
    {
        List<LogicNetList> netLists = new List<LogicNetList>();
        private List<string> removePort = new List<string>();

        public NetListIntegrator()
        {
        }

        public void AddNetList(LogicNetList netList)
        {
            netLists.Add(netList);
        }

        public Dictionary<int, LogicPortOutput> SearchOutputPort(string outputPort)
        {
            foreach (var netList in netLists)
            {
                foreach (var output in netList.outputs)
                {
                    if (output.Key == outputPort)
                    {
                        return output.Value;
                    }
                }
            }

            throw new Exception("Not Found Output:" + outputPort);
        }

        public List<Dictionary<int, LogicPortInput>> SearchInputPort(string inputPort)
        {
            var res = new List<Dictionary<int, LogicPortInput>>();
            foreach (var netList in netLists)
            {
                foreach (var input in netList.inputs)
                {
                    if (input.Key == inputPort)
                    {
                        res.Add(input.Value);
                    }
                }
            }

            if (res.Count == 0)
            {
                throw new Exception("Not Found Input:" + inputPort);
            }

            return res;
        }

        public void Combine(string outputPort, string inputPort)
        {
            removePort.Add(outputPort);
            removePort.Add(inputPort);
            var outPort = SearchOutputPort(outputPort);
            var inPort = SearchInputPort(inputPort);

            if (outPort.Count == 0)
            {
                throw new Exception("Invalid outputPort:" + outputPort);
            }

            if (inPort.Count == 0)
            {
                throw new Exception("Invalid inputPort:" + inputPort);
            }

            LogicNetList outParentNetList = outPort[0].parentNetList;
            List<LogicNetList> inParentNetList = new List<LogicNetList>();
            foreach (var input in inPort)
            {
                inParentNetList.Add(input[0].parentNetList);
            }

            foreach (var output in outPort)
            {
                LogicPortOutput outport = output.Value;
                int outbit = output.Key;
                if (outport.cellBits.Count == 0)
                {
                    continue;
                }

                var parentLogic = outport.cellBits[0];
                foreach (var input in inPort)
                {
                    LogicPortInput logicPortInput = input[outbit];
                    if (logicPortInput == null)
                    {
                        continue;
                    }

                    foreach (var childLogic in logicPortInput.cellBits)
                    {
                        parentLogic.RemoveAttachOutputLogic(outport, childLogic);
                        childLogic.RemoveAttachInputLogic(logicPortInput, parentLogic);
                    }
                }
            }
        }

        public LogicNetList Integrate()
        {
            var res = new LogicNetList();
            foreach (var netList in netLists)
            {
                foreach (var cell in netList.cells)
                {
                    res.Add(cell);
                }

                foreach (var port in netList.ports)
                {
                    if (!removePort.Contains(port.portName))
                    {
                        res.Add(port);
                    }
                }
            }

            return res;
        }
    }
}