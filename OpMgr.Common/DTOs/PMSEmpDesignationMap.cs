using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class PMSEmpDesignationMapDTO
    {
        public int PmsEmpDesmapId { get; set; }

        public UserMasterDTO CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public UserMasterDTO UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool Active { get; set; }

        public EmployeeDetailsDTO Employee { get; set; }

        public PMSDesignationDTO PmsDesignation { get; set; }

        public DateTime ? AppraisalStartDate { get; set; }

        public DateTime? AppraisalEndDate { get; set; }
    }
}
