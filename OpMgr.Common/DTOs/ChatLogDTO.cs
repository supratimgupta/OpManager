using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class ChatLogDTO
    {
        public int Id { get; set; }

        public UserMasterDTO SentByUser { get; set; }

        public UserMasterDTO SentToUser { get; set; }

        public DateTime DateTimeLog { get; set; }

        public string Message { get; set; }

        public ChatGroupDTO Group { get; set; }

        public bool IsRead { get; set; }

        public string MessageSentTimesAgo { get; set; }
    }
}
