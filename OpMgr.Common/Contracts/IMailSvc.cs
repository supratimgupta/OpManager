using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.Contracts
{
    public interface IMailSvc
    {
        void SendMail(DTOs.MailDTO mail);
    }
}
