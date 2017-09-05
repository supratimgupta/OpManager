using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Utilities
{
    public static class Utility
    {
        public static string MakeEmployeeId(int idNo, string name, int maxNoCountLength, int maxPrefixLength)
        {
            string[] arrNames = name.Split(' ');
            string prefix = string.Empty;
            int counter = 0;
            while (prefix.Length <= maxPrefixLength && counter < arrNames.Length)
            {
                if(string.IsNullOrEmpty(arrNames[counter]) || string.IsNullOrWhiteSpace(arrNames[counter]) 
                    || string.Equals(arrNames[counter],"MR", StringComparison.OrdinalIgnoreCase) 
                    || string.Equals(arrNames[counter],"MR.", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(arrNames[counter], "MS", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(arrNames[counter], "MS.", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(arrNames[counter], "MRS", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(arrNames[counter], "MRS.", StringComparison.OrdinalIgnoreCase))
                {
                    counter++;
                    continue;
                }
                prefix = prefix + arrNames[counter].Trim().Substring(0, 1).ToUpper();
                if (prefix.Length == maxPrefixLength)
                {
                    break;
                }
                counter++;
            }
            string strIdNo = idNo.ToString().PadLeft(maxNoCountLength,'0');
            return prefix + strIdNo;
        }
    }
}
