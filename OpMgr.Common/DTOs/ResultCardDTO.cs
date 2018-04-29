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

        public List<ResultCardRows> ResultRows { get; set; }
    }

    public class ResultCardRows
    {
        public string SubjectName { get; set; }

        public List<ResultCardColumns> ResultColumns { get; set; }
    }

    public class ResultCardColumns
    {
        public string ColumnName { get; set; }

        public string ColumnValue { get; set; }

        public int ColumnSequence { get; set; }
    }
}
