using Ninject;
using Ninject.Extensions.Xml;
using OpMgr.Common.Contracts;
using OpMgr.Configurations.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OperationsManager.Helpers
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;

        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null
                       ? null
                       : (IController)ninjectKernel.Get(controllerType);
        }

        private void AddBindings()
        {
            ninjectKernel.Load(System.Configuration.ConfigurationManager.AppSettings["DIXMLPath"]);
        }
    }
}