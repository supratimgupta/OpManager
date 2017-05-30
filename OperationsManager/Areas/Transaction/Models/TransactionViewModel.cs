using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationsManager.Areas.Transaction.Models
{
    public class TransactionViewModel : TransactionLogDTO
    {
        public SelectList UserList { get; set; }

        public SelectList TransactionRuleList { get; set; }

        public SelectList LocationList { get; set; }

        public SelectList TransactionTypeList { get; set; }

        public SelectList StandardSectionList { get; set; }

        public SelectList TransactionMasterList { get; set; }

        public List<TransactionViewModel> SearchResult { get; set; }

        public MvcHtmlString Message { get; set; }

        public bool IsSuccessMessage { get; set; }

        public double CurrentAmount { get; set; }

        public double CurrentAdjusting { get; set; }

        public string EncryptedTransactionLogId { get; set; }

        public bool HideSaveButton { get; set; }

        public bool IsSelected { get; set; }

        public string MODE { get; set; }

        public List<TransactionViewModel> paymentDetailsList { get; set; }

        public string RegistrationNo { get; set; }

        public string StudentName { get; set; }

        public string TransactionDateString { get; set; }

        public string TransactionDueDateString { get; set; }

        public int StudentInfoId { get; set; }

        public int TransactionMasterId { get; set; }
    }
}