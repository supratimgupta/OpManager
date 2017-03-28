using OperationsManager.Areas.Transaction.Models;
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
    //[OpMgrAuth]
    public class TransactionRuleController : Controller
    {
        private ITransactionRuleSvc _trRule;

        private IConfigSvc _config;

        private ILogSvc _logger;

        private IDropdownRepo _ddlRepo;

        private ITransactionMasterSvc _trnsMaster;

        public TransactionRuleController(IDropdownRepo ddlRepo, ITransactionRuleSvc trRule, IConfigSvc config, ILogSvc logger, ITransactionMasterSvc trnsMaster)
        {
            _ddlRepo = ddlRepo;
            _trRule = trRule;
            _logger = logger;
            _config = config;
            _trnsMaster = trnsMaster;
        }

        // GET: Transaction/TransactionRule
        [OpMgrAuth]
        public ActionResult TransactionRule(string mode, int? id)
        {
            Helpers.UIDropDownRepo uiDDLRepo = new Helpers.UIDropDownRepo(_ddlRepo);
            TransactionRuleVM trRuleVM = new TransactionRuleVM();
            if(string.Equals(mode, "EDIT", StringComparison.OrdinalIgnoreCase))
            {
                StatusDTO<TransactionRuleDTO> trRule = _trRule.Select(id.Value);
                if(trRule!=null)
                {
                    trRuleVM.Active = trRule.ReturnObj.Active;
                    trRuleVM.ActualAmount = trRule.ReturnObj.ActualAmount;
                    trRuleVM.ClassType = trRule.ReturnObj.ClassType;
                    trRuleVM.DueDateIncreasesBy = trRule.ReturnObj.DueDateIncreasesBy;
                    trRuleVM.FirstDueAfterDays = trRule.ReturnObj.FirstDueAfterDays;
                    trRuleVM.IsdifferentTo = trRule.ReturnObj.IsdifferentTo;
                    trRuleVM.PenaltyAmount = trRule.ReturnObj.PenaltyAmount;
                    trRuleVM.PenaltyCalculatedIn = trRule.ReturnObj.PenaltyCalculatedIn;
                    trRuleVM.PenaltyTransactionRule = trRule.ReturnObj.PenaltyTransactionRule;
                    trRuleVM.PenaltyTransactionType = trRule.ReturnObj.PenaltyTransactionType;
                    trRuleVM.RuleName = trRule.ReturnObj.RuleName;
                    trRuleVM.Section = trRule.ReturnObj.Section;
                    trRuleVM.Standard = trRule.ReturnObj.Standard;
                    trRuleVM.TranMaster = trRule.ReturnObj.TranMaster;
                    trRuleVM.TranRuleId = trRule.ReturnObj.TranRuleId;
                    trRuleVM.UserDTO = trRule.ReturnObj.UserDTO;

                    trRuleVM.MODE = "EDIT";

                    if(trRuleVM.TranMaster!=null)
                    {
                        trRuleVM.SelectedFrequency = _trnsMaster.GetFreq(trRuleVM.TranMaster.TranMasterId);
                    }

                    trRuleVM.TransactionMasters = uiDDLRepo.getTransactionMasters(trRuleVM.SelectedFrequency);
                }
                else
                {
                    trRuleVM.MODE = "ADD";
                    trRuleVM.Active = true;
                    trRuleVM.TransactionMasters = new SelectList(new List<TransactionMasterDTO>(), "TranMasterId", "TransactionName");
                }
            }
            else
            {
                trRuleVM.MODE = "ADD";
                trRuleVM.Active = true;
                trRuleVM.TransactionMasters = new SelectList(new List<TransactionMasterDTO>(), "TranMasterId", "TransactionName");
            }
            trRuleVM.TransactionFrequencies = uiDDLRepo.getTransactionFrequencies();
            trRuleVM.Users = uiDDLRepo.getUserDropDown();
            trRuleVM.Standards = uiDDLRepo.getStandardDropDown();
            trRuleVM.Sections = uiDDLRepo.getSectionDropDown();
            trRuleVM.PenaltyCalcIn = uiDDLRepo.getCalcType();
            trRuleVM.ClassTypes = uiDDLRepo.getClassTypeDropDown();
            trRuleVM.PenaltyTransactionTypes = uiDDLRepo.getTransactionTypes();
            
            trRuleVM.PenaltyTransactionRules = uiDDLRepo.getTransactionRules();
            trRuleVM.ErrorMsg = string.Empty;
            trRuleVM.SuccessMsg = string.Empty;
            trRuleVM.IsPostBack = "FALSE";
            return View(trRuleVM);
        }

        [HttpPost]
        public JsonResult GetTransactionMasters(TransactionMasterDTO trnsMaster)
        {
            List<TransactionMasterDTO> transactionMasters = _ddlRepo.GetTransactionMasters(trnsMaster.Frequency);
            return Json(new { status = true, data = transactionMasters }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetIsDifferentTo(TransactionMasterDTO trnsMaster)
        {
            string isDiffTo = _trnsMaster.GetIsDifferentTo(trnsMaster.TranMasterId);
            return Json(new { status = true, diffTo = isDiffTo }, JsonRequestBehavior.AllowGet);
        }

        private bool Validate(TransactionRuleVM trRuleVM, out string message, out string diffTo)
        {
            diffTo = string.Empty;
            message = string.Empty;
            if (trRuleVM.TranMaster == null && trRuleVM.TranMaster.TranMasterId <= 0)
            {
                message = message + "Please select a transaction name.";
                return false;
            }
            diffTo = _trnsMaster.GetIsDifferentTo(trRuleVM.TranMaster.TranMasterId);
            if(diffTo=="USER")
            {
                if(trRuleVM.UserDTO==null || trRuleVM.UserDTO.UserMasterId<=0)
                {
                    message = message + " Please select a user for this transaction name.";
                }
            }
            if (diffTo == "CLASS-TYPE")
            {
                if (trRuleVM.ClassType == null || trRuleVM.ClassType.ClassTypeId <= 0)
                {
                    message = message + " Please select a class type for this transaction name.";
                }
            }
            if (diffTo == "STANDARD")
            {
                if (trRuleVM.Standard == null || trRuleVM.Standard.StandardId <= 0)
                {
                    message = message + " Please select a standard for this transaction name.";
                }
            }
            if (diffTo == "SECTION")
            {
                if (trRuleVM.Standard == null || trRuleVM.Standard.StandardId <= 0 || trRuleVM.Section==null || trRuleVM.Section.SectionId<=0)
                {
                    message = message + " Please select standard and section for this transaction name.";
                }
            }
            if(trRuleVM.ActualAmount==null || trRuleVM.ActualAmount<=0)
            {
                message = message + " Please enter a valid amount for this rule.";
            }
            if(trRuleVM.FirstDueAfterDays==null && (string.IsNullOrEmpty(trRuleVM.PenaltyCalculatedIn) || trRuleVM.PenaltyCalculatedIn == "-1") 
                && trRuleVM.PenaltyAmount==null && trRuleVM.DueDateIncreasesBy==null)
            {

            }
            else if (trRuleVM.FirstDueAfterDays != null && (!string.IsNullOrEmpty(trRuleVM.PenaltyCalculatedIn) && trRuleVM.PenaltyCalculatedIn != "-1")
                && trRuleVM.PenaltyAmount != null && trRuleVM.DueDateIncreasesBy != null)
            {

            }
            else
            {
                message = message + " Please enter first due date, penalty calculated in, penalty amount, due date increases by values.";    
            }
            if(!string.IsNullOrEmpty(message))
            {
                return false;
            }
            return true;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TransactionRule(TransactionRuleVM trRuleVM)
        {
            Helpers.UIDropDownRepo uiDDLRepo = new Helpers.UIDropDownRepo(_ddlRepo);
            string message = string.Empty;
            string diffTo = string.Empty;
            if (Validate(trRuleVM, out message, out diffTo))
            {
                //string diffTo = _trnsMaster.GetIsDifferentTo(trRuleVM.TranMaster.TranMasterId);

                if(!_trRule.IsDuplicate(trRuleVM.TranMaster.TranMasterId, trRuleVM.Standard==null?-1: trRuleVM.Standard.StandardId, trRuleVM.Section==null?-1: trRuleVM.Section.SectionId, trRuleVM.ClassType==null?-1: trRuleVM.ClassType.ClassTypeId, trRuleVM.UserDTO==null?-1: trRuleVM.UserDTO.UserMasterId, diffTo, trRuleVM.MODE, trRuleVM.TranRuleId))
                {
                    if (string.Equals(trRuleVM.MODE, "EDIT"))
                    {
                        _trRule.Update(trRuleVM);
                        return RedirectToAction("Search");
                    }
                    _trRule.Insert(trRuleVM);
                    ModelState.Clear();

                    trRuleVM.MODE = "ADD";
                    trRuleVM.Active = true;

                    trRuleVM.SuccessMsg = "Rule added successfully.";
                    trRuleVM.ErrorMsg = string.Empty;
                }
                else
                {
                    trRuleVM.ErrorMsg = "Rule is not unique for this transaction name.";
                    trRuleVM.SuccessMsg = string.Empty;
                }
            }
            else
            {
                trRuleVM.ErrorMsg = message;
                trRuleVM.SuccessMsg = string.Empty;
            }
            trRuleVM.TransactionFrequencies = uiDDLRepo.getTransactionFrequencies();
            trRuleVM.Users = uiDDLRepo.getUserDropDown();
            trRuleVM.Standards = uiDDLRepo.getStandardDropDown();
            trRuleVM.Sections = uiDDLRepo.getSectionDropDown();
            trRuleVM.PenaltyCalcIn = uiDDLRepo.getCalcType();
            trRuleVM.ClassTypes = uiDDLRepo.getClassTypeDropDown();
            trRuleVM.PenaltyTransactionTypes = uiDDLRepo.getTransactionTypes();
            trRuleVM.TransactionMasters = uiDDLRepo.getTransactionMasters();
            trRuleVM.PenaltyTransactionRules = uiDDLRepo.getTransactionRules();
            trRuleVM.IsPostBack = "TRUE";


            return View(trRuleVM);
        }


        public ActionResult Search()
        {
            List<TransactionRuleDTO> trRules = _trRule.GetAllRulesWithInactive();
            return View(trRules);
        }
    }
}