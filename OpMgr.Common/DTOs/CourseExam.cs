using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class CourseExam
    {
        public int CourseExamId { get; set; }

        public bool Active { get; set; }

        public ExamTypeDTO ExamType { get; set; }

        public ExamSubTypeDTO ExamSubType { get; set; }

        public DateTime ExamDate { get; set; }

        public DateTime ExamStartTime { get; set; }

        public DateTime ExamEndTime { get; set; }

        public CourseMappingDTO CourseMapping { get; set; }
    }
}
