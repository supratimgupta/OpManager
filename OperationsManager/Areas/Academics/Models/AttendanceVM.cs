using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationsManager.Areas.Academics.Models
{
    public class AttendanceVM :StudentDTO
    {
        public List<AttendanceVM> studentList { get; set; }

        public SelectList StandardSectionList { get; set; }

        public Boolean IsSearchSuccessful { get; set; }

        public string SuccessOrFailureMessage { get; set; }

        public string MsgColor { get; set; }

        public SelectList LocationList { get; set; }

        public SelectList AttendanceList { get; set; }

        public string AttendanceDateString { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }
        public string Status { get; set; }

        public Boolean IsCommandPromote { get; set; }
    }
}