using OperationsManager.Areas.PMS.Models;
using OperationsManager.Controllers;
using OpMgr.Common.Contracts.Modules;
using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace OperationsManager.Areas.PMS.Controllers
{
    public class PMSController : BaseController
    {
        private IUserSvc _userSvc;
        //private ILogSvc _logSvc;
        private IDropdownRepo _dropDownRepo;
        private Helpers.UIDropDownRepo _uiddlRepo;

        private OpMgr.Common.Contracts.Modules.IPMSSvc _pmsSvc;

        public PMSController(OpMgr.Common.Contracts.Modules.IPMSSvc pmsSvc)
        {
            _pmsSvc = pmsSvc;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult GoalSheetForAll()
        {
            Models.PMSVM pmsVM = new Models.PMSVM();
            OpMgr.Common.DTOs.EmployeeGoalLogDTO empGoalLog = new OpMgr.Common.DTOs.EmployeeGoalLogDTO();
            empGoalLog.EmployeeAppraisalMaster = new OpMgr.Common.DTOs.EmployeeAppraisalMasterDTO();
            empGoalLog.EmployeeAppraisalMaster.Employee = new OpMgr.Common.DTOs.EmployeeDetailsDTO();
            empGoalLog.EmployeeAppraisalMaster.Employee.EmployeeId = 120;
            empGoalLog.EmployeeAppraisalMaster.Employee.Designation = new OpMgr.Common.DTOs.DesignationDTO();
            empGoalLog.EmployeeAppraisalMaster.Employee.Designation.DesignationId = 15;
            List<OpMgr.Common.DTOs.EmployeeGoalLogDTO> empGoalLogs = _pmsSvc.Select(empGoalLog).ReturnObj;
            pmsVM = pmsVM.GetGoals(empGoalLogs);
            return View(pmsVM);
        }

        [HttpPost]
        public ActionResult GoalSheetForAll(Models.PMSVM pmsVM)
        {
            List<GoalViewModel> lstGoalVM = pmsVM.GoalsAsList;
            foreach(GoalViewModel gVM in lstGoalVM)
            {
                using(TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    try
                    {
                        foreach(EmployeeGoalLogDTO empGoal in gVM.GoalLog)
                        {
                            try
                            {
                                //Insert goal in goal log
                            }
                            catch(Exception exp)
                            {
                                throw exp;
                            }                            
                        }
                    }
                    catch(Exception ex)
                    {
                        throw ex;
                    }
                    ts.Complete();
                }
            }
            return View(pmsVM);
        }
    }
}