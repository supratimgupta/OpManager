using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class StudentRemarksDTO
    {
        public int StudentRemarksId { get; set; }

        public StudentDTO Student { get; set; }

        public DateTime? CourseFromDate { get; set; }

        public DateTime? CourseToDate { get; set; }

        public string ExamResultType { get; set; }

        public string Remarks { get; set; }

        public string AttendancePercent { get; set; }
    }
}
