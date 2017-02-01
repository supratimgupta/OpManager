using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs.Users
{
    /// <summary>
    /// Maps department model
    /// </summary>
    public class DepartmentDTO
    {
        public int RowId { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public EmployeeDTO SupervisorDetails { get; set; }

        public int CurrentPageNo { get; set; }

        public int NumberOfRecordsPerPage { get; set; }

        public int TotalNoOfRecords { get; set; }
    }
}
