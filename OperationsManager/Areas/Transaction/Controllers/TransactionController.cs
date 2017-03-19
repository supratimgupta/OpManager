using OperationsManager.Attributes;
using OpMgr.Common.Contracts;
using OpMgr.Common.Contracts.Modules;
using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationsManager.Areas.Transaction.Controllers
{
    [OpMgrAuth]
    public class TransactionController : Controller
    {
        private IDropdownRepo _ddlRepo;

        private IUserSvc _userSvc;

        private ICommonConfigSvc _commonConfig;

        private ITransactionLogSvc _transactionLog;

        public TransactionController(IDropdownRepo ddlRepo, IUserSvc userSvc, ICommonConfigSvc commonConfig, ITransactionLogSvc transactionLog)
        {
            _ddlRepo = ddlRepo;
            _userSvc = userSvc;
            _commonConfig = commonConfig;
            _transactionLog = transactionLog;
        }
        [HttpGet]
        public ActionResult AddTransaction()
        {
            Helpers.UIDropDownRepo uiDDLRepo = new Helpers.UIDropDownRepo(_ddlRepo);
            Models.TransactionViewModel trViewModel = new Models.TransactionViewModel();
            trViewModel.UserList = uiDDLRepo.getUserDropDown();
            trViewModel.LocationList = uiDDLRepo.getLocationDropDown();
            trViewModel.TransactionTypeList = uiDDLRepo.getTransactionTypes();
            trViewModel.TransactionRuleList = uiDDLRepo.getTransactionRules();
            trViewModel.StandardSectionList = uiDDLRepo.getStandardSectionDropDown();
            return View(trViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTransaction(Models.TransactionViewModel trViewModel)
        {
            trViewModel.Active = true;
            trViewModel.IsCompleted = false;
            trViewModel.CreatedDate = DateTime.Today.Date;
            _transactionLog.Insert(trViewModel);
            trViewModel = new Models.TransactionViewModel();
            ModelState.Clear();
            Helpers.UIDropDownRepo uiDDLRepo = new Helpers.UIDropDownRepo(_ddlRepo);
            
            trViewModel.UserList = uiDDLRepo.getUserDropDown();
            trViewModel.LocationList = uiDDLRepo.getLocationDropDown();
            trViewModel.TransactionTypeList = uiDDLRepo.getTransactionTypes();
            trViewModel.TransactionRuleList = uiDDLRepo.getTransactionRules();
            trViewModel.StandardSectionList = uiDDLRepo.getStandardSectionDropDown();

            //Add user session when implemented
            
            return View(trViewModel);
        }

        //[HttpPost]
        //public JsonResult IsUserAStudent(int userMasterId)
        //{
        //    string userRole = _userSvc.GetUserRole(userMasterId);
        //    if(string.Equals(userRole, _commonConfig["STUD_ROLE_ID"]))
        //    {
        //        return Json(new { isStud = true }, JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(new { isStud = false }, JsonRequestBehavior.AllowGet);
        //}
    }
}