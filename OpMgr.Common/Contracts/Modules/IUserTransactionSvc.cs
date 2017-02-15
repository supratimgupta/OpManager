using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace OpMgr.Common.Contracts.Modules
{
    public interface IUserTransactionSvc : ICRUDSvc<DTOs.UserTransactionDTO, DTOs.UserTransactionDTO>
    {
        IDataReader GetUserTransactions();
    }
}
