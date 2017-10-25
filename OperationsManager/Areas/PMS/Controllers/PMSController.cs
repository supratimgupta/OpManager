using OperationsManager.Controllers;
using OpMgr.Common.Contracts.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}