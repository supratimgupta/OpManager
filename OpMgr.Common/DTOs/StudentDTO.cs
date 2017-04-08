using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class StudentDTO
    {
        public int StudentInfoId { get; set; }

        public UserMasterDTO CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public UserMasterDTO UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool Active { get; set; }

        public UserMasterDTO UserDetails { get; set; }

        [Required(ErrorMessage = "Roll Number is required")]
        public string RollNumber { get; set; }

        [Required(ErrorMessage = "Registration Number is required")]
        public string RegistrationNumber { get; set; }

        [Required(ErrorMessage = "Admission Date is required")]
        public DateTime? AdmissionDate { get; set; }

        [Required(ErrorMessage = "Guardian Contact is required")]
        public string GuardianContact { get; set; }

        [Required(ErrorMessage = "Guardian Name is required")]
        public string GuardianName { get; set; }

        public string GuardianEmailId { get; set; }

        public string Status { get; set; }

        public int NewStandardSectionId { get; set; }

        public StandardSectionMapDTO StandardSectionMap { get; set; }

        public HouseTypeDTO HouseType { get; set; }

        
    }
}
