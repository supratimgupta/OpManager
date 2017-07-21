using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class ExamMarksDTO
    {
        public int ExamMarksId { get; set; }

        public UserMasterDTO CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public UserMasterDTO UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool Active { get; set; }

        public StudentClassMapDTO StudentClassMap { get; set; }

        public ExamTypeDTO ExamType { get; set; }

        public double? MarksObtained { get; set; }

        public SubjectDTO SubjectId { get; set; }

        public double? CalculatedMarks { get; set; }

        public ExamRuleDTO ExamRule { get; set; }

        public double? TotalMarks { get; set; }

        public ExamSubTypeDTO ExamSubType { get; set; }

        public StudentDTO Student { get; set; }

        public StandardSectionMapDTO StandardSection { get; set; }
    }
}
