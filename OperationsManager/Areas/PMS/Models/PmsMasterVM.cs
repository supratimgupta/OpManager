using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OperationsManager.Areas.PMS.Models
{
    public class PmsMasterVM : PMSMasterDTO
    {
        public bool IsSearchSuccessful{ get; set; }
        public List<PmsMasterVM> pmsViewList { get; set; }

    }
}