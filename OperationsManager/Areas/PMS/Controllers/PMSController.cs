using OperationsManager.Areas.PMS.Models;
using OperationsManager.Controllers;
using OpMgr.Common.Contracts;
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
        // IUserSvc _userSvc;
        //private ILogSvc _logSvc;
        //private IDropdownRepo _dropDownRepo;
        //private Helpers.UIDropDownRepo _uiddlRepo;

        private OpMgr.Common.Contracts.Modules.IPMSSvc _pmsSvc;
        private ISessionSvc _sessionSvc;
        private IDropdownRepo _ddlRepo;
        private Helpers.UIDropDownRepo _uiddlRepo;
        public PMSController(OpMgr.Common.Contracts.Modules.IPMSSvc pmsSvc, ISessionSvc sessionSvc, IDropdownRepo ddlRepo)
        {
            _pmsSvc = pmsSvc;
            _sessionSvc = sessionSvc;
            _ddlRepo = ddlRepo;
            _uiddlRepo = new Helpers.UIDropDownRepo(_ddlRepo);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult GoalSheetForAll(int? apprMasterId)
        {
            Models.PMSVM pmsVM = new Models.PMSVM();

            pmsVM.ImprovementsLoader = new MvcHtmlString("");
            pmsVM.StrengthsLoader = new MvcHtmlString("");
            pmsVM.ImprovementsShow = string.Empty;
            pmsVM.StrengthsShow = string.Empty;

            OpMgr.Common.DTOs.EmployeeGoalLogDTO empGoalLog = new OpMgr.Common.DTOs.EmployeeGoalLogDTO();
            empGoalLog.EmployeeAppraisalMaster = new OpMgr.Common.DTOs.EmployeeAppraisalMasterDTO();
            empGoalLog.EmployeeAppraisalMaster.EmployeeAppraisalMasterId = -1;
            if(apprMasterId.HasValue)
            {
                empGoalLog.EmployeeAppraisalMaster.EmployeeAppraisalMasterId = apprMasterId.Value;
            }
            empGoalLog.EmployeeAppraisalMaster.Employee = new OpMgr.Common.DTOs.EmployeeDetailsDTO();
            empGoalLog.EmployeeAppraisalMaster.Employee.EmployeeId = _sessionSvc.GetUserSession().UniqueEmployeeId;
            empGoalLog.EmployeeAppraisalMaster.Employee.Designation = new OpMgr.Common.DTOs.DesignationDTO();
            empGoalLog.EmployeeAppraisalMaster.Employee.Designation.DesignationId = 15;
            List<OpMgr.Common.DTOs.EmployeeGoalLogDTO> empGoalLogs = _pmsSvc.Select(empGoalLog).ReturnObj;

            if(empGoalLog==null || empGoalLogs.Count==0)
            {
                return RedirectToAction("AccessDenied", "Login", new { area = "Login" });
            }

            pmsVM.SumOfAcheivement = empGoalLogs.Sum(m => m.Achievement);
            pmsVM.SumOfAppraiserRating = empGoalLogs.Sum(m => m.AppraiserRating);
            pmsVM.ReviewerRating = empGoalLogs[0].EmployeeAppraisalMaster.ReviewerFinalRating;

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

            pmsVM.MODE = this.SetupMode(pmsVM.EmployeeAppraisalMasterId);

            if (string.Equals(pmsVM.MODE, "NO_ACCESS"))
            {
                return RedirectToAction("AccessDenied", "Login", new { area = "Login" });
            }
            if (string.Equals(pmsVM.MODE, "COMPETENCY_CHECK"))
            {
                pmsVM.CompetencyDDLSource = _uiddlRepo.getCompetencyDropDown();
            }
            if (string.Equals(pmsVM.MODE, "COMPETENCY_CHECK") || string.Equals(pmsVM.MODE, "PROCESS_ENDED"))
            {
                this.CreateCompetencyLoaders(ref pmsVM);
            }
            return View(pmsVM);
        }


        private void CreateCompetencyLoaders(ref PMSVM pmsVM)
        {
            List<KeyValuePair<string, string>> improvements = new List<KeyValuePair<string, string>>();
            List<KeyValuePair<string, string>> strengths = new List<KeyValuePair<string, string>>();
            _pmsSvc.GetCompetencies(pmsVM.EmployeeAppraisalMasterId, out strengths, out improvements);
            if(improvements!=null && improvements.Count>0)
            {
                string loaderContext = string.Empty;
                string showContent = string.Empty;
                foreach(KeyValuePair<string, string> kvPair in improvements)
                {
                    loaderContext = loaderContext + "$('#improvement_txt').tagsinput('add', { id: '" + kvPair.Key + "', label: '" + kvPair.Value + "' });";
                    showContent = showContent + kvPair.Value + ", ";
                }
                pmsVM.ImprovementsShow = showContent;
                pmsVM.ImprovementsLoader = new MvcHtmlString(loaderContext);
            }

            if (strengths != null && strengths.Count > 0)
            {
                string loaderContext = string.Empty;
                string showContent = string.Empty;
                foreach (KeyValuePair<string, string> kvPair in strengths)
                {
                    loaderContext = loaderContext + "$('#strength_txt').tagsinput('add', { id: '" + kvPair.Key + "', label: '" + kvPair.Value + "' });";
                    showContent = showContent + kvPair.Value + ", ";
                }
                pmsVM.StrengthsShow = showContent;
                pmsVM.StrengthsLoader = new MvcHtmlString(loaderContext);
            }
        }

        private string SetupMode(int apprMasterId)
        {
            string accessStatus = _pmsSvc.AccessStatus(apprMasterId);
            return accessStatus;
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
                if (string.Equals(pmsVM.MODE, "REVIEWER_REVIEW") && (string.Equals(pmsVM.SAVE_MODE, "SAVE") || string.Equals(pmsVM.SAVE_MODE, "SUBMIT")))
                {
                    _pmsSvc.UpdateReviewerReview(pmsVM.EmployeeAppraisalMasterId, pmsVM.ReviewerRating);
                }
                else if (string.Equals(pmsVM.MODE, "COMPETENCY_CHECK") && (string.Equals(pmsVM.SAVE_MODE, "SAVE") || string.Equals(pmsVM.SAVE_MODE, "SUBMIT")))
                {
                    _pmsSvc.SaveCompetency(pmsVM.EmployeeAppraisalMasterId, pmsVM.ImprovementArea, pmsVM.Strengths);
                }
                else
                {
                    foreach (GoalViewModel gVM in lstGoalVM)
                    {

                        try
                        {
                            foreach (EmployeeGoalLogDTO empGoal in gVM.GoalLog)
                            {
                                try
                                {
                                    empGoal.EmployeeAppraisalMaster = new EmployeeAppraisalMasterDTO();
                                    empGoal.EmployeeAppraisalMaster.EmployeeAppraisalMasterId = pmsVM.EmployeeAppraisalMasterId;
                                    //Insert goal in goal log
                                    if (string.Equals(pmsVM.SAVE_MODE, "SAVE") || string.Equals(pmsVM.SAVE_MODE, "SUBMIT"))
                                    {
                                        if (empGoal.EmployeeGoalLogId == -1 && string.Equals(pmsVM.MODE, "STAFF_FORM_FILLUP"))
                                        {
                                            _pmsSvc.Insert(empGoal);
                                        }
                                        else if (empGoal.EmployeeGoalLogId > 0)
                                        {
                                            if (string.Equals(pmsVM.MODE, "STAFF_FORM_FILLUP"))
                                            {
                                                if(string.Equals(empGoal.NeedsUpdate, "Y"))
                                                {
                                                    _pmsSvc.Update(empGoal);
                                                }                                                    
                                            }
                                            else if (string.Equals(pmsVM.MODE, "APPRAISER_REVIEW"))
                                            {
                                                if (string.Equals(empGoal.NeedsAppraiserUpdate, "Y"))
                                                {
                                                    _pmsSvc.UpdateAppraiserRating(empGoal);
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception exp)
                                {
                                    throw exp;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
                
                if (string.Equals(pmsVM.SAVE_MODE, "SUBMIT"))
                {
                    int currentStatus = _pmsSvc.GetCurrentStatus(pmsVM.EmployeeAppraisalMasterId);
                    if (_pmsSvc.MoveFwdBckwd(pmsVM.EmployeeAppraisalMasterId, currentStatus))
                    {
                        ts.Complete();
                    }
                }
                else if (string.Equals(pmsVM.SAVE_MODE, "SEND_BACK"))
                {
                    int currentStatus = _pmsSvc.GetCurrentStatus(pmsVM.EmployeeAppraisalMasterId);
                    if (_pmsSvc.MoveFwdBckwd(pmsVM.EmployeeAppraisalMasterId, currentStatus, true))
                    {
                        ts.Complete();
                    }
                }
                else
                {
                    ts.Complete();
                }
            }
            return RedirectToAction("GoalSheetForAll", new { apprMasterId = pmsVM.EmployeeAppraisalMasterId  });
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult SearchAppraisee()
        {
            StatusDTO<List<EmployeeAppraisalMasterDTO>> status = _pmsSvc.SearchAppraisee(null);
            PMSVM pmsview = null;

            if (status.ReturnObj != null && status.ReturnObj.Count > 0)
            {
                pmsview = new PMSVM(); // Instantiating PMS View model
                pmsview.PMSVMList = new List<PMSVM>(); // instantiating list of PMSVM

                //Fetch the StandardSection List
                pmsview.GenderList = _uiddlRepo.getGenderDropDown();
               // pmsview.LocationList = _uiddlRepo.getLocationDropDown();
                pmsview.AppraisalTypeList = _uiddlRepo.getAppraisalType();
                pmsview.AppraisalStatusList = _uiddlRepo.getAppraisalStatus();

                if (status.IsSuccess && !status.IsException)
                {
                    //studView = new List<StudentVM>();
                    PMSVM searchItem = null;
                    foreach (EmployeeAppraisalMasterDTO appraisalmaster in status.ReturnObj)
                    {
                        if (appraisalmaster != null)
                        {
                            searchItem = new PMSVM(); // instantiating each PMVM                            
                            searchItem.UserDetails = new UserMasterDTO();
                            searchItem.UserDetails.FName = appraisalmaster.Employee.UserDetails.FName;
                            searchItem.UserDetails.MName = appraisalmaster.Employee.UserDetails.MName;
                            searchItem.UserDetails.LName = appraisalmaster.Employee.UserDetails.LName;

                            searchItem.FullName = appraisalmaster.Employee.UserDetails.FName;
                            if (!string.IsNullOrEmpty(appraisalmaster.Employee.UserDetails.FName))
                            {
                                searchItem.FullName = searchItem.FullName + " " + searchItem.UserDetails.MName;
                            }

                            searchItem.FullName = searchItem.FullName + " " + searchItem.UserDetails.LName;
                            searchItem.UserDetails.Gender = appraisalmaster.Employee.UserDetails.Gender;
                            searchItem.EmployeeAppraisalMasterId = appraisalmaster.EmployeeAppraisalMasterId;
                            searchItem.AppraisalType = appraisalmaster.AppraisalType;
                            searchItem.AppraisalStatus = new AppraisalStatusDTO();
                            searchItem.AppraisalStatus.AppraisalStatusDescription = appraisalmaster.AppraisalStatus.AppraisalStatusDescription;

                            searchItem.UserDetails.Location = new LocationDTO();
                            searchItem.UserDetails.Location.LocationDescription = appraisalmaster.Employee.UserDetails.Location.LocationDescription;
                            searchItem.Employee = new EmployeeDetailsDTO();
                            searchItem.Employee.StaffEmployeeId = appraisalmaster.Employee.StaffEmployeeId;
                            searchItem.Employee.Designation = new DesignationDTO();
                            searchItem.Employee.Designation.DesignationDescription = appraisalmaster.Employee.Designation.DesignationDescription;
                            //Add into PMSView vIew Model List
                            pmsview.PMSVMList.Add(searchItem);
                            pmsview.IsSearchSuccessful = true;

                        }
                    }
                }
                if (status.IsException)
                {
                    throw new Exception(status.ExceptionMessage);
                }
            }
            else
            {
                pmsview = new PMSVM();

                pmsview.GenderList = _uiddlRepo.getGenderDropDown();
               // pmsview.LocationList = _uiddlRepo.getLocationDropDown();
                pmsview.AppraisalTypeList = _uiddlRepo.getAppraisalType();
                pmsview.AppraisalStatusList = _uiddlRepo.getAppraisalStatus();

                pmsview.IsSearchSuccessful = true;
                //pmsview.MsgColor = "green";
                //pmsview.SuccessOrFailureMessage = "Please Select atleast 1 Search Criteria";
            }

            return View(pmsview);
        }
    }
}