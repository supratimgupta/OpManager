using OpMgr.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpMgr.Common.DTOs;
using System.Net.Mail;
using System.Net;

namespace OpMgr.Mailer
{
    public class MailSvc : IMailSvc
    {
        public bool SendMail(MailDTO mail)
        {
            bool isMailSent = false;
            try
            {
                if(mail!=null)
                {
                    using (MailMessage mailMsg = new MailMessage())
                    {
                        foreach(string m in mail.ToList)
                        {
                            mailMsg.To.Add(new MailAddress(m));
                        }
                        mailMsg.From =new MailAddress(mail.From);
                        mailMsg.Subject = mail.MailSubject;
                        mailMsg.Body = mail.MailBody;
                        mailMsg.IsBodyHtml = mail.IsBodyHtml;

                        SmtpClient smtp = new SmtpClient();// instantiating to send mail
                        smtp.EnableSsl = mail.EnableSSL;
                        smtp.UseDefaultCredentials = true;// mail.UseDefaultCredentials; NOT TAKING TRUE VALUE??
                        NetworkCredential network = new NetworkCredential(mailMsg.From.ToString(), "allcreater04");
                        smtp.Credentials = network;
                        smtp.Port = mail.SmtpPort;
                        smtp.Host = mail.SmtpServer;
                        smtp.Send(mailMsg);

                        isMailSent = true;//sigals that mail has been sen properly
                    }
                }
            }
            catch(Exception ex)
            {

            }
            return isMailSent;
        }
    }
}
