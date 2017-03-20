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

        private ISessionSvc _sessionSvc;

        public TransactionController(IDropdownRepo ddlRepo, IUserSvc userSvc, ICommonConfigSvc commonConfig, ITransactionLogSvc transactionLog, ISessionSvc sessionSvc)
        {
            _ddlRepo = ddlRepo;
            _userSvc = userSvc;
            _commonConfig = commonConfig;
            _transactionLog = transactionLog;
            _sessionSvc = sessionSvc;
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
                    trVM.Message = new MvcHtmlString("No related transaction record found.");
                }
            }
            else
            {
                trVM.SearchResult = null;
                trVM.Message = new MvcHtmlString("Query returned with error.");
            }

            Helpers.UIDropDownRepo uiDDLRepo = new Helpers.UIDropDownRepo(_ddlRepo);

            trVM.StandardSectionList = uiDDLRepo.getStandardSectionDropDown();
            trVM.TransactionTypeList = uiDDLRepo.getTransactionTypes();

            return View(trVM);
        }


        [HttpGet]
        public ActionResult MyTransactions()
        {
            Models.TransactionViewModel trVM = new Models.TransactionViewModel();

            Helpers.UIDropDownRepo uiDDLRepo = new Helpers.UIDropDownRepo(_ddlRepo);

            trVM.StandardSectionList = uiDDLRepo.getStandardSectionDropDown();
            trVM.TransactionTypeList = uiDDLRepo.getTransactionTypes();

            return View(trVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MyTransactions(Models.TransactionViewModel trVM)
        {
            SessionDTO session = _sessionSvc.GetUserSession();
            trVM.User = new UserMasterDTO();
            trVM.User.UserMasterId = session.UserMasterId;
            StatusDTO<List<TransactionLogDTO>> status = _transactionLog.Select(trVM);
            if (status.IsSuccess)
            {
                trVM.SearchResult = new List<Models.TransactionViewModel>();
                if (status.ReturnObj != null && status.ReturnObj.Count > 0)
                {
                    Models.TransactionViewModel trSR = null;
                    for (int i = 0; i < status.ReturnObj.Count; i++)
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
                    trVM.Message = new MvcHtmlString("No related transaction record found.");
                }
            }
            else
            {
                trVM.SearchResult = null;
                trVM.Message = new MvcHtmlString("Query returned with error.");
            }

            Helpers.UIDropDownRepo uiDDLRepo = new Helpers.UIDropDownRepo(_ddlRepo);

            trVM.StandardSectionList = uiDDLRepo.getStandardSectionDropDown();
            trVM.TransactionTypeList = uiDDLRepo.getTransactionTypes();

            return View(trVM);
        }


        [HttpGet]
        public ActionResult WorkOnTransaction(int id)
        {
            Models.TransactionViewModel trSR = null;
            StatusDTO<TransactionLogDTO> status = _transactionLog.Select(id);
            if(status.IsSuccess && status.ReturnObj!=null)
            {
                trSR = new Models.TransactionViewModel();
                trSR.User = new UserMasterDTO();
                trSR.User.FName = status.ReturnObj.User.FName;
                trSR.User.MName = status.ReturnObj.User.MName;
                trSR.User.LName = status.ReturnObj.User.LName;
                trSR.TransactionLogId = status.ReturnObj.TransactionLogId;
                trSR.TransactionDate = status.ReturnObj.TransactionDate;
                trSR.TransactionDueDate = status.ReturnObj.TransactionDueDate;
                trSR.ParentTransactionLogId = status.ReturnObj.ParentTransactionLogId;
                trSR.IsCompleted = status.ReturnObj.IsCompleted;
                trSR.CompletedOn = status.ReturnObj.CompletedOn;
                trSR.AmountImposed = status.ReturnObj.AmountImposed;
                trSR.AmountGiven = status.ReturnObj.AmountGiven;
                trSR.DueAmount = status.ReturnObj.DueAmount;
                trSR.TransferMode = status.ReturnObj.TransferMode;
                trSR.Location = status.ReturnObj.Location;
                trSR.TransactionType = status.ReturnObj.TransactionType;
                trSR.HasPenalty = status.ReturnObj.HasPenalty;
                trSR.OriginalTransLog = status.ReturnObj.OriginalTransLog;
                trSR.TransactionRule = status.ReturnObj.TransactionRule;

                Encryption encrypt = new Encryption();
                trSR.EncryptedTransactionLogId = encrypt.encryption(trSR.TransactionLogId.ToString());

                trSR.IsSuccessMessage = true;

                if(trSR.IsCompleted.Value || trSR.DueAmount==0 || trSR.AmountGiven == trSR.AmountImposed)
                {
                    trSR.HideSaveButton = true;
                }
            }
            return View(trSR);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult WorkOnTransaction(Models.TransactionViewModel trVM)
        {
            Encryption encrypt = new Encryption();
            string encryptedId = encrypt.encryption(trVM.TransactionLogId.ToString());
            double currentAmt = trVM.CurrentAmount;
            if(string.Equals(encryptedId, trVM.EncryptedTransactionLogId))
            {
                //Models.TransactionViewModel trSR = null;
                StatusDTO<TransactionLogDTO> status = _transactionLog.Select(trVM.TransactionLogId);
                if (status.IsSuccess && status.ReturnObj != null)
                {
                    trVM = new Models.TransactionViewModel();
                    trVM.CurrentAmount = currentAmt;
                    trVM.User = new UserMasterDTO();
                    trVM.User.FName = status.ReturnObj.User.FName;
                    trVM.User.MName = status.ReturnObj.User.MName;
                    trVM.User.LName = status.ReturnObj.User.LName;
                    trVM.TransactionLogId = status.ReturnObj.TransactionLogId;
                    trVM.TransactionDate = status.ReturnObj.TransactionDate;
                    trVM.TransactionDueDate = status.ReturnObj.TransactionDueDate;
                    trVM.ParentTransactionLogId = status.ReturnObj.ParentTransactionLogId;
                    trVM.IsCompleted = status.ReturnObj.IsCompleted;
                    trVM.CompletedOn = status.ReturnObj.CompletedOn;
                    trVM.AmountImposed = status.ReturnObj.AmountImposed;
                    trVM.AmountGiven = status.ReturnObj.AmountGiven;
                    trVM.DueAmount = status.ReturnObj.DueAmount;
                    trVM.TransferMode = status.ReturnObj.TransferMode;
                    trVM.Location = status.ReturnObj.Location;
                    trVM.TransactionType = status.ReturnObj.TransactionType;
                    trVM.HasPenalty = status.ReturnObj.HasPenalty;
                    trVM.OriginalTransLog = status.ReturnObj.OriginalTransLog;
                    trVM.TransactionRule = status.ReturnObj.TransactionRule;

                    double originalDueAmt = trVM.DueAmount.Value;
                    double originalAmtGiven = trVM.AmountGiven.Value;

                    double dueAmount = originalDueAmt - trVM.CurrentAmount;
                    if(dueAmount < 0)
                    {
                        trVM.IsSuccessMessage = false;
                        trVM.Message = new MvcHtmlString("Please enter amount less than due amount.");
                        return View(trVM);
                    }

                    trVM.DueAmount = dueAmount;
                    trVM.AmountGiven = trVM.AmountGiven.Value + trVM.CurrentAmount;

                    if(dueAmount==0 && trVM.AmountGiven==trVM.AmountImposed)
                    {
                        trVM.IsCompleted = true;
                        trVM.CompletedOn = DateTime.Today.Date;
                    }
                    status = _transactionLog.Update(trVM);
                    if(status.IsSuccess)
                    {
                        return RedirectToAction("Search");
                    }
                    else
                    {
                        trVM.DueAmount = originalDueAmt;
                        trVM.AmountGiven = originalAmtGiven;
                        trVM.IsSuccessMessage = false;
                        trVM.HideSaveButton = true;
                        trVM.Message = new MvcHtmlString("Error in updating the entry. Please return to <a href=\"/Transaction/Transaction/Search\">search</a>.");
                        return View(trVM);
                    }
                }
                else
                {
                    trVM.IsSuccessMessage = false;
                    trVM.HideSaveButton = true;
                    trVM.Message = new MvcHtmlString("No transaction is found with this id. Please return to <a href=\"/Transaction/Transaction/Search\">search</a>.");
                    return View(trVM);
                }
            }
            else
            {
                trVM.HideSaveButton = true;
                trVM.IsSuccessMessage = false;
                trVM.Message = new MvcHtmlString("Transaction Id is modified from UI. Please return to <a href=\"/Transaction/Transaction/Search\">search</a>.");
                return View(trVM);
            }
        }
    }
}