using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationsManager.Areas.Academics.Controllers
{
    public class AttendanceController : Controller
    {
        // GET: Academics/Attendance
        public ActionResult Index()
        {
            return View();
        }
    }
}