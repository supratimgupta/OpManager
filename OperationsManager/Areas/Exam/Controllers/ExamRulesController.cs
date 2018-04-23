using OperationsManager.Areas.Exam.Models;
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
    public class ExamRulesController : Controller
    {

        private IExamRuleSvc _examRuleSvc;
        private IConfigSvc _configSvc;
        private ISessionSvc _sessionSvc;
        public ExamRulesController(IExamRuleSvc examRuleSvc, IConfigSvc configSvc, ISessionSvc sessionSvc)
        {
            _examRuleSvc = examRuleSvc;
            _configSvc = configSvc;
            _sessionSvc = sessionSvc;
        }

        // For mode ADD rowId is CourseExamId, for mode edit rowId is ExamRuleId
        public ActionResult Rule(int rowId, string mode)
        {
            ExamRuleVM examRuleVM = new ExamRuleVM();
            if (rowId < 0 || string.IsNullOrEmpty(mode))
            {
                examRuleVM.IsError = true;
                examRuleVM.ErrorMessage = "MODE or COURSE EXAM ID is blank in URL.";
                return View(examRuleVM);
            }
            examRuleVM.CourseExam = new OpMgr.Common.DTOs.CourseExamDTO();
            examRuleVM.CourseExam.CourseExamId = rowId;
            if(string.Equals(mode, "ADD", StringComparison.OrdinalIgnoreCase) && _examRuleSvc.Select(examRuleVM).ReturnObj!=null)
            {
                mode = "EDIT";
            }
            if (string.Equals(mode, "EDIT", StringComparison.OrdinalIgnoreCase))
            {
                ExamRuleDTO erDTO = _examRuleSvc.Select(rowId).ReturnObj;
                if(erDTO==null)
                {
                    mode = "ADD";
                }
                else
                {
                    examRuleVM.ExamRuleId = erDTO.ExamRuleId;
                    examRuleVM.AssesmentMarks = erDTO.AssesmentMarks;
                    examRuleVM.ActualFullMarks = erDTO.ActualFullMarks;
                    examRuleVM.PassMarks = erDTO.PassMarks;
                    examRuleVM.Active = erDTO.Active;
                }
            }
            examRuleVM.IsError = false;
            examRuleVM.ErrorMessage = string.Empty;
            examRuleVM.Mode = mode;
            return View(examRuleVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Rule(ExamRuleVM examRuleVM)
        {
            StatusDTO<ExamRuleDTO> status = new StatusDTO<ExamRuleDTO>();
            examRuleVM.IsError = false;
            examRuleVM.ErrorMessage = string.Empty;
            if(string.Equals(examRuleVM.Mode, "ADD", StringComparison.OrdinalIgnoreCase))
            {
                examRuleVM.CreatedBy = new UserMasterDTO();
                examRuleVM.CreatedBy.UserMasterId = _sessionSvc.GetUserSession().UserMasterId;
                status = _examRuleSvc.Insert(examRuleVM);
            }
            else if(string.Equals(examRuleVM.Mode, "EDIT", StringComparison.OrdinalIgnoreCase))
            {
                examRuleVM.Active = true;
                examRuleVM.UpdatedBy = new UserMasterDTO();
                examRuleVM.UpdatedBy.UserMasterId = _sessionSvc.GetUserSession().UserMasterId;
                status = _examRuleSvc.Update(examRuleVM);
            }
            if(!status.IsSuccess)
            {
                examRuleVM.IsError = true;
                examRuleVM.ErrorMessage = "ADD / EDIT FAILED!!!";
            }
            return View(examRuleVM);
        }
    }
}