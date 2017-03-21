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
    [OpMgrAuth]
    public class TransactionRuleController : Controller
    {
        private ITransactionRuleSvc _trRule;

        private IConfigSvc _config;

        private ILogSvc _logger;

        private IDropdownRepo _ddlRepo;

        public TransactionRuleController(IDropdownRepo ddlRepo, ITransactionRuleSvc trRule, IConfigSvc config, ILogSvc logger)
        {
            _ddlRepo = ddlRepo;
            _trRule = trRule;
            _logger = logger;
            _config = config;
        }

        // GET: Transaction/TransactionRule
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
                }
                else
                {
                    trRuleVM.MODE = "ADD";
                    trRuleVM.Active = true;
                }
            }
            else
            {
                trRuleVM.MODE = "ADD";
                trRuleVM.Active = true;
            }
            trRuleVM.Users = uiDDLRepo.getUserDropDown();
            trRuleVM.Standards = uiDDLRepo.getStandardDropDown();
            trRuleVM.Sections = uiDDLRepo.getSectionDropDown();
            trRuleVM.PenaltyCalcIn = uiDDLRepo.getCalcType();
            trRuleVM.ClassTypes = uiDDLRepo.getClassTypeDropDown();
            trRuleVM.PenaltyTransactionTypes = uiDDLRepo.getTransactionTypes();
            trRuleVM.TransactionMasters = uiDDLRepo.getTransactionMasters();
            trRuleVM.PenaltyTransactionRules = uiDDLRepo.getTransactionRules();
            trRuleVM.SuccessMsg = string.Empty;
            return View(trRuleVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TransactionRule(TransactionRuleVM trRuleVM)
        {
            Helpers.UIDropDownRepo uiDDLRepo = new Helpers.UIDropDownRepo(_ddlRepo);
            if(string.Equals(trRuleVM.MODE, "EDIT"))
            {
                _trRule.Update(trRuleVM);
                return RedirectToAction("Search");
            }
            _trRule.Insert(trRuleVM);
            ModelState.Clear();

            trRuleVM.MODE = "ADD";
            trRuleVM.Active = true;

            trRuleVM.Users = uiDDLRepo.getUserDropDown();
            trRuleVM.Standards = uiDDLRepo.getStandardDropDown();
            trRuleVM.Sections = uiDDLRepo.getSectionDropDown();
            trRuleVM.PenaltyCalcIn = uiDDLRepo.getCalcType();
            trRuleVM.ClassTypes = uiDDLRepo.getClassTypeDropDown();
            trRuleVM.PenaltyTransactionTypes = uiDDLRepo.getTransactionTypes();
            trRuleVM.TransactionMasters = uiDDLRepo.getTransactionMasters();
            trRuleVM.PenaltyTransactionRules = uiDDLRepo.getTransactionRules();

            trRuleVM.SuccessMsg = "Rule added successfully.";

            return View(trRuleVM);
        }


        public ActionResult Search()
        {
            List<TransactionRuleDTO> trRules = _trRule.GetAllRulesWithInactive();
            return View(trRules);
        }
    }
}