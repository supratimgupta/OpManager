using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace OpMgr.Common.Contracts.Modules
{
    public interface IUserTransactionSvc : ICRUDSvc<DTOs.UserTransactionDTO, DTOs.UserTransactionDTO>, IDisposable
    {
        DataTable GetUserTransactions(DateTime? runDate);

        DTOs.StatusDTO UpdateTransLastRunNextRun(DTOs.UserTransactionDTO userTrans);

        List<DTOs.UserTransactionDTO> GetUserTransactions(int userMasterId);

        List<DTOs.UserTransactionDTO> GetUserTransactions(int trMasterId, int userMasterId);
    }
}
