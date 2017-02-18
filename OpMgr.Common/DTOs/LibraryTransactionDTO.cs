using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class LibraryTransactionDTO
    {
        public int LibraryTranId { get; set; }

        public UserMasterDTO CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public UserMasterDTO UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool Active { get; set; }

        public UserMasterDTO User { get; set; }

        public BookMasterDTO Book { get; set; }

        public DateTime? IssuedDate { get; set; }

        public DateTime? DueDate { get; set; }

        public bool IsRemindedSubmission { get; set; }

        public DateTime? SubmissionDate { get; set; }

        public TransactionLogDTO TransactionIdForDue { get; set; }

        public bool IsReturned { get; set; }

        public bool IsMovedToTransaction { get; set; }
    }
}
