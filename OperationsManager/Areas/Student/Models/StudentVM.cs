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

        public Boolean IsSearchSuccessful { get; set; }

        public SelectList LocationList { get; set; }

        public SelectList GenderList { get; set; }

        public SelectList RoleList { get; set; }

        public SelectList HouseList { get; set; }

        public SelectList ClassTypeList { get; set; }

        public SelectList SectionList { get; set; }

        public string MODE { get; set; }
        public int HdnUserMasterId { get; set; }

    }
}