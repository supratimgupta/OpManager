using OperationsManager.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationsManager.Areas.Library.Controllers
{
    [OpMgrAuth]
    public class LibraryController : Controller
    {
        // GET: Library/Library
        [HttpGet]
        public ActionResult RegisterBooks()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult RegisterBooks()
        //{
        //    return View();
        //}
    }
}