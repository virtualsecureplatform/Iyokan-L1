using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Iyokan_L1.Utils
{
    public static class Extension
    {
        public static string ToString<T>(this List<T> list)
        {
            var strs = (from p in list select p.ToString()).ToArray();
            var sb = new StringBuilder("[");
            foreach (var str in strs)
            {
                sb.AppendFormat("{0}, ", str);
            }

            if (sb.Length < 2)
            {
                return "[]";
            }

            sb.Remove(sb.Length - 2, 2);
            sb.Append("]");
            return sb.ToString();
        }
    }
}