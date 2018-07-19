using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpMgr.Common.DTOs
{
    public class PMSMasterModel
    {
        public int EmployeeAppraisalMasterId { get; set; }
        public int pmsdesignationid { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string locationdescription { get; set; }
        public int StaffEmployeeId { get; set; }
        public string EducationQualification { get; set; }
        public string PmsDesignationDescription { get; set; }
        public string ApproverName { get; set; }
        public string ReviewerName { get; set; }
        public string AppraisalType { get; set; }
        public float ReviewerFinalRating { get; set; }
        public int ReviewerRatingLevel { get; set; }
        public string ReviewerComment { get; set; }
        public string appraisalstatusdescription { get; set; }
    }
}