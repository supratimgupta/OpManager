using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required(ErrorMessage = "Education Qualification is required")]
        public string EducationalQualification { get; set; }

        [Required(ErrorMessage = "Date of Joining is required")]
        public DateTime? DateOfJoining { get; set; }

        public DepartmentDTO Department { get; set; }

        public DesignationDTO Designation { get; set; }

        public ClassTypeDTO ClassType { get; set; }

        public SubjectDTO Subject { get; set; }

        [Required(ErrorMessage = "Staff Employee Id is required")]
        public string StaffEmployeeId { get; set; }
        
        //below propertiess added to get Direct Supervisor while PMS
        public string ApproverName { get; set; }

    }
}
