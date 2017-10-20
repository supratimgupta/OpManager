using OpMgr.Common.DTOs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationsManager.Areas.PMS.Models
{
    public class PMSVM : EmployeeAppraisalMasterDTO
    {
        public List<GoalViewModel> Goals { get; set; }

        public UserMasterDTO UserDetails { get; set; }

        public string FullName { get; set; }
                
        public EmployeeDetailsDTO EmployeeDetails { get; set; }

        public ApprovalMappingDTO ApprovalMapping { get; set; }

        public string ApproverName { get; set; }

        public decimal SelfAvgRating { get; set; }

        public EmployeeCompetencyDTO EmpCompetency { get; set; }

        public SelectList CompetencyList { get; set; }
    }
}