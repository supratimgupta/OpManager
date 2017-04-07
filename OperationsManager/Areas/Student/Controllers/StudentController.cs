using OperationsManager.Areas.Student.Models;
using OperationsManager.Attributes;
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
   // [OpMgrAuth]
    [HandleError()]
    public class StudentController : Controller
    {
        private IStudentSvc _studSvc;
        private ILogSvc _logSvc;
        private IDropdownRepo _dropDwnRepo;
        private Helpers.UIDropDownRepo _uiddlRepo;
        

        public StudentController(IStudentSvc studSvc, IDropdownRepo dropDwnRepo)
        {
            _studSvc = studSvc;
            _dropDwnRepo = dropDwnRepo;
            _uiddlRepo = new Helpers.UIDropDownRepo(_dropDwnRepo);
            //_logSvc = logSvc;
        }
        // GET: Student/Student
        [HttpGet]
        public ActionResult Search()
        {
            StatusDTO<List<StudentDTO>> status = _studSvc.Select(null);
            StudentVM studView = null; 

            if (status.ReturnObj != null && status.ReturnObj.Count > 0)
            {
                studView = new StudentVM(); // Instantiating Student View model
                studView.studentList = new List<StudentVM>(); // instantiating list of Students

                //Fetch the StandardSection List
                studView.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();

                if (status.IsSuccess && !status.IsException)
                {
                    //studView = new List<StudentVM>();

                    StudentVM searchItem = null;
                    foreach (StudentDTO student in status.ReturnObj)
                    {
                        if (student != null)
                        {
                            searchItem = new StudentVM(); // instantiating each student

                            searchItem.Active = student.Active;
                            searchItem.GuardianContact = student.GuardianContact;
                            searchItem.RegistrationNumber = student.RegistrationNumber;
                            searchItem.RollNumber = student.RollNumber;


                            searchItem.UserDetails = new UserMasterDTO();
                            searchItem.UserDetails.UserMasterId = student.UserDetails.UserMasterId;
                            searchItem.UserDetails.FName = student.UserDetails.FName;
                            searchItem.UserDetails.MName = student.UserDetails.MName;
                            searchItem.UserDetails.LName = student.UserDetails.LName;

                            searchItem.Name = searchItem.UserDetails.FName;
                            if (!string.IsNullOrEmpty(searchItem.UserDetails.MName))
                            {
                                searchItem.Name = searchItem.Name + " " + searchItem.UserDetails.MName;
                            }
                            
                                searchItem.Name = searchItem.Name + " " + searchItem.UserDetails.LName;
                           

                            searchItem.StandardSectionMap = new StandardSectionMapDTO();
                            searchItem.StandardSectionMap.Standard = new StandardDTO();
                            searchItem.StandardSectionMap.Section = new SectionDTO();


                            searchItem.StandardSectionMap.Standard.StandardName = student.StandardSectionMap.Standard.StandardName;
                            searchItem.StandardSectionMap.Section.SectionName = student.StandardSectionMap.Section.SectionName;

                            //Add into Student vIew Model List
                            studView.studentList.Add(searchItem);
                            studView.IsSearchSuccessful = true;

                        }
                    }


                }
                if (status.IsException)
                {
                    throw new Exception(status.ExceptionMessage);
                }
            }

            return View(studView);
        }
        [HttpGet]
        public ActionResult Edit(int studentId)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(StudentVM studentView)
        {
            StudentVM studView = null;
            StudentDTO student = null;

            //Fetch the StandardSection List
            studentView.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();



            if (studentView!=null)
           {
                student = new StudentDTO();
                student.UserDetails = new UserMasterDTO();

                // Search for FName LName and MName

                student.UserDetails.FName = studentView.UserDetails.FName;
                student.UserDetails.MName = studentView.UserDetails.MName;
                student.UserDetails.LName = studentView.UserDetails.LName;


                student.StandardSectionMap = new StandardSectionMapDTO();
                student.StandardSectionMap.Standard = new StandardDTO();
                student.StandardSectionMap.Section = new SectionDTO();

                // Search for Class

                student.StandardSectionMap.StandardSectionId = studentView.StandardSectionMap.StandardSectionId;

                // Search for Roll and Registration

                student.RollNumber = studentView.RollNumber;
                student.RegistrationNumber = studentView.RegistrationNumber;

                StatusDTO<List<StudentDTO>> status = _studSvc.Select(student);
                

                if (status.ReturnObj != null && status.ReturnObj.Count > 0)
                {
                    studView = new StudentVM(); // Instantiating Student View model
                    studView.studentList = new List<StudentVM>(); // instantiating list of Students

                    //Fetch the StandardSection List
                    studView.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();

                    if (status.IsSuccess && !status.IsException)
                    {
                        //studView = new List<StudentVM>();

                        StudentVM searchItem = null;
                        foreach (StudentDTO stud in status.ReturnObj)
                        {
                            if (stud != null)
                            {
                                searchItem = new StudentVM(); // instantiating each student

                                searchItem.Active = stud.Active;
                                searchItem.GuardianContact = stud.GuardianContact;
                                searchItem.RegistrationNumber = stud.RegistrationNumber;
                                searchItem.RollNumber = stud.RollNumber;


                                searchItem.UserDetails = new UserMasterDTO();
                                searchItem.UserDetails.UserMasterId = student.UserDetails.UserMasterId;
                                searchItem.UserDetails.FName = stud.UserDetails.FName;
                                searchItem.UserDetails.MName = stud.UserDetails.MName;
                                searchItem.UserDetails.LName = stud.UserDetails.LName;

                                searchItem.Name = searchItem.UserDetails.FName;
                                if (!string.IsNullOrEmpty(searchItem.UserDetails.MName))
                                {
                                    searchItem.Name = searchItem.Name + " " + searchItem.UserDetails.MName;
                                }
                                
                                    searchItem.Name = searchItem.Name + " " + searchItem.UserDetails.LName;
                                

                                searchItem.StandardSectionMap = new StandardSectionMapDTO();
                                searchItem.StandardSectionMap.Standard = new StandardDTO();
                                searchItem.StandardSectionMap.Section = new SectionDTO();


                                searchItem.StandardSectionMap.Standard.StandardName = stud.StandardSectionMap.Standard.StandardName;
                                searchItem.StandardSectionMap.Section.SectionName = stud.StandardSectionMap.Section.SectionName;

                                //Add into Student vIew Model List
                                studView.studentList.Add(searchItem);
                                studView.IsSearchSuccessful = true;

                            }
                        }


                    }
                }
                else
                {
                    studView = studentView;
                    studentView.IsSearchSuccessful = false;
                }
            }

           return View(studView);
        }

        [HttpGet]
        public ActionResult PromoteToNewClass()
        {
            StudentVM studView = new StudentVM();
            
            //Fetch the StandardSection List
            studView.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();

            return View(studView);
        }


        [HttpPost]
        public ActionResult PromoteToNewClass(StudentVM studentView, string Command)
        {
            int StandardSectionId = 0;
            int currentStandardId = 0;
            StudentVM studView = null;
            List<StudentDTO> studListPass = null;

            if (Command != null)
            {

                // storing all data in studPass object and Passing it to PromoteToNewClass()
                studListPass = new List<StudentDTO>();

                

                //For Storing exact string values instead of keys in database
                if(string.Equals(Command,"Promote") && studentView!=null)
                {
                    
                    foreach (StudentVM studVM in studentView.studentList) 
                    {
                        StudentDTO student = new StudentDTO();
                        student.StudentInfoId = studVM.StudentInfoId;

                        if (string.Equals(studVM.Status, "1"))
                        {
                            student.Status = "Passed";
                        }
                        else if (string.Equals(studVM.Status, "2"))
                        {
                            student.Status = "Promotion Confirmed";
                        }
                        else if (string.Equals(studVM.Status, "3"))
                        {
                            student.Status = "Failed";
                        }
                        student.NewStandardSectionId = studVM.NewStandardSectionId;

                        studListPass.Add(student);
                        
                    }
                }

                // satndard section id is for both command Promote and command GetStandardStudents
                //It is for all the students--Outer object
                StandardSectionId = studentView.StandardSectionMap.StandardSectionId;

                
                   
                
                

                //Get Students for that Particular class or Promote Students to New Class
                StatusDTO<List<StudentDTO>> status = _studSvc.PromoteToNewClass(studListPass, Command, StandardSectionId);

                if (status.ReturnObj != null && status.ReturnObj.Count > 0)
                {
                    studView = new StudentVM(); // Instantiating Student View model As a Whole Class
                    studView.studentList = new List<StudentVM>(); // instantiating list of Students

                    //Binding th Standard Section Id which is returned from the database
                    studView.StandardSectionMap = new StandardSectionMapDTO();//instantiating StandardSectionMap for passing View Model
                    studView.StandardSectionMap.StandardSectionId = studentView.StandardSectionMap.StandardSectionId;
                    
                    //Fetch the StandardSection List for Upper Dropdown
                    studView.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();

                    //Fetch the Promotion Status List 
                    if (string.Equals(Command, "Standard"))
                    {
                       studView.PromotionStatusList = _uiddlRepo.getPromotionStatusDropDown();
                    }



                    if (status.IsSuccess && !status.IsException)
                    {

                        StudentVM studentV = null; // for each student
                        foreach (StudentDTO stud in status.ReturnObj)
                        {
                            if (stud != null)
                            {
                                studentV = new StudentVM(); // instantiating each student

                                studentV.Active = stud.Active;
                                studentV.RollNumber = stud.RollNumber;
                                studentV.StudentInfoId = stud.StudentInfoId;
                                

                                if (string.Equals(Command,"Promote") && stud.Status!=null && stud.NewStandardSectionId!=0)
                                {
                                    studentV.Status = stud.Status;
                                    studentV.NewStandardSectionId = stud.NewStandardSectionId;
                                }



                                //Fetch the Next StandardSectionList w.r.t Current
                                if (string.Equals(Command, "Standard"))
                                {
                                    currentStandardId = stud.StandardSectionMap.Standard.StandardId;
                                }

                                //Fetch New Standard and Section if they are assigned
                                studentV.StandardSectionMap = new StandardSectionMapDTO();
                                studentV.StandardSectionMap.Standard = new StandardDTO();
                                studentV.StandardSectionMap.Section = new SectionDTO();

                                studentV.StandardSectionMap.StandardSectionId = stud.StandardSectionMap.StandardSectionId;

                                
                                
                                
                                studentV.StandardSectionMap.Standard.StandardName = stud.StandardSectionMap.Standard.StandardName;
                                studentV.StandardSectionMap.Section.SectionName = studentV.StandardSectionMap.Section.SectionName;
                                

                                studentV.UserDetails = new UserMasterDTO();
                                studentV.UserDetails.UserMasterId = stud.UserDetails.UserMasterId;
                                studentV.UserDetails.FName = stud.UserDetails.FName;
                                studentV.UserDetails.MName = stud.UserDetails.MName;
                                studentV.UserDetails.LName = stud.UserDetails.LName;

                                studentV.Name = studentV.UserDetails.FName;
                                if (!string.IsNullOrEmpty(studentV.UserDetails.MName))
                                {
                                    studentV.Name = studentV.Name + " " + studentV.UserDetails.MName;
                                }

                                studentV.Name = studentV.Name + " " + studentV.UserDetails.LName;


                                ////Fetch the Promotion Status List 
                                //if (string.Equals(Command, "Standard"))
                                //{
                                //    studentV.PromotionStatusList = _uiddlRepo.getPromotionStatusDropDown();
                                //}

                                //Fetch New Class for each student
                                 


                                //Add Into Student View Model List
                                studView.studentList.Add(studentV);
                                studView.IsSearchSuccessful = true;

                                if(studentV.Status!=null)
                                 studView.IsCommandPromote = true;

                            }
                        }
                    }

                }
            }

           if(string.Equals(Command,"Standard"))
             studView.NextStandardSectionList = _uiddlRepo.getNextStandardSectionDropDown(currentStandardId);
            return View(studView);
        }
    }
}