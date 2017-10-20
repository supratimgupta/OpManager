﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class EmployeeAppraisalMasterDTO
    {
        public int EmployeeAppraisalMasterId { get; set; }

        public EmployeeDetailsDTO Employee { get; set; }

        public string AppraisalType { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime ValidTo { get; set; }

        public string  LatestNote { get; set; }

        public DateTime OpenFrom { get; set; }

        public DateTime OpenTo { get; set; }

        public string AppraiserComment { get; set; }

        public decimal AppraiserFinalRating { get; set; }

        public string ReviewerComment { get; set; }

        public decimal ReviewerFinalRating { get; set; }

    }
}