using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.ImageHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            AbsDataHelper dataHelper = UtilityFactory.GetDataHelper(ConfigurationManager.AppSettings["ACTION"]);
            AbsUtility utility = UtilityFactory.GetUtilityClass(ConfigurationManager.AppSettings["ACTION"], dataHelper);
            utility.DoAction();
        }
    }
}
