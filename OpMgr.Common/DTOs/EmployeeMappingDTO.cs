using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class EmployeeMappingDTO
    {
        public int EmployeeMappingId { get; set; }

        public UserMasterDTO CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public UserMasterDTO UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool Active { get; set; }

        public EmployeeDetailsDTO Employee { get; set; }

        public string YrsOfExp { get; set; }

        public int ReportingEmpId { get; set; }

    }
}
