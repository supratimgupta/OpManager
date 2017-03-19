using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationsManager.Areas.Student.Models
{
    public class StudentVM : StudentDTO
    {
        public string Name { get; set; }

        public List<StudentVM> studentList { get; set;}

        public SelectList StandardSectionList { get; set; }
    }
}