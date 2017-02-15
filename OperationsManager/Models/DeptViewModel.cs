using OpMgr.Common.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OperationsManager.Models
{
    public class DeptViewModel : DepartmentDTO
    {
        public UserDTO User { get; set; }
    }
}