using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.Contracts.Modules
{
    public interface ITransactionLogSvc : ICRUDSvc<DTOs.TransactionLogDTO, DTOs.TransactionLogDTO>, IDisposable
    {
        DataTable GetPendingTransactions(DateTime? runDate);

        bool UpdateHasPenaltyFlag(int trnsLogId, bool? hasPenalty, DateTime dueDate, int penaltyTransactionRule);

        StatusDTO<List<TransactionLogDTO>> GetPendingPrincipalApprovals(TransactionLogDTO trLog);

        bool ApproveCancelAdjustedAmt(List<TransactionLogDTO> lstTransaction);
    }
}
