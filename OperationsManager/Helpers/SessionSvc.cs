using OperationsManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationsManager.Helpers
{
    public class SessionSvc : OpMgr.Common.Contracts.ISessionSvc
    {
        public static string _sessionIdentifier = "USER_SESSION";
        public void SetUserSession(OpMgr.Common.DTOs.SessionDTO session)
        {
            System.Web.HttpContext.Current.Session[_sessionIdentifier] = session;
        }

        public OpMgr.Common.DTOs.SessionDTO GetUserSession()
        {
            return System.Web.HttpContext.Current.Session[_sessionIdentifier] as OpMgr.Common.DTOs.SessionDTO;
        }
    }
}