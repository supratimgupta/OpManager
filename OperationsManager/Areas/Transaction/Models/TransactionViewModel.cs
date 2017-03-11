﻿using OpMgr.Common.DTOs;
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
    }
}