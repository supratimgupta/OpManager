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
using System.IO;
using OperationsManager.Controllers;

namespace OperationsManager.Areas.Student.Controllers
{
    public class StudentController : BaseController
    {
        private IStudentSvc _studSvc;
        //private ILogSvc _logSvc;
        private IDropdownRepo _dropDwnRepo;
        private ISessionSvc _sessionSvc;
        private Helpers.UIDropDownRepo _uiddlRepo;
        Encryption encrypt = new Encryption();
        private IUserTransactionSvc _userTrans;

        private IConfigSvc _configSvc;

        public StudentController(IStudentSvc studSvc, IDropdownRepo dropDwnRepo, IUserTransactionSvc userTrans, IConfigSvc configSvc, ISessionSvc sessionSvc)
        {
            _studSvc = studSvc;
            _dropDwnRepo = dropDwnRepo;
            _uiddlRepo = new Helpers.UIDropDownRepo(_dropDwnRepo);
            //_logSvc = logSvc;
            _userTrans = userTrans;
            _configSvc = configSvc;
            _sessionSvc = sessionSvc;
        }
        // GET: Student/Student


        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetTransactionMasterDDL()
        {
            return Json(_dropDwnRepo.GetTransactionMasters(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetCalcIn()
        {
            return Json(_uiddlRepo.getCalcTypeDic(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult DeleteUserTransaction(UserTransactionDTO uTrans)
        {
            if (_userTrans.Delete(uTrans).IsSuccess)
            {
                return Json(new { status = true, message = "Deleted successfully." }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = false, message = "Delete failed." }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult HealthReport(string mode, string id)
        {
            Models.StudentVM studView = new Models.StudentVM();
            studView.UserDetails = new UserMasterDTO();
            studView.MODE = mode;
            studView.DisabledClass = "";
            if (string.Equals(mode, "EDIT", StringComparison.OrdinalIgnoreCase))
            {
                studView.UserDetails.UserMasterId = int.Parse(id);
            }
            if (string.Equals(mode, "VIEW", StringComparison.OrdinalIgnoreCase))
            {
                studView.DisabledClass = "disabledPlace";
            }
            studView.Transactions = new List<UserTransactionDTO>();
            //if (mode != null && (string.Equals(mode, "EDIT", StringComparison.OrdinalIgnoreCase) || string.Equals(mode, "VIEW", StringComparison.OrdinalIgnoreCase)))
            {
                //Populate edit data using id passed in URL, if id==null then show error message
                StatusDTO<StudentDTO> dto = _studSvc.Select(Convert.ToInt32(id));
                studView.UserDetails = new UserMasterDTO();
                studView.UserDetails.UserMasterId = dto.ReturnObj.UserDetails.UserMasterId;
                //uvModel.UserMasterId = dto.ReturnObj.UserMasterId;
                studView.UserDetails.FName = dto.ReturnObj.UserDetails.FName;
                studView.UserDetails.MName = dto.ReturnObj.UserDetails.MName;
                studView.UserDetails.LName = dto.ReturnObj.UserDetails.LName;
                // studView.UserDetails.Gender = dto.ReturnObj.UserDetails.Gender;


                //studView.Student = new StudentDTO();
                studView.RollNumber = dto.ReturnObj.RollNumber;
                studView.RegistrationNumber = dto.ReturnObj.RegistrationNumber;
                studView.AdmissionDate = dto.ReturnObj.AdmissionDate;
                studView.StandardSectionMap = dto.ReturnObj.StandardSectionMap;
                studView.UserDetails.Gender = dto.ReturnObj.UserDetails.Gender;
                studView.UserDetails.Location = dto.ReturnObj.UserDetails.Location;
                studView.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();
                studView.GenderList = _uiddlRepo.getGenderDropDown();
                studView.LocationList = _uiddlRepo.getLocationDropDown();
                if (mode != null && (string.Equals(mode, "EDIT", StringComparison.OrdinalIgnoreCase) || string.Equals(mode, "VIEW", StringComparison.OrdinalIgnoreCase)))
                {
                    studView.UserDetails.Height = dto.ReturnObj.Height;
                    studView.UserDetails.Weight = dto.ReturnObj.Weight;
                    studView.UserDetails.BMI = dto.ReturnObj.BMI;
                    studView.UserDetails.DV = dto.ReturnObj.DV;
                    studView.UserDetails.NV = dto.ReturnObj.NV;
                    studView.UserDetails.Pulserate = dto.ReturnObj.Pulserate;
                    studView.UserDetails.DrRemarks = dto.ReturnObj.DrRemarks;
                    studView.UserDetails.DrSugg = dto.ReturnObj.DrSugg;
                    studView.UserDetails.Majorillness = dto.ReturnObj.Majorillness;
                    studView.UserDetails.GenHealth = dto.ReturnObj.GenHealth;
                }


            }


            return View(studView);
        }

        [HttpGet]
        public ActionResult PrintHealthReport(string mode, string id)
        {
            StatusDTO<List<StudentDTO>> status = _studSvc.Select(null);
            StudentVM studView = null;

            if (Session["SEARCH_RESULT"] != null)
            {
                studView = (StudentVM)Session["SEARCH_RESULT"];
                Session["SEARCH_RESULT"] = null;
                return View(studView);
            }

            if (status.ReturnObj != null && status.ReturnObj.Count > 0)
            {
                studView = new StudentVM(); // Instantiating Student View model
                studView.studentList = new List<StudentVM>(); // instantiating list of Students

                //Fetch the StandardSection List
                studView.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();
                studView.LocationList = _uiddlRepo.getLocationDropDown();

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
                            searchItem.FatherContact = student.FatherContact;
                            searchItem.RegistrationNumber = student.RegistrationNumber;
                            searchItem.RollNumber = student.RollNumber;

                            searchItem.UserDetails = new UserMasterDTO();
                            searchItem.UserDetails.UserMasterId = student.UserDetails.UserMasterId;
                            searchItem.UserDetails.FName = student.UserDetails.FName;
                            searchItem.UserDetails.MName = student.UserDetails.MName;
                            searchItem.UserDetails.LName = student.UserDetails.LName;

                            searchItem.Name = student.UserDetails.FName;
                            if (!string.IsNullOrEmpty(student.UserDetails.FName))
                            {
                                searchItem.Name = searchItem.Name + " " + student.UserDetails.MName;
                            }

                            searchItem.Name = searchItem.Name + " " + searchItem.UserDetails.LName;


                            searchItem.StandardSectionMap = new StandardSectionMapDTO();
                            searchItem.StandardSectionMap.Standard = new StandardDTO();
                            searchItem.StandardSectionMap.Section = new SectionDTO();
                            searchItem.UserDetails = new UserMasterDTO();
                            searchItem.UserDetails.Location = new LocationDTO();

                            searchItem.StandardSectionMap.Standard.StandardName = student.StandardSectionMap.Standard.StandardName;
                            searchItem.StandardSectionMap.Section.SectionName = student.StandardSectionMap.Section.SectionName;
                            searchItem.UserDetails.Location.LocationDescription = student.UserDetails.Location.LocationDescription;

                            searchItem.Height = student.Height;
                            searchItem.Weight = student.Weight;
                            searchItem.BMI = student.BMI;
                            searchItem.DV = student.DV;
                            searchItem.NV = student.NV;
                            searchItem.Pulserate = student.Pulserate;
                            searchItem.DrRemarks = student.DrRemarks;
                            searchItem.DrSugg = student.DrSugg;
                            searchItem.Majorillness = student.Majorillness;
                            //Add into Student vIew Model List
                            searchItem.studentList.Add(searchItem);
                            searchItem.IsSearchSuccessful = true;

                        }
                    }
                }
                if (status.IsException)
                {
                    throw new Exception(status.ExceptionMessage);
                }
            }
            else
            {
                studView = new StudentVM();
                //Fetch the StandardSection List
                studView.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();
                studView.LocationList = _uiddlRepo.getLocationDropDown();
                studView.IsSearchSuccessful = false;
                studView.MsgColor = "green";
                studView.SuccessOrFailureMessage = "Please Select atleast 1 Search Criteria";
            }



            return View(studView);

        }
        [HttpPost]
        public ActionResult PrintHealthReport(StudentVM studentView, string Command)
        {
            //if Command Add then Redirect it to Add
            if (string.Equals(Command, "Add"))
            {
                return RedirectToAction("Register");
            }
            StudentVM studView = null;
            StudentDTO student = null;

            //Fetch the StandardSection List
            studentView.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();

            if (studentView != null)
            {
                student = new StudentDTO();
                //studentView.UserDetails = new UserMasterDTO();
                student.UserDetails = new UserMasterDTO();
                // Search for FName LName and MName

                student.UserDetails.FName = studentView.UserDetails.FName;
                //student.UserDetails.MName = studentView.UserDetails.MName;
                student.UserDetails.LName = studentView.UserDetails.LName;

                student.StandardSectionMap = new StandardSectionMapDTO();
                student.StandardSectionMap.Standard = new StandardDTO();
                student.StandardSectionMap.Section = new SectionDTO();
                student.UserDetails.Location = new LocationDTO();

                // Search for Class
                student.StandardSectionMap.StandardSectionId = studentView.StandardSectionMap.StandardSectionId;

                //Search by Location
                student.UserDetails.Location.LocationId = studentView.UserDetails.Location.LocationId;

                // Search for Roll and Registration
                //student.RollNumber = studentView.RollNumber;
                student.RegistrationNumber = studentView.RegistrationNumber;

                StatusDTO<List<StudentDTO>> status = _studSvc.Select(student);

                if (status.ReturnObj != null && status.ReturnObj.Count > 0)
                {
                    studView = new StudentVM(); // Instantiating Student View model
                    studView.studentList = new List<StudentVM>(); // instantiating list of Students

                    //Fetch the StandardSection List
                    studView.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();
                    studView.LocationList = _uiddlRepo.getLocationDropDown();

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
                                searchItem.FatherContact = stud.FatherContact;
                                searchItem.RegistrationNumber = stud.RegistrationNumber;
                                searchItem.RollNumber = stud.RollNumber;

                                searchItem.UserDetails = new UserMasterDTO();
                                searchItem.UserDetails.UserMasterId = stud.UserDetails.UserMasterId;
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
                                searchItem.UserDetails.Location = new LocationDTO();

                                searchItem.StandardSectionMap.Standard.StandardName = stud.StandardSectionMap.Standard.StandardName;
                                searchItem.StandardSectionMap.Section.SectionName = stud.StandardSectionMap.Section.SectionName;

                                searchItem.UserDetails.Location.LocationDescription = stud.UserDetails.Location.LocationDescription;
                                searchItem.UserDetails.ContactNo = stud.UserDetails.ContactNo;


                                searchItem.Height = stud.Height;
                                searchItem.Weight = stud.Weight;
                                searchItem.BMI = stud.BMI;
                                searchItem.DV = stud.DV;
                                searchItem.NV = stud.NV;
                                searchItem.Pulserate = stud.Pulserate;
                                searchItem.DrRemarks = stud.DrRemarks;
                                searchItem.DrSugg = stud.DrSugg;
                                searchItem.Majorillness = stud.Majorillness;
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
                    //Fetch the StandardSection List
                    studentView.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();
                    studView.LocationList = _uiddlRepo.getLocationDropDown();
                }
            }

            Session["SEARCH_RESULT"] = studView;
            return View(studView);
        }

        [HttpPost]
        public ActionResult HealthReport(Models.StudentVM studentView, HttpPostedFileBase file)
        {
            string folderName = string.Empty;


            DateTime dtValidator = new DateTime();
            if (DateTime.TryParse(studentView.DOBString, out dtValidator))
            {
                if (studentView.UserDetails == null)
                {
                    studentView.UserDetails = new UserMasterDTO();
                }
                studentView.UserDetails.DOB = dtValidator;
            }

            if (string.Equals(studentView.MODE, "EDIT", StringComparison.OrdinalIgnoreCase))
            {
                //Call update
                //if (ModelState.IsValid)
                //{ 
                StatusDTO<StudentDTO> status = _studSvc.UpdateHealthReport(studentView);
                if (status.IsSuccess)
                {


                    return RedirectToAction("Search");
                }
                studentView.ErrorMessage = status.FailureReason;
                //}
            }
            else
            {
                //Call insert

                //if (ModelState.IsValid)
                //{   
                //string pass = !string.IsNullOrEmpty(studentView.UserDetails.Password) ? encrypt.encryption(studentView.UserDetails.Password) : null;
                //studentView.UserDetails.Password = pass;
                StatusDTO<StudentDTO> status = _studSvc.InsertHealthReport(studentView);
                studentView.UserDetails = new UserMasterDTO();
                //      studentView.UserDetails.UserMasterId = status.ReturnObj.UserDetails.UserMasterId;
                if (status.IsSuccess)
                {
                    //if (studentView.Transactions != null && studentView.Transactions.Count > 0)
                    //{
                    //    for (int i = 0; i < studentView.Transactions.Count; i++)
                    //    {
                    //        if (studentView.Transactions[i].UserTransactionId > 0)
                    //        {
                    //            _userTrans.Update(studentView.Transactions[i]);
                    //        }
                    //        else
                    //        {
                    //            studentView.Transactions[i].User = new UserMasterDTO();
                    //            studentView.Transactions[i].User.UserMasterId = status.ReturnObj.UserDetails.UserMasterId;
                    //            _userTrans.Insert(studentView.Transactions[i]);
                    //        }
                    //    }
                    //}

                    //return RedirectToAction("Register", new { mode = "EDIT", id = studentView.UserDetails.UserMasterId.ToString() });
                    return RedirectToAction("Search");
                }
                studentView.ErrorMessage = status.FailureReason;
                //}                
            }
            //if(ModelState.IsValid)
            //{
            //}
            //ModelState.Clear();

            studentView.TransactionMasters = _uiddlRepo.getTransactionMasters();
            studentView.GraceAmountOnList = _uiddlRepo.getCalcType();

            studentView.CalcInSelectList = _uiddlRepo.getCalcTypeDic();
            studentView.TransactionMasterSelectList = _dropDwnRepo.GetTransactionMasters();

            studentView.GenderList = _uiddlRepo.getGenderDropDown();
            studentView.LocationList = _uiddlRepo.getLocationDropDown();
            studentView.RoleList = _uiddlRepo.getRoleDropDown();
            //uvModel.ClassTypeList = _uiddlRepo.getClassTypeDropDown();
            studentView.SectionList = _uiddlRepo.getSectionDropDown();
            studentView.HouseList = _uiddlRepo.getHouseDropDown();
            //uvModel.BookCategoryList = _uiddlRepo.getBookCategoryDropDown();
            //uvModel.DepartmentList = _uiddlRepo.getDepartmentDropDown();
            //uvModel.DesignationList = _uiddlRepo.getDesignationDropDown();
            studentView.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();

            studentView.Transactions = _userTrans.GetUserTransactions(studentView.UserDetails.UserMasterId);

            return View(studentView);
        }

        [HttpGet]
        public ActionResult Register(string mode, string id)
        {
            Models.StudentVM studView = new Models.StudentVM();
            studView.UserDetails = new UserMasterDTO();
            studView.MODE = mode;
            studView.DisabledClass = "";
            if (string.Equals(mode, "EDIT", StringComparison.OrdinalIgnoreCase))
            {
                studView.UserDetails.UserMasterId = int.Parse(id);
            }
            if (string.Equals(mode, "VIEW", StringComparison.OrdinalIgnoreCase))
            {
                studView.DisabledClass = "disabledPlace";
            }
            studView.Transactions = new List<UserTransactionDTO>();
            if (mode != null && (string.Equals(mode, "EDIT", StringComparison.OrdinalIgnoreCase) || string.Equals(mode, "VIEW", StringComparison.OrdinalIgnoreCase)))
            {
                //Populate edit data using id passed in URL, if id==null then show error message
                StatusDTO<StudentDTO> dto = _studSvc.Select(Convert.ToInt32(id));
                studView.UserDetails = new UserMasterDTO();
                studView.UserDetails.UserMasterId = dto.ReturnObj.UserDetails.UserMasterId;
                //uvModel.UserMasterId = dto.ReturnObj.UserMasterId;
                studView.UserDetails.FName = dto.ReturnObj.UserDetails.FName;
                studView.UserDetails.MName = dto.ReturnObj.UserDetails.MName;
                studView.UserDetails.LName = dto.ReturnObj.UserDetails.LName;
                studView.UserDetails.Gender = dto.ReturnObj.UserDetails.Gender;
                studView.UserDetails.Image = dto.ReturnObj.UserDetails.Image;
                studView.UserDetails.DOB = dto.ReturnObj.UserDetails.DOB;
                studView.DOBString = studView.UserDetails.DOB.HasValue ? studView.UserDetails.DOB.Value.ToString("dd-MMM-yyyy") : string.Empty;
                studView.UserDetails.EmailId = dto.ReturnObj.UserDetails.EmailId;
                studView.UserDetails.ResidentialAddress = dto.ReturnObj.UserDetails.ResidentialAddress;
                studView.UserDetails.PermanentAddress = dto.ReturnObj.UserDetails.PermanentAddress;
                studView.UserDetails.ContactNo = dto.ReturnObj.UserDetails.ContactNo;
                studView.UserDetails.AltContactNo = dto.ReturnObj.UserDetails.AltContactNo;
                studView.UserDetails.BloodGroup = dto.ReturnObj.UserDetails.BloodGroup;
                studView.UserDetails.Location = dto.ReturnObj.UserDetails.Location;
                studView.UserDetails.Role = dto.ReturnObj.UserDetails.Role;

                //studView.Student = new StudentDTO();
                studView.RollNumber = dto.ReturnObj.RollNumber;
                studView.RegistrationNumber = dto.ReturnObj.RegistrationNumber;
                studView.AdmissionDate = dto.ReturnObj.AdmissionDate;
                studView.FatherContact = dto.ReturnObj.FatherContact;
                studView.GuardianName = dto.ReturnObj.GuardianName;
                studView.FatherEmailId = dto.ReturnObj.FatherEmailId;
                studView.HouseType = dto.ReturnObj.HouseType;
                studView.StandardSectionMap = dto.ReturnObj.StandardSectionMap;
                studView.FatherName = dto.ReturnObj.FatherName;
                studView.FatherQualification = dto.ReturnObj.FatherQualification;
                studView.FatherOccupation = dto.ReturnObj.FatherOccupation;
                studView.FatherDesignation = dto.ReturnObj.FatherDesignation;
                studView.FatherOrganisationName = dto.ReturnObj.FatherOrganisationName;
                studView.FatherDepartment = dto.ReturnObj.FatherDepartment;
                studView.FatherOfficeAddress = dto.ReturnObj.FatherOfficeAddress;
                studView.FatherOfficePhNo = dto.ReturnObj.FatherOfficePhNo;
                studView.FatherTypeOfBusiness = dto.ReturnObj.FatherTypeOfBusiness;
                studView.FatherAnnualIncome = dto.ReturnObj.FatherAnnualIncome;
                studView.MotherName = dto.ReturnObj.MotherName;
                studView.MotherQualification = dto.ReturnObj.MotherQualification;
                studView.MotherOccupation = dto.ReturnObj.MotherOccupation;
                studView.MotherAnnualIncome = dto.ReturnObj.MotherAnnualIncome;
                studView.MotherOrganisationName = dto.ReturnObj.MotherOrganisationName;
                studView.MotherDepartment = dto.ReturnObj.MotherDepartment;
                studView.MotherDesignation = dto.ReturnObj.MotherDesignation;
                studView.MotherOfficeAddress = dto.ReturnObj.MotherOfficeAddress;
                studView.MotherOfficePhNo = dto.ReturnObj.MotherOfficePhNo;
                studView.MotherTypeOfBusiness = dto.ReturnObj.MotherTypeOfBusiness;

                if (string.Equals(dto.ReturnObj.IsChristian, "YES") || string.Equals(dto.ReturnObj.IsChristian, "Yes") || string.Equals(dto.ReturnObj.IsChristian, "1"))
                {
                    studView.IsChristian = "1";
                }
                else if (string.Equals(dto.ReturnObj.IsChristian, "NO") || string.Equals(dto.ReturnObj.IsChristian, "No") || string.Equals(dto.ReturnObj.IsChristian, "2"))
                {
                    studView.IsChristian = "2";
                }

                if (string.Equals(dto.ReturnObj.IsParentTeacher, "YES") || string.Equals(dto.ReturnObj.IsParentTeacher, "Yes") || string.Equals(dto.ReturnObj.IsParentTeacher, "1"))
                {
                    studView.IsParentTeacher = "1";
                }
                else if (string.Equals(dto.ReturnObj.IsParentTeacher, "NO") || string.Equals(dto.ReturnObj.IsParentTeacher, "No") || string.Equals(dto.ReturnObj.IsParentTeacher, "2"))
                {
                    studView.IsParentTeacher = "2";
                }

                if (string.Equals(dto.ReturnObj.IsParentFromEngMedium, "YES") || string.Equals(dto.ReturnObj.IsParentFromEngMedium, "Yes") || string.Equals(dto.ReturnObj.IsParentFromEngMedium, "1"))
                {
                    studView.IsParentFromEngMedium = "1";
                }
                else if (string.Equals(dto.ReturnObj.IsParentFromEngMedium, "NO") || string.Equals(dto.ReturnObj.IsParentFromEngMedium, "No") || string.Equals(dto.ReturnObj.IsParentFromEngMedium, "2"))
                {
                    studView.IsParentFromEngMedium = "2";
                }

                if (string.Equals(dto.ReturnObj.IsJointOrNuclearFamily, "JOINT") || string.Equals(dto.ReturnObj.IsJointOrNuclearFamily, "Joint") || string.Equals(dto.ReturnObj.IsJointOrNuclearFamily, "1"))
                {
                    studView.IsJointOrNuclearFamily = "1";
                }
                else if (string.Equals(dto.ReturnObj.IsJointOrNuclearFamily, "NUCLEAR") || string.Equals(dto.ReturnObj.IsJointOrNuclearFamily, "Nuclear") || string.Equals(dto.ReturnObj.IsJointOrNuclearFamily, "2"))
                {
                    studView.IsJointOrNuclearFamily = "2";
                }

                if (string.Equals(dto.ReturnObj.SiblingsInStadOrNot, "YES") || string.Equals(dto.ReturnObj.SiblingsInStadOrNot, "Yes") || string.Equals(dto.ReturnObj.SiblingsInStadOrNot, "1"))
                {
                    studView.SiblingsInStadOrNot = "1";
                }
                else if (string.Equals(dto.ReturnObj.SiblingsInStadOrNot, "NO") || string.Equals(dto.ReturnObj.SiblingsInStadOrNot, "No") || string.Equals(dto.ReturnObj.SiblingsInStadOrNot, "2"))
                {
                    studView.SiblingsInStadOrNot = "2";
                }

                if (string.Equals(dto.ReturnObj.AnyAlumuniMember, "YES") || string.Equals(dto.ReturnObj.AnyAlumuniMember, "Yes") || string.Equals(dto.ReturnObj.AnyAlumuniMember, "1"))
                {
                    studView.AnyAlumuniMember = "1";
                }
                else if (string.Equals(dto.ReturnObj.AnyAlumuniMember, "NO") || string.Equals(dto.ReturnObj.AnyAlumuniMember, "No") || string.Equals(dto.ReturnObj.AnyAlumuniMember, "2"))
                {
                    studView.AnyAlumuniMember = "2";
                }

                if (string.Equals(dto.ReturnObj.StuInPrivateTution, "YES") || string.Equals(dto.ReturnObj.StuInPrivateTution, "Yes") || string.Equals(dto.ReturnObj.StuInPrivateTution, "1"))
                {
                    studView.StuInPrivateTution = "1";
                }
                else if (string.Equals(dto.ReturnObj.StuInPrivateTution, "NO") || string.Equals(dto.ReturnObj.StuInPrivateTution, "No") || string.Equals(dto.ReturnObj.StuInPrivateTution, "2"))
                {
                    studView.StuInPrivateTution = "2";
                }

                studView.SubjectNameTheyTeach = dto.ReturnObj.SubjectNameTheyTeach;
                //studView.SiblingsInStadOrNot = dto.ReturnObj.SiblingsInStadOrNot;
                //studView.AnyAlumuniMember = dto.ReturnObj.AnyAlumuniMember;
                //studView.StuInPrivateTution = dto.ReturnObj.StuInPrivateTution;
                studView.NoOfTution = dto.ReturnObj.NoOfTution;
                studView.FeesPaidForTution = dto.ReturnObj.FeesPaidForTution;
                studView.Religion = dto.ReturnObj.Religion;
                studView.Caste = dto.ReturnObj.Caste;
                studView.ModeOfTransport = dto.ReturnObj.ModeOfTransport;
                studView.DropPoint = dto.ReturnObj.DropPoint;
                studView.TransportDetails = dto.ReturnObj.TransportDetails;
                studView.TransportContactNo = dto.ReturnObj.TransportContactNo;
                studView.NameOf1stPerson = dto.ReturnObj.NameOf1stPerson;
                studView.NameOf2ndPerson = dto.ReturnObj.NameOf2ndPerson;
                studView.RelationWithChild1stPerson = dto.ReturnObj.RelationWithChild1stPerson;
                studView.RelationWithChild2ndPerson = dto.ReturnObj.RelationWithChild2ndPerson;
                studView.NoOfSiblings = dto.ReturnObj.NoOfSiblings;

                if (string.Equals(dto.ReturnObj.BrotherSisterInSchool, "YES") || string.Equals(dto.ReturnObj.BrotherSisterInSchool, "Yes") || string.Equals(dto.ReturnObj.BrotherSisterInSchool, "1"))
                {
                    studView.BrotherSisterInSchool = "1";
                }
                else if (string.Equals(dto.ReturnObj.BrotherSisterInSchool, "NO") || string.Equals(dto.ReturnObj.BrotherSisterInSchool, "No") || string.Equals(dto.ReturnObj.BrotherSisterInSchool, "2"))
                {
                    studView.BrotherSisterInSchool = "2";
                }

                if (string.Equals(dto.ReturnObj.LikeToPartinCCA, "YES") || string.Equals(dto.ReturnObj.LikeToPartinCCA, "Yes") || string.Equals(dto.ReturnObj.LikeToPartinCCA, "1"))
                {
                    studView.LikeToPartinCCA = "1";
                }
                else if (string.Equals(dto.ReturnObj.LikeToPartinCCA, "NO") || string.Equals(dto.ReturnObj.LikeToPartinCCA, "No") || string.Equals(dto.ReturnObj.LikeToPartinCCA, "2"))
                {
                    studView.LikeToPartinCCA = "2";
                }

                if (string.Equals(dto.ReturnObj.LikeToPartInGames, "YES") || string.Equals(dto.ReturnObj.LikeToPartInGames, "Yes") || string.Equals(dto.ReturnObj.LikeToPartInGames, "1"))
                {
                    studView.LikeToPartInGames = "1";
                }
                else if (string.Equals(dto.ReturnObj.LikeToPartInGames, "NO") || string.Equals(dto.ReturnObj.LikeToPartInGames, "No") || string.Equals(dto.ReturnObj.LikeToPartInGames, "2"))
                {
                    studView.LikeToPartInGames = "2";
                }
                studView.Transactions = _userTrans.GetUserTransactions(dto.ReturnObj.UserDetails.UserMasterId);
                studView.TransactionMasters = _uiddlRepo.getTransactionMasters();

                string studentImageFolder = _configSvc.GetStudentImagesFolder();
                string fatherImageFolder = _configSvc.GetFatherImagesFolder();
                string motherImageFolder = _configSvc.GetMotherImagesFolder();

                studView.StudentImagePath = _configSvc.GetStudentImagesRelPath() + "/" + GetImageFileName(studView.RegistrationNumber, studentImageFolder) + "?ver=" + DateTime.UtcNow.Ticks;
                studView.FatherImagePath = _configSvc.GetFatherImagesRelPath() + "/" + GetImageFileName(studView.RegistrationNumber, fatherImageFolder) + "?ver=" + DateTime.UtcNow.Ticks;
                studView.MotherImagePath = _configSvc.GetMotherImagesRelPath() + "/" + GetImageFileName(studView.RegistrationNumber, motherImageFolder) + "?ver=" + DateTime.UtcNow.Ticks;
            }

            //studView.Transactions = _userTrans.GetUserTransactions(dto.ReturnObj.UserDetails.UserMasterId);
            studView.TransactionMasters = _uiddlRepo.getTransactionMasters();

            studView.CalcInSelectList = _uiddlRepo.getCalcTypeDic();
            studView.TransactionMasterSelectList = _dropDwnRepo.GetTransactionMasters();

            studView.GenderList = _uiddlRepo.getGenderDropDown();
            studView.LocationList = _uiddlRepo.getLocationDropDown();
            studView.RoleList = _uiddlRepo.getRoleDropDown();
            studView.ClassTypeList = _uiddlRepo.getClassTypeDropDown();
            studView.SectionList = _uiddlRepo.getSectionDropDown();
            studView.HouseList = _uiddlRepo.getHouseDropDown();
            studView.IsChristianList = _uiddlRepo.getSelectValueDropDown();
            studView.IsParentTeacherList = _uiddlRepo.getSelectValueDropDown();
            studView.IsParentFromEngMedList = _uiddlRepo.getSelectValueDropDown();
            studView.JointOrNuclearFamilyList = _uiddlRepo.getSelectJointNuclearDropDown();
            studView.SiblingsInStdOrNotList = _uiddlRepo.getSelectValueDropDown();
            studView.AnyAlumunimemberList = _uiddlRepo.getSelectValueDropDown();
            studView.StudentinPvtTutionList = _uiddlRepo.getSelectValueDropDown();
            studView.BrotherSisterInSchoolList = _uiddlRepo.getSelectValueDropDown();
            studView.LikeToPartCCAList = _uiddlRepo.getSelectValueDropDown();
            studView.LiketoPartGameList = _uiddlRepo.getSelectValueDropDown();

            //uvModel.BookCategoryList = _uiddlRepo.getBookCategoryDropDown();
            //uvModel.DepartmentList = _uiddlRepo.getDepartmentDropDown();
            //uvModel.DesignationList = _uiddlRepo.getDesignationDropDown();
            studView.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();
            studView.GraceAmountOnList = _uiddlRepo.getCalcType();

            return View(studView);
        }
        [HttpGet]
        public ActionResult Admission(string mode, string id)
        {
            Models.StudentVM studView = new Models.StudentVM();
            studView.UserDetails = new UserMasterDTO();
            studView.MODE = mode;
            studView.DisabledClass = "";
            if (string.Equals(mode, "EDIT", StringComparison.OrdinalIgnoreCase))
            {
                studView.UserDetails.AdmissionId = int.Parse(id);
            }
            if (string.Equals(mode, "VIEW", StringComparison.OrdinalIgnoreCase))
            {
                studView.DisabledClass = "disabledPlace";
            }
            studView.Transactions = new List<UserTransactionDTO>();
            if (mode != null && (string.Equals(mode, "EDIT", StringComparison.OrdinalIgnoreCase) || string.Equals(mode, "VIEW", StringComparison.OrdinalIgnoreCase)))
            {
                //Populate edit data using id passed in URL, if id==null then show error message
                StatusDTO<StudentDTO> dto = _studSvc.SelectAdmission(Convert.ToInt32(id)); //Admission
                studView.UserDetails = new UserMasterDTO();
                studView.UserDetails.UserMasterId = dto.ReturnObj.UserDetails.UserMasterId;
                //uvModel.UserMasterId = dto.ReturnObj.UserMasterId;
                studView.UserDetails.FName = dto.ReturnObj.UserDetails.FName;
                studView.UserDetails.MName = dto.ReturnObj.UserDetails.MName;
                studView.UserDetails.LName = dto.ReturnObj.UserDetails.LName;
                studView.UserDetails.Gender = dto.ReturnObj.UserDetails.Gender;
                //   studView.UserDetails.Image = dto.ReturnObj.UserDetails.Image;
                studView.UserDetails.DOB = dto.ReturnObj.UserDetails.DOB;
                studView.DOBString = studView.UserDetails.DOB.HasValue ? studView.UserDetails.DOB.Value.ToString("dd-MMM-yyyy") : string.Empty;
                studView.UserDetails.EmailId = dto.ReturnObj.UserDetails.EmailId;
                studView.UserDetails.ResidentialAddress = dto.ReturnObj.UserDetails.ResidentialAddress;
                studView.UserDetails.PermanentAddress = dto.ReturnObj.UserDetails.PermanentAddress;
                studView.UserDetails.ContactNo = dto.ReturnObj.UserDetails.ContactNo;
                studView.UserDetails.AltContactNo = dto.ReturnObj.UserDetails.AltContactNo;
                studView.UserDetails.BloodGroup = dto.ReturnObj.UserDetails.BloodGroup;
                studView.UserDetails.Location = dto.ReturnObj.UserDetails.Location;
                // studView.UserDetails.Role = dto.ReturnObj.UserDetails.Role;

                //studView.Student = new StudentDTO();
                //studView.RollNumber = dto.ReturnObj.RollNumber;
                //studView.RegistrationNumber = dto.ReturnObj.RegistrationNumber;
                //  studView.AdmissionDate = dto.ReturnObj.AdmissionDate;
                studView.FatherContact = dto.ReturnObj.FatherContact;
                studView.GuardianName = dto.ReturnObj.GuardianName;
                studView.FatherEmailId = dto.ReturnObj.FatherEmailId;
                // studView.HouseType = dto.ReturnObj.HouseType;
                //  studView.StandardSectionMap = dto.ReturnObj.StandardSectionMap;
                studView.FatherName = dto.ReturnObj.FatherName;
                studView.FatherQualification = dto.ReturnObj.FatherQualification;
                studView.FatherOccupation = dto.ReturnObj.FatherOccupation;
                studView.FatherDesignation = dto.ReturnObj.FatherDesignation;
                studView.FatherOrganisationName = dto.ReturnObj.FatherOrganisationName;
                studView.FatherDepartment = dto.ReturnObj.FatherDepartment;
                studView.FatherOfficeAddress = dto.ReturnObj.FatherOfficeAddress;
                // studView.FatherOfficePhNo = dto.ReturnObj.FatherOfficePhNo;
                //  studView.FatherTypeOfBusiness = dto.ReturnObj.FatherTypeOfBusiness;
                studView.FatherAnnualIncome = dto.ReturnObj.FatherAnnualIncome;
                studView.MotherName = dto.ReturnObj.MotherName;
                studView.MotherQualification = dto.ReturnObj.MotherQualification;
                studView.MotherOccupation = dto.ReturnObj.MotherOccupation;
                studView.MotherAnnualIncome = dto.ReturnObj.MotherAnnualIncome;
                studView.MotherOrganisationName = dto.ReturnObj.MotherOrganisationName;
                studView.MotherDepartment = dto.ReturnObj.MotherDepartment;
                studView.MotherDesignation = dto.ReturnObj.MotherDesignation;
                studView.MotherOfficeAddress = dto.ReturnObj.MotherOfficeAddress;
                studView.MotherOfficePhNo = dto.ReturnObj.MotherOfficePhNo;
                studView.MotherTypeOfBusiness = dto.ReturnObj.MotherTypeOfBusiness;
                

                studView.Religion = dto.ReturnObj.Religion;
                studView.Caste = dto.ReturnObj.Caste;
                studView.classAppld = dto.ReturnObj.classAppld;
                studView.Currclass = dto.ReturnObj.Currclass;
                studView.sibName = dto.ReturnObj.sibName;
                studView.sibclass = dto.ReturnObj.sibclass;
                studView.sibGender = dto.ReturnObj.sibGender;
                studView.Category = dto.ReturnObj.Category;
                studView.Nationality = dto.ReturnObj.Nationality;
                studView.Secondlang = dto.ReturnObj.Secondlang;
                studView.Prevmedium = dto.ReturnObj.Prevmedium;
                studView.Prevschool = dto.ReturnObj.Prevschool;
                studView.Prevstream = dto.ReturnObj.Prevstream;
                studView.GenderList = _uiddlRepo.getGenderDropDown();
                studView.LocationList = _uiddlRepo.getLocationDropDown();
                studView.AdmissionStatus = new AdmissionStatusDTO();
                studView.AdmissionStatus.AdmissionStatusId = dto.ReturnObj.AdmissionStatus.AdmissionStatusId;
                studView.AdmissionExamDate = dto.ReturnObj.AdmissionExamDate;
                studView.AdmissionInterviewDate = dto.ReturnObj.AdmissionInterviewDate;
                studView.AdmissionDate = dto.ReturnObj.AdmissionDate;
                studView.UserDetails.AdmissionId = dto.ReturnObj.UserDetails.AdmissionId;
                studView.admissionformno = dto.ReturnObj.admissionformno;
                //to show admission status in edit mode
                studView.AdmissionStatusList = _uiddlRepo.getAdmissionStatusDropdown();

            }

            studView.GenderList = _uiddlRepo.getGenderDropDown();
            studView.LocationList = _uiddlRepo.getLocationDropDown();



            return View(studView);
        }
        [HttpPost]
        public ActionResult Admission(Models.StudentVM studentView, HttpPostedFileBase file)
        {
            string folderName = string.Empty;


            DateTime dtValidator = new DateTime();
            if (DateTime.TryParse(studentView.DOBString, out dtValidator))
            {
                if (studentView.UserDetails == null)
                {
                    studentView.UserDetails = new UserMasterDTO();
                }
                studentView.UserDetails.DOB = dtValidator;
            }

            if (string.Equals(studentView.MODE, "EDIT", StringComparison.OrdinalIgnoreCase))
            {
                //Call update
                //if (ModelState.IsValid)
                //{ 
                //StudentDTO student = null;
                //student = new StudentDTO();
                //student.AdmissionStatus = new AdmissionStatusDTO();
                //student.AdmissionStatus.AdmissionStatusId = studentView.AdmissionStatus.AdmissionStatusId;
                StatusDTO<StudentDTO> status = _studSvc.UpdateAdmission(studentView);
                if (status.IsSuccess)
                {


                    return RedirectToAction("AdmissionSearch");
                }
                studentView.ErrorMessage = status.FailureReason;
                //}
            }
            else
            {
                //Call insert

                //if (ModelState.IsValid)
                //{   
                string pass = !string.IsNullOrEmpty(studentView.UserDetails.Password) ? encrypt.encryption(studentView.UserDetails.Password) : null;
                studentView.UserDetails.Password = pass;
                StatusDTO<StudentDTO> status = _studSvc.InsertAdmission(studentView);
                studentView.UserDetails = new UserMasterDTO();
                studentView.UserDetails.AdmissionId = status.ReturnObj.UserDetails.AdmissionId;
                return RedirectToAction("AdmissionSearch");
                //}

            }

            return View(studentView);
        }
        [HttpGet]
        public ActionResult AdmissionSearch()
        {
            StatusDTO<List<StudentDTO>> status = _studSvc.Select(null);
            StudentVM studView = null;

            if (Session["SEARCH_RESULT"] != null)
            {
                studView = (StudentVM)Session["SEARCH_RESULT"];
                Session["SEARCH_RESULT"] = null;
                return View(studView);
            }

            if (status.ReturnObj != null && status.ReturnObj.Count > 0)
            {
                studView = new StudentVM(); // Instantiating Student View model
                studView.studentList = new List<StudentVM>(); // instantiating list of Students

                //Fetch the StandardSection List
                studView.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();
                studView.LocationList = _uiddlRepo.getLocationDropDown();

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
                            searchItem.FatherContact = student.FatherContact;
                            searchItem.RegistrationNumber = student.RegistrationNumber;
                            searchItem.RollNumber = student.RollNumber;

                            searchItem.UserDetails = new UserMasterDTO();
                            searchItem.UserDetails.UserMasterId = student.UserDetails.UserMasterId;
                            searchItem.UserDetails.FName = student.UserDetails.FName;
                            searchItem.UserDetails.MName = student.UserDetails.MName;
                            searchItem.UserDetails.LName = student.UserDetails.LName;

                            searchItem.Name = student.UserDetails.FName;
                            if (!string.IsNullOrEmpty(student.UserDetails.FName))
                            {
                                searchItem.Name = searchItem.Name + " " + student.UserDetails.MName;
                            }

                            searchItem.Name = searchItem.Name + " " + searchItem.UserDetails.LName;


                            searchItem.StandardSectionMap = new StandardSectionMapDTO();
                            searchItem.StandardSectionMap.Standard = new StandardDTO();
                            searchItem.StandardSectionMap.Section = new SectionDTO();
                            searchItem.UserDetails = new UserMasterDTO();
                            searchItem.UserDetails.Location = new LocationDTO();

                            searchItem.StandardSectionMap.Standard.StandardName = student.StandardSectionMap.Standard.StandardName;
                            searchItem.StandardSectionMap.Section.SectionName = student.StandardSectionMap.Section.SectionName;
                            searchItem.UserDetails.Location.LocationDescription = student.UserDetails.Location.LocationDescription;

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
            else
            {
                studView = new StudentVM();
                //Fetch the StandardSection List
                studView.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();
                studView.LocationList = _uiddlRepo.getLocationDropDown();
                studView.IsSearchSuccessful = false;
                studView.MsgColor = "green";
                studView.SuccessOrFailureMessage = "Please Select atleast 1 Search Criteria";
            }



            return View(studView);
        }

        [HttpPost]
        public ActionResult AdmissionSearch(StudentVM studentView, string Command)
        {
            //if Command Add then Redirect it to Add
            if (string.Equals(Command, "Add"))
            {
                return RedirectToAction("Register");
            }
            StudentVM studView = null;
            StudentDTO student = null;

            //Fetch the StandardSection List
            //  studentView.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();

            if (studentView != null)
            {
                student = new StudentDTO();
                //studentView.UserDetails = new UserMasterDTO();
                student.UserDetails = new UserMasterDTO();
                // Search for FName LName and MName

                student.UserDetails.FName = studentView.UserDetails.FName;
                //student.UserDetails.MName = studentView.UserDetails.MName;
                student.UserDetails.LName = studentView.UserDetails.LName;

                student.StandardSectionMap = new StandardSectionMapDTO();
                student.StandardSectionMap.Standard = new StandardDTO();
                student.StandardSectionMap.Section = new SectionDTO();
                student.UserDetails.Location = new LocationDTO();

                // Search for Class
                student.StandardSectionMap.StandardSectionId = studentView.StandardSectionMap.StandardSectionId;

                //Search by Location
                student.UserDetails.Location.LocationId = studentView.UserDetails.Location.LocationId;

                // Search for Roll and Registration
                //student.RollNumber = studentView.RollNumber;
                student.RegistrationNumber = studentView.RegistrationNumber;

                StatusDTO<List<StudentDTO>> status = _studSvc.AdmissionSearch(student);  //Admision

                if (status.ReturnObj != null && status.ReturnObj.Count > 0)
                {
                    studView = new StudentVM(); // Instantiating Student View model
                    studView.studentList = new List<StudentVM>(); // instantiating list of Students

                    //Fetch the StandardSection List
                    studView.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();
                    studView.LocationList = _uiddlRepo.getLocationDropDown();

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
                                searchItem.FatherContact = stud.FatherContact;
                                searchItem.RegistrationNumber = stud.RegistrationNumber;
                                searchItem.RollNumber = stud.RollNumber;

                                searchItem.UserDetails = new UserMasterDTO();
                                searchItem.UserDetails.UserMasterId = stud.UserDetails.UserMasterId;
                                searchItem.UserDetails.FName = stud.UserDetails.FName;
                                searchItem.UserDetails.MName = stud.UserDetails.MName;
                                searchItem.UserDetails.LName = stud.UserDetails.LName;

                                searchItem.Name = searchItem.UserDetails.FName;
                                if (!string.IsNullOrEmpty(searchItem.UserDetails.MName))
                                {
                                    searchItem.Name = searchItem.Name + " " + searchItem.UserDetails.MName;
                                }

                                searchItem.Name = searchItem.Name + " " + searchItem.UserDetails.LName;
                                searchItem.classAppld = stud.classAppld;

                                searchItem.StandardSectionMap = new StandardSectionMapDTO();
                                searchItem.StandardSectionMap.Standard = new StandardDTO();
                                searchItem.StandardSectionMap.Section = new SectionDTO();
                                searchItem.UserDetails.Location = new LocationDTO();
                                searchItem.UserDetails.ContactNo = stud.UserDetails.ContactNo;
                                searchItem.UserDetails.Location.LocationId = stud.UserDetails.Location.LocationId;
                                searchItem.UserDetails.AdmissionId = stud.UserDetails.AdmissionId;
                                // if (searchItem.UserDetails.Location.LocationId == 1)
                                // {
                                //     stud.UserDetails.Location.LocationDescription = "Barrackpur";
                                // }
                                //else if (searchItem.UserDetails.Location.LocationId == 2)
                                // {
                                //     stud.UserDetails.Location.LocationDescription = "Shyamnagar";
                                // }
                                //else
                                // {
                                //     stud.UserDetails.Location.LocationDescription = "Manipukur";
                                // }
                                // searchItem.LocationList = _uiddlRepo.getLocationDropDown();
                                //          searchItem.StandardSectionMap.Standard.StandardName = stud.StandardSectionMap.Standard.StandardName;
                                //          searchItem.StandardSectionMap.Section.SectionName = stud.StandardSectionMap.Section.SectionName;

                                //          searchItem.UserDetails.Location.LocationDescription = stud.UserDetails.Location.LocationDescription;
                                //         searchItem.UserDetails.ContactNo = stud.UserDetails.ContactNo;

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
                    //Fetch the StandardSection List
                    studentView.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();
                    studView.LocationList = _uiddlRepo.getLocationDropDown();
                }
            }

            Session["SEARCH_RESULT"] = studView;
            return View(studView);
        }

        public string GetImageFileName(string registrationNo, string folder)
        {
            string fileName = string.Empty;
            registrationNo = registrationNo.Replace('/', '_').Replace('\\', '_');
            string[] similarFiles = Directory.GetFiles(folder, registrationNo + ".*");
            if (similarFiles != null && similarFiles.Length > 0)
            {
                fileName = similarFiles[0];
                string[] fileParts = fileName.Split('\\');
                fileName = fileParts[fileParts.Length - 1];
            }
            return fileName;
        }


        [HttpGet]
        public ActionResult Search()
        {
            StatusDTO<List<StudentDTO>> status = _studSvc.Select(null);
            StudentVM studView = null;

            if (Session["SEARCH_RESULT"] != null)
            {
                studView = (StudentVM)Session["SEARCH_RESULT"];
                Session["SEARCH_RESULT"] = null;
                return View(studView);
            }

            if (status.ReturnObj != null && status.ReturnObj.Count > 0)
            {
                studView = new StudentVM(); // Instantiating Student View model
                studView.studentList = new List<StudentVM>(); // instantiating list of Students

                //Fetch the StandardSection List
                studView.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();
                studView.LocationList = _uiddlRepo.getLocationDropDown();

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
                            searchItem.FatherContact = student.FatherContact;
                            searchItem.RegistrationNumber = student.RegistrationNumber;
                            searchItem.RollNumber = student.RollNumber;

                            searchItem.UserDetails = new UserMasterDTO();
                            searchItem.UserDetails.UserMasterId = student.UserDetails.UserMasterId;
                            searchItem.UserDetails.FName = student.UserDetails.FName;
                            searchItem.UserDetails.MName = student.UserDetails.MName;
                            searchItem.UserDetails.LName = student.UserDetails.LName;

                            searchItem.Name = student.UserDetails.FName;
                            if (!string.IsNullOrEmpty(student.UserDetails.FName))
                            {
                                searchItem.Name = searchItem.Name + " " + student.UserDetails.MName;
                            }

                            searchItem.Name = searchItem.Name + " " + searchItem.UserDetails.LName;


                            searchItem.StandardSectionMap = new StandardSectionMapDTO();
                            searchItem.StandardSectionMap.Standard = new StandardDTO();
                            searchItem.StandardSectionMap.Section = new SectionDTO();
                            searchItem.UserDetails = new UserMasterDTO();
                            searchItem.UserDetails.Location = new LocationDTO();

                            searchItem.StandardSectionMap.Standard.StandardName = student.StandardSectionMap.Standard.StandardName;
                            searchItem.StandardSectionMap.Section.SectionName = student.StandardSectionMap.Section.SectionName;
                            searchItem.UserDetails.Location.LocationDescription = student.UserDetails.Location.LocationDescription;

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
            else
            {
                studView = new StudentVM();
                //Fetch the StandardSection List
                studView.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();
                studView.LocationList = _uiddlRepo.getLocationDropDown();
                studView.IsSearchSuccessful = false;
                studView.MsgColor = "green";
                studView.SuccessOrFailureMessage = "Please Select atleast 1 Search Criteria";
            }



            return View(studView);
        }

        [HttpGet]
        public ActionResult Edit(int studentId)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(StudentVM studentView, string Command)
        {
            //if Command Add then Redirect it to Add
            if (string.Equals(Command, "Add"))
            {
                return RedirectToAction("Register");
            }
            StudentVM studView = null;
            StudentDTO student = null;

            //Fetch the StandardSection List
            studentView.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();

            if (studentView != null)
            {
                student = new StudentDTO();
                //studentView.UserDetails = new UserMasterDTO();
                student.UserDetails = new UserMasterDTO();
                // Search for FName LName and MName

                student.UserDetails.FName = studentView.UserDetails.FName;
                //student.UserDetails.MName = studentView.UserDetails.MName;
                student.UserDetails.LName = studentView.UserDetails.LName;

                student.StandardSectionMap = new StandardSectionMapDTO();
                student.StandardSectionMap.Standard = new StandardDTO();
                student.StandardSectionMap.Section = new SectionDTO();
                student.UserDetails.Location = new LocationDTO();

                // Search for Class
                student.StandardSectionMap.StandardSectionId = studentView.StandardSectionMap.StandardSectionId;

                //Search by Location
                student.UserDetails.Location.LocationId = studentView.UserDetails.Location.LocationId;

                // Search for Roll and Registration
                //student.RollNumber = studentView.RollNumber;
                student.RegistrationNumber = studentView.RegistrationNumber;

                StatusDTO<List<StudentDTO>> status = _studSvc.Select(student);

                if (status.ReturnObj != null && status.ReturnObj.Count > 0)
                {
                    studView = new StudentVM(); // Instantiating Student View model
                    studView.studentList = new List<StudentVM>(); // instantiating list of Students

                    //Fetch the StandardSection List
                    studView.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();
                    studView.LocationList = _uiddlRepo.getLocationDropDown();

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
                                searchItem.FatherContact = stud.FatherContact;
                                searchItem.RegistrationNumber = stud.RegistrationNumber;
                                searchItem.RollNumber = stud.RollNumber;

                                searchItem.UserDetails = new UserMasterDTO();
                                searchItem.UserDetails.UserMasterId = stud.UserDetails.UserMasterId;
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
                                searchItem.UserDetails.Location = new LocationDTO();

                                searchItem.StandardSectionMap.Standard.StandardName = stud.StandardSectionMap.Standard.StandardName;
                                searchItem.StandardSectionMap.Section.SectionName = stud.StandardSectionMap.Section.SectionName;

                                searchItem.UserDetails.Location.LocationDescription = stud.UserDetails.Location.LocationDescription;
                                searchItem.UserDetails.ContactNo = stud.UserDetails.ContactNo;

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
                    //Fetch the StandardSection List
                    studentView.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();
                    studView.LocationList = _uiddlRepo.getLocationDropDown();
                }
            }

            Session["SEARCH_RESULT"] = studView;
            return View(studView);
        }



        private void SaveImageFiles(string directoryPath, string uploadedFileName, string regNo, HttpPostedFileBase currentFile)
        {
            regNo = regNo.Replace('/', '_').Replace('\\', '_');
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            string fileName = uploadedFileName;
            string[] arrNameWithExtension = fileName.Split('.');
            string currentExtension = arrNameWithExtension[arrNameWithExtension.Length - 1];
            string filePath = directoryPath + "\\" + regNo + "." + currentExtension;
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            currentFile.SaveAs(filePath);
        }

        [HttpPost]
        public ActionResult Register(Models.StudentVM studentView, HttpPostedFileBase file)
        {
            string folderName = string.Empty;
            //if (file != null)
            //{
            //if (file.ContentLength > 0)
            //{
            if (Request.Files != null && Request.Files.Count > 0)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    if (Request.Files[i].ContentLength > 0 && Request.Files[i].FileName.Trim().Length > 0)
                    {
                        string keyName = Request.Files.Keys[i];
                        switch (keyName)
                        {
                            case "fuFatherImage":
                                folderName = _configSvc.GetFatherImagesFolder();
                                SaveImageFiles(folderName, Request.Files[i].FileName, studentView.RegistrationNumber, Request.Files[i]);
                                break;
                            case "fuMotherImage":
                                folderName = _configSvc.GetMotherImagesFolder();
                                SaveImageFiles(folderName, Request.Files[i].FileName, studentView.RegistrationNumber, Request.Files[i]);
                                break;
                            case "fuStudentImage":
                                folderName = _configSvc.GetStudentImagesFolder();
                                SaveImageFiles(folderName, Request.Files[i].FileName, studentView.RegistrationNumber, Request.Files[i]);
                                break;
                        }
                    }
                }
            }

            //}
            //}

            DateTime dtValidator = new DateTime();
            if (DateTime.TryParse(studentView.DOBString, out dtValidator))
            {
                if (studentView.UserDetails == null)
                {
                    studentView.UserDetails = new UserMasterDTO();
                }
                studentView.UserDetails.DOB = dtValidator;
            }

            if (string.Equals(studentView.MODE, "EDIT", StringComparison.OrdinalIgnoreCase))
            {
                //Call update
                //if (ModelState.IsValid)
                //{ 
                StatusDTO<StudentDTO> status = _studSvc.Update(studentView);
                if (status.IsSuccess)
                {
                    if (studentView.Transactions != null && studentView.Transactions.Count > 0)
                    {
                        for (int i = 0; i < studentView.Transactions.Count; i++)
                        {
                            if (studentView.Transactions[i].UserTransactionId > 0)
                            {
                                _userTrans.Update(studentView.Transactions[i]);
                            }
                            else
                            {
                                studentView.Transactions[i].User = new UserMasterDTO();
                                studentView.Transactions[i].User.UserMasterId = studentView.UserDetails.UserMasterId;
                                _userTrans.Insert(studentView.Transactions[i]);
                            }
                        }
                    }

                    return RedirectToAction("Search");
                }
                studentView.ErrorMessage = status.FailureReason;
                //}
            }
            else
            {
                //Call insert

                //if (ModelState.IsValid)
                //{   
                string pass = !string.IsNullOrEmpty(studentView.UserDetails.Password) ? encrypt.encryption(studentView.UserDetails.Password) : null;
                studentView.UserDetails.Password = pass;
                StatusDTO<StudentDTO> status = _studSvc.Insert(studentView);
                studentView.UserDetails = new UserMasterDTO();
                studentView.UserDetails.UserMasterId = status.ReturnObj.UserDetails.UserMasterId;
                if (status.IsSuccess)
                {
                    if (studentView.Transactions != null && studentView.Transactions.Count > 0)
                    {
                        for (int i = 0; i < studentView.Transactions.Count; i++)
                        {
                            if (studentView.Transactions[i].UserTransactionId > 0)
                            {
                                _userTrans.Update(studentView.Transactions[i]);
                            }
                            else
                            {
                                studentView.Transactions[i].User = new UserMasterDTO();
                                studentView.Transactions[i].User.UserMasterId = status.ReturnObj.UserDetails.UserMasterId;
                                _userTrans.Insert(studentView.Transactions[i]);
                            }
                        }
                    }

                    //return RedirectToAction("Register", new { mode = "EDIT", id = studentView.UserDetails.UserMasterId.ToString() });
                    return RedirectToAction("Search");
                }
                studentView.ErrorMessage = status.FailureReason;
                //}                
            }
            //if(ModelState.IsValid)
            //{
            //}
            //ModelState.Clear();

            studentView.TransactionMasters = _uiddlRepo.getTransactionMasters();
            studentView.GraceAmountOnList = _uiddlRepo.getCalcType();

            studentView.CalcInSelectList = _uiddlRepo.getCalcTypeDic();
            studentView.TransactionMasterSelectList = _dropDwnRepo.GetTransactionMasters();

            studentView.GenderList = _uiddlRepo.getGenderDropDown();
            studentView.LocationList = _uiddlRepo.getLocationDropDown();
            studentView.RoleList = _uiddlRepo.getRoleDropDown();
            //uvModel.ClassTypeList = _uiddlRepo.getClassTypeDropDown();
            studentView.SectionList = _uiddlRepo.getSectionDropDown();
            studentView.HouseList = _uiddlRepo.getHouseDropDown();
            //uvModel.BookCategoryList = _uiddlRepo.getBookCategoryDropDown();
            //uvModel.DepartmentList = _uiddlRepo.getDepartmentDropDown();
            //uvModel.DesignationList = _uiddlRepo.getDesignationDropDown();
            studentView.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();

            studentView.Transactions = _userTrans.GetUserTransactions(studentView.UserDetails.UserMasterId);

            return View(studentView);
        }

        [HttpGet]
        public ActionResult PromoteToNewClass()
        {
            StudentVM studView = new StudentVM();

            //Fetch the StandardSection List
            studView.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();
            //fetch the location list
            studView.LocationList = _uiddlRepo.getLocationDropDown();

            return View(studView);
        }


        [HttpPost]
        public ActionResult PromoteToNewClass(StudentVM studentView, string Command)
        {
            int StandardSectionId = 0;
            int currentSerial = 0;
            int Serial = 0;
            StudentVM studView = null;
            List<StudentDTO> studListPass = null;
            int LocationId = 0;

            if (Command != null)
            {
                // storing all data in studPass object and Passing it to PromoteToNewClass()
                studListPass = new List<StudentDTO>();

                if (string.Equals(Command, "Promotion Confirmed"))
                {
                    StatusDTO<List<StudentDTO>> batchStatus = _studSvc.PromoteToNewClass(null, Command, 0, 0);

                    if (batchStatus.ReturnObj != null)
                    {
                        return RedirectToAction("PromoteToNewClass");
                    }
                }

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
                if (studentView != null && (string.Equals(Command, "Promote")))
                {
                    foreach (StudentVM studVM in studentView.studentList)
                    {
                        StudentDTO student = new StudentDTO();
                        student.StudentInfoId = studVM.StudentInfoId;

                        if (string.Equals(studVM.Status, "1"))
                        {
                            student.Status = "Promotion Confirmed";
                            student.NewStandardSectionId = studVM.NewStandardSectionId;
                        }
                        else if (string.Equals(studVM.Status, "2"))
                        {
                            student.Status = "Failed";
                            student.NewStandardSectionId = StandardSectionId;
                        }
                        studListPass.Add(student);
                    }
                }

                //Get Students for that Particular class or Promote Students to New Class
                StatusDTO<List<StudentDTO>> status = _studSvc.PromoteToNewClass(studListPass, Command, StandardSectionId, LocationId);

                if ((status.ReturnObj != null))
                {
                    studView = new StudentVM(); // Instantiating Student View model As a Parent Whole
                    studView.studentList = new List<StudentVM>(); // instantiating list of Students

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

                                if (!string.IsNullOrEmpty(studentV.Status))
                                    studView.IsCommandPromote = true;
                            }
                        }
                    }

                }
            }
            if (string.Equals(Command, "Standard"))
            {
                studView.NextStandardSectionList = _uiddlRepo.getNextStandardSectionDropDown(currentSerial);
            }
            return View(studView);
        }
        [HttpGet]
        public ActionResult PromotionBatch()
        {
            StudentVM studView = null;

            StatusDTO<List<StudentDTO>> studList = _studSvc.RunPromotionBatch();
            if (studList != null && studList.IsSuccess)
            {
                if (studList.ReturnObj != null && studList.ReturnObj.Count > 0)
                {
                    studView = new StudentVM();// instantiating parent
                    studView.studentList = new List<StudentVM>();//instantiating list of parent
                    foreach (StudentDTO student in studList.ReturnObj)
                    {
                        if (student != null)
                        {
                            StudentVM studV = new StudentVM();// for each student
                            studV.StandardSectionMap = new StandardSectionMapDTO();
                            studV.StandardSectionMap.StandardSectionDesc = student.StandardSectionMap.StandardSectionDesc;
                            studV.Status = student.Status;
                            studV.NoOfStudents = student.NoOfStudents;
                            studView.studentList.Add(studV);
                        }
                    }
                }
            }
            else
            {
                studView = new StudentVM();
            }
            return View(studView);
        }
        [HttpPost]
        public ActionResult PromotionBatch(StudentVM studView)
        {
            bool isSuccess = false;
            int loggedInUser;
            DateTime date;
            string status;// right now hard coded
            if (studView != null)
            {
                SessionDTO sessionRet = _sessionSvc.GetUserSession();//Get Data from User Seesion
                status = "Promotion Confirmed";
                loggedInUser = sessionRet.UserMasterId;
                if (status != null && loggedInUser != 0)
                {
                    isSuccess = _studSvc.UpdatePromotedStudents(loggedInUser, status);
                    if (isSuccess)
                    {
                        date = DateTime.Now;
                        studView.SuccessOrFailureMessage = "The students of all classes have been promoted for academic year" + (int)(date.Year - 1) + "/" + (date.Year);
                        studView.MsgColor = "green";
                    }
                    else
                    {
                        studView.SuccessOrFailureMessage = "All students have not been Promoted yet";
                        studView.MsgColor = "red";
                    }
                }
            }
            return View(studView);
        }
        [HttpPost]
        [AllowAnonymous]
        public JsonResult Delete(StudentDTO stud)
        {
            //StudentDTO stud = null;
            if (stud.UserDetails != null && stud.UserDetails.UserMasterId != 0)
            {
                //stud = new StudentDTO();
                //stud.UserDetails = new UserMasterDTO();
                //stud.UserDetails.UserMasterId = id;
                StatusDTO<StudentDTO> status = _studSvc.Delete(stud);
                if (status != null && status.IsSuccess)
                {

                }
            }
            return Json(new { status = "true", message = "Deleted!!!" }, JsonRequestBehavior.AllowGet);
        }
    }
}

