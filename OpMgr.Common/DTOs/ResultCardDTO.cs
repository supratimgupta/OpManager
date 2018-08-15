using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class ResultCardDTO
    {
        public string ResultType { get; set; }

        public string StudentName { get; set; }

        public string StudentRegNo { get; set; }

        public string StudentRollNo { get; set; }

        public string StudentInfoId { get; set; }

        public string ClassName { get; set; }

        public string SectionName { get; set; }

        public string SessionStart { get; set; }

        public string SessionEnd { get; set; }

        public List<ResultCardRows> ResultRows { get; set; }

        public List<ResultCardRows> GradeResultRows { get; set; }

        public double TotalMarks { get; set; }

        public string CurrentRemarks { get; set; }

        public string AttendancePercent { get; set; }

        public List<GraphRecords> GraphRecords { get; set; }
    }

    public class ResultCardRows
    {
        public string SubjectName { get; set; }

        public string SubjectId { get; set; }

        public List<ResultCardColumns> ResultColumns { get; set; }

        public double GraphValue { get; set; }
    }

    public class ResultCardColumns
    {
        public string ColumnName { get; set; }

        public string ColumnValue { get; set; }

        public int ColumnSequence { get; set; }

        public bool IsAllowedForGrade { get; set; }

        public bool IsUsedForTotal { get; set; }
    }

    public class GraphRecords
    {
        public string SubjectName { get; set; }

        public double ClassHighest { get; set; }

        public double ClassAverage { get; set; }

        public double CurrentMarks { get; set; }
    }
}
