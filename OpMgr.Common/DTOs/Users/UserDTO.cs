using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs.Users
{
    public class UserDTO
    {
        public int RowId { get; set; }

        public EmployeeDTO EmployeeDetails { get; set; }

        public string LoginId { get; set; }

        public string SSOLoginId { get; set; }

        public string Password { get; set; }

        public DateTime LastLoginDateTime { get; set; }

        public string PasswordLastUpdated { get; set; }

        public int NumberOfTries { get; set; }

        public DateTime LastTryTime { get; set; }

        public bool IsActive { get; set; }

        public bool IsLocked { get; set; }
    }
}
