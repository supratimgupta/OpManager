using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class EmployeeGoalLogDTO
    {
        public int EmployeeGoalLogId { get; set; }

        public EmployeeAppraisalMasterDTO EmployeeAppraisalMaster { get; set; }

        public GoalAttributeDTO GoalAttribute { get; set; }

        public decimal Achievement { get; set; }

        public int AchivementPercent { get; set; }

        public string SelfRating { get; set; }

        public decimal AppraiserRating { get; set; }

        public int AppraiserRatingPercent { get; set; }

        public string AppraiserRatingLevel { get; set; }

        public DateTime EmployeeRatingDate { get; set; }

        public DateTime AppraiserRatingDate { get; set; }

        public DateTime ReviewerRatingDate { get; set; }

        public string EmployeeGoalComment { get; set; }

        public string AppraiserGoalComment { get; set; }

        public string ReviewerGoalComment { get; set; }

        public string NeedsUpdate { get; set; }

        public string NeedsAppraiserUpdate { get; set; }

        public PMSDesignationDTO PMSDesignation { get; set; }
    }
}
