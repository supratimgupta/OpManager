using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class CourseMappingDTO
    {
        public int CourseMappingId { get; set; }

        public UserMasterDTO CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public UserMasterDTO UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool Active { get; set; }

        public StudentClassMapDTO StudentClassMap { get; set; }

        public SubjectDTO Subject { get; set; }

        public LocationDTO Location { get; set; }

        public StandardSectionMapDTO StandardSection { get; set; }

        public EmployeeDetailsDTO Employee { get; set; }

        public DateTime? CourseFrom { get; set; }

        public DateTime? CourseTo { get; set; }
    }
}
