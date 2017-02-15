using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class ExamRuleDTO
    {
        public int ExamRuleId { get; set; }

        public double? AssesmentMarks { get; set; }

        public double? ConvertedMarks { get; set; }

        public ExamSubTypeDTO ExamSubType { get; set; }

        public StandardSectionMapDTO StandardSection { get; set; }

        public ExamTypeDTO ExamType { get; set; }

        public DateTime? DateTimeLog { get; set; }

        public UserMasterDTO CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public UserMasterDTO UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool Active { get; set; }
    }
}
