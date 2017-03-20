using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.Contracts.Modules
{
    public interface ITransactionMasterSvc : ICRUDSvc<DTOs.TransactionMasterDTO, DTOs.TransactionMasterDTO>
    {
        DataTable GetAllTransactions();

        DTOs.StatusDTO<List<DTOs.TransactionMasterDTO>> GetAllTransactioMasters();
    }
}
