using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.Contracts.Modules
{
    public interface ITransactionLogSvc : ICRUDSvc<DTOs.TransactionLogDTO, DTOs.TransactionLogDTO>
    {
        DataTable GetPendingTransactions();
    }
}
