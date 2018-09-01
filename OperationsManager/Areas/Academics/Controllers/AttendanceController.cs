using OperationsManager.Areas.Academics.Models;
using OperationsManager.Attributes;
using OperationsManager.Controllers;
using OpMgr.Common.Contracts;
using OpMgr.Common.Contracts.Modules;
using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationsManager.Areas.Academics.Controllers
{
    public class AttendanceController : BaseController
    {
        private IAttendanceSvc _studSvc;        
        private IDropdownRepo _dropDwnRepo;
        private ISessionSvc _sessionSvc;
        private Helpers.UIDropDownRepo _uiddlRepo;
        Encryption encrypt = new Encryption();        
        private IConfigSvc _configSvc;

        public AttendanceController(IAttendanceSvc studSvc, IDropdownRepo dropDwnRepo, IUserTransactionSvc userTrans, IConfigSvc configSvc, ISessionSvc sessionSvc)
        {
            _studSvc = studSvc;
            _dropDwnRepo = dropDwnRepo;
            _uiddlRepo = new Helpers.UIDropDownRepo(_dropDwnRepo);            
            _configSvc = configSvc;
            _sessionSvc = sessionSvc;
        }

        [HttpGet]
        public ActionResult AttendanceCollection()
        {
            AttendanceVM attnView = new AttendanceVM();

            //Fetch the StandardSection List
            attnView.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();
            //fetch the location list
            attnView.LocationList = _uiddlRepo.getLocationDropDown();

            return View(attnView);
        }


        [HttpPost]
        public ActionResult AttendanceCollection(AttendanceVM studentView, string Command)
        {
            int StandardSectionId = 0;
            int currentSerial = 0;
            int Serial = 0;
            AttendanceVM studView = null;
            List<StudentDTO> studListPass = null;
            int LocationId = 0;
            string AbsentStudentInfoIds = string.Empty;

            if (Command != null)
            {
                // storing all data in studPass object and Passing it to PromoteToNewClass()
                studListPass = new List<StudentDTO>();

                //if (string.Equals(Command, "Apply Attendance"))
                //{
                //    StatusDTO<List<StudentDTO>> batchStatus = _studSvc.MarkAttendance(null, Command, 0, 0);

                //    if (batchStatus.ReturnObj != null)
                //    {
                //        return RedirectToAction("AttendanceCollection");
                //    }
                //}

                // To store StandardSectionId for Promote and Standard
                if (string.Equals(Command, "Promote"))
                {
                    StandardSectionId = (int)TempData.Peek("StandardSection");
                    //currentStandardId = StandardSectionId;
                }
                else if (string.Equals(Command, "Standard"))
                {
                    StandardSectionId = studentView.StandardSectionMap.StandardSectionId;
                    TempData["StandardSection"] = StandardSectionId;

                    LocationId = studentView.UserDetails.Location.LocationId;
                    TempData["LocationId"] = LocationId;
                }


                //For Storing exact string values instead of keys in database
                if (studentView != null && (string.Equals(Command, "Apply Attendance")))
                {
                    LocationId = (int)TempData.Peek("LocationId");
                    StandardSectionId = (int)TempData.Peek("StandardSection");
                    
                    foreach (AttendanceVM studVM in studentView.studentList)
                    {
                        StudentDTO student = new StudentDTO();
                        student.StudentInfoId = studVM.StudentInfoId;

                        if (string.Equals(studVM.Status, "1"))
                        {
                            student.Status = "Present";                            
                        }
                        else if (string.Equals(studVM.Status, "2"))
                        {
                            student.Status = "Absent";
                            AbsentStudentInfoIds= AbsentStudentInfoIds + studVM.StudentInfoId + ",";
                        }
                        studListPass.Add(student);
                    }

                    AbsentStudentInfoIds = AbsentStudentInfoIds.Substring(0, AbsentStudentInfoIds.Length - 1);
                }

                //Get Students for that Particular class or Promote Students to New Class
                StatusDTO<List<StudentDTO>> status = _studSvc.MarkAttendance(AbsentStudentInfoIds, Command, StandardSectionId, LocationId);

                if ((status.ReturnObj != null))
                {
                    studView = new AttendanceVM(); // Instantiating Student View model As a Parent Whole
                    studView.studentList = new List<AttendanceVM>(); // instantiating list of Students

                    //Binding the Standard Section Id which is returned from the database
                    studView.StandardSectionMap = new StandardSectionMapDTO();
                    studView.StandardSectionMap.StandardSectionId = StandardSectionId;

                    //Fetch the StandardSection List for Upper Dropdown
                    studView.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();

                    //fetch the location list
                    studView.LocationList = _uiddlRepo.getLocationDropDown();

                    //Fetch the Promotion Status List 
                    if (string.Equals(Command, "Standard"))
                    {
                        studView.AttendanceList = _uiddlRepo.getAttendanceStatusDropDown();
                    }

                    if (status.IsSuccess && !status.IsException)
                    {
                        AttendanceVM studentV = null; // for each student
                        foreach (StudentDTO stud in status.ReturnObj)
                        {
                            if (stud != null)
                            {
                                studentV = new AttendanceVM(); // instantiating each student

                                studentV.Active = stud.Active;
                                studentV.RollNumber = stud.RollNumber;
                                studentV.StudentInfoId = stud.StudentInfoId;

                                studentV.Status = stud.Status;
                                studentV.NewStandardSectionId = stud.NewStandardSectionId;


                                //Fetch the Next StandardSectionList w.r.t Current
                                if (string.Equals(Command, "Standard"))
                                {
                                    currentSerial = stud.StandardSectionMap.Serial;
                                }

                                //Fetch New Standard and Section if they are assigned
                                studentV.StandardSectionMap = new StandardSectionMapDTO();
                                studentV.StandardSectionMap.Standard = new StandardDTO();
                                studentV.StandardSectionMap.Section = new SectionDTO();

                                studentV.StandardSectionMap.Standard.StandardName = stud.StandardSectionMap.Standard.StandardName;
                                studentV.StandardSectionMap.Section.SectionName = studentV.StandardSectionMap.Section.SectionName;

                                studentV.NewStandardSectionMap = stud.NewStandardSectionMap;


                                studentV.UserDetails = new UserMasterDTO();
                                studentV.UserDetails.UserMasterId = stud.UserDetails.UserMasterId;
                                studentV.UserDetails.FName = stud.UserDetails.FName;
                                studentV.UserDetails.MName = stud.UserDetails.MName;
                                studentV.UserDetails.LName = stud.UserDetails.LName;

                                studentV.UserDetails.Location = stud.UserDetails.Location;

                                studentV.Location = studentV.UserDetails.Location.LocationDescription;

                                studentV.Name = studentV.UserDetails.FName;
                                if (!string.IsNullOrEmpty(studentV.UserDetails.MName))
                                {
                                    studentV.Name = studentV.Name + " " + studentV.UserDetails.MName;
                                }

                                studentV.Name = studentV.Name + " " + studentV.UserDetails.LName;

                                //Add Into Student View Model List
                                studView.studentList.Add(studentV);
                                studView.IsSearchSuccessful = true;

                                //if (!string.IsNullOrEmpty(studentV.Status))
                                //    studView.IsCommandPromote = true;
                            }
                        }
                    }

                }
            }
            //if (string.Equals(Command, "Standard"))
            //{
            //    studView.NextStandardSectionList = _uiddlRepo.getNextStandardSectionDropDown(currentSerial);
            //}
            return View(studView);
        }

        //GET: Academics/Attendance
        public ActionResult Index()
        {
            return View();
        }
    }
}