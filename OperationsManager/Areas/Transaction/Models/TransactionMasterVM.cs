using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationsManager.Areas.Transaction.Models
{
    public class TransactionMasterVM : TransactionMasterDTO
    {
        public List<TransactionMasterVM> SearchList { get; set; }

        public SelectList TransTypeList { get; set; }

        public SelectList IsDiffToList { get; set; }

        public SelectList FrequencyList { get; set; }

        public string MODE { get; set; }
    }
}