using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class FacultyCourseStudentMapDTO
    {
        public int FacultyCourseStuMapId { get; set; }

        public UserMasterDTO CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public UserMasterDTO UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool Active { get; set; }

        public EmployeeDetailsDTO Employee { get; set; }

        public SubjectDTO Subject { get; set; }

        public StandardDTO Standard { get; set; }
    }
}
