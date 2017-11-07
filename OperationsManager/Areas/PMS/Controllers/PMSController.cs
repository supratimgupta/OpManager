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
        public ActionResult GoalSheetForAll(int? apprMasterId)
        {
            Models.PMSVM pmsVM = new Models.PMSVM();
            OpMgr.Common.DTOs.EmployeeGoalLogDTO empGoalLog = new OpMgr.Common.DTOs.EmployeeGoalLogDTO();
            empGoalLog.EmployeeAppraisalMaster = new OpMgr.Common.DTOs.EmployeeAppraisalMasterDTO();
            empGoalLog.EmployeeAppraisalMaster.Employee = new OpMgr.Common.DTOs.EmployeeDetailsDTO();
            empGoalLog.EmployeeAppraisalMaster.Employee.EmployeeId = 120;
            empGoalLog.EmployeeAppraisalMaster.Employee.Designation = new OpMgr.Common.DTOs.DesignationDTO();
            empGoalLog.EmployeeAppraisalMaster.Employee.Designation.DesignationId = 15;
            List<OpMgr.Common.DTOs.EmployeeGoalLogDTO> empGoalLogs = _pmsSvc.Select(empGoalLog).ReturnObj;
            pmsVM.EmployeeAppraisalMasterId = empGoalLogs[0].EmployeeAppraisalMaster.EmployeeAppraisalMasterId;
            pmsVM = pmsVM.GetGoals(empGoalLogs);

            // to get data in first grid
            pmsVM.FullName = empGoalLogs[0].EmployeeAppraisalMaster.Employee.UserDetails.FName + " " + empGoalLogs[0].EmployeeAppraisalMaster.Employee.UserDetails.LName;
            pmsVM.Employee = new OpMgr.Common.DTOs.EmployeeDetailsDTO();
            pmsVM.Employee.EducationalQualification = empGoalLogs[0].EmployeeAppraisalMaster.Employee.EducationalQualification;
            pmsVM.Employee.Designation = new OpMgr.Common.DTOs.DesignationDTO();
            pmsVM.Employee.Designation.DesignationDescription = empGoalLogs[0].EmployeeAppraisalMaster.Employee.Designation.DesignationDescription;
            pmsVM.Employee.UserDetails = new OpMgr.Common.DTOs.UserMasterDTO();
            pmsVM.Employee.UserDetails.Location = new OpMgr.Common.DTOs.LocationDTO();
            pmsVM.Employee.UserDetails.Location.LocationDescription = empGoalLogs[0].EmployeeAppraisalMaster.Employee.UserDetails.Location.LocationDescription;

            bool isSelf = apprMasterId == null;

            pmsVM.MODE = this.SetupMode(pmsVM.EmployeeAppraisalMasterId, isSelf);

            return View(pmsVM);
        }

        private string SetupMode(int apprMasterId, bool isSelf=false)
        {
            return "STAFF_FORM_FILLUP";
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetRatingLevel(int ratingPercent)
        {
            string ratingLevel = _pmsSvc.getSelfRating(ratingPercent).ReturnObj.SelfRating;
            return Json(new { status = true, data = ratingLevel }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult GoalSheetForAll(Models.PMSVM pmsVM)
        {
            List<GoalViewModel> lstGoalVM = pmsVM.GoalsAsList;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                foreach(GoalViewModel gVM in lstGoalVM)
                {
                
                    try
                    {
                        foreach(EmployeeGoalLogDTO empGoal in gVM.GoalLog)
                        {
                            try
                            {
                                empGoal.EmployeeAppraisalMaster = new EmployeeAppraisalMasterDTO();
                                empGoal.EmployeeAppraisalMaster.EmployeeAppraisalMasterId = pmsVM.EmployeeAppraisalMasterId;
                                //Insert goal in goal log
                                if (string.Equals(pmsVM.SAVE_MODE, "STAFF_FORM_SAVE") || string.Equals(pmsVM.SAVE_MODE, "STAFF_FORM_SUBMIT"))
                                {
                                    if (empGoal.EmployeeGoalLogId == -1)
                                    {
                                        _pmsSvc.Insert(empGoal);
                                    }
                                    else
                                    {
                                        if (string.Equals(empGoal.NeedsUpdate, "Y"))
                                        {
                                            _pmsSvc.Update(empGoal);
                                        }
                                    }
                                }                                
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
                }
                if (string.Equals(pmsVM.SAVE_MODE, "STAFF_FORM_SUBMIT"))
                {
                    int currentStatus = _pmsSvc.GetCurrentStatus(pmsVM.EmployeeAppraisalMasterId);
                    if (_pmsSvc.MoveFwdBckwd(pmsVM.EmployeeAppraisalMasterId, currentStatus))
                    {
                        ts.Complete();
                    }
                }
                else
                {
                    ts.Complete();
                }
            }
            return View(pmsVM);
        }
    }
}