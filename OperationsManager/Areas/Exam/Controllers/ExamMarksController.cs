using OpMgr.Common.Contracts;
using OpMgr.Common.Contracts.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OpMgr.Common.DTOs;

namespace OperationsManager.Areas.Exam.Controllers
{
    public class ExamMarksController : Controller
    {
        private IExamMarksSvc _examMarksSvc;
        private IDropdownRepo _dropDwnRepo;
        private Helpers.UIDropDownRepo _uiddlRepo;
        private IConfigSvc _configSvc;
        private ISessionSvc _sessionSvc;

        public ExamMarksController(IExamMarksSvc examMarksSvc, IDropdownRepo dropDwnRepo, IConfigSvc configSvc, ISessionSvc sessionSvc)
        {
            _examMarksSvc = examMarksSvc;
            _dropDwnRepo = dropDwnRepo;
            _configSvc = configSvc;
            _sessionSvc = sessionSvc;
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
            //examMarksVM.FromDateString = examMarksVM.FromDate ? examMarksVM.DOB.Value.ToString("dd-MMM-yyyy") : string.Empty;
            SessionDTO sessionRet = _sessionSvc.GetUserSession();

            examMarksVM.hdnEmployeeId = sessionRet.UniqueEmployeeId;

            return View(examMarksVM);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetCourseMappingDetails(CourseMappingDTO coursemap)
        {
            StatusDTO<CourseMappingDTO> status = _examMarksSvc.GetCourseMappingDetails(coursemap);
            if (status.IsSuccess)
            {
                Models.ExamMarksVM examMarksVM = new Models.ExamMarksVM();
                examMarksVM.hdncoursemapid = status.ReturnObj.CourseMappingId;
                examMarksVM.FromDateString = status.ReturnObj.CourseFrom.ToShortDateString();
                examMarksVM.ToDateString = status.ReturnObj.CourseTo.ToShortDateString();
                return Json(new { data = status.ReturnObj, message = "", status = true, examMarksVM.FromDateString, examMarksVM.ToDateString }, JsonRequestBehavior.AllowGet);
            }
            if (status.IsException)
            {
                return Json(new { data = new CourseMappingDTO(), message = "Exception: " + status.ExceptionMessage, status = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = new CourseMappingDTO(), message = "Course is not registered or you doesn't have access to this course", status = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetExamRuleDetails(CourseExam courseexam)
        {
            StatusDTO<ExamRuleDTO> status = _examMarksSvc.GetExamRuleDetails(courseexam);
            if (status.IsSuccess)
            {
                Models.ExamMarksVM examMarksVM = new Models.ExamMarksVM();
                examMarksVM.hdnExamRuleId = status.ReturnObj.ExamRuleId;
                return Json(new { data = status.ReturnObj, message = "", status = true }, JsonRequestBehavior.AllowGet);
            }
            if (status.IsException)
            {
                return Json(new { data = new ExamRuleDTO(), message = "Exception: " + status.ExceptionMessage, status = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = new ExamRuleDTO(), message = "ExamRule is not registered for proper course", status = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetStudentDetailsMarks(CourseMappingDTO coursemap)
        {

            StatusDTO<List<ExamMarksDTO>> status = _examMarksSvc.GetStudentDetailsForMarksEntry(coursemap.Location.LocationId, coursemap.StandardSection.StandardSectionId);
            if (status.IsSuccess)
            {
                if (status.ReturnObj != null && status.ReturnObj.Count > 0)
                {
                    Models.ExamMarksVM exammarksvm = null;
                    foreach (ExamMarksDTO exammarksdto in status.ReturnObj)
                    {
                        if (exammarksdto != null)
                        {
                            exammarksvm = new Models.ExamMarksVM();
                            exammarksvm.ExamMarksId = exammarksdto.ExamMarksId;

                            exammarksvm.CourseExam = new CourseExam();
                            exammarksvm.CourseExam.CourseExamId = exammarksdto.CourseExam.CourseExamId;
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
                            exammarksvm.Student.UserDetails.FName = exammarksdto.Student.UserDetails.FName;
                            exammarksvm.Student.UserDetails.LName = exammarksdto.Student.UserDetails.LName;
                            exammarksvm.Student.UserDetails.Location.LocationDescription = exammarksdto.Student.UserDetails.Location.LocationDescription;

                        }
                    }

                }




                return Json(new { data = status.ReturnObj, message = "", status = true }, JsonRequestBehavior.AllowGet);
            }
            if (status.IsException)
            {
                return Json(new { data = new ExamRuleDTO(), message = "Exception: " + status.ExceptionMessage, status = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = new ExamRuleDTO(), message = "ExamRule is not registered for proper course", status = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(Models.ExamMarksVM examMarksVM)
        {
            Models.ExamMarksVM examVM = null;
            //ExamMarksDTO exammarksdto = null;
            if (examMarksVM != null)
            {
                //exammarksdto.CourseExam = new CourseExam();
                //exammarksdto.CourseExam.CourseMapping = new CourseMappingDTO();
                //exammarksdto.CourseExam.CourseMapping.Location = new LocationDTO();
                //exammarksdto.CourseExam.CourseMapping.StandardSection = new StandardSectionMapDTO();
                //exammarksdto.CourseExam.CourseMapping.Location.LocationId = examMarksVM.CourseExam.CourseMapping.Location.LocationId;
                //exammarksdto.CourseExam.CourseMapping.StandardSection.StandardSectionId = examMarksVM.CourseExam.CourseMapping.StandardSection.StandardSectionId;

                StatusDTO<List<ExamMarksDTO>> status = _examMarksSvc.GetStudentDetailsForMarksEntry(examMarksVM.CourseExam.CourseMapping.Location.LocationId, examMarksVM.CourseExam.CourseMapping.StandardSection.StandardSectionId);

                if (status.IsSuccess)
                {
                    if (status.ReturnObj != null && status.ReturnObj.Count > 0)
                    {
                        examVM = new Models.ExamMarksVM();
                        examVM.ExamMarksList = new List<Models.ExamMarksVM>();

                        examVM.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();
                        examVM.SubjectList = _uiddlRepo.getSubjectDropDown();
                        examVM.LocationList = _uiddlRepo.getLocationDropDown();
                        examVM.ExamTypeList = _uiddlRepo.getExamTypeDropDown();
                        examVM.ExamSubTypeList = _uiddlRepo.getExamSubTypeDropDown();

                        Models.ExamMarksVM exammarksvm = null;
                        foreach (ExamMarksDTO exammarksdto in status.ReturnObj)
                        {
                            if (exammarksdto != null)
                            {
                                exammarksvm = new Models.ExamMarksVM();
                                exammarksvm.ExamMarksId = exammarksdto.ExamMarksId;

                                exammarksvm.CourseExam = new CourseExam();
                                exammarksvm.CourseExam.CourseExamId = exammarksdto.CourseExam.CourseExamId;
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
                    examMarksVM.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();
                    examMarksVM.SubjectList = _uiddlRepo.getSubjectDropDown();
                    examMarksVM.LocationList = _uiddlRepo.getLocationDropDown();
                    examMarksVM.ExamTypeList = _uiddlRepo.getExamTypeDropDown();
                    examMarksVM.ExamSubTypeList = _uiddlRepo.getExamSubTypeDropDown();
                }
            }
            return View(examVM);
            //return RedirectToAction("SubmitMarks", "ExamMarks", examVM);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult SubmitMarks(Models.ExamMarksVM examVm)
        {
            if(examVm.IsSearchSuccessful)
            {
               if (examVm.ExamMarksList !=null && examVm.ExamMarksList.Count > 0)
                {
                    for(int i=0; i < examVm.ExamMarksList.Count; i ++)
                    {
                        if(examVm.ExamMarksList[i].ExamMarksId > 0)
                        {

                        }
                        else
                        {

                        }
                    }
                }
            }
            return RedirectToAction("Register");
        }
    }
}