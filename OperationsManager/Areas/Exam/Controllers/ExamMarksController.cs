using OpMgr.Common.Contracts;
using OpMgr.Common.Contracts.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OpMgr.Common.DTOs;
using System.Data;

namespace OperationsManager.Areas.Exam.Controllers
{
    public class ExamMarksController : Controller
    {
        private IExamMarksSvc _examMarksSvc;
        private IDropdownRepo _dropDwnRepo;
        private Helpers.UIDropDownRepo _uiddlRepo;
        private IConfigSvc _configSvc;
        private ISessionSvc _sessionSvc;
        private IExamRuleSvc _examRuleSvc;

        public ExamMarksController(IExamMarksSvc examMarksSvc, IDropdownRepo dropDwnRepo, IConfigSvc configSvc, ISessionSvc sessionSvc, IExamRuleSvc examRuleSvc)
        {
            _examMarksSvc = examMarksSvc;
            _dropDwnRepo = dropDwnRepo;
            _configSvc = configSvc;
            _sessionSvc = sessionSvc;
            _examRuleSvc = examRuleSvc;
            _uiddlRepo = new Helpers.UIDropDownRepo(_dropDwnRepo);
        }
        // GET: Exam/ExamMarks
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            Models.ExamMarksVM examMarksVM = new Models.ExamMarksVM();
            examMarksVM.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();
            examMarksVM.SubjectList = _uiddlRepo.getSubjectDropDown();
            examMarksVM.LocationList = _uiddlRepo.getLocationDropDown();
            examMarksVM.ExamTypeList = _uiddlRepo.getExamTypeDropDown();
            examMarksVM.ExamSubTypeList = _uiddlRepo.getExamSubTypeDropDown();
            examMarksVM.AcademicSessions = _uiddlRepo.getAcademicSessionDropDown();
            //examMarksVM.FromDateString = examMarksVM.FromDate ? examMarksVM.DOB.Value.ToString("dd-MMM-yyyy") : string.Empty;
            SessionDTO sessionRet = _sessionSvc.GetUserSession();
            examMarksVM.IsRuleOk = true;
            examMarksVM.IsRuleNeededToBeAdded = false;
            examMarksVM.Rule = null;
            examMarksVM.Mode = "SEARCH";
            return View(examMarksVM);
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetExamSubType(int examType)
        {
            List<ExamSubTypeDTO> lstSubTypes = _dropDwnRepo.getExamSubType(examType);
            return Json(new { Status = true, Data = lstSubTypes }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetSubjectDropdownData(int locationId, int StandardSectionId)
        {
            List<SubjectDTO> lstSubject = _dropDwnRepo.getSubjectDropdown(locationId, StandardSectionId);
            return Json(new { Status = true, Data = lstSubject }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(Models.ExamMarksVM examMarksVM)
        {
            examMarksVM.FromDateString = examMarksVM.SelectedAcademicSession.Split(';')[0];
            examMarksVM.ToDateString = examMarksVM.SelectedAcademicSession.Split(';')[1];
            examMarksVM.CourseFrom = DateTime.Parse(examMarksVM.FromDateString);
            examMarksVM.CourseTo = DateTime.Parse(examMarksVM.ToDateString);
            if (string.Equals(examMarksVM.Mode, "SEARCH", StringComparison.OrdinalIgnoreCase))
            {
                Models.ExamMarksVM examVM = null;
                //ExamMarksDTO exammarksdto = null;
                if (examMarksVM != null)
                {
                    examVM = new Models.ExamMarksVM();

                    examVM.StandardSectionId = examMarksVM.CourseExam.CourseMapping.StandardSection.StandardSectionId;
                    examVM.SubjectId = examMarksVM.CourseExam.CourseMapping.Subject.SubjectId;


                    StatusDTO<List<ExamMarksDTO>> status = _examMarksSvc.GetStudentDetailsForMarksEntry(examMarksVM.CourseExam.CourseMapping.Location.LocationId, examMarksVM.CourseExam.CourseMapping.StandardSection.StandardSectionId, examMarksVM.CourseExam.CourseMapping.Subject.SubjectId, DateTime.Parse(examMarksVM.FromDateString), DateTime.Parse(examMarksVM.ToDateString), examMarksVM.CourseExam.ExamType.ExamTypeId, examMarksVM.CourseExam.ExamSubType.ExamSubTypeId);

                    if (status.IsSuccess)
                    {
                        if (status.ReturnObj != null && status.ReturnObj.Count > 0)
                        {
                            examVM.ExamMarksList = new List<Models.ExamMarksVM>();

                            examVM.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();
                            examVM.SubjectList = _uiddlRepo.getSubjectDropDown(examMarksVM.CourseExam.CourseMapping.Location.LocationId, examMarksVM.CourseExam.CourseMapping.StandardSection.StandardSectionId);
                            examVM.LocationList = _uiddlRepo.getLocationDropDown();
                            examVM.ExamTypeList = _uiddlRepo.getExamTypeDropDown();
                            examVM.ExamSubTypeList = _uiddlRepo.getExamSubTypeDropDown(examMarksVM.CourseExam.ExamType.ExamTypeId);
                            examVM.AcademicSessions = _uiddlRepo.getAcademicSessionDropDown();
                            examVM.Grades = _uiddlRepo.getGradesDropDown(examMarksVM.CourseExam.CourseMapping.Location.LocationId);
                            examVM.Rule = null;
                            //examVM.CourseExam.CourseMapping.Subject.SubjectId = examVM.SubjectId;

                            Models.ExamMarksVM exammarksvm = null;
                            ExamMarksDTO exammarksdto = null;
                            for (int i = 0; i < status.ReturnObj.Count; i++)
                            {
                                exammarksdto = status.ReturnObj[i];
                                if (exammarksdto != null)
                                {
                                    exammarksvm = new Models.ExamMarksVM();
                                    exammarksvm.ExamMarksId = exammarksdto.ExamMarksId;
                                    if (exammarksdto.MarksObtained.HasValue)
                                    {
                                        exammarksvm.DisplayedObtainedMarks = exammarksdto.MarksObtained.Value.ToString();
                                    }
                                    else
                                    {
                                        exammarksvm.DisplayedObtainedMarks = "NA";
                                    }
                                    if (exammarksdto.CalculatedMarks.HasValue)
                                    {
                                        exammarksvm.DisplayedCalculatedMarks = exammarksdto.CalculatedMarks.Value.ToString();
                                    }
                                    else
                                    {
                                        exammarksvm.DisplayedCalculatedMarks = "NA";
                                    }
                                    exammarksvm.DirectGrade = exammarksdto.DirectGrade;
                                    exammarksvm.SubjectExamType = exammarksdto.SubjectExamType;

                                    if (string.Equals(exammarksdto.SubjectExamType, "G", StringComparison.OrdinalIgnoreCase))
                                    {
                                        examVM.IsRuleNeededToBeAdded = false;
                                        examVM.Rule = null;
                                        examVM.IsRuleOk = true;
                                        examVM.CourseExamId = exammarksdto.CourseExam.CourseExamId;
                                    }
                                    else
                                    {
                                        if (examVM.Rule == null)
                                        {
                                            ExamRuleDTO rule = new ExamRuleDTO();
                                            rule.CourseExam = new CourseExamDTO();
                                            rule.CourseExam.CourseExamId = exammarksdto.CourseExam.CourseExamId;
                                            List<ExamRuleDTO> rules = _examRuleSvc.Select(rule).ReturnObj;

                                            if (rules == null || rules.Count == 0)
                                            {
                                                examVM.IsRuleOk = false;
                                                examVM.RuleAdditionMessage = "Please add marks rule for this exam first.";
                                                examVM.IsRuleNeededToBeAdded = true;
                                                break;
                                            }
                                            else if (rules.Count > 1)
                                            {
                                                examVM.IsRuleOk = false;
                                                examVM.RuleAdditionMessage = "More than 1 rule added for this exam. Please contact dev team to fix this. Please provide the Course Exam Id - " + exammarksdto.CourseExam.CourseExamId + ".";
                                                examVM.IsRuleNeededToBeAdded = false;
                                                break;
                                            }
                                            else
                                            {
                                                examVM.IsRuleOk = true;
                                                examVM.Rule = rules[0];
                                                examVM.RuleAdditionMessage = string.Empty;
                                                examVM.IsRuleNeededToBeAdded = false;
                                                examVM.RuleId = examVM.Rule.ExamRuleId;
                                                examVM.CourseExamId = rule.CourseExam.CourseExamId;
                                            }
                                        }
                                    }

                                    exammarksvm.StandardSection = new StandardSectionMapDTO();
                                    exammarksvm.StandardSection.StandardSectionId = exammarksdto.StandardSection.StandardSectionId;

                                    exammarksvm.CourseExam = new CourseExam();
                                    exammarksvm.CourseExam.CourseExamId = exammarksdto.CourseExam.CourseExamId;

                                    exammarksvm.Student = new StudentDTO();

                                    exammarksvm.Student.StandardSectionMap = new StandardSectionMapDTO();
                                    exammarksvm.Student.StandardSectionMap.Standard = new StandardDTO();
                                    exammarksvm.Student.StandardSectionMap.Section = new SectionDTO();
                                    exammarksvm.Student.UserDetails = new UserMasterDTO();
                                    exammarksvm.Student.UserDetails.Location = new LocationDTO();

                                    exammarksvm.Student.StudentInfoId = exammarksdto.Student.StudentInfoId;
                                    exammarksvm.Student.RegistrationNumber = exammarksdto.Student.RegistrationNumber;
                                    exammarksvm.Student.RollNumber = exammarksdto.Student.RollNumber;
                                    exammarksvm.FullName = exammarksdto.Student.UserDetails.FName + " " + exammarksdto.Student.UserDetails.LName;
                                    exammarksvm.Class = exammarksdto.Student.StandardSectionMap.Standard.StandardName + " " + exammarksdto.Student.StandardSectionMap.Section.SectionName;
                                    exammarksvm.Student.UserDetails.Location.LocationDescription = exammarksdto.Student.UserDetails.Location.LocationDescription;

                                    examVM.ExamMarksList.Add(exammarksvm);
                                    examVM.IsSearchSuccessful = true;
                                }
                            }

                        }
                    }
                    else
                    {
                        examVM = examMarksVM;
                        examVM.IsSearchSuccessful = false;
                        examVM.IsRuleOk = false;
                        examVM.IsRuleNeededToBeAdded = false;
                        examVM.RuleAdditionMessage = status.FailureReason;
                        if (status.FailureReason.Contains("PLEASE ADD RULE"))
                        {
                            examVM.IsRuleNeededToBeAdded = true;
                        }
                        if (status.FailureReason.Split('^').Length > 1)
                        {
                            examVM.CourseExamId = int.Parse(status.FailureReason.Split('^')[1]);
                        }
                        examVM.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();
                        examVM.SubjectList = _uiddlRepo.getSubjectDropDown(examMarksVM.CourseExam.CourseMapping.Location.LocationId, examMarksVM.CourseExam.CourseMapping.StandardSection.StandardSectionId);

                        examVM.LocationList = _uiddlRepo.getLocationDropDown();
                        examVM.ExamTypeList = _uiddlRepo.getExamTypeDropDown();
                        examVM.ExamSubTypeList = _uiddlRepo.getExamSubTypeDropDown(examMarksVM.CourseExam.ExamType.ExamTypeId);
                        examVM.AcademicSessions = _uiddlRepo.getAcademicSessionDropDown();
                        examVM.Grades = _uiddlRepo.getGradesDropDown(examMarksVM.CourseExam.CourseMapping.Location.LocationId);
                    }
                }
                return View(examVM);
            }
            else if(string.Equals(examMarksVM.Mode, "Excel", StringComparison.OrdinalIgnoreCase))
            {
                Models.ExamMarksVM examVM = null;

                StatusDTO<List<ExamMarksDTO>> status = _examMarksSvc.GetStudentDetailsForMarksEntryExcel(examMarksVM.CourseExam.CourseMapping.Location.LocationId, examMarksVM.CourseExam.CourseMapping.StandardSection.StandardSectionId, examMarksVM.CourseExam.CourseMapping.Subject.SubjectId, DateTime.Parse(examMarksVM.FromDateString), DateTime.Parse(examMarksVM.ToDateString), examMarksVM.CourseExam.ExamType.ExamTypeId, examMarksVM.CourseExam.ExamSubType.ExamSubTypeId);

                examVM = examMarksVM;
                examVM.IsSearchSuccessful = false;
                examVM.IsRuleOk = false;
                examVM.IsRuleNeededToBeAdded = false;
                examVM.RuleAdditionMessage = status.FailureReason;
               
                examVM.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();
                examVM.SubjectList = _uiddlRepo.getSubjectDropDown(examMarksVM.CourseExam.CourseMapping.Location.LocationId, examMarksVM.CourseExam.CourseMapping.StandardSection.StandardSectionId);

                examVM.LocationList = _uiddlRepo.getLocationDropDown();
                examVM.ExamTypeList = _uiddlRepo.getExamTypeDropDown();
                examVM.ExamSubTypeList = _uiddlRepo.getExamSubTypeDropDown(examMarksVM.CourseExam.ExamType.ExamTypeId);
                examVM.AcademicSessions = _uiddlRepo.getAcademicSessionDropDown();
                examVM.Grades = _uiddlRepo.getGradesDropDown(examMarksVM.CourseExam.CourseMapping.Location.LocationId);
                return View(examVM);
            }
            else if (string.Equals(examMarksVM.Mode, "SAVE", StringComparison.OrdinalIgnoreCase))
            {
                if (examMarksVM.ExamMarksList != null && examMarksVM.ExamMarksList.Count > 0)
                {
                    List<IDbCommand> commandList = new List<IDbCommand>();
                    for (int i = 0; i < examMarksVM.ExamMarksList.Count; i++)
                    {
                        examMarksVM.ExamMarksList[i].ExamRule = new ExamRuleDTO();
                        examMarksVM.ExamMarksList[i].ExamRule.ExamRuleId = examMarksVM.RuleId;
                        //examVm.ExamMarksList[i].CourseExam = new CourseExam();
                        //examVm.ExamMarksList[i].CourseExam.CourseExamId = examVm.hd
                        if (examMarksVM.ExamMarksList[i].ExamMarksId > 0)
                        {
                            double calculatedMarks = 0.0;
                            double obtainedMarks = 0.0;
                            if (double.TryParse(examMarksVM.ExamMarksList[i].DisplayedCalculatedMarks, out calculatedMarks)
                                && double.TryParse(examMarksVM.ExamMarksList[i].DisplayedObtainedMarks, out obtainedMarks))
                            {
                                examMarksVM.ExamMarksList[i].MarksObtained = obtainedMarks;
                                examMarksVM.ExamMarksList[i].CalculatedMarks = calculatedMarks;
                                //commandList.Add(_examMarksSvc.GetUpdateMarksCommand(examMarksVM.ExamMarksList[i]));

                                //batch was not working for insertion more than 1 record so reverting previous code
                                // Marks will updated by only "Admin" Role
                                var val = _sessionSvc.GetUserSession().EntitleMentList.Find(x => x.RoleName.Contains("Admin"));
                                if (val != null)
                                {
                                    if (val.RoleName == "Admin")
                                    {
                                        _examMarksSvc.Update(examMarksVM.ExamMarksList[i]);
                                    }
                                }
                            }
                            else
                            {
                                //batch was not working for insertion more than 1 record so reverting previous code
                                _examMarksSvc.Delete(examMarksVM.ExamMarksList[i]);
                                //commandList.Add(_examMarksSvc.GetDeleteMarksCommand(examMarksVM.ExamMarksList[i]));
                            }
                            //_examMarksSvc.Update(examMarksVM.ExamMarksList[i]);
                        }
                        else
                        {
                            double calculatedMarks = 0.0;
                            double obtainedMarks = 0.0;
                            if (double.TryParse(examMarksVM.ExamMarksList[i].DisplayedCalculatedMarks, out calculatedMarks)
                                && double.TryParse(examMarksVM.ExamMarksList[i].DisplayedObtainedMarks, out obtainedMarks))
                            {
                                examMarksVM.ExamMarksList[i].MarksObtained = obtainedMarks;
                                examMarksVM.ExamMarksList[i].CalculatedMarks = calculatedMarks;
                                //commandList.Add(_examMarksSvc.GetInsertMarksCommand(examMarksVM.ExamMarksList[i], examMarksVM.CourseExamId, examMarksVM.CourseExam.CourseMapping.StandardSection.StandardSectionId, examMarksVM.CourseExam.CourseMapping.Subject.SubjectId, DateTime.Parse(examMarksVM.FromDateString), DateTime.Parse(examMarksVM.ToDateString), examMarksVM.ExamMarksList[i].DirectGrade));

                                //batch was not working for insertion more than 1 record so reverting previous code
                                _examMarksSvc.InsertMarks(examMarksVM.ExamMarksList[i], examMarksVM.CourseExamId, examMarksVM.CourseExam.CourseMapping.StandardSection.StandardSectionId, examMarksVM.CourseExam.CourseMapping.Subject.SubjectId, DateTime.Parse(examMarksVM.FromDateString), DateTime.Parse(examMarksVM.ToDateString), examMarksVM.ExamMarksList[i].DirectGrade);
                            }
                            //_examMarksSvc.InsertMarks(examMarksVM.ExamMarksList[i], examMarksVM.CourseExamId, examMarksVM.CourseExam.CourseMapping.StandardSection.StandardSectionId, examMarksVM.CourseExam.CourseMapping.Subject.SubjectId, DateTime.Parse(examMarksVM.FromDateString), DateTime.Parse(examMarksVM.ToDateString), examMarksVM.ExamMarksList[i].DirectGrade);
                        }
                    }
                    if (commandList != null && commandList.Count > 0)
                    {
                        _examMarksSvc.BatchCommandProcess(commandList);
                    }
                }

                return RedirectToAction("Register");
            }
            else
            {
                return View(examMarksVM);
            }
        }

        //[HttpPost]
        //[AllowAnonymous]
        //public JsonResult GetSubjectDropdownData(CourseMappingDTO coursemap)
        //{
        //    StatusDTO<List<SubjectDTO>> status = _examMarksSvc.GetSubjectDropdownData(coursemap.Location.LocationId, coursemap.StandardSection.StandardSectionId);
        //    if (status.IsSuccess)
        //    {               
        //        return Json(new { data = status.ReturnObj, message = "", status = true }, JsonRequestBehavior.AllowGet);
        //    }
        //    if (status.IsException)
        //    {
        //        return Json(new { data = new SubjectDTO(), message = "Exception: " + status.ExceptionMessage, status = true }, JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(new { data = new SubjectDTO(), message = "Subject is not mapped for Location and StandardSection", status = true }, JsonRequestBehavior.AllowGet);
        //}
    }
}