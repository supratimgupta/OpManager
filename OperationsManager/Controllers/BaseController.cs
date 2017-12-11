using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationsManager.Controllers
{
    [Attributes.OpMgrAuth]
    [Attributes.OpMgrHandleError]
    public class BaseController : Controller
    {

    }
}