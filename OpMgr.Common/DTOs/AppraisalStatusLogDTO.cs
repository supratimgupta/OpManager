using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class AppraisalStatusLogDTO
    {
        public int AppraisalStatusLogId { get; set; }

        public EmployeeAppraisalMasterDTO EmployeeAppraisalMaster { get; set; }

        public AppraisalStatusDTO AppraisalStatus { get; set; }

        public DateTime LogDatetime { get; set; }

        public string AppraisalComment { get; set; }

        public EmployeeDetailsDTO Employee { get; set; }
    }
}
