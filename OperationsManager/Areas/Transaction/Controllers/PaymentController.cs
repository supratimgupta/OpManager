using OperationsManager.Areas.Transaction.Models;
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
    public class PaymentController : Controller
    {
        private ITransactionLogSvc _transactionLogSvc;

        private ILogSvc _logger;

        private ISessionSvc _sessionSvc;

        public PaymentController(ITransactionLogSvc transactionLogSvc, ILogSvc logger, ISessionSvc sessionSvc)
        {
            _transactionLogSvc = transactionLogSvc;
            _logger = logger;
            _sessionSvc = sessionSvc;
        }

        // GET: Transaction/Payment
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetPayment()
        {
            Models.PaymentVM paymentvm = new PaymentVM();
            return View(paymentvm);
        }

        [HttpPost]
        public ActionResult GetPayment(PaymentVM paymentvm)
        {
            PaymentVM paymentview = null;
            StudentDTO student = null;
            if (paymentvm != null)
            {
                if (!String.IsNullOrEmpty(paymentvm.RegistrationNumber))
                {
                    student = new StudentDTO();
                    student.RegistrationNumber = paymentvm.RegistrationNumber;
                }
                if (!String.IsNullOrEmpty(paymentvm.StaffEmployeeId))
                {
                    student.UserDetails.Employee = new EmployeeDetailsDTO();
                    student.UserDetails.Employee.StaffEmployeeId = paymentvm.StaffEmployeeId;
                }
                StatusDTO<List<TransactionLogDTO>> status = _transactionLogSvc.SelectPayment(student);
                if (status.ReturnObj != null && status.ReturnObj.Count > 0)
                {
                    paymentview = new PaymentVM(); // Instantiating Payment View model
                    paymentview.paymentDetailsList = new List<PaymentVM>(); // instantiating list of Payment

                    if (status.IsSuccess && !status.IsException)
                    {
                        PaymentVM searchItem = null;
                        foreach (TransactionLogDTO tranlog in status.ReturnObj)
                        {
                            if (tranlog != null)
                            {
                                searchItem = new PaymentVM();
                                searchItem.TransactionLogId = tranlog.TransactionLogId;
                                searchItem.HdnTransactionLogId = tranlog.TransactionLogId;
                                searchItem.User = tranlog.User;
                                searchItem.User.FName = tranlog.User.FName;
                                searchItem.User.LName = tranlog.User.LName;
                                if (!String.IsNullOrEmpty(tranlog.User.LName))
                                {
                                    searchItem.Name = searchItem.User.FName + " " + searchItem.User.LName;
                                }
                                searchItem.TransactionDate = tranlog.TransactionDate;
                                searchItem.TransactionDueDate = tranlog.TransactionDueDate;
                                searchItem.TransactionPreviousDueDate = tranlog.TransactionPreviousDueDate;
                                if (tranlog.ParentTransactionLogId != null)
                                {
                                    if (tranlog.ParentTransactionLogId.TransactionLogId > 0)
                                    {
                                        searchItem.ParentTransactionLogId = new TransactionLogDTO();
                                        searchItem.ParentTransactionLogId.TransactionLogId = tranlog.ParentTransactionLogId.TransactionLogId;
                                    }
                                }
                                searchItem.IsCompleted = tranlog.IsCompleted;
                                searchItem.CompletedOn = tranlog.CompletedOn;
                                searchItem.AmountImposed = tranlog.AmountImposed;
                                searchItem.AmountGiven = tranlog.AmountGiven;
                                searchItem.DueAmount = tranlog.DueAmount;
                                searchItem.HasPenalty = tranlog.HasPenalty;
                                searchItem.AdjustedAmount = tranlog.AdjustedAmount;
                                searchItem.IsPrincipalApproved = tranlog.IsPrincipalApproved;
                                if (searchItem.IsPrincipalApproved == 0)
                                {
                                    searchItem.PrincipalApproved = "Pending";
                                }
                                else if (searchItem.IsPrincipalApproved == 1)
                                {
                                    searchItem.PrincipalApproved = "Approved";
                                }
                                else if (searchItem.IsPrincipalApproved == 2)
                                {
                                    searchItem.PrincipalApproved = "Rejected";
                                }
                                else
                                {
                                    searchItem.PrincipalApproved = String.Empty;
                                }
                                if (tranlog.OriginalTransLog != null)
                                {
                                    if (tranlog.OriginalTransLog.TransactionLogId > 0)
                                    {
                                        searchItem.OriginalTransLog = new TransactionLogDTO();
                                        searchItem.OriginalTransLog.TransactionLogId = tranlog.OriginalTransLog.TransactionLogId;
                                    }
                                }
                                searchItem.TransactionRule = new TransactionRuleDTO();
                                searchItem.TransactionRule.TranMaster = new TransactionMasterDTO();
                                searchItem.TransactionRule.TranMaster.TransactionName = tranlog.TransactionRule.TranMaster.TransactionName;

                                paymentview.paymentDetailsList.Add(searchItem);
                                paymentview.IsSearchSuccessful = true;
                            }
                        }
                    }

                }
                else
                {
                    paymentview = paymentvm;
                    paymentvm.IsSearchSuccessful = false;
                }
            }
            return View(paymentview);
        }

        [HttpPost]
        public JsonResult updateRowPayment(TransactionLogDTO tranlogDTO)
        {

            _transactionLogSvc.UpdatePayment(tranlogDTO);
            return Json(new { status = true, data = tranlogDTO }, JsonRequestBehavior.AllowGet);
        }
    }
}
    
    


    