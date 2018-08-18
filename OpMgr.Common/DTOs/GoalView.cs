using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpMgr.Common.DTOs
{
    public class GoalView
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
        public bool Active { get; set; }
        public float AppraiseeRating { get; set; }
        public float AppriaserRating { get; set; }
        public string EmployeeGoalComment { get; set; }
        public string AppraiserGoalComment { get; set; }
        public string ReviewerGoalComment { get; set; }
    }

    public class GoalViewList
    {
        public List<GoalView> gvList { get; set; }
    }
}