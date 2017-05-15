using Ninject;
using OpMgr.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace OperationsManager.Attributes
{
    public class OpMgrHandleErrorAttribute : HandleErrorAttribute
    {
        private ILogSvc _logger;

        public OpMgrHandleErrorAttribute()
        {
            string diResolverPath = System.Configuration.ConfigurationManager.AppSettings["DIXMLPath"];
            StandardKernel kernel = new StandardKernel();
            kernel.Load(diResolverPath);
            _logger = kernel.Get<ILogSvc>();
        }

        public override void OnException(ExceptionContext filterContext)
        {
            Exception ex = filterContext.Exception;

            _logger.Log(ex);

            filterContext.ExceptionHandled = true;

            var controllerName = (string)filterContext.RouteData.Values["controller"];
            var actionName = (string)filterContext.RouteData.Values["action"];

            var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);

            filterContext.ExceptionHandled = true;

            filterContext.Result = new ViewResult()
            {
                ViewName = "~/Views/Error/Error.cshtml",
                ViewData = new ViewDataDictionary<HandleErrorInfo>(model)
            };
        }
    }
}