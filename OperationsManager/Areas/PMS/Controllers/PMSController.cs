using OperationsManager.Areas.PMS.Models;
using OperationsManager.Controllers;
using OpMgr.Common.Contracts;
using OpMgr.Common.Contracts.Modules;
using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
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
        private IConfigSvc _configSvc;
        private INotificationSvc _notiSvc;

        public PMSController(OpMgr.Common.Contracts.Modules.IPMSSvc pmsSvc, ISessionSvc sessionSvc, IDropdownRepo ddlRepo, IConfigSvc configSvc, INotificationSvc notiSvc)
        {
            _pmsSvc = pmsSvc;
            _sessionSvc = sessionSvc;
            _ddlRepo = ddlRepo;
            _configSvc = configSvc;
            _uiddlRepo = new Helpers.UIDropDownRepo(_ddlRepo);
            _notiSvc = notiSvc;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult GoalSheetForAll(int? apprMasterId, int? designationId)
        {
            Models.PMSVM pmsVM = new Models.PMSVM();

            pmsVM.ImprovementsLoader = new MvcHtmlString("");
            pmsVM.StrengthsLoader = new MvcHtmlString("");
            pmsVM.ImprovementsShow = string.Empty;
            pmsVM.StrengthsShow = string.Empty;

            OpMgr.Common.DTOs.EmployeeGoalLogDTO empGoalLog = new OpMgr.Common.DTOs.EmployeeGoalLogDTO();
            empGoalLog.EmployeeAppraisalMaster = new OpMgr.Common.DTOs.EmployeeAppraisalMasterDTO();
            empGoalLog.EmployeeAppraisalMaster.EmployeeAppraisalMasterId = -1;
            if (apprMasterId.HasValue)
            {
                empGoalLog.EmployeeAppraisalMaster.EmployeeAppraisalMasterId = apprMasterId.Value;
            }
            empGoalLog.EmployeeAppraisalMaster.Employee = new OpMgr.Common.DTOs.EmployeeDetailsDTO();
            empGoalLog.EmployeeAppraisalMaster.Employee.EmployeeId = _sessionSvc.GetUserSession().UniqueEmployeeId;
            //empGoalLog.EmployeeAppraisalMaster.Employee.Designation = new OpMgr.Common.DTOs.DesignationDTO();
            //empGoalLog.EmployeeAppraisalMaster.Employee.Designation.DesignationId = 15;
            List<OpMgr.Common.DTOs.EmployeeGoalLogDTO> empGoalLogs = _pmsSvc.Select(empGoalLog).ReturnObj;

            if (empGoalLogs == null || empGoalLogs.Count == 0)
            {
                return RedirectToAction("AccessDenied", "Login", new { area = "Login" });
            }

            pmsVM.SumOfAcheivement = empGoalLogs.Sum(m => m.Achievement);
            pmsVM.SumOfAppraiserRating = empGoalLogs.Sum(m => m.AppraiserRating);
            pmsVM.SumOfWeitage = empGoalLogs.Sum(m => m.GoalAttribute.WeightAge);

            if (pmsVM.SumOfWeitage > 0)
            {
                int percentAchievement = Convert.ToInt32(Math.Ceiling(((pmsVM.SumOfAcheivement / Convert.ToDecimal(pmsVM.SumOfWeitage)) * 100)));
                int percentAppraiser = Convert.ToInt32(Math.Ceiling(((pmsVM.SumOfAppraiserRating / Convert.ToDecimal(pmsVM.SumOfWeitage)) * 100)));
                if (Convert.ToInt32(percentAchievement) > 0)
                {
                    pmsVM.SummaryOfAcheivement = _pmsSvc.getSelfRating(Convert.ToInt32(percentAchievement)).ReturnObj.SelfRating;
                }
                if (Convert.ToInt32(percentAppraiser) > 0)
                {
                    pmsVM.SummaryOfAppraisal = _pmsSvc.getSelfRating(Convert.ToInt32(percentAppraiser)).ReturnObj.SelfRating;
                }

            }

            pmsVM.ReviewerRating = empGoalLogs[0].EmployeeAppraisalMaster.ReviewerFinalRating;
            if (pmsVM.ReviewerRating > 0)
            {
                pmsVM.ReviewerRatingLevel = _pmsSvc.getSelfRating(Convert.ToInt32(pmsVM.ReviewerRating)).ReturnObj.SelfRating;
            }

            //get appraiser Final rating which will be after discussion with PMS head and will be displayed as Level to Appraise
            pmsVM.AppraiserFinalRating = empGoalLogs[0].EmployeeAppraisalMaster.AppraiserFinalRating;
            if (pmsVM.AppraiserFinalRating > 0)
            {
                pmsVM.AppraiserFinalRatingLevel = _pmsSvc.getSelfRating(Convert.ToInt32(pmsVM.AppraiserFinalRating)).ReturnObj.SelfRating;
            }

            pmsVM.EmployeeAppraisalMasterId = empGoalLogs[0].EmployeeAppraisalMaster.EmployeeAppraisalMasterId;
            pmsVM = pmsVM.GetGoals(empGoalLogs);

            // to get data in first grid
            pmsVM.IndividualInitiative = empGoalLogs[0].EmployeeAppraisalMaster.IndividualInitiative;
            pmsVM.InstitutionalSupport = empGoalLogs[0].EmployeeAppraisalMaster.InstitutionalSupport;

            pmsVM.AppraiserComment = empGoalLogs[0].EmployeeAppraisalMaster.AppraiserComment;
            pmsVM.ReviewerComment = empGoalLogs[0].EmployeeAppraisalMaster.ReviewerComment;

            pmsVM.FullName = empGoalLogs[0].EmployeeAppraisalMaster.Employee.UserDetails.FName + " " + empGoalLogs[0].EmployeeAppraisalMaster.Employee.UserDetails.LName;
            pmsVM.Employee = new OpMgr.Common.DTOs.EmployeeDetailsDTO();
            pmsVM.Employee.EducationalQualification = empGoalLogs[0].EmployeeAppraisalMaster.Employee.EducationalQualification;
            pmsVM.Employee.DateOfJoining = empGoalLogs[0].EmployeeAppraisalMaster.Employee.DateOfJoining;
            pmsVM.Employee.StaffEmployeeId = empGoalLogs[0].EmployeeAppraisalMaster.Employee.StaffEmployeeId;
            pmsVM.Employee.ApproverName = empGoalLogs[0].EmployeeAppraisalMaster.Employee.ApproverName;
            pmsVM.Employee.Designation = new OpMgr.Common.DTOs.DesignationDTO();
            //pmsVM.Employee.Designation.DesignationDescription = empGoalLogs[0].EmployeeAppraisalMaster.Employee.Designation.DesignationDescription;
            pmsVM.PMSDesignation = new PMSDesignationDTO();
            pmsVM.PMSDesignation.PmsDesignationDescription = empGoalLogs[0].EmployeeAppraisalMaster.PMSDesignation.PmsDesignationDescription;
            pmsVM.Employee.UserDetails = new OpMgr.Common.DTOs.UserMasterDTO();
            pmsVM.Employee.UserDetails.Location = new OpMgr.Common.DTOs.LocationDTO();
            pmsVM.Employee.UserDetails.Location.LocationDescription = empGoalLogs[0].EmployeeAppraisalMaster.Employee.UserDetails.Location.LocationDescription;
            string employeeImageFolder = _configSvc.GetEmployeeImagesFolder();

            pmsVM.Employee.UserDetails.UserMasterId = empGoalLogs[0].EmployeeAppraisalMaster.Employee.UserDetails.UserMasterId;

            pmsVM.employeeimagepath = _configSvc.GetEmployeeImagesRelPath() + "/" + GetImageFileName(pmsVM.Employee.StaffEmployeeId, employeeImageFolder) + "?ver=" + DateTime.UtcNow.Ticks;

            bool isSelf = apprMasterId == null;

            pmsVM.MODE = this.SetupMode(pmsVM.EmployeeAppraisalMasterId);

            if (string.Equals(pmsVM.MODE, "NO_ACCESS"))
            {
                return RedirectToAction("AccessDenied", "Login", new { area = "Login" });
            }
            if (string.Equals(pmsVM.MODE, "REVIEWER_REVIEW"))
            {
                pmsVM.RatingDropDown = _uiddlRepo.getAppraisalRatings();
            }
            if (string.Equals(pmsVM.MODE, "COMPETENCY_CHECK"))
            {
                pmsVM.CompetencyDDLSource = _uiddlRepo.getCompetencyDropDown();
                pmsVM.RatingDropDown = _uiddlRepo.getAppraisalRatings();
            }
            if (string.Equals(pmsVM.MODE, "Rating_Acceptance"))
            {
                this.CreateCompetencyLoaders(ref pmsVM);
            }
            if (string.Equals(pmsVM.MODE, "COMPETENCY_CHECK") || string.Equals(pmsVM.MODE, "PROCESS_ENDED") || string.Equals(pmsVM.MODE, "EMP_PROCESS_ENDED"))
            {
                this.CreateCompetencyLoaders(ref pmsVM);

            }
            return View(pmsVM);
        }

        public string GetImageFileName(string staffempid, string folder)
        {
            string fileName = string.Empty;
            string[] similarFiles = Directory.GetFiles(folder, staffempid + ".*");
            if (similarFiles != null && similarFiles.Length > 0)
            {
                fileName = similarFiles[0];
                string[] fileParts = fileName.Split('\\');
                fileName = fileParts[fileParts.Length - 1];
            }
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = staffempid + ".jpg";
            }

            return fileName;
        }


        private void CreateCompetencyLoaders(ref PMSVM pmsVM)
        {
            List<KeyValuePair<string, string>> improvements = new List<KeyValuePair<string, string>>();
            List<KeyValuePair<string, string>> strengths = new List<KeyValuePair<string, string>>();
            _pmsSvc.GetCompetencies(pmsVM.EmployeeAppraisalMasterId, out strengths, out improvements);
            if (improvements != null && improvements.Count > 0)
            {
                string loaderContext = string.Empty;
                string showContent = string.Empty;
                foreach (KeyValuePair<string, string> kvPair in improvements)
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
                    _pmsSvc.UpdateReviewerReview(pmsVM.EmployeeAppraisalMasterId, pmsVM.ReviewerRating, pmsVM.ReviewerComment);

                }
                else if (string.Equals(pmsVM.MODE, "COMPETENCY_CHECK") && (string.Equals(pmsVM.SAVE_MODE, "SAVE") || string.Equals(pmsVM.SAVE_MODE, "SUBMIT") || string.Equals(pmsVM.SAVE_MODE, "NOTIFY")))
                {
                    if (string.Equals(pmsVM.SAVE_MODE, "NOTIFY"))
                    {
                        NotificationDTO noti = new NotificationDTO();
                        noti.User = new UserMasterDTO();
                        noti.User.UserMasterId = pmsVM.Employee.UserDetails.UserMasterId;
                        noti.NotificationText = pmsVM.NotificationText;
                        noti.NotificationActiveFrom = DateTime.Today.Date;
                        if (_notiSvc.Insert(noti).IsSuccess)
                        {
                            Hubs.NotificationHub notifyHub = new Hubs.NotificationHub();
                            //Save notification details - if status true send message to connected user
                            notifyHub.SendNotification(_sessionSvc.GetUserSession().UserMasterId, pmsVM.Employee.UserDetails.UserMasterId, pmsVM.NotificationText);
                        }
                    }
                    else
                    {
                        _pmsSvc.SaveCompetency(pmsVM.EmployeeAppraisalMasterId, pmsVM.ImprovementArea, pmsVM.Strengths);
                        _pmsSvc.UpdateInitiativeandSupport(pmsVM.EmployeeAppraisalMasterId, pmsVM.IndividualInitiative, pmsVM.InstitutionalSupport);
                        _pmsSvc.UpdateAppraiserFinalRating(pmsVM.EmployeeAppraisalMasterId, pmsVM.AppraiserFinalRating, pmsVM.AppraiserComment);
                    }
                }
                else if (string.Equals(pmsVM.MODE, "Rating_Acceptance") && (string.Equals(pmsVM.SAVE_MODE, "AcceptRating")))
                {

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
                                                if (string.Equals(empGoal.NeedsUpdate, "Y"))
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
                else if (string.Equals(pmsVM.SAVE_MODE, "AcceptRating"))
                {
                    int currentStatus = _pmsSvc.GetCurrentStatus(pmsVM.EmployeeAppraisalMasterId);
                    if (_pmsSvc.MoveFwdBckwd(pmsVM.EmployeeAppraisalMasterId, currentStatus, false))
                    {
                        ts.Complete();
                    }
                }
                else
                {
                    ts.Complete();
                }
            }
            return RedirectToAction("GoalSheetForAll", new { apprMasterId = pmsVM.EmployeeAppraisalMasterId });
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
                //pmsview.AppraisalStatusList = _uiddlRepo.getAppraisalStatus();
                pmsview.PMSDesignationList = _uiddlRepo.getPMSDesignationDropDown();

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

                            searchItem.EmpAppPmsMasterId = appraisalmaster.EmpAppPmsMasterId;// added by Navajit for fetching appraisal details

                            searchItem.FullName = appraisalmaster.Employee.UserDetails.FName;
                            if (!string.IsNullOrEmpty(appraisalmaster.Employee.UserDetails.FName))
                            {
                                searchItem.FullName = searchItem.FullName + " " + searchItem.UserDetails.MName;
                            }

                            searchItem.FullName = searchItem.FullName + " " + searchItem.UserDetails.LName;
                            searchItem.UserDetails.Gender = appraisalmaster.Employee.UserDetails.Gender;
                            searchItem.EmployeeAppraisalMasterId = appraisalmaster.EmployeeAppraisalMasterId;
                            searchItem.AppraisalType = appraisalmaster.AppraisalType;
                            //searchItem.AppraisalStatus = new AppraisalStatusDTO();
                            //searchItem.AppraisalStatus.AppraisalStatusDescription = appraisalmaster.AppraisalStatus.AppraisalStatusDescription;

                            //searchItem.UserDetails.Location = new LocationDTO();
                            //searchItem.UserDetails.Location.LocationDescription = appraisalmaster.Employee.UserDetails.Location.LocationDescription;
                            searchItem.Employee = new EmployeeDetailsDTO();
                            searchItem.Employee.StaffEmployeeId = appraisalmaster.Employee.StaffEmployeeId;
                            //searchItem.Employee.Designation = new DesignationDTO();
                            //searchItem.Employee.Designation.DesignationDescription = appraisalmaster.Employee.Designation.DesignationDescription;

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
                pmsview.PMSDesignationList = _uiddlRepo.getPMSDesignationDropDown();

                pmsview.IsSearchSuccessful = false;
                //pmsview.MsgColor = "green";
                //pmsview.SuccessOrFailureMessage = "Please Select atleast 1 Search Criteria";
            }

            return View(pmsview);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult AppraisalDetails(int pmsId)
        {

            PmsMasterVM pmsView = new PmsMasterVM();//main model to be passed to View
            StatusDTO<List<PMSMasterDTO>> statusPms = _pmsSvc.GetAppraisalDetails(pmsId);
            if (statusPms != null && statusPms.ReturnObj != null && statusPms.IsSuccess && !statusPms.IsException)
            {
                if (statusPms.ReturnObj.Count > 0)
                {
                    pmsView.pmsViewList = new List<PmsMasterVM>();//List in View Model
                    foreach (PMSMasterDTO pms in statusPms.ReturnObj)
                    {
                        PmsMasterVM pmsViewSearchItem = new PmsMasterVM();

                        pmsViewSearchItem.AppraisalStatus = new AppraisalStatusDTO();
                        pmsViewSearchItem.AppraisalStatus.AppraisalStatusDescription = pms.AppraisalStatus.AppraisalStatusDescription;

                        pmsViewSearchItem.Employee = new EmployeeDetailsDTO();
                        pmsViewSearchItem.Employee.UserDetails = new UserMasterDTO();

                        pmsViewSearchItem.Employee.UserDetails.Location = new LocationDTO();
                        pmsViewSearchItem.Employee.UserDetails.Location.LocationDescription = pms.Employee.UserDetails.Location.LocationDescription;

                        pmsViewSearchItem.EmployeeAppraisalMasterId = pms.EmployeeAppraisalMasterId;

                        pmsViewSearchItem.Employee.Designation = new DesignationDTO();
                        pmsViewSearchItem.Employee.Designation.DesignationDescription = pms.Employee.Designation.DesignationDescription;

                        pmsView.pmsViewList.Add(pmsViewSearchItem);
                        if (pmsView.pmsViewList != null && pmsView.pmsViewList.Count > 0)
                        {
                            pmsView.IsSearchSuccessful = true;
                        }
                    }
                }

            }
            else
            {
                pmsView.IsSearchSuccessful = false;

            }
            return View(pmsView);



        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult SearchAppraisee(PMSVM pmsvm)
        {
            PMSVM pmsview = null;
            EmployeeAppraisalMasterDTO empappraisalmasterdto = null;

            if (pmsvm != null)
            {
                if (string.Equals(pmsvm.MODE, "Search"))
                {
                    
                    empappraisalmasterdto = new EmployeeAppraisalMasterDTO();
                    empappraisalmasterdto.Employee = new EmployeeDetailsDTO();
                    empappraisalmasterdto.Employee.UserDetails = new UserMasterDTO();

                    empappraisalmasterdto.Employee.UserDetails.FName = pmsvm.Employee.UserDetails.FName;
                    empappraisalmasterdto.Employee.UserDetails.LName = pmsvm.Employee.UserDetails.LName;
                    empappraisalmasterdto.Employee.StaffEmployeeId = pmsvm.Employee.StaffEmployeeId;
                    empappraisalmasterdto.AppraisalType = pmsvm.AppraisalType;
                    empappraisalmasterdto.AppraisalStatus = pmsvm.AppraisalStatus;
                    empappraisalmasterdto.Employee.UserDetails.Gender = pmsvm.Employee.UserDetails.Gender;

                    StatusDTO<List<EmployeeAppraisalMasterDTO>> status = _pmsSvc.SearchAppraisee(pmsvm);
                    if (status.ReturnObj != null && status.ReturnObj.Count > 0)
                    {
                        pmsview = new PMSVM(); // Instantiating PMS View model
                        pmsview.PMSVMList = new List<PMSVM>(); // instantiating list of PMSVM

                        //Fetch the StandardSection List
                        pmsview.GenderList = _uiddlRepo.getGenderDropDown();
                        // pmsview.LocationList = _uiddlRepo.getLocationDropDown();
                        pmsview.AppraisalTypeList = _uiddlRepo.getAppraisalType();
                        pmsview.AppraisalStatusList = _uiddlRepo.getAppraisalStatus();
                        pmsview.PMSDesignationList = _uiddlRepo.getPMSDesignationDropDown();

                        if (status.IsSuccess && !status.IsException)
                        {
                            //studView = new List<StudentVM>();
                            PMSVM searchItem = null;
                            foreach (EmployeeAppraisalMasterDTO appraisalmaster in status.ReturnObj)
                            {
                                if (appraisalmaster != null)
                                {
                                    searchItem = new PMSVM(); // instantiating each PMVM         

                                    searchItem.EmpAppPmsMasterId = appraisalmaster.EmpAppPmsMasterId;
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
                                    //searchItem.AppraisalStatus = new AppraisalStatusDTO();
                                    //searchItem.AppraisalStatus.AppraisalStatusDescription = appraisalmaster.AppraisalStatus.AppraisalStatusDescription;

                                    //searchItem.UserDetails.Location = new LocationDTO();
                                    //searchItem.UserDetails.Location.LocationDescription = appraisalmaster.Employee.UserDetails.Location.LocationDescription;
                                    searchItem.Employee = new EmployeeDetailsDTO();
                                    searchItem.Employee.StaffEmployeeId = appraisalmaster.Employee.StaffEmployeeId;
                                    
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
                }
                else if (string.Equals(pmsvm.MODE, "PMSHeadApprove"))
                {
                    
                    _pmsSvc.UpdatePMSHeadApproval(pmsvm);

                    pmsview = new PMSVM();

                    pmsview.GenderList = _uiddlRepo.getGenderDropDown();
                    // pmsview.LocationList = _uiddlRepo.getLocationDropDown();
                    pmsview.AppraisalTypeList = _uiddlRepo.getAppraisalType();
                    pmsview.AppraisalStatusList = _uiddlRepo.getAppraisalStatus();
                    pmsview.PMSDesignationList = _uiddlRepo.getPMSDesignationDropDown();

                }
                else if (string.Equals(pmsvm.MODE, "ExcelForPMSHead"))
                {
                    _pmsSvc.ExcelDataForPMSHead(pmsvm);
                }
                else
                {
                    pmsview = new PMSVM();

                    pmsview.GenderList = _uiddlRepo.getGenderDropDown();
                    // pmsview.LocationList = _uiddlRepo.getLocationDropDown();
                    pmsview.AppraisalTypeList = _uiddlRepo.getAppraisalType();
                    pmsview.AppraisalStatusList = _uiddlRepo.getAppraisalStatus();
                    pmsview.PMSDesignationList = _uiddlRepo.getPMSDesignationDropDown();

                    pmsview.IsSearchSuccessful = false;
                    //pmsview.MsgColor = "green";
                    //pmsview.SuccessOrFailureMessage = "Please Select atleast 1 Search Criteria";
                }
            }
            return View(pmsview);
        }

        // To get multiple goal sheet for same employee multiple designation 
        [HttpGet]
        [AllowAnonymous]
        public ActionResult PMSMUltipleGoalSheet()
        {
            StatusDTO<List<EmployeeAppraisalMasterDTO>> status = _pmsSvc.GetAppraiseePMSLIst(_sessionSvc.GetUserSession().UniqueEmployeeId);
            PMSVM pmsview = null;

            if (status.ReturnObj != null && status.ReturnObj.Count > 0)
            {
                pmsview = new PMSVM(); // Instantiating PMS View model
                pmsview.PMSVMList = new List<PMSVM>(); // instantiating list of PMSVM

                pmsview.AppraisalTypeList = _uiddlRepo.getAppraisalType();
                pmsview.MODE = "Initiated";

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
                            if (Convert.ToInt32(appraisalmaster.AvgFinalRating) > 0)
                            {
                                searchItem.AvgFinalLevel = _pmsSvc.getSelfRating(Convert.ToInt32(appraisalmaster.AvgFinalRating)).ReturnObj.SelfRating;
                            }
                            searchItem.AppraisalType = appraisalmaster.AppraisalType;
                            searchItem.AppraisalStatus = new AppraisalStatusDTO();
                            searchItem.AppraisalStatus.AppraisalStatusDescription = appraisalmaster.AppraisalStatus.AppraisalStatusDescription;

                            searchItem.UserDetails.Location = new LocationDTO();
                            searchItem.UserDetails.Location.LocationDescription = appraisalmaster.Employee.UserDetails.Location.LocationDescription;
                            searchItem.Employee = new EmployeeDetailsDTO();
                            searchItem.Employee.StaffEmployeeId = appraisalmaster.Employee.StaffEmployeeId;
                            searchItem.Employee.Designation = new DesignationDTO();
                            searchItem.Employee.Designation.DesignationId = Convert.ToInt32(appraisalmaster.Employee.Designation.DesignationId);
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
                pmsview.AppraisalTypeList = _uiddlRepo.getAppraisalType();
                pmsview.IsSearchSuccessful = false;
                pmsview.MODE = "NotInitiated";
                //pmsview.MsgColor = "green";
                //pmsview.SuccessOrFailureMessage = "Please Select atleast 1 Search Criteria";
            }

            return View(pmsview);
        }

        //below method will call while initiate the the goalsheet
        [HttpPost]
        [AllowAnonymous]
        public ActionResult PMSMUltipleGoalSheet(PMSVM pmsvm)
        {
            PMSVM pmsview = null;
            
            if (pmsvm != null)
            {
                if (string.Equals(pmsvm.MODE, "NotInitiated"))
                {
                    StatusDTO<EmployeeAppraisalMasterDTO> status = _pmsSvc.InitiateAppraisal(pmsvm.AppraisalType);
                    

                    if (status.IsSuccess)
                    {
                        pmsview = new PMSVM(); // Instantiating PMS View model
                        
                        //pmsview.AppraisalTypeList = _uiddlRepo.getAppraisalType();
                        pmsview.MODE = "Initiated";
                        //pmsview.IsSearchSuccessful = true;
                    }
                    else
                    {
                        pmsview = new PMSVM();
                        //pmsview.AppraisalTypeList = _uiddlRepo.getAppraisalType();
                        //pmsview.IsSearchSuccessful = false;
                        pmsview.MODE = "NotInitiated";
                    }
                }
            }

            return RedirectToAction("PMSMUltipleGoalSheet");
        }
    }
}