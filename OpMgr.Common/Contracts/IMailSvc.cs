using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.Contracts
{
    public interface IMailSvc
    {
        bool SendMail(DTOs.MailDTO mail);

        int CheckMail(string emailId);// return UserID if mail is valid
    }
}
