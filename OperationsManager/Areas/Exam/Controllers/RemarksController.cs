using OpMgr.Common.Contracts;
using OpMgr.Common.Contracts.Modules;
using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationsManager.Areas.Exam.Controllers
{
    public class RemarksController : Controller
    {
        private IStudentRemarksSvc _remarksSvc;
        private IDropdownRepo _dropDwnRepo;
        private Helpers.UIDropDownRepo _uiddlRepo;
        private IConfigSvc _configSvc;
        private ISessionSvc _sessionSvc;

        public RemarksController(IStudentRemarksSvc remarksSvc, IDropdownRepo dropDwnRepo, IConfigSvc configSvc, ISessionSvc sessionSvc)
        {
            _remarksSvc = remarksSvc;
            _dropDwnRepo = dropDwnRepo;
            _configSvc = configSvc;
            _sessionSvc = sessionSvc;
            _uiddlRepo = new Helpers.UIDropDownRepo(_dropDwnRepo);
        }

        // GET: Exam/Remarks
        public ActionResult Remarks()
        {
            Models.StudentRemarksVM remarksVM = new Models.StudentRemarksVM();
            remarksVM.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();
            remarksVM.LocationList = _uiddlRepo.getLocationDropDown();
            //resultVM.ExamTypeList = _uiddlRepo.getExamTypeDropDown();
            remarksVM.ResultTypes = _uiddlRepo.getResultTypeDropDown();
            remarksVM.AcademicSessions = _uiddlRepo.getAcademicSessionDropDown();
            remarksVM.SearchResult = null;
            remarksVM.MODE = "SEARCH";
            return View(remarksVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Remarks(Models.StudentRemarksVM remarks)
        {
            if(string.Equals(remarks.MODE, "SEARCH", StringComparison.OrdinalIgnoreCase))
            {
                remarks.SearchResult = _remarksSvc.GetStudentRemarks(remarks.SelectedStandardSection, remarks.SelectedResultType, DateTime.Parse(remarks.SelectedAcademicSession.Split(';')[0]), DateTime.Parse(remarks.SelectedAcademicSession.Split(';')[1]), remarks.SelectedLocation);
                remarks.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();
                remarks.LocationList = _uiddlRepo.getLocationDropDown();
                //resultVM.ExamTypeList = _uiddlRepo.getExamTypeDropDown();
                remarks.ResultTypes = _uiddlRepo.getResultTypeDropDown();
                remarks.AcademicSessions = _uiddlRepo.getAcademicSessionDropDown();
                remarks.MODE = "SEARCH";
                return View(remarks);
            }
            else if(string.Equals(remarks.MODE, "SAVE", StringComparison.OrdinalIgnoreCase))
            {
                foreach(StudentRemarksDTO remark in remarks.SearchResult)
                {
                    try
                    {
                        remark.ExamResultType = remarks.SelectedResultType;
                        remark.CourseFromDate = DateTime.Parse(remarks.SelectedAcademicSession.Split(';')[0]);
                        remark.CourseToDate = DateTime.Parse(remarks.SelectedAcademicSession.Split(';')[1]);
                        if (remark.StudentRemarksId > 0)
                        {
                            _remarksSvc.UpdateStudentRemarks(remark);
                        }
                        else
                        {
                            _remarksSvc.InsertStudentRemarks(remark);
                        }
                    }
                    catch { }
                }
            }
            return RedirectToAction("Remarks");
        }
    }
}