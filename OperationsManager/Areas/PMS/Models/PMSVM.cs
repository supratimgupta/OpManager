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
        public Dictionary<int, GoalViewModel> Goals
        {
            get;
            set;
        }

        public List<GoalViewModel> GoalsAsList { get; set; }

        public UserMasterDTO UserDetails { get; set; }

        public string FullName { get; set; }

        public string Designation { get; set; }

        public string Location { get; set; }

        public string Qualification { get; set; }

        public EmployeeDetailsDTO EmployeeDetails { get; set; }

        public ApprovalMappingDTO ApprovalMapping { get; set; }

        public string ApproverName { get; set; }

        public decimal SelfAvgRating { get; set; }

        public EmployeeCompetencyDTO EmpCompetency { get; set; }

        public SelectList CompetencyList { get; set; }

        public string MODE { get; set; }

        public string SAVE_MODE { get; set; }

        public decimal SumOfAcheivement { get; set; }

        public decimal SumOfAppraiserRating { get; set; }

        public decimal ReviewerRating { get; set; }

        public string ImprovementArea { get; set; }

        public string ImprovementsShow { get; set; }

        public string Strengths { get; set; }

        public string StrengthsShow { get; set; }

        public SelectList CompetencyDDLSource { get; set; }

        public int SelectedCompetency { get; set; }

        public MvcHtmlString ImprovementsLoader { get; set; }

        public MvcHtmlString StrengthsLoader { get; set; }

        public PMSVM GetGoals(List<EmployeeGoalLogDTO> lstEmpGoals)
        {
            this.Goals = new Dictionary<int, GoalViewModel>();
            this.EmployeeAppraisalMasterId = lstEmpGoals[0].EmployeeAppraisalMaster.EmployeeAppraisalMasterId;
            foreach (EmployeeGoalLogDTO empGoal in lstEmpGoals)
            {
                if (!this.Goals.Keys.Contains(empGoal.GoalAttribute.Goal.GoalId))
                {
                    GoalViewModel goal = new GoalViewModel();
                    goal.GoalDescription = empGoal.GoalAttribute.Goal.GoalDescription;
                    goal.GoalId = empGoal.GoalAttribute.Goal.GoalId;
                    goal.GoalLog = lstEmpGoals.Where(g => g.GoalAttribute.Goal.GoalId == empGoal.GoalAttribute.Goal.GoalId).ToList();
                    this.Goals.Add(empGoal.GoalAttribute.Goal.GoalId, goal);
                }
            }
            this.GoalsAsList = this.Goals.Values.ToList();
            return this;
        }
    }
}