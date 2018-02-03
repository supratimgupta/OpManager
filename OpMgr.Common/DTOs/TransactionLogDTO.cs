using System;
using System.ComponentModel.DataAnnotations;
using OpMgr.Resources;
using OpMgr.Resources.Transaction;

namespace OpMgr.Common.DTOs
{
    public class TransactionLogDTO
    {
        public int TransactionLogId { get; set; }
        public UserMasterDTO CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public UserMasterDTO UpdatedBy { get; set; }
        public DateTime? UpdatedDate{ get; set; }
        public bool? Active { get; set; }


        [Range(1, int.MaxValue, ErrorMessageResourceName = "UserValidMessage", ErrorMessageResourceType =typeof(TransactionLogAdd))]
        public UserMasterDTO User { get; set; }

        [Required(ErrorMessageResourceName = "TransactionDateValidMsg", ErrorMessageResourceType = typeof(TransactionLogAdd))]
        public DateTime? TransactionDate { get; set; }

        [Required(ErrorMessageResourceName = "DueDateValidMsg", ErrorMessageResourceType = typeof(TransactionLogAdd))]
        public DateTime? TransactionDueDate { get; set; }

        public DateTime? TransactionPreviousDueDate { get; set; }
        public TransactionLogDTO ParentTransactionLogId { get; set; }
        public bool? IsCompleted { get; set; }

        [Range(1, int.MaxValue, ErrorMessageResourceName = "TranRuleValidMsg", ErrorMessageResourceType = typeof(TransactionLogAdd))]
        public TransactionRuleDTO TransactionRule { get; set; }

        public DateTime? CompletedOn { get; set; }

        [Range(0, int.MaxValue, ErrorMessageResourceName = "AmountImposedValidMsg", ErrorMessageResourceType = typeof(TransactionLogAdd))]
        public double? AmountImposed { get; set; }

        public double? AmountGiven { get; set; }
        public double? DueAmount { get; set; }
        public string TransferMode { get; set; }
        public LocationDTO Location { get; set; }

        [Range(1, 3, ErrorMessageResourceName = "ValidTranTypeMsg", ErrorMessageResourceType = typeof(TransactionLogAdd))]
        public string TransactionType { get; set; }

        public bool? HasPenalty { get; set; }

        public TransactionLogDTO OriginalTransLog { get; set; }
        public double? AdjustedAmount { get; set; }

        public int? IsPrincipalApproved { get; set; }

        public StandardSectionMapDTO StandardSectionMap { get; set; }

        public TransactionRuleDTO PenaltyTransactionRule { get; set; }

        //Search params
        public DateTime? SearchFrom { get; set; }

        public DateTime? SearchUpto { get; set; }

        public string SearchStudRegId { get; set; }

        public string SearchEmployeeId { get; set; }

        public string SearchLoginName { get; set; }

        public int? SearchStdSectionId { get; set; }

        //public double? AdjustedAmount { get; set; }

        //public bool IsChequeEncashed { get; set; }

        //public int? IsPrincipalApprroved { get; set; }

        public bool? HasPartialPayment { get; set; }

        public bool? RevertedTotal { get; set; }

        public string PaymentChequeNo { get; set; }

        public DateTime? PaymentDate { get; set; }

        public double? MonthlyBacklogAmount { get; set; }

        public double? LateFineBacklogAmount { get; set; }

        public double? YearlyBacklogAmount { get; set; }

        public double? BusBacklogAmount { get; set; }

        public DateTime? BacklogFromDate { get; set; }

        public DateTime? BacklogToDate { get; set; }
    }
}
