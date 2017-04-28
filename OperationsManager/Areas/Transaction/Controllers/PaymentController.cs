using OperationsManager.Areas.Transaction.Models;
using OpMgr.Common.Contracts;
using OpMgr.Common.Contracts.Modules;
using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace OperationsManager.Areas.Transaction.Controllers
{
    [Attributes.OpMgrHandleError]
    //[Attributes.OpMgrAuth]
    public class PaymentController : Controller
    {
        private ITransactionLogSvc _transactionLogSvc;

        private ILogSvc _logger;

        private ISessionSvc _sessionSvc;

        private IDropdownRepo _ddlRepo;

        private ITransactionLogPaymentSvc _paymentSvc;

        public PaymentController(ITransactionLogSvc transactionLogSvc, ILogSvc logger, ISessionSvc sessionSvc, IDropdownRepo ddlRepo, ITransactionLogPaymentSvc paymentSvc)
        {
            _transactionLogSvc = transactionLogSvc;
            _logger = logger;
            _sessionSvc = sessionSvc;
            _ddlRepo = ddlRepo;
            _paymentSvc = paymentSvc;
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
            paymentvm.TransferMode = "-1";
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
                    student = new StudentDTO();
                    student.UserDetails = new UserMasterDTO();
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

                                searchItem.TransferMode = "-1";

                                paymentview.paymentDetailsList.Add(searchItem);
                                paymentview.IsSearchSuccessful = true;
                            }
                        }
                    }
                    Helpers.UIDropDownRepo uiDDLRepo = new Helpers.UIDropDownRepo(_ddlRepo);
                    paymentview.PaymentModeList = uiDDLRepo.getTransferModeDropdown();
                }
                else
                {
                    paymentview = paymentvm;
                    paymentvm.IsSearchSuccessful = false;
                }
            }
            paymentview.TransferMode = "-1";
            return View(paymentview);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult resendRequest(Models.TransactionViewModel tranlogDTO)
        {
            if(_transactionLogSvc.ResendRequest(tranlogDTO.TransactionLogId))
            {
                return Json(new { status = true, message = "Successful" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = false, message = "Failed to resend request" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult updateRowPayment(Models.TransactionViewModel tranlogDTO)
        {
            if(tranlogDTO.AmountGiven==null)
            {
                tranlogDTO.AmountGiven = 0;
            }
            if(tranlogDTO.AdjustedAmount==null)
            {
                tranlogDTO.AdjustedAmount = 0;
            }
            if(tranlogDTO.DueAmount==null)
            {
                tranlogDTO.DueAmount = 0;
            }
            tranlogDTO.HasPartialPayment = true;
            tranlogDTO.PaymentDate = DateTime.Now;
            if(tranlogDTO.AmountImposed==(tranlogDTO.CurrentAmount + tranlogDTO.CurrentAdjusting))
            {
                tranlogDTO.HasPartialPayment = false;
            }
            if(!string.Equals(tranlogDTO.TransferMode,"CHQ"))
            {
                tranlogDTO.PaymentChequeNo = string.Empty;
            }
            tranlogDTO.AmountGiven = tranlogDTO.AmountGiven + tranlogDTO.CurrentAmount;
            tranlogDTO.AdjustedAmount = tranlogDTO.AdjustedAmount + tranlogDTO.CurrentAdjusting;
            tranlogDTO.DueAmount = tranlogDTO.DueAmount - (tranlogDTO.CurrentAmount + tranlogDTO.CurrentAdjusting);
            //tranlogDTO.IsPrincipalApproved = null;
            bool principalApprovedChanged = false;
            int? oldPrincipalApproved = null;
            if (tranlogDTO.CurrentAdjusting>0)
            {
                tranlogDTO.IsPrincipalApproved = 0;
                oldPrincipalApproved = 0;
                principalApprovedChanged = true;
            }
            tranlogDTO.IsCompleted = false;
            if((tranlogDTO.IsPrincipalApproved==null || tranlogDTO.IsPrincipalApproved==1) && tranlogDTO.DueAmount==0)
            {
                tranlogDTO.IsCompleted = true;
            }
            
            if(!principalApprovedChanged)
            {
                oldPrincipalApproved = tranlogDTO.IsPrincipalApproved;
                tranlogDTO.IsPrincipalApproved = null;
            }
            if(tranlogDTO.DueAmount>=0)
            {
                StatusDTO<TransactionLogDTO> status = _transactionLogSvc.UpdatePayment(tranlogDTO);
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    if (status.IsSuccess)
                    {
                        tranlogDTO.IsPrincipalApproved = oldPrincipalApproved;

                        if (tranlogDTO.HasPartialPayment.Value)
                        {
                            TransactionLogPaymentsDTO payment = new TransactionLogPaymentsDTO();
                            payment.CurrentAmount = tranlogDTO.CurrentAmount;
                            payment.PaymentChequeNo = tranlogDTO.PaymentChequeNo;
                            payment.PaymentDate = tranlogDTO.PaymentDate;
                            payment.PaymentMode = tranlogDTO.TransferMode;
                            payment.TransactionLog = tranlogDTO;
                            payment.CurrentAdjustingAmount = tranlogDTO.CurrentAdjusting;
                            if(_paymentSvc.Insert(payment).IsSuccess)
                            {
                                ts.Complete();
                            }
                        }
                        return Json(new { status = true, data = tranlogDTO, message = "Successful" }, JsonRequestBehavior.AllowGet);
                    }
                }  
                return Json(new { status = false, data = tranlogDTO, message = "Payment failed." }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = false, data = tranlogDTO, message="Paid amount is greater than due amount" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult payAllTransactions(Models.TransactionViewModel transactions)
        {
            double currentTotalPay = transactions.CurrentAmount;
            double currentTotalAdjusting = transactions.CurrentAdjusting;
            double currentTotalDue = transactions.DueAmount.Value;
            if(currentTotalPay+currentTotalAdjusting>0)
            {
                if(currentTotalPay+currentTotalAdjusting <= currentTotalDue)
                {
                    using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                    {
                        try
                        {
                            foreach (Models.TransactionViewModel tranlogDTO in transactions.paymentDetailsList)
                            {
                                tranlogDTO.TransferMode = transactions.TransferMode;
                                tranlogDTO.PaymentChequeNo = transactions.PaymentChequeNo;

                                if(tranlogDTO.DueAmount!=null && tranlogDTO.DueAmount.Value>0)
                                {
                                    if (tranlogDTO.AmountGiven == null)
                                    {
                                        tranlogDTO.AmountGiven = 0;
                                    }
                                    if (tranlogDTO.AdjustedAmount == null)
                                    {
                                        tranlogDTO.AdjustedAmount = 0;
                                    }
                                    if (tranlogDTO.DueAmount == null)
                                    {
                                        tranlogDTO.DueAmount = 0;
                                    }

                                    double currentGivenAmt = ((tranlogDTO.DueAmount.Value > currentTotalPay) ? currentTotalPay : tranlogDTO.DueAmount.Value);
                                    tranlogDTO.AmountGiven = tranlogDTO.AmountGiven + currentGivenAmt;
                                    tranlogDTO.DueAmount = tranlogDTO.DueAmount.Value - currentGivenAmt;

                                    tranlogDTO.PaymentDate = DateTime.Now;
                                    
                                    if (!string.Equals(tranlogDTO.TransferMode, "CHQ"))
                                    {
                                        tranlogDTO.PaymentChequeNo = string.Empty;
                                    }

                                    double currentAdjustedAmt = 0.0;
                                    if (tranlogDTO.DueAmount.Value>0)
                                    {
                                        currentAdjustedAmt = (tranlogDTO.DueAmount.Value > currentTotalAdjusting) ? currentTotalAdjusting : tranlogDTO.DueAmount.Value;
                                        tranlogDTO.AdjustedAmount = tranlogDTO.AdjustedAmount + currentAdjustedAmt;
                                        tranlogDTO.DueAmount = tranlogDTO.DueAmount.Value - currentAdjustedAmt;
                                    }

                                    tranlogDTO.HasPartialPayment = true;

                                    if (tranlogDTO.AmountImposed == (currentGivenAmt + currentAdjustedAmt))
                                    {
                                        tranlogDTO.HasPartialPayment = false;
                                    }

                                    bool principalApprovedChanged = false;
                                    int? oldPrincipalApproved = null;
                                    if (currentAdjustedAmt > 0)
                                    {
                                        tranlogDTO.IsPrincipalApproved = 0;
                                        oldPrincipalApproved = 0;
                                        principalApprovedChanged = true;
                                    }
                                    tranlogDTO.IsCompleted = false;
                                    if ((tranlogDTO.IsPrincipalApproved == null || tranlogDTO.IsPrincipalApproved == 1) && tranlogDTO.DueAmount == 0)
                                    {
                                        tranlogDTO.IsCompleted = true;
                                    }

                                    if (!principalApprovedChanged)
                                    {
                                        oldPrincipalApproved = tranlogDTO.IsPrincipalApproved;
                                        tranlogDTO.IsPrincipalApproved = null;
                                    }
                                    if (tranlogDTO.DueAmount >= 0)
                                    {
                                        StatusDTO<TransactionLogDTO> status = _transactionLogSvc.UpdatePayment(tranlogDTO);
                                        if (!status.IsSuccess)
                                        {
                                            throw new Exception("Error encountered in one transaction. Rolled back.");
                                        }
                                        else
                                        {
                                            if(tranlogDTO.HasPartialPayment.Value)
                                            {
                                                TransactionLogPaymentsDTO payment = new TransactionLogPaymentsDTO();
                                                payment.CurrentAmount = currentGivenAmt;
                                                payment.CurrentAdjustingAmount = currentAdjustedAmt;
                                                payment.PaymentChequeNo = tranlogDTO.PaymentChequeNo;
                                                payment.PaymentDate = tranlogDTO.PaymentDate;
                                                payment.PaymentMode = tranlogDTO.TransferMode;
                                                payment.TransactionLog = tranlogDTO;
                                                if (!_paymentSvc.Insert(payment).IsSuccess)
                                                {
                                                    throw new Exception("Payment sub details not added");
                                                }
                                            }
                                        }
                                    }

                                    currentTotalPay = currentTotalPay - currentGivenAmt;
                                    currentTotalAdjusting = currentTotalAdjusting - currentAdjustedAmt;
                                }
                            }
                        }
                        catch (Exception exp)
                        {
                            return Json(new { status = false, message = exp.Message }, JsonRequestBehavior.AllowGet);
                        }
                        ts.Complete();
                        return Json(new { status = true, message = "Transaction completed successfully." }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { status = false, message = "Total current and adjusted amount must be less than or equal to total due." }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = false, message = "No amount is given" }, JsonRequestBehavior.AllowGet);
        }
    }
}
    
    


    