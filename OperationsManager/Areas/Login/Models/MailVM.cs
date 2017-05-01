using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OperationsManager.Areas.Login.Models
{
    public class MailVM:MailDTO
    {
        public string To { get; set; }
        public int UserId { get; set; }
        public string SuccessOrFailureMessage { get; set; }
        public System.Drawing.Color MessageColor { get; set; }
    }
}