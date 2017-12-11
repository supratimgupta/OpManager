using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class SessionDTO : DTOs.UserMasterDTO
    {
        public List<ActionDTO> ActionList { get; set; }
        public List<EntitlementDTO> EntitleMentList { get; set; }
        public List<LocationDTO> LocationList { get; set; }

        public string IconImagePath { get; set; }

        public List<NotificationDTO> Notifications { get; set; }

        public int NotificationCounts { get; set; }

        //public string CurrentRequestedPage { get; set; }
    }
}
