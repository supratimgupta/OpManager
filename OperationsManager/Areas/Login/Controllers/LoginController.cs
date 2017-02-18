using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationsManager.Areas.Login.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login/Login
        public ActionResult Login()
        {
            return View();
        }
    }
}