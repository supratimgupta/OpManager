using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs.Users
{
    public class ActionDTO
    {
        public int RowId { get; set; }

        public string Name { get; set; }

        public bool IsChildAction { get; set; }

        public ActionDTO ParentAction { get; set; }

        public string URL { get; set; }

        public string DisabledControlId { get; set; }

        public string HiddenControlId { get; set; }

        public bool IsActive { get; set; }

        public string GroupName { get; set; }
    }
}
