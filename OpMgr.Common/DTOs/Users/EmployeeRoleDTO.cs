using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs.Users
{
    /// <summary>
    /// Maps employee role model
    /// </summary>
    public class EmployeeRoleDTO
    {
        public int RowId { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }
    }
}
