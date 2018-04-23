﻿using OpMgr.Common.Contracts;
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
            //examMarksVM.FromDateString = examMarksVM.FromDate ? examMarksVM.DOB.Value.ToString("dd-MMM-yyyy") : string.Empty;
            SessionDTO sessionRet = _sessionSvc.GetUserSession();
            examMarksVM.IsRuleOk = true;
            examMarksVM.IsRuleNeededToBeAdded = false;
            examMarksVM.Rule = null;
            examMarksVM.Mode = "SEARCH";
            return View(examMarksVM);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(Models.ExamMarksVM examMarksVM)
        {
            if(string.Equals(examMarksVM.Mode,"SEARCH", StringComparison.OrdinalIgnoreCase))
            {
                Models.ExamMarksVM examVM = null;
                //ExamMarksDTO exammarksdto = null;
                if (examMarksVM != null)
                {
                    examVM = new Models.ExamMarksVM();

                    examVM.StandardSectionId = examMarksVM.CourseExam.CourseMapping.StandardSection.StandardSectionId;
                    examVM.SubjectId = examMarksVM.CourseExam.CourseMapping.Subject.SubjectId;

                    StatusDTO<List<ExamMarksDTO>> status = _examMarksSvc.GetStudentDetailsForMarksEntry(examMarksVM.CourseExam.CourseMapping.Location.LocationId, examMarksVM.CourseExam.CourseMapping.StandardSection.StandardSectionId, examMarksVM.CourseExam.CourseMapping.Subject.SubjectId, DateTime.Parse(examMarksVM.FromDateString), DateTime.Parse(examMarksVM.ToDateString));

                    if (status.IsSuccess)
                    {
                        if (status.ReturnObj != null && status.ReturnObj.Count > 0)
                        {
                            examVM.ExamMarksList = new List<Models.ExamMarksVM>();

                            examVM.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();
                            examVM.SubjectList = _uiddlRepo.getSubjectDropDown();
                            examVM.LocationList = _uiddlRepo.getLocationDropDown();
                            examVM.ExamTypeList = _uiddlRepo.getExamTypeDropDown();
                            examVM.ExamSubTypeList = _uiddlRepo.getExamSubTypeDropDown();

                            examVM.Rule = null;

                            Models.ExamMarksVM exammarksvm = null;
                            ExamMarksDTO exammarksdto = null;
                            for (int i = 0; i < status.ReturnObj.Count; i++)
                            {
                                exammarksdto = status.ReturnObj[i];
                                if (exammarksdto != null)
                                {
                                    exammarksvm = new Models.ExamMarksVM();
                                    exammarksvm.ExamMarksId = exammarksdto.ExamMarksId;
                                    if (exammarksdto.MarksObtained > 0)
                                    {
                                        exammarksvm.MarksObtained = exammarksdto.MarksObtained;
                                    }
                                    if (exammarksdto.CalculatedMarks > 0)
                                    {
                                        exammarksvm.CalculatedMarks = exammarksdto.CalculatedMarks;
                                    }

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
                                            examVM.RuleAdditionMessage = "More than 1 rule added for this exam. Please contact dev team to fix this.";
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

                        if (examVM.Rule == null)
                        {
                            ExamRuleDTO rule = new ExamRuleDTO();
                            rule.CourseExam = new CourseExamDTO();
                            string result = _examMarksSvc.GetCourseExamId(examMarksVM.CourseExam.CourseMapping.Location.LocationId, examMarksVM.CourseExam.CourseMapping.StandardSection.StandardSectionId, examMarksVM.CourseExam.CourseMapping.Subject.SubjectId, DateTime.Parse(examVM.FromDateString), DateTime.Parse(examVM.ToDateString));
                            int test = -1;
                            if (int.TryParse(result, out test))
                            {
                                rule.CourseExam.CourseExamId = test;
                                List<ExamRuleDTO> rules = _examRuleSvc.Select(rule).ReturnObj;

                                if (rules == null || rules.Count == 0)
                                {
                                    examVM.IsRuleOk = false;
                                    examVM.RuleAdditionMessage = "Please add marks rule for this exam first.";
                                    examVM.IsRuleNeededToBeAdded = true;
                                }
                                else if (rules.Count > 1)
                                {
                                    examVM.IsRuleOk = false;
                                    examVM.RuleAdditionMessage = "More than 1 rule added for this exam. Please contact dev team to fix this.";
                                    examVM.IsRuleNeededToBeAdded = false;
                                }
                                else
                                {
                                    examVM.IsRuleOk = true;
                                    examVM.Rule = rules[0];
                                    examVM.RuleAdditionMessage = string.Empty;
                                    examVM.IsRuleNeededToBeAdded = false;
                                }

                                examVM.CourseExam = new CourseExam();
                                examVM.CourseExam.CourseExamId = test;
                            }
                            else
                            {
                                examVM.IsRuleOk = false;
                                examVM.RuleAdditionMessage = "Course mapping and course exam has not been setup properly, please contact dev team.";
                                examVM.Rule = null;
                                //examVM.IsRuleNeededToBeAdded = true;
                            }
                        }

                        examVM.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();
                        examVM.SubjectList = _uiddlRepo.getSubjectDropDown();
                        examVM.LocationList = _uiddlRepo.getLocationDropDown();
                        examVM.ExamTypeList = _uiddlRepo.getExamTypeDropDown();
                        examVM.ExamSubTypeList = _uiddlRepo.getExamSubTypeDropDown();
                    }
                }
                return View(examVM);
            }
            else if(string.Equals(examMarksVM.Mode,"SAVE", StringComparison.OrdinalIgnoreCase))
            {
                if (examMarksVM.ExamMarksList != null && examMarksVM.ExamMarksList.Count > 0)
                {
                    for (int i = 0; i < examMarksVM.ExamMarksList.Count; i++)
                    {
                        examMarksVM.ExamMarksList[i].ExamRule = new ExamRuleDTO();
                        examMarksVM.ExamMarksList[i].ExamRule.ExamRuleId = examMarksVM.RuleId;
                        //examVm.ExamMarksList[i].CourseExam = new CourseExam();
                        //examVm.ExamMarksList[i].CourseExam.CourseExamId = examVm.hd
                        if (examMarksVM.ExamMarksList[i].ExamMarksId > 0)
                        {
                            _examMarksSvc.Update(examMarksVM.ExamMarksList[i]);
                        }
                        else
                        {
                            _examMarksSvc.InsertMarks(examMarksVM.ExamMarksList[i], examMarksVM.CourseExamId, examMarksVM.CourseExam.CourseMapping.StandardSection.StandardSectionId, examMarksVM.CourseExam.CourseMapping.Subject.SubjectId, DateTime.Parse(examMarksVM.FromDateString), DateTime.Parse(examMarksVM.ToDateString));
                        }
                    }
                }

                return RedirectToAction("Register");
            }
            else
            {
                return View(examMarksVM);
            }
        }
    }
}