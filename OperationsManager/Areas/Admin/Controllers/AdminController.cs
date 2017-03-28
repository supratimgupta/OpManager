using OperationsManager.Areas.Admin.Models;
using OpMgr.Common.Contracts;
using OpMgr.Common.Contracts.Modules;
using OpMgr.Common.DTOs;
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

        private IEntitlementActionSvc _entitlementActionSvc;

        private ISessionSvc _sessionSvc;

        private IDropdownRepo _ddlRepo;

        private Helpers.UIDropDownRepo _uiddlRepo;

        public AdminController(IEntitlementActionSvc entitlementActionSvc, IDropdownRepo ddlRepo, ISessionSvc sessionSvc)
        {
            _entitlementActionSvc = entitlementActionSvc;
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
        public ActionResult AddStyle()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ActionUserRoleMapping()
        {
            Models.AdminViewModel adminViewModel = new Models.AdminViewModel();
            adminViewModel.ActionList = _uiddlRepo.getActionLinkDropDown();
            adminViewModel.EntitlementList = _uiddlRepo.getEntitleMentDropDown();
            EntitlementActionDTO data = new EntitlementActionDTO();
            data.ActionDetails = new ActionDTO();
            data.RoleDetails = new EntitlementDTO();
            StatusDTO<List<EntitlementActionDTO>> status = _entitlementActionSvc.Select(data);

            if (status.ReturnObj != null && status.ReturnObj.Count > 0)
            {
                adminViewModel.entitlementactionList = new List<AdminViewModel>(); // instantiating list of AdminViewmodel

                if (status.IsSuccess && !status.IsException)
                {                    
                    AdminViewModel adviewmodel;
                    foreach (EntitlementActionDTO entitlement in status.ReturnObj)
                    {
                        if (entitlement != null)
                        {
                            adviewmodel = new AdminViewModel(); // instantiating each student

                            adviewmodel.RowId = entitlement.RowId;
                            adviewmodel.ActionDetails = entitlement.ActionDetails;
                            adviewmodel.RoleDetails = entitlement.RoleDetails;

                            //Add into Student vIew Model List
                            adminViewModel.entitlementactionList.Add(adviewmodel);
                            adminViewModel.IsSearchSuccessful = true;
                        }
                    }
                }
            }
                return View(adminViewModel);
        }

        [HttpPost]
        public ActionResult ActionUserRoleMapping(AdminViewModel adminViewModel)
        {
            adminViewModel.UserMaster = new UserMasterDTO();
            _entitlementActionSvc.Insert(adminViewModel);
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