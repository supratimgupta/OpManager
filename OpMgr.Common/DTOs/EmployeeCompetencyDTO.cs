using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
   public class EmployeeCompetencyDTO
    {
        public int EmpCompetencyId { get; set; }

        public bool Active { get; set; }

        public EmployeeAppraisalMasterDTO EmployeeAppraisalMaster { get; set; }

        public CompetencyDTO Competency { get; set; }
    }
}
