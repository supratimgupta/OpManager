using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationsManager.Areas.Transaction.Models
{
    public class UserTransactionViewModel : UserTransactionDTO
    {
        public SelectList getCalculationType { get; set; }
        
        public SelectList getUserFName { get; set; }

        public SelectList getTransactionMasters { get; set; }
    }
}