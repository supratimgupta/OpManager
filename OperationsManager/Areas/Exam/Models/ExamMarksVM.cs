using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationsManager.Areas.Exam.Models
{
    public class ExamMarksVM :ExamMarksDTO
    {
        public SelectList StandardSectionList { get; set; }

        public SelectList ExamTypeList { get; set; }

        public SelectList ExamSubTypeList { get; set; }

        public SelectList SubjectList { get; set; }
    }
}