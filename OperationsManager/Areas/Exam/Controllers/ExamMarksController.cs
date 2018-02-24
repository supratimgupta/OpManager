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

        public ExamMarksController(IExamMarksSvc examMarksSvc, IDropdownRepo dropDwnRepo, IConfigSvc configSvc,ISessionSvc sessionSvc)
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
                return Json(new { data = status.ReturnObj, message = "", status = true}, JsonRequestBehavior.AllowGet);
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
            //if (status.IsSuccess)
            //{
            //    Exam examMarksVM = new Models.ExamMarksVM();
            //    examMarksVM.hdnExamRuleId = status.ReturnObj.ExamRuleId;
            //    return Json(new { data = status.ReturnObj, message = "", status = true }, JsonRequestBehavior.AllowGet);
            //}
            //if (status.IsException)
            //{
            //    return Json(new { data = new ExamRuleDTO(), message = "Exception: " + status.ExceptionMessage, status = true }, JsonRequestBehavior.AllowGet);
            //}
            return Json(new { data = new ExamRuleDTO(), message = "ExamRule is not registered for proper course", status = true }, JsonRequestBehavior.AllowGet);
        }
    }
}