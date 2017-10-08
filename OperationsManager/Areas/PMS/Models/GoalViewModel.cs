using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OperationsManager.Areas.PMS.Models
{
    public class GoalViewModel
    {
        public List<GoalAttributeDTO> GoalAttributes { get; set; }
    }
}