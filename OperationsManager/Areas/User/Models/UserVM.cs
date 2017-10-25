using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationsManager.Areas.User.Models
{
    public class UserVM:UserMasterDTO
    {
        public string Name { get; set; }

        public List<UserVM> UserList { get; set; }

        public SelectList GenderList { get; set; }

        public Boolean IsSearchSuccessful { get; set; }

        public SelectList DepartmentList { get; set; }

        public SelectList LocationList { get; set; }
    }
}