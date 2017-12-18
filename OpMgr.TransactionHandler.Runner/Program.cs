using Ninject;
using OpMgr.Common.Contracts;
using OpMgr.Common.Contracts.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.TransactionHandler.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            string diResolverPath = System.Configuration.ConfigurationManager.AppSettings["DIResolverPath"];
            StandardKernel kernel = new StandardKernel();
            kernel.Load(diResolverPath);
            try
            {

                Console.WriteLine("*** RUNNING TRANSACTION RULES ***");
                Console.WriteLine("*** WINDOW WILL BE CLOSED AUTOMATICALLY WHEN COMPLETED ***");
                Console.WriteLine("*** PLEASE DON'T CLOSE WINDOW ***");

                ITransactionSvc trnsSvc = kernel.Get<ITransactionSvc>();

                string activeModules = System.Configuration.ConfigurationManager.AppSettings["ActiveModules"];

                if(activeModules.Contains("REGULAR"))
                {
                    trnsSvc.AddRegularTransactions();
                }
                if (activeModules.Contains("FINE"))
                {
                    trnsSvc.CheckDuesAndAddFine();
                }
                if (activeModules.Contains("LIBRARY_FINE"))
                {
                    trnsSvc.CheckLibraryDueAndAddFine();
                }                
            }
            catch(Exception exp)
            {
                ILogSvc logger = kernel.Get<ILogSvc>();
                logger.Log(exp);
            }
        }
    }
}
