using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class ActionDTO
    {
        public int RowId { get; set; }

        public string ActionName { get; set; }

        public bool IsChildAction { get; set; }

        public ActionDTO ParentAction { get; set; }

        public string ActionLink { get; set; }

        public string DisabledControlId { get; set; }

        public string HiddenControlId { get; set; }

        public bool IsActive { get; set; }

        public string GroupName { get; set; }

        public string MenuText { get; set; }

        public string MenuImage { get; set; }
    }
}
