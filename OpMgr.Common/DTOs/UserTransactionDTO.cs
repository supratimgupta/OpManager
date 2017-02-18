using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class UserTransactionDTO
    {
        public int UserTransactionId { get; set; }

        public UserMasterDTO CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public UserMasterDTO UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool Active { get; set; }

        public UserMasterDTO User { get; set; }

        public TransactionMasterDTO Transaction { get; set; }

        //Percent/ Absolute
        public string GraceAmountIn { get; set; }

        public double? GraceAmount { get; set; }

        public DateTime? LastAutoTransactionOn { get; set; }

        public DateTime? NextAutoTransactionOn { get; set; }
    }
}
