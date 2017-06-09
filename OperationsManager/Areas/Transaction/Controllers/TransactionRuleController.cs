using OperationsManager.Areas.Transaction.Models;
using OperationsManager.Attributes;
using OperationsManager.Controllers;
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
    public class TransactionRuleController : BaseController
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
            trRuleVM.IsPostBack = "FALSE";
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
                    trRuleVM.TransactionMasters = new SelectList(new List<TransactionMasterDTO>(), "TranMasterId", "TransactionName");

                    if (TempData.Peek("RuleSearchCriteria")!=null)
                    {
                        trRuleVM = (TransactionRuleVM)TempData.Peek("RuleSearchCriteria");
                        trRuleVM.TransactionMasters = new SelectList(_ddlRepo.GetTransactionMasters(trRuleVM.SelectedFrequency), "TranMasterId", "TransactionName");
                        trRuleVM.IsPostBack = "TRUE";
                    }

                    trRuleVM.MODE = "ADD";
                    trRuleVM.Active = true;
                    
                }
            }
            else
            {
                trRuleVM.TransactionMasters = new SelectList(new List<TransactionMasterDTO>(), "TranMasterId", "TransactionName");

                if (TempData.Peek("RuleSearchCriteria") != null)
                {
                    trRuleVM = (TransactionRuleVM)TempData.Peek("RuleSearchCriteria");
                    trRuleVM.TransactionMasters = new SelectList(_ddlRepo.GetTransactionMasters(trRuleVM.SelectedFrequency), "TranMasterId", "TransactionName");
                    trRuleVM.IsPostBack = "TRUE";
                }

                trRuleVM.MODE = "ADD";
                trRuleVM.Active = true;
                
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
            
            return View(trRuleVM);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetTransactionMasters(TransactionMasterDTO trnsMaster)
        {
            List<TransactionMasterDTO> transactionMasters = _ddlRepo.GetTransactionMasters(trnsMaster.Frequency);
            return Json(new { status = true, data = transactionMasters }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
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
                        return RedirectToAction("SearchRules");
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


            return RedirectToAction("SearchRules");
        }

        [OpMgrAuth]
        public ActionResult Search()
        {
            List<TransactionRuleDTO> trRules = _trRule.GetAllRulesWithInactive();
            return View(trRules);
        }

        public ActionResult SearchRules()
        {
            Helpers.UIDropDownRepo uiDDLRepo = new Helpers.UIDropDownRepo(_ddlRepo);

            TransactionRuleVM tranRule = new TransactionRuleVM();

            tranRule.TransactionMasters = new SelectList(new List<TransactionMasterDTO>(), "TranMasterId", "TransactionName");

            tranRule.IsPostBack = "FALSE";

            if (TempData.Peek("RuleSearchCriteria")!=null)
            {
                tranRule = (TransactionRuleVM)TempData.Peek("RuleSearchCriteria");
                this.AddSearchResult(ref tranRule);
                tranRule.TransactionMasters = new SelectList(_ddlRepo.GetTransactionMasters(tranRule.SelectedFrequency), "TranMasterId", "TransactionName");

                tranRule.IsPostBack = "TRUE";
            }
            else
            {
                tranRule.SearchedResult = new List<TransactionRuleVM>();
            }

            tranRule.TransactionFrequencies = uiDDLRepo.getTransactionFrequencies();
            tranRule.Users = uiDDLRepo.getUserDropDown();
            tranRule.Standards = uiDDLRepo.getStandardDropDown();
            tranRule.Sections = uiDDLRepo.getSectionDropDown();
            tranRule.ClassTypes = uiDDLRepo.getClassTypeDropDown();
            

            

            tranRule.SuccessMsg = "";
            tranRule.ErrorMsg = "";
            return View(tranRule);
        }


        private void AddSearchResult(ref TransactionRuleVM tranRule)
        {
            List<TransactionRuleDTO> searchResult = null;

            if (tranRule.TranMaster != null)
            {
                switch(tranRule.IsdifferentTo.ToUpper())
                {
                    case "NONE":
                        if (tranRule.TranMaster.TranMasterId > 0)
                        {
                            searchResult = _trRule.GetNoneLevelRules(tranRule.TranMaster.TranMasterId);
                        }
                        else
                        {
                            searchResult = _trRule.GetNoneLevelRules();
                        }
                        break;
                    case "CLASS-TYPE":
                        if (tranRule.ClassType != null && tranRule.ClassType.ClassTypeId > 0)
                        {
                            searchResult = _trRule.GetClassTypeLevelRules(tranRule.TranMaster.TranMasterId, tranRule.ClassType.ClassTypeId);
                        }
                        else
                        {
                            searchResult = _trRule.GetClassTypeLevelRules(tranRule.TranMaster.TranMasterId);
                        }
                        break;
                    case "STANDARD":
                        if (tranRule.Standard != null && tranRule.Standard.StandardId > 0)
                        {
                            searchResult = _trRule.GetStandardLevelRules(tranRule.TranMaster.TranMasterId, tranRule.Standard.StandardId);
                        }
                        else
                        {
                            searchResult = _trRule.GetStandardLevelRules(tranRule.TranMaster.TranMasterId);
                        }
                        break;
                    case "SECTION":
                        if (tranRule.Section != null && tranRule.Section.SectionId > 0)
                        {
                            searchResult = _trRule.GetStandardSectionLevelRules(tranRule.TranMaster.TranMasterId, tranRule.Standard.StandardId, tranRule.Section.SectionId);
                        }
                        else
                        {
                            searchResult = _trRule.GetStandardSectionLevelRules(tranRule.TranMaster.TranMasterId, tranRule.Standard.StandardId);
                        }
                        break;
                    case "USER":
                        searchResult = _trRule.GetUserLevelRules(tranRule.TranMaster.TranMasterId, tranRule.UserDTO.UserMasterId);
                        break;
                }
                if (searchResult != null && searchResult.Count > 0)
                {
                    tranRule.SearchedResult = new List<TransactionRuleVM>();
                    TransactionRuleVM item = null;
                    foreach (TransactionRuleDTO trDto in searchResult)
                    {
                        item = new TransactionRuleVM();
                        item.TranRuleId = trDto.TranRuleId;
                        item.TranMaster = trDto.TranMaster;
                        item.RuleName = trDto.RuleName;
                        item.ClassType = trDto.ClassType;
                        item.UserDTO = trDto.UserDTO;
                        item.Standard = trDto.Standard;
                        item.Section = trDto.Section;
                        item.ActualAmount = trDto.ActualAmount;
                        tranRule.SearchedResult.Add(item);
                    }
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchRules(TransactionRuleVM tranRule)
        {
            string validMsg = string.Empty;
            Helpers.UIDropDownRepo uiDDLRepo = new Helpers.UIDropDownRepo(_ddlRepo);
            tranRule.IsPostBack = "TRUE";
            if (ValidateSearch(tranRule, out validMsg))
            {
                //Need to change as it will also store the search result and the memory usage will be high
                TempData["RuleSearchCriteria"] = tranRule;
                
                tranRule.SearchedResult = new List<TransactionRuleVM>();

                this.AddSearchResult(ref tranRule);
                //tranRule.IsPostBack = "TRUE";
            }
            else
            {
                tranRule.ErrorMsg = validMsg;
            }

            tranRule.TransactionFrequencies = uiDDLRepo.getTransactionFrequencies();
            tranRule.Users = uiDDLRepo.getUserDropDown();
            tranRule.Standards = uiDDLRepo.getStandardDropDown();
            tranRule.Sections = uiDDLRepo.getSectionDropDown();
            tranRule.ClassTypes = uiDDLRepo.getClassTypeDropDown();
            tranRule.TransactionMasters = new SelectList(_ddlRepo.GetTransactionMasters(tranRule.SelectedFrequency), "TranMasterId", "TransactionName");

            return View(tranRule);
        }

        private bool ValidateSearch(TransactionRuleVM trRule, out string message)
        {
            message = string.Empty;
            if(string.IsNullOrEmpty(trRule.IsdifferentTo) || string.IsNullOrEmpty(trRule.SelectedFrequency) || trRule.TranMaster==null || trRule.TranMaster.TranMasterId<=0)
            {
                message = "Please select a frequency and a fees name to proceed.";
                return false;
            }

            if(string.Equals(trRule.IsdifferentTo,"USER"))
            {
                if(trRule.UserDTO==null || trRule.UserDTO.UserMasterId<=0)
                {
                    message = "Please select a user.";
                }
            }

            if(string.Equals(trRule.IsdifferentTo, "SECTION"))
            {
                if(trRule.Standard==null || trRule.Standard.StandardId<=0)
                {
                    message = "Please select a standard.";
                }
            }

            if(!string.IsNullOrEmpty(message))
            {
                return false;
            }
            return true;
        }


        public ActionResult PrepareForAdd(string isDiffTo, int? userId, int? classTypeId, int? standardId, int? sectionId)
        {
            if(TempData.Peek("RuleSearchCriteria")!=null)
            {
                TransactionRuleVM tranRule = (TransactionRuleVM)TempData.Peek("RuleSearchCriteria");
                switch (isDiffTo)
                {
                    case "USER":
                        tranRule.UserDTO = new UserMasterDTO();
                        tranRule.UserDTO.UserMasterId = userId.Value;
                        break;
                    case "CLASS-TYPE":
                        tranRule.ClassType = new ClassTypeDTO();
                        tranRule.ClassType.ClassTypeId = classTypeId.Value;
                        break;
                    case "STANDARD":
                        tranRule.Standard = new StandardDTO();
                        tranRule.Standard.StandardId = standardId.Value;
                        break;
                    case "SECTION":
                        tranRule.Section = new SectionDTO();
                        tranRule.Section.SectionId = sectionId.Value;
                        tranRule.Standard = new StandardDTO();
                        tranRule.Standard.StandardId = standardId.Value;
                        break;
                }
                TempData["RuleSearchCriteria"] = tranRule;
            }
            return RedirectToAction("TransactionRule");
        }
    }
}