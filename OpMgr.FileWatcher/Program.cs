using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.FileWatcher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if (DEBUG)
            FileWatcherSvc_Debug debugSvc = new FileWatcherSvc_Debug();
            Console.ReadLine();
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new FileWatcherSvc()
            };
            ServiceBase.Run(ServicesToRun);
#endif
            // here Process is my Service function
            // that will run when my service onstart is call
            // you need to call your own method or function name here instead of Process();
        }
    }
}
