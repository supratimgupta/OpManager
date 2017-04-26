using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class TransactionLogPaymentsDTO
    {
        public int TransactionLogPaymentId { get; set; }

        public TransactionLogDTO TransactionLog { get; set; }

        public DateTime? PaymentDate { get; set; }

        public double? CurrentAmount { get; set; }

        public bool? IsReverted { get; set; }

        public bool? IsParentReverted { get; set; }

        public string PaymentChequeNo { get; set; }

        public string PaymentMode { get; set; }

        public double? CurrentAdjustingAmount { get; set; }
    }
}
