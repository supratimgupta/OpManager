using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class UserMasterDTO
    {
        public int UserMasterId { get; set; }

        public UserMasterDTO CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public UserMasterDTO UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool Active { get; set; }

        [Required(ErrorMessage ="Name is required")]
        public string FName { get; set; }

        public string MName { get; set; }

        public string LName { get; set; }


        public string Gender { get; set; }

        public string Image { get; set; }

        public DateTime? DOB { get; set; }

        public string EmailId { get; set; }

        [Required(ErrorMessage = "Residential Address is required")]
        public string ResidentialAddress { get; set; }

        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [CompareAttribute("Password", ErrorMessage = "Password doesn't match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Permanent Address is required")]
        public string PermanentAddress { get; set; }

        [Required(ErrorMessage = "Contact Number is required")]
        public string ContactNo { get; set; }

        public string AltContactNo { get; set; }

        [Required(ErrorMessage = "Blood Group is required")]
        public string BloodGroup { get; set; }
        
        public LocationDTO Location { get; set; }

        public RoleDTO Role { get; set; }

        //public StudentDTO Student { get; set; }

        public EmployeeDetailsDTO Employee { get; set; }

        public bool RememberMe { get; set; }
        
        public string LoginFailedMsg { get; set; }      

        public int StudentCount { get; set; }

        public int StaffCount { get; set; }

        public int PaidStudentCount { get; set; }

        public int PendingPaymentCount { get; set; }

        public int PaidCountlast7day { get; set; }

        public string UniqueId { get; set; }

        public string UserType { get; set; }

        public int UniqueEmployeeId { get; set; }
    }
}
