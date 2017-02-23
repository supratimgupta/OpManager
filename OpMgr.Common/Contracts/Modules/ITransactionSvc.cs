using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.Contracts.Modules
{
    public interface ITransactionSvc : IDisposable
    {
        void AddRegularTransactions();

        void CheckDuesAndAddFine();

        void CheckLibraryDueAndAddFine();
    }
}
