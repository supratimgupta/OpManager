using System;
using System.Collections.Generic;
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

        public string FName { get; set; }

        public string MName { get; set; }

        public string LName { get; set; }

        public string Gender { get; set; }

        public string Image { get; set; }

        public DateTime? DOB { get; set; }

        public string EmailId { get; set; }

        public string ResidentialAddress { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string ParmanentAddress { get; set; }

        public string ContactNo { get; set; }

        public string AlContactNo { get; set; }

        public string BloodGroup { get; set; }
        
        public LocationDTO Location { get; set; }

        public RoleDTO Role { get; set; }
    }
}
