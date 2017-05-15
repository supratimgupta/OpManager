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
    public class TransactionMasterController : BaseController
    {
        private ITransactionMasterSvc _trMaster;

        private IConfigSvc _config;

        private ILogSvc _logger;

        private IDropdownRepo _ddlRepo;

        public TransactionMasterController(IDropdownRepo ddlRepo, ITransactionMasterSvc trMaster, IConfigSvc config, ILogSvc logger)
        {
            _ddlRepo = ddlRepo;
            _trMaster = trMaster;
            _logger = logger;
            _config = config;
        }

        // GET: Transaction/TransactionMaster
        public ActionResult TransactionMaster()
        {
            Helpers.UIDropDownRepo uiDDLRepo = new Helpers.UIDropDownRepo(_ddlRepo);
            TransactionMasterVM trMasterVM = new TransactionMasterVM();
            trMasterVM.TransTypeList = uiDDLRepo.getTransactionTypes();
            trMasterVM.IsDiffToList = uiDDLRepo.getTransactionIsDiffTo();
            trMasterVM.FrequencyList = uiDDLRepo.getTransactionFrequencies();
            trMasterVM.MODE = "ADD";
            trMasterVM.Active = true;
            StatusDTO<List<TransactionMasterDTO>> status = _trMaster.GetAllTransactioMasters();

            if(status.IsSuccess && status.ReturnObj!=null && status.ReturnObj.Count>0)
            {
                trMasterVM.SearchList = new List<TransactionMasterVM>();
                TransactionMasterVM trVM = null;
                foreach(TransactionMasterDTO trDto in status.ReturnObj)
                {
                    trVM = new TransactionMasterVM();
                    trVM.DayToRun = trDto.DayToRun;
                    trVM.Frequency = trDto.Frequency;
                    trVM.IsDiffTo = trDto.IsDiffTo;
                    trVM.IsPenalty = trDto.IsPenalty;
                    trVM.TranMasterId = trDto.TranMasterId;
                    trVM.TransactionName = trDto.TransactionName;
                    trVM.TransactionType = trDto.TransactionType;
                    trVM.YearlyDayToRun = trDto.YearlyDayToRun;
                    trVM.Active = trDto.Active;
                    trMasterVM.SearchList.Add(trVM);
                }
            }

            return View(trMasterVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TransactionMaster(TransactionMasterVM trMasterVM)
        {
            StatusDTO<TransactionMasterDTO> status = new StatusDTO<TransactionMasterDTO>();
            if(trMasterVM.MODE=="EDIT")
            {
                status = _trMaster.Update(trMasterVM);
            }
            if(trMasterVM.MODE=="ADD")
            {
                status = _trMaster.Insert(trMasterVM);
            }
            return RedirectToAction("TransactionMaster");
        }
    }
}