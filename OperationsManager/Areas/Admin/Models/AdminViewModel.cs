using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OpMgr.Common.DTOs;
using System.Web.Mvc;

namespace OperationsManager.Areas.Admin.Models
{
    public class AdminViewModel : EntitlementActionDTO
    {
        public UserMasterDTO UserMaster { get; set; }

        public SelectList ActionList { get; set; }

        public SelectList EntitlementList { get; set; }
    }
}