using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
   public class MailDTO
    {
        public string To;
        public string Cc;
        public string Bcc;
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
