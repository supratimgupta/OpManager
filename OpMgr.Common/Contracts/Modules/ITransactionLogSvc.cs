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

        StatusDTO<List<TransactionLogDTO>> SelectPayment(StudentDTO student);

        StatusDTO<TransactionLogDTO> UpdatePayment(TransactionLogDTO tranlog);

        StatusDTO<List<TransactionLogDTO>> GetPendingPrincipalApprovals(TransactionLogDTO tranlog);

        bool ApproveCancelAdjustedAmt(List<TransactionLogDTO> lstTRLog);

        bool ResendRequest(int transactionLogId);

        StatusDTO<List<TransactionLogDTO>> GetPaidTransaction(TransactionLogDTO trnsLog, string identifier, string role);

        bool InsertBacklogAmount(TransactionLogDTO transLog);

        List<TransactionLogDTO> GetBacklogDues(string registrationNo);

        bool ClearAllBacklogs(int userMasterId);
    }
}
