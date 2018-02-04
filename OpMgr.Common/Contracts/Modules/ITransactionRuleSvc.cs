using OpMgr.Common.DTOs;
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

        List<TransactionRuleDTO> GetAllRulesWithInactive();

        bool IsDuplicate(int location, int trnsMasterId, int standardId, int sectionId, int classTypeId, int userMasterId, string isDiffTo, string mode, int ruleId);

        List<TransactionRuleDTO> GetUserLevelRules(int location, int transactionMasterId, int userRowId);

        List<TransactionRuleDTO> GetClassTypeLevelRules(int location, int transactionMasterId, int? classTypeRowId=null);

        List<TransactionRuleDTO> GetStandardLevelRules(int location, int transactionMasterId, int? standardRowId=null);

        List<TransactionRuleDTO> GetStandardSectionLevelRules(int location, int transactionMasterId, int standardId, int? sectionId=null);

        List<TransactionRuleDTO> GetNoneLevelRules(int location, int? transactionMasterId=null);

        int? GetFirstDueAfterDays(int trRuleId);
    }
}
