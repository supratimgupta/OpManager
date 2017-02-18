using OpMgr.Common.Contracts;
using OpMgr.Common.Contracts.Modules;
using OpMgr.Common.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationsManager.Controllers
{
    public class DepartmentController : Controller
    {
        // GET: Department
        IDepartmentSvc _deptSvc;
        //IConfigSvc _configSvc;
        ILogSvc _logger;

        public DepartmentController(IDepartmentSvc deptSvc, ILogSvc logger)
        {
            _deptSvc = deptSvc;
            //_configSvc = configSvc;
            _logger = logger;
        }

        public ActionResult AddDept(Models.DeptViewModel dept)
        {
            _deptSvc.Insert(dept);
            return View();
        }


    }
}