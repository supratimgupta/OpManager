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

        public string NotificationText { get; set; }

        public double SumOfWeitage { get; set; }

        public string SummaryOfAcheivement { get; set; }

        public string SummaryOfAppraisal { get; set; }

        public string ReviewerRatingComment { get; set; }

        public SelectList RatingDropDown { get; set; }

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

        public SelectList AppraisalTypeList { get; set; }

        public SelectList AppraisalStatusList { get; set; }

        public SelectList GenderList { get; set; }

        public SelectList LocationList { get; set; }

        public Boolean IsSearchSuccessful { get; set; }

        public List<PMSVM> PMSVMList { get; set; }

        public string employeeimagepath { get; set; }

        public string IndividualInitiative { get; set; }

        public string InstitutionalSupport { get; set; }

        public string ReviewerRatingLevel { get; set; }
        
        public string AppraiserFinalRatingLevel { get; set; }

        public SelectList PMSDesignationList { get; set; }
    }

    public class Designation
    {
        public SelectList PMSDesignationList { get; set; }
        public int DesignationID { get; set; }
        public string DesignationDescription { get; set; }
    }

    public class GoalAttributes
    {
        public int GoalAttributeID { get; set; }
        public string Goal { get; set; }
        public string Target { get; set; }
        public string KPI { get; set; }
        public string KRA { get; set; }
        public int weightage { get; set; }
        public List<GoalAttributes> listattributes { get; set; }
    }

    public class UserModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }

        public static List<UserModel> getUsers()
        {
            List<UserModel> users = new List<UserModel>()
            {
                new UserModel (){ ID=1, Name="Anubhav", SurName="Chaudhary" },
                new UserModel (){ ID=2, Name="Mohit", SurName="Singh" },
                new UserModel (){ ID=3, Name="Sonu", SurName="Garg" },
                new UserModel (){ ID=4, Name="Shalini", SurName="Goel" },
                new UserModel (){ ID=5, Name="James", SurName="Bond" },
            };
            return users;
        }
    }
}