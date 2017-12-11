using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OperationsManager.Areas.Notification.Models
{
    public class NotificationVM : NotificationDTO
    {
        public bool MarkAsRead { get; set; }

        public List<NotificationVM> Notifications { get; set; }
    }
}