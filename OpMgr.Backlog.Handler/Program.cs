using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Backlog.Handler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("***** RUNNING THE BACKLOG HANDLER *****");
            Console.WriteLine("***** PLEASE WAIT THE PROCESS TO END *****");
            BacklogHandler blh = new BacklogHandler();
            blh.Handlebacklog();
            Console.WriteLine("***** PROCESS ENDED, YOU MAY NOW CLOSE THE WINDOW *****");
            Console.ReadLine();
        }
    }
}
