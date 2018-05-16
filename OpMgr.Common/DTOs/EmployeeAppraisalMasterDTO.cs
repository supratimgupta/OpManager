using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class EmployeeAppraisalMasterDTO
    {
        public int EmployeeAppraisalMasterId { get; set; }

        public int EmpAppPmsMasterId { get; set; }

        public EmployeeDetailsDTO Employee { get; set; }

        public string AppraisalType { get; set; }

        public AppraisalStatusDTO AppraisalStatus { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime ValidTo { get; set; }

        public string  LatestNote { get; set; }

        public DateTime OpenFrom { get; set; }

        public DateTime OpenTo { get; set; }

        public string AppraiserComment { get; set; }

        public decimal AppraiserFinalRating { get; set; }

        public string ReviewerComment { get; set; }

        public decimal ReviewerFinalRating { get; set; }

        public string IndividualInitiative { get; set; }

        public string InstitutionalSupport { get; set; }

        public decimal AvgFinalRating { get; set; }

        public string  AvgFinalLevel { get; set; }

        public PMSDesignationDTO PMSDesignation { get; set; }
    }
}
