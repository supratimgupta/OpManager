using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class PMSMasterExcelDTO
    {
        public int EmployeeAppraisalMasterId { get; set; }

        public string Fname { get; set; }
        public string Lname { get; set; }
        public string locationdescription { get; set; }
        public string StaffEmployeeId { get; set; }
        public string EducationQualification { get; set; }
        public string PmsDesignationDescription { get; set; }
        public string ApproverName { get; set; }
        public string ReviewerName { get; set; }
        public string AppraisalType { get; set; }
        public string ReviewerFinalRating { get; set; }
        public string ReviewerRatingLevel { get; set; }

        public string ReviewerComment { get; set; }
        public string appraisalstatusdescription { get; set; }
    }
}
