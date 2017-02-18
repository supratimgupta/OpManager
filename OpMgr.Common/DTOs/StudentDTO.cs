using System;
using System.Collections.Generic;
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

        public string RollNumber { get; set; }

        public string RegistrationNumber { get; set; }

        public DateTime? AdmissionDate { get; set; }

        public string GuardianContact { get; set; }

        public string GuardianName { get; set; }

        public string GuardianEmailId { get; set; }



        //public string GuardianContact
    }
}
