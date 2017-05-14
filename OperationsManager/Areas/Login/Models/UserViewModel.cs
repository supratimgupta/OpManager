using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationsManager.Areas.Login.Models
{
    public class UserViewModel : UserMasterDTO
    {
        public SelectList LocationList { get; set; }

        public SelectList GenderList { get; set; }

        public SelectList RoleList { get; set; }

        public SelectList DepartmentList { get; set; }

        public SelectList DesignationList { get; set; }

        public string MODE { get; set; }

        public int HdnUserMasterId { get; set; }

        public string NewPassword { get; set; }

        public string Name { get; set; }

        public string SuccessorFailureMessage { get; set; }
        public string MessageColor { get; set; } 


        //public BookMasterDTO BookMaster { get; set; }
    }
}