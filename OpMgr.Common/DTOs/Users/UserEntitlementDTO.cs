using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs.Users
{
    public class UserEntitlementDTO
    {
        public int RowId { get; set; }

        public UserDTO UserDetails { get; set; }

        public EntitlementDTO RoleDetails { get; set; }

        public bool IsActive { get; set; }
    }
}
