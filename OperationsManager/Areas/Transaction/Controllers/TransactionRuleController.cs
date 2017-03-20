using OperationsManager.Areas.Transaction.Models;
using OpMgr.Common.Contracts;
using OpMgr.Common.Contracts.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationsManager.Areas.Transaction.Controllers
{
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
        public ActionResult TransactionRule()
        {
            Helpers.UIDropDownRepo uiDDLRepo = new Helpers.UIDropDownRepo(_ddlRepo);
            TransactionRuleVM trRuleVM = new TransactionRuleVM();
            trRuleVM.Users = uiDDLRepo.getUserDropDown();
            trRuleVM.Standards = uiDDLRepo.getStandardDropDown();
            trRuleVM.Sections = uiDDLRepo.getSectionDropDown();
            trRuleVM.PenaltyCalcIn = uiDDLRepo.getPenaltyCalcIn();
            trRuleVM.ClassTypes = uiDDLRepo.getClassTypeDropDown();
            trRuleVM.PenaltyTransactionTypes = uiDDLRepo.getTransactionTypes();


            return View();
        }
    }
}