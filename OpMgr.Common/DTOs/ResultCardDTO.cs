using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class ResultCardDTO
    {
        public string StudentName { get; set; }

        public string StudentRegNo { get; set; }

        public string StudentRollNo { get; set; }

        public string StudentInfoId { get; set; }

        public string ClassName { get; set; }

        public string SectionName { get; set; }

        public string SessionStart { get; set; }

        public string SessionEnd { get; set; }

        public List<ResultLineItemDTO> LineItems { get; set; }
    }

    public class ResultLineItemDTO
    {
        public string Subject { get; set; }

        public string SubjectId { get; set; }

        public List<ResultItem> ResultItems { get; set; }
    }

    public class ResultItem
    {
        public string ExamTypeName {get;set;}

        public string ExamTypeId { get; set; }

        public List<ResultSubItem> ResultSubItems {get;set;}

        public string TotalMarksForSubItems { get; set; }
    }

    public class ResultSubItem
    {
        public string ExamSubTypeName {get;set;}

        public string ExamSubTypeId { get; set; }

        public string ObtainedMarks {get;set;}

        public string PassMarks { get; set; }

        public string FullMarks { get; set; }
    }
}
