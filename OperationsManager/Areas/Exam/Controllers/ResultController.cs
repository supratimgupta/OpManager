using OperationsManager.Controllers;
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
    public class ResultController : Controller
    {
        private IResultSvc _resultSvc;
        private IDropdownRepo _dropDwnRepo;
        private Helpers.UIDropDownRepo _uiddlRepo;
        private IConfigSvc _configSvc;
        private ISessionSvc _sessionSvc;

        public ResultController(IResultSvc resultSvc, IDropdownRepo dropDwnRepo, IConfigSvc configSvc, ISessionSvc sessionSvc)
        {
            _resultSvc = resultSvc;
            _dropDwnRepo = dropDwnRepo;
            _configSvc = configSvc;
            _sessionSvc = sessionSvc;
            _uiddlRepo = new Helpers.UIDropDownRepo(_dropDwnRepo);
        }

        // GET: Exam/Result
        public ActionResult Results()
        {
            Models.ResultVM resultVM = new Models.ResultVM();
            resultVM.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();
            resultVM.LocationList = _uiddlRepo.getLocationDropDown();
            resultVM.ExamTypeList = _uiddlRepo.getExamTypeDropDown();
            resultVM.AcademicSessions = _uiddlRepo.getAcademicSessionDropDown();
            return View(resultVM);
        }

        [HttpPost]
        public ActionResult Results(Models.ResultVM result)
        {
            List<ResultCardDTO> resultCards = _resultSvc.GetResult(result.SelectedLocation, result.SelectedStandardSection, new List<int> { 2 }, DateTime.Parse(result.SelectedAcademicSession.Split(';')[0]), DateTime.Parse(result.SelectedAcademicSession.Split(';')[1]));
            result.ResultCards = resultCards;
            result.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();
            result.LocationList = _uiddlRepo.getLocationDropDown();
            result.ExamTypeList = _uiddlRepo.getExamTypeDropDown();
            result.AcademicSessions = _uiddlRepo.getAcademicSessionDropDown();
            return View(result);
        }
    }
}