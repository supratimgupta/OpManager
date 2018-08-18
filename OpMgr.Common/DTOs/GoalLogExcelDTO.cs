using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class GoalLogExcelDTO
    {
        public int EmployeeAppraisalMasterId { get; set; }
        public int goalattributeid { get; set; }
        public int GoalId { get; set; }
        public string goaldescription { get; set; }
        public string Target { get; set; }
        public string KRA { get; set; }
        public string KPI { get; set; }
        public string Weightage { get; set; }
        public int EmployeeGoalLogId { get; set; }
        public string AppraiseeRating { get; set; }
        public string AppriaserRating { get; set; }
        public string EmployeeGoalComment { get; set; }
        public string AppraiserGoalComment { get; set; }
        public string ReviewerGoalComment { get; set; }

    }
}
