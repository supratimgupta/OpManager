using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationsManager.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin/Admin
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddMasterData()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult AddMasterData()
        //{
        //    return View();
        //}
    }
}