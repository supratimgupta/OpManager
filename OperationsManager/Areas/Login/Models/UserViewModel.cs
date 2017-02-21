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
    }
}