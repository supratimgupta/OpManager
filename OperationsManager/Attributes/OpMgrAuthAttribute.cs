using OperationsManager.Helpers;
using OpMgr.Common.Contracts;
using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OperationsManager.Attributes
{
    public class OpMgrAuthAttribute : AuthorizeAttribute
    {
        ISessionSvc _sessionSvc;
        public OpMgrAuthAttribute()
        {
            _sessionSvc = new SessionSvc();
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            SessionDTO session = _sessionSvc.GetUserSession();
            if(session==null)
            {
                return false;
            }
            var url = session.ActionList.FirstOrDefault(al => (!string.IsNullOrEmpty(al.ActionLink) && !string.IsNullOrEmpty(al.ActionLink.Trim()) && httpContext.Request.Path.ToUpper().Contains(al.ActionLink.ToUpper())));
            if(url!=null)
            {
                return true;
            }
            return false;
        }


        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            SessionDTO session = _sessionSvc.GetUserSession();
            if (session == null)
            {
                filterContext.Result = new RedirectToRouteResult(new
                RouteValueDictionary(new { controller = "Login", action = "Login", area="Login" }));
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new
                RouteValueDictionary(new { controller = "Login", action = "AccessDenied", area= "Login" }));
            }
        }
    }
}