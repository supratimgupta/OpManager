using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs.Users
{
    /// <summary>
    /// Maps designation model
    /// </summary>
    public class DesignationDTO
    {
        public int RowId { get; set; }

        public string Name { get; set; }

        public string IsActive { get; set; }
    }
}
