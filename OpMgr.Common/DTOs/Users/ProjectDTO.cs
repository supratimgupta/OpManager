using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs.Users
{
    /// <summary>
    /// Maps project model
    /// </summary>
    public class ProjectDTO
    {
        public int RowId { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public EmployeeDTO ManagerDetails { get; set; }
    }
}
