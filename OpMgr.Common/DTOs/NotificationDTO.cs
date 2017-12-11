using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class NotificationDTO
    {
        public int NotificationReminderId { get; set; }

        public UserMasterDTO User { get; set; }

        public string NotificationText { get; set; }

        public bool Active { get; set; }

        public bool Viewed { get; set; }

        public DateTime NotificationActiveFrom { get; set; }
    }
}
