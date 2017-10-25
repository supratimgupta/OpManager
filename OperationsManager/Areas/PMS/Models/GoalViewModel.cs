using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OperationsManager.Areas.PMS.Models
{
    public class GoalViewModel : GoalDTO
    {
        public List<EmployeeGoalLogDTO> GoalLog { get; set; }
    }
}