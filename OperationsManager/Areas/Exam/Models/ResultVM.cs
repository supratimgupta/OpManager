using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationsManager.Areas.Exam.Models
{
    public class ResultVM
    {
        public SelectList StandardSectionList { get; set; }

        public SelectList ExamTypeList { get; set; }

        public SelectList LocationList { get; set; }

        public SelectList AcademicSessions { get; set; }

        public int SelectedLocation { get; set; }

        public int SelectedExamType { get; set; }

        public int SelectedStandardSection { get; set; }

        public string SelectedAcademicSession { get; set; }

        public List<ResultCardDTO> ResultCards { get; set; }
    }
}