using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class EmployeeDetailsDTO
    {
        public int EmployeeId { get; set; }

        public UserMasterDTO CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public UserMasterDTO UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool Active { get; set; }

        public UserMasterDTO UserDetails { get; set; }

        public string EducationalQualification { get; set; }

        public DateTime? DateOfJoining { get; set; }

        public DepartmentDTO Department { get; set; }

        public DesignationDTO Designation { get; set; }

        public string StaffEmployeeId { get; set; }

    }
}
