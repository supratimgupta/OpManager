using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.Contracts.Modules
{
    public interface ITransactionRuleSvc : ICRUDSvc<DTOs.TransactionRuleDTO, DTOs.TransactionRuleDTO>
    {
        DataTable GetAllRules();
    }
}
