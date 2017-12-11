using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OperationsManager.Models
{
    public class ChatViewModel
    {
        public string UserName { get; set; }

        public int UnreadChat { get; set; }

        public string LastSentAtString { get; set; }

        public DateTime LastSentAt { get; set; }

        public int UserMasterId { get; set; }
    }
}