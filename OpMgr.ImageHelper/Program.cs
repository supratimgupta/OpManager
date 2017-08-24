using System;
using System.Collections.Generic;
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
            AbsUtility utility = new Utility(new DataHelper());
            utility.ChangeImage();
        }
    }
}
