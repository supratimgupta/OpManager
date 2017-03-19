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
            trViewModel.DueAmount = trViewModel.AmountImposed;
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

        [HttpGet]
        public ActionResult Search()
        {
            Models.TransactionViewModel trVM = new Models.TransactionViewModel();

            Helpers.UIDropDownRepo uiDDLRepo = new Helpers.UIDropDownRepo(_ddlRepo);

            trVM.StandardSectionList = uiDDLRepo.getStandardSectionDropDown();
            trVM.TransactionTypeList = uiDDLRepo.getTransactionTypes();

            return View(trVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(Models.TransactionViewModel trVM)
        {
            StatusDTO<List<TransactionLogDTO>> status = _transactionLog.Select(trVM);
            if(status.IsSuccess)
            {
                trVM.SearchResult = new List<Models.TransactionViewModel>();
                if(status.ReturnObj!=null && status.ReturnObj.Count>0)
                {
                    Models.TransactionViewModel trSR = null;
                    for(int i=0;i<status.ReturnObj.Count;i++)
                    {
                        trSR = new Models.TransactionViewModel();

                        trSR.User = new UserMasterDTO();
                        trSR.User.FName = status.ReturnObj[i].User.FName;
                        trSR.User.MName = status.ReturnObj[i].User.MName;
                        trSR.User.LName = status.ReturnObj[i].User.LName;
                        trSR.TransactionLogId = status.ReturnObj[i].TransactionLogId;
                        trSR.TransactionDate = status.ReturnObj[i].TransactionDate;
                        trSR.TransactionDueDate = status.ReturnObj[i].TransactionDueDate;
                        trSR.ParentTransactionLogId = status.ReturnObj[i].ParentTransactionLogId;
                        trSR.IsCompleted = status.ReturnObj[i].IsCompleted;
                        trSR.CompletedOn = status.ReturnObj[i].CompletedOn;
                        trSR.AmountImposed = status.ReturnObj[i].AmountImposed;
                        trSR.AmountGiven = status.ReturnObj[i].AmountGiven;
                        trSR.DueAmount = status.ReturnObj[i].DueAmount;
                        trSR.TransferMode = status.ReturnObj[i].TransferMode;
                        trSR.Location = status.ReturnObj[i].Location;
                        trSR.TransactionType = status.ReturnObj[i].TransactionType;
                        trSR.HasPenalty = status.ReturnObj[i].HasPenalty;
                        trSR.OriginalTransLog = status.ReturnObj[i].OriginalTransLog;
                        trSR.TransactionRule = status.ReturnObj[i].TransactionRule;

                        trVM.SearchResult.Add(trSR);
                    }
                }
                else
                {
                    trVM.Message = "No related transaction record found.";
                }
            }
            else
            {
                trVM.SearchResult = null;
                trVM.Message = "Query returned with error.";
            }

            Helpers.UIDropDownRepo uiDDLRepo = new Helpers.UIDropDownRepo(_ddlRepo);

            trVM.StandardSectionList = uiDDLRepo.getStandardSectionDropDown();
            trVM.TransactionTypeList = uiDDLRepo.getTransactionTypes();

            return View(trVM);
        }
    }
}