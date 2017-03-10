using OperationsManager.Areas.Student.Models;
using OpMgr.Common.Contracts;
using OpMgr.Common.Contracts.Modules;
using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationsManager.Areas.Student.Controllers
{
    [HandleError()]
    public class StudentController : Controller
    {
        private IStudentSvc _studSvc;
        private ILogSvc _logSvc;

        public StudentController(IStudentSvc studSvc)
        {
            _studSvc = studSvc;
            //_logSvc = logSvc;
        }
        // GET: Student/Student
        [HttpGet]
        public ActionResult Search()
        {
            StatusDTO<List<StudentDTO>> status = _studSvc.Select(null);
            List<StudentVM> studViewList = null;

            if (status.ReturnObj != null && status.ReturnObj.Count > 0)
            {


                if (status.IsSuccess && !status.IsException)
                {
                    studViewList = new List<StudentVM>();
                    foreach (StudentDTO student in status.ReturnObj)
                    {
                        if (student != null)
                        {
                            StudentVM studView = new StudentVM();

                            studView.Active = student.Active;
                            studView.GuardianContact = student.GuardianContact;
                            studView.RegistrationNumber = student.RegistrationNumber;
                            studView.RollNumber = student.RollNumber;


                            studView.UserDetails = new UserMasterDTO();
                            studView.UserDetails.FName = student.UserDetails.FName;
                            studView.UserDetails.LName = student.UserDetails.LName;

                            studView.Name = studView.UserDetails.FName;
                            if (!string.IsNullOrEmpty(studView.UserDetails.MName))
                            {
                                studView.Name = studView.Name + " " + studView.UserDetails.MName;
                            }
                            else
                            {
                                studView.Name = studView.Name + " " + studView.UserDetails.LName;
                            }

                            studView.StandardSectionMap = new StandardSectionMapDTO();
                            studView.StandardSectionMap.Standard = new StandardDTO();
                            studView.StandardSectionMap.Section = new SectionDTO();


                            studView.StandardSectionMap.Standard.StandardName = student.StandardSectionMap.Standard.StandardName;
                            studView.StandardSectionMap.Section.SectionName = student.StandardSectionMap.Section.SectionName;

                            //Add into Student vIew Model List
                            studViewList.Add(studView);

                        }
                    }


                }
                if (status.IsException)
                {
                    throw new Exception(status.ExceptionMessage);
                }
            }

            return View(studViewList);
        }
        [HttpGet]
        public ActionResult Edit(int studentId)
        {
            return View();
        }
    }
}