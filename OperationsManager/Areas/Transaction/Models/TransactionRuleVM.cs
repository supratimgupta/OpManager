using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationsManager.Areas.Transaction.Models
{
    public class TransactionRuleVM : TransactionRuleDTO
    {
        public SelectList Users { get; set; }

        public SelectList TransactionMasters { get; set; }

        public SelectList Standards { get; set; }

        public SelectList Sections { get; set; }

        public SelectList PenaltyCalcIn { get; set; }

        public SelectList ClassTypes { get; set; }

        public SelectList PenaltyTransactionTypes { get; set; }

        public SelectList PenaltyTransactionRules { get; set; }

        public SelectList TransactionFrequencies { get; set; }

        public string MODE { get; set; }

        public string SuccessMsg { get; set; }

        public string ErrorMsg { get; set; }

        public string SelectedFrequency { get; set; }

        public string IsPostBack { get; set; }
    }
}