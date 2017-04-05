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

namespace OperationsManager.Areas.Student.Controllers
{
    [OpMgrAuth]
    [HandleError()]
    public class StudentController : Controller
    {
        private IStudentSvc _studSvc;
        //private ILogSvc _logSvc;
        private IDropdownRepo _dropDwnRepo;
        private Helpers.UIDropDownRepo _uiddlRepo;
        Encryption encrypt = new Encryption();
        private IUserTransactionSvc _userTrans;

        private IConfigSvc _configSvc;

        public StudentController(IStudentSvc studSvc, IDropdownRepo dropDwnRepo, IUserTransactionSvc userTrans, IConfigSvc configSvc)
        {
            _studSvc = studSvc;
            _dropDwnRepo = dropDwnRepo;
            _uiddlRepo = new Helpers.UIDropDownRepo(_dropDwnRepo);
            //_logSvc = logSvc;
            _userTrans = userTrans;
            _configSvc = configSvc;
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
            if(_userTrans.Delete(uTrans).IsSuccess)
            {
                return Json(new { status = true, message = "Deleted successfully." }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = false, message = "Delete failed." }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Register(string mode, string id)
        {
            Models.StudentVM studView = new Models.StudentVM();
            studView.UserDetails = new UserMasterDTO();
            studView.MODE = mode;
            
            if (string.Equals(mode, "EDIT", StringComparison.OrdinalIgnoreCase))
            {
                studView.UserDetails.UserMasterId = int.Parse(id);
            }
            studView.Transactions = new List<UserTransactionDTO>();
            if (mode != null && string.Equals(mode, "EDIT", StringComparison.OrdinalIgnoreCase))
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
                studView.GuardianContact = dto.ReturnObj.GuardianContact;
                studView.GuardianName = dto.ReturnObj.GuardianName;
                studView.GuardianEmailId = dto.ReturnObj.GuardianEmailId;
                studView.HouseType = dto.ReturnObj.HouseType;
                studView.StandardSectionMap = dto.ReturnObj.StandardSectionMap;

                studView.Transactions = _userTrans.GetUserTransactions(dto.ReturnObj.UserDetails.UserMasterId);
                studView.TransactionMasters = _uiddlRepo.getTransactionMasters();
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
            //uvModel.BookCategoryList = _uiddlRepo.getBookCategoryDropDown();
            //uvModel.DepartmentList = _uiddlRepo.getDepartmentDropDown();
            //uvModel.DesignationList = _uiddlRepo.getDesignationDropDown();
            studView.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();
            studView.GraceAmountOnList = _uiddlRepo.getCalcType();

            return View(studView);
        }
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

                            searchItem.Name = student.UserDetails.FName;
                            if (!string.IsNullOrEmpty(student.UserDetails.FName))
                            {
                                searchItem.Name = searchItem.Name + " " + student.UserDetails.MName;
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

            if (studentView != null)
            {
                student = new StudentDTO();
                //studentView.UserDetails = new UserMasterDTO();
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

        private void SaveImageFiles(string directoryPath, string uploadedFileName, string regNo)
        {
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
            Request.Files[0].SaveAs(filePath);
        }

        [HttpPost]
        public ActionResult Register(Models.StudentVM studentView)
        {
            string folderName = string.Empty;
            if(Request.Files.Count>0)
            {
                folderName = _configSvc.GetFatherImagesFolder();
                SaveImageFiles(folderName, Request.Files[0].FileName, studentView.RegistrationNumber);
            }

            if(Request.Files.Count>1)
            {
                folderName = _configSvc.GetMotherImagesFolder();
                SaveImageFiles(folderName, Request.Files[0].FileName, studentView.RegistrationNumber);
            }

            if (Request.Files.Count > 1)
            {
                folderName = _configSvc.GetStudentImagesFolder();
                SaveImageFiles(folderName, Request.Files[0].FileName, studentView.RegistrationNumber);
            }

            if (string.Equals(studentView.MODE, "EDIT", StringComparison.OrdinalIgnoreCase))
            {
                //Call update
                //if (ModelState.IsValid)
                //{ 
                StatusDTO<StudentDTO> status = _studSvc.Update(studentView);
                if(status.IsSuccess)
                {
                    if(studentView.Transactions!=null && studentView.Transactions.Count>0)
                    {
                        for(int i=0;i<studentView.Transactions.Count;i++)
                        {
                            if(studentView.Transactions[i].UserTransactionId>0)
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
                }                    
                
                //}
            }
            else
            {
                //Call insert

                //if (ModelState.IsValid)
                //{   
                string pass = encrypt.encryption(studentView.UserDetails.Password);
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

                    return RedirectToAction("Register", new { mode = "EDIT", id = studentView.UserDetails.UserMasterId.ToString() });
                }
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
    }
}