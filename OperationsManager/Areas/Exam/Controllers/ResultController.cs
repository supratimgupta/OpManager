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
            //resultVM.ExamTypeList = _uiddlRepo.getExamTypeDropDown();
            resultVM.ResultTypes = _uiddlRepo.getResultTypeDropDown();
            resultVM.AcademicSessions = _uiddlRepo.getAcademicSessionDropDown();
            return View(resultVM);
        }

        [HttpPost]
        public ActionResult Results(Models.ResultVM result)
        {
            List<ResultCardDTO> resultCards = _resultSvc.GetResult(result.SelectedLocation, result.SelectedStandardSection, result.SelectedResultType, DateTime.Parse(result.SelectedAcademicSession.Split(';')[0]), DateTime.Parse(result.SelectedAcademicSession.Split(';')[1]));
            result.ResultCards = resultCards;
            result.ClassAverage = Math.Round(resultCards.Average(rc => rc.TotalMarks));
            result.ClassHeighest = Math.Round(resultCards.Max(rc => rc.TotalMarks));
            result.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();
            result.LocationList = _uiddlRepo.getLocationDropDown();
            //result.ExamTypeList = _uiddlRepo.getExamTypeDropDown();
            result.ResultTypes = _uiddlRepo.getResultTypeDropDown();
            result.AcademicSessions = _uiddlRepo.getAcademicSessionDropDown();
            return View(result);
        }
    }
}