using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class ApprovalMappingDTO
    {
        public int ApprovalMappingId { get; set; }

        public EmployeeDetailsDTO Employee { get; set; }

        public EmployeeDetailsDTO Approver { get; set; }

        public DesignationDTO EmpDesignation { get; set; }

        public DesignationDTO ApproverDesignation { get; set; }

        public int Canmovefromstatus { get; set; }

        public int Canmovetostatus { get; set; }

        public bool Canviewforotherstatus { get; set; }
    }
}
