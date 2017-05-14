using OpMgr.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpMgr.Common.DTOs;
using System.Net.Mail;
using System.Net;
using System.Data;
using MySql.Data.MySqlClient;
using OpMgr.DataAccess.Implementations;

namespace OpMgr.Mailer
{
    public class MailSvc : IMailSvc
    {

        /// <summary>
        /// Required for getting connection related configs
        /// </summary>
        private IConfigSvc _configSvc;
        private DataSet _dsMailSet;

        /// <summary>
        /// Needed for exception handling
        /// </summary>
        private ILogSvc _logger;

        public MailSvc(IConfigSvc configSvc, ILogSvc logger)
        {
            _configSvc = configSvc;
            _logger = logger;
        }


        public int CheckMail(string EmailId)
        {
            int UserMasterId = 0;
            string selectClause = null;
            try
            {
                if (!string.IsNullOrEmpty(EmailId))
                {
                    using (IDbSvc dbSvc = new DbSvc(_configSvc))
                    {
                        dbSvc.OpenConnection();
                        MySqlCommand command = new MySqlCommand();
                        command.Connection = dbSvc.GetConnection() as MySqlConnection;// establish connection

                        selectClause = "SELECT UserMasterId FROM usermaster WHERE EmailId=@EmailId AND Active=1";
                        command.Parameters.Add("@EmailId", MySqlDbType.String).Value = EmailId;
                        command.CommandText = selectClause;

                        command.ExecuteScalar();

                        MySqlDataAdapter msda = new MySqlDataAdapter(command);
                        _dsMailSet = new DataSet();
                        msda.Fill(_dsMailSet);
                        if (_dsMailSet != null && _dsMailSet.Tables.Count > 0)
                        {
                            if (_dsMailSet.Tables[0].Rows.Count > 0)
                            {
                                UserMasterId = (int)_dsMailSet.Tables[0].Rows[0]["UserMasterId"];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return UserMasterId;           
        }

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
