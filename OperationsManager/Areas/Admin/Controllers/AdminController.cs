using OpMgr.Common.Contracts;
using OpMgr.Common.Contracts.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationsManager.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        private ILogSvc _logger;

        private ISessionSvc _sessionSvc;

        private IDropdownRepo _ddlRepo;

        private Helpers.UIDropDownRepo _uiddlRepo;

        public AdminController(IDropdownRepo ddlRepo, ISessionSvc sessionSvc)
        {            
            _ddlRepo = ddlRepo;            
            _uiddlRepo = new Helpers.UIDropDownRepo(_ddlRepo);
            _sessionSvc = sessionSvc;
        }

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


        [HttpGet]
        public ActionResult ActionUserRoleMapping()
        {
            Models.AdminViewModel adminViewModel = new Models.AdminViewModel();
            adminViewModel.ActionList = _uiddlRepo.getActionLinkDropDown();
            adminViewModel.EntitlementList = _uiddlRepo.getEntitleMentDropDown();
            return View(adminViewModel);
        }
        //[HttpPost]
        //public ActionResult AddMasterData()
        //{
        //    return View();
        //}
    }
}