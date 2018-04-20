using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class CourseExamDTO
    {
        public int CourseExamId { get; set; }

        public bool? Active { get; set; }

        public ExamSubTypeDTO ExamSubType { get; set; }

        public ExamTypeDTO ExamType { get; set; }

        public DateTime? ExamDate { get; set; }

        public TimeSpan? ExamStartTime { get; set; }

        public TimeSpan? ExamEndTime { get; set; }

        public CourseMappingDTO CourseMapping { get; set; }
    }
}
