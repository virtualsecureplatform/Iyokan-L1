using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Iyokan_L1.Models
{
    public abstract class LogicPort : Logic
    {
        public string name { get; set; }
        [JsonIgnore] public List<Logic> cellBits;

        [JsonIgnore] public int yosysBit { get; set; }
        
        public List<int> bits;
        
        public bool ContainOutputNet(int netID)
        {
            if (type != "output")
            {
                return false; 
            }

            return yosysBit == netID;
        }

        public bool ContainInputNet(int netID)
        {
            if (type != "input")
            {
                return false;
            }

            return yosysBit == netID;

        }
        
        public override void Serialize()
        {
            for (int i = 0; i < cellBits.Count; i++)
            {
                bits[i] = cellBits[i].id;
            }
        }

        public override string ToString()
        {
            var res = $"[Port] name:{this.name} id:{this.id} type:{this.type} yosysBit:{yosysBit}";
            return res;
        }
    }
}