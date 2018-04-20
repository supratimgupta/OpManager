using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OperationsManager.Areas.Exam.Models
{
    public class ExamRuleVM : ExamRuleDTO
    {
        public string ErrorMessage { get; set; }

        public bool IsError { get; set; }

        public string Mode { get; set; }
    }
}