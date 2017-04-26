using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationsManager.Areas.Transaction.Models
{
    public class PaymentVM : TransactionLogDTO
    {
        public List<PaymentVM> paymentDetailsList { get; set; }
        public Boolean IsSearchSuccessful { get; set; }
        public string StaffEmployeeId { get; set; }
        public string RegistrationNumber { get; set; }
        public int radiobuttonId { get; set; }

        public string Name { get; set; }

        public double? rowCurrentlyPaying { get; set; }

        public double? rowAdjustingAmount { get; set; }

        public string  PrincipalApproved { get; set; }

        public int HdnTransactionLogId { get; set; }

        public string LabelTotalDue { get; set; }

        public string LabelCurrentlyPaying { get; set; }

        public string LabelCurrentlyAdjust { get; set; }

        public SelectList PaymentModeList { get; set; }
    }
}