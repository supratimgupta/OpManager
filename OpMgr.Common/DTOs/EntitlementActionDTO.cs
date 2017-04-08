using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class EntitlementActionDTO
    {
        public int RowId { get; set; }

        public UserMasterDTO UserMaster { get; set; }

        public ActionDTO ActionDetails{ get; set; }

        public EntitlementDTO RoleDetails { get; set; }

        public bool IsActive { get; set; }
    }
}
