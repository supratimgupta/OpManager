using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class TransactionLogDTO
    {
        public int TransactionLogId { get; set; }
        public UserMasterDTO CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public UserMasterDTO UpdatedBy { get; set; }
        public DateTime? UpdatedDate{ get; set; }
        public bool Active { get; set; }
        public UserMasterDTO User { get; set; }
        public DateTime? TransactionDate { get; set; }
        public DateTime? TransactionDueDate { get; set; }
        public DateTime? TransactionPreviousDueDate { get; set; }
        public TransactionLogDTO ParentTransactionLogId { get; set; }
        public bool IsCompleted { get; set; }
        public TransactionMasterDTO TranMaster { get; set; }
        public DateTime? CompletedOn { get; set; }
        public double? AmountImposed { get; set; }
        public double? AmountGiven { get; set; }
        public double? DueAmount { get; set; }
        public string TransferMode { get; set; }
        public LocationDTO Location { get; set; }

        public StudentClassMapDTO StudentClassMap { get; set; }
    }
}
