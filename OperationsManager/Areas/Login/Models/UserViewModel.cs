using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationsManager.Areas.Login.Models
{
    public class UserViewModel : UserMasterDTO
    {
        public SelectList LocationList { get; set; }

        public SelectList RoleList { get; set; }

        public SelectList HouseList { get; set; }

        public SelectList ClassTypeList { get; set; }

        public SelectList SectionList { get; set; }        

        public SelectList DepartmentList { get; set; }

        public SelectList BookCategoryList { get; set; }

        public SelectList DesignationList { get; set; }

        public SelectList StandardList { get; set; }

        public SelectList StandardSectionList { get; set; }

        public StudentDTO Student { get; set; }

        public ClassTypeDTO ClassType { get; set; }

        public StandardSectionMapDTO StandardSection { get; set; }

        public EmployeeDetailsDTO EmployeeDetails { get; set; }
    }
}