using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class EntitlementDTO
    {
        public int UserRoleId { get; set; }

        public string RoleName { get; set; }

        public bool IsRoleActive { get; set; }
    }
}
