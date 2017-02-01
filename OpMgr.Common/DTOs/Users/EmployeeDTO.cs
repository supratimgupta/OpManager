using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs.Users
{
    /// <summary>
    /// Maps employee model
    /// </summary>
    public class EmployeeDTO
    {
        public int RowId { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string FatherName { get; set; }

        public string BloodGroup { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string OfficialMailId { get; set; }

        public string PersonalMailId { get; set; }

        public string CurrentAddressLine1 { get; set; }

        public string CurrentAddressLine2 { get; set; }

        public string CurrentAddressLine3 { get; set; }

        public string CurrentAddressCity { get; set; }

        public string CurrentAddressState { get; set; }

        public string CurrentAddressPincode { get; set; }

        public string ParmanentAddressLine1 { get; set; }

        public string ParmanentAddressLine2 { get; set; }

        public string ParmanentAddressLine3 { get; set; }

        public string ParmanentAddressCity { get; set; }

        public string ParmanentAddressState { get; set; }

        public string ParmanentAddressPincode { get; set; }

        public string ContactNumber { get; set; }

        public string AlternateNumber { get; set; }

        public bool IsManager { get; set; }

        public bool IsLeaveApprover { get; set; }

        public EmployeeDTO ManagerDetails { get; set; }

        public EmployeeDTO ReviewerDetails { get; set; }

        public bool IsReviewer { get; set; }

        public bool IsActive { get; set; }

        public DateTime DateOfJoining { get; set; }

        public DateTime DateOfExit { get; set; }

        public DepartmentDTO DepartmentDetails { get; set; }

        public DesignationDTO DesignationDetails { get; set; }

        public ProjectDTO ProjectDetails { get; set; }

        public EmployeeRoleDTO RoleDetails { get; set; }

        public WorkLocationDTO LocationDetails { get; set; }
    }
}
