using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationsManager.Areas.Exam.Models
{
    public class ExamMarksVM : ExamMarksDTO
    {
        public SelectList StandardSectionList { get; set; }

        public SelectList ExamTypeList { get; set; }

        public SelectList ExamSubTypeList { get; set; }

        public SelectList SubjectList { get; set; }

        public SelectList LocationList { get; set; }

        public string FromDateString { get; set; }

        public string ToDateString { get; set; }

        public List<ExamMarksVM> ExamMarksList { get; set; }

        public Boolean IsSearchSuccessful { get; set; }

        public string FullName { get; set; }

        public string Class { get; set; }

        public ExamRuleDTO Rule { get; set; }

        public bool IsRuleOk { get; set; }

        public bool IsRuleNeededToBeAdded { get; set; }

        public string RuleAdditionMessage { get; set; }

        public string Mode { get; set; }

        public int StandardSectionId { get; set; }

        public int SubjectId { get; set; }

        public int CourseExamId { get; set; }

        public int RuleId { get; set; }
    }
}