﻿using System;
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

        public Decimal Achievement { get; set; }

        public int SelfRating { get; set; }

        public Decimal AppraiserRating { get; set; }

        public DateTime EmployeeRatingDate { get; set; }

        public DateTime AppraiserRatingDate { get; set; }

        public DateTime ReviewerRatingDate { get; set; }

        public string EmployeeGoalComment { get; set; }

        public string AppraiserGoalComment { get; set; }

        public string ReviewerGoalComment { get; set; }


    }
}
