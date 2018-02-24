﻿using OpMgr.Common.DTOs;
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

        public SelectList LocationList { get; set; }

        public int hdnEmployeeId { get; set; }

        public int hdncoursemapid { get; set; }

        public int hdnExamRuleId { get; set; }

        public string FromDateString { get; set; }

        public string ToDateString { get; set; }

        public List<ExamMarksDTO> ExamMarksList { get; set; }
    }
}