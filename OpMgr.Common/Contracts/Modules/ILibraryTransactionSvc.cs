using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.Contracts.Modules
{
    public interface ILibraryTransactionSvc : ICRUDSvc<DTOs.LibraryTransactionDTO, DTOs.LibraryTransactionDTO>, IDisposable
    {
        DataTable GetPendingTransactions(DateTime? runDate);

        bool MoveLibTransToCashTrans(int libTrnsId, bool? IsMovedToTransaction, int? cashTrnsId);
    }
}
