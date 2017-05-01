using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class MailDTO
    {
        public string MailSubject { get; set; }

        public string MailBody { get; set; }

        public List<string> ToList { get; set; }

        public List<string> CcList { get; set; }

        public List<string> BccList { get; set; }

        public string From { get; set; }

        public string FromAlias { get; set; }

        public string SmtpServer { get; set; }

        public int SmtpPort { get; set; }

        public bool IsBodyHtml { get; set; }

        public bool UseDefaultCredentials { get; set; }

        public bool EnableSSL { get; set; }
    }
}
