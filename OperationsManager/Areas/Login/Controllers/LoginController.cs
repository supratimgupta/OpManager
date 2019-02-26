using OpMgr.Common.Contracts;
using OpMgr.Common.Contracts.Modules;
using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OperationsManager.Attributes;
using OperationsManager.Models;
using OperationsManager.Areas.Login.Models;
using System.Net.Mail;
using System.Net;
using System.Drawing;
using OperationsManager.Controllers;
using System.IO;

namespace OperationsManager.Areas.Login.Controllers
{
    public class LoginController : BaseController
    {
        private IUserSvc _userSvc;
        private IResetPasswordSvc _resetPassSvc;

        //private ILogSvc _logger;

        private ISessionSvc _sessionSvc;
        private IDropdownRepo _ddlRepo;
        private PasswordGenerator.PasswordGenerator _passGen;// taken for dependency but not used DOUBT!!!
        private IMailSvc _mail;
        private IConfigSvc _configSvc;
        private INotificationSvc _notiSvc;

        private Helpers.UIDropDownRepo _uiddlRepo;

        Encryption encrypt = new Encryption();

        public LoginController(IUserSvc userSvc, IDropdownRepo ddlRepo, ISessionSvc sessionSvc, IResetPasswordSvc resetPassSvc, IMailSvc mail, IConfigSvc configSvc, INotificationSvc notiSvc)
        {
            _userSvc = userSvc;
            _ddlRepo = ddlRepo;
            //new OpMgr.DataAccess.Implementations.DropdownRepo(new OpMgr.Configurations.Implementations.ConfigSvc());
            _uiddlRepo = new Helpers.UIDropDownRepo(_ddlRepo);
            //_logger = logger;
            _configSvc = configSvc;
            _sessionSvc = sessionSvc;
            _resetPassSvc = resetPassSvc;
            _mail = mail;//dependency injected for sending mails
            _notiSvc = notiSvc;
        }

        // GET: Login/Login
        // GET: Login/Login
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            UserMasterDTO userDto = new UserMasterDTO();

            if (Request.Cookies["userDetails"] != null)
            {
                var userId = Request.Cookies["userDetails"]["uid"];
                var pwd = Request.Cookies["userDetails"]["pwd"];

                if (!string.IsNullOrEmpty(userId) && pwd != null && !string.IsNullOrEmpty(pwd))
                {
                    //userDto = new UserMasterDTO();
                    userDto.RememberMe = true;
                    userDto.UserName = userId;
                    userDto.Password = pwd;
                    return View(userDto);
                }
            }
            return View(userDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Login(UserMasterDTO data)
        {
            List<EntitlementDTO> lstEntitleMent = new List<EntitlementDTO>();
            List<ActionDTO> lstAction = new List<ActionDTO>();
            List<LocationDTO> lstLocation = new List<LocationDTO>();

            string unencryptedPass = data.Password;
            string pass = encrypt.encryption(data.Password);
            data.Password = pass;

            StatusDTO<UserMasterDTO> status = _userSvc.Login(data, out lstEntitleMent, out lstAction, out lstLocation);
            if (status.IsSuccess)
            {
                if (data.RememberMe)
                {
                    HttpCookie cookie = new HttpCookie("userDetails");
                    cookie["uid"] = data.UserName;
                    cookie["pwd"] = unencryptedPass;
                    cookie.Expires = DateTime.Now + new TimeSpan(1, 0, 0, 0);

                    if (Request.Cookies["userDetails"] != null)
                    {
                        Response.Cookies.Set(cookie);
                    }
                    else
                    {
                        Response.Cookies.Add(cookie);
                    }
                }
                else
                {
                    Response.Cookies.Remove("userDetails");
                }

                SessionDTO session = new SessionDTO();
                session.UserMasterId = status.ReturnObj.UserMasterId;
                session.UserName = status.ReturnObj.UserName;
                session.FName = status.ReturnObj.FName;
                session.MName = status.ReturnObj.MName;
                session.LName = status.ReturnObj.LName;
                session.StudentCount = status.ReturnObj.StudentCount;
                session.StaffCount = status.ReturnObj.StaffCount;
                session.PaidStudentCount = status.ReturnObj.PaidStudentCount;
                session.PendingPaymentCount = status.ReturnObj.PendingPaymentCount;
                session.PaidCountlast7day = status.ReturnObj.PaidCountlast7day;

                session.UniqueEmployeeId = status.ReturnObj.UniqueEmployeeId;

                session.ActionList = lstAction;
                session.EntitleMentList = lstEntitleMent;
                session.LocationList = lstLocation;

                if (string.Equals(status.ReturnObj.UserType, "STUDENT", StringComparison.OrdinalIgnoreCase))
                {
                    session.IconImagePath = _configSvc.GetStudentImagesRelPath() + "/" + status.ReturnObj.UniqueId + ".jpg";
                }
                if (string.Equals(status.ReturnObj.UserType, "STAFF", StringComparison.OrdinalIgnoreCase))
                {
                    session.IconImagePath = _configSvc.GetEmployeeImagesRelPath() + "/" + status.ReturnObj.UniqueId + ".jpg";
                }
                NotificationDTO nDTO = new NotificationDTO();
                nDTO.User = new UserMasterDTO();
                nDTO.User.UserMasterId = session.UserMasterId;
                session.Notifications = _notiSvc.Select(nDTO).ReturnObj;
                if (session.Notifications != null)
                {
                    session.NotificationCounts = session.Notifications.Count;
                }
                else
                {
                    session.NotificationCounts = 0;
                }
                _sessionSvc.SetUserSession(session);
                SessionDTO sessionRet = _sessionSvc.GetUserSession();
            }
            else
            {
                data.LoginFailedMsg = status.FailureReason;
                return View(data);
            }

            return RedirectToAction("Landing", "Login", new { area = "Login" });
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Welcome()
        {
            SessionDTO sessionRet = _sessionSvc.GetUserSession();

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Landing()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Register(string mode, string id)
        {
            Models.UserViewModel uvModel = new Models.UserViewModel();
            uvModel.MODE = mode;
            uvModel.DisabledClass = string.Empty;
            if (string.Equals(mode, "EDIT", StringComparison.OrdinalIgnoreCase) || string.Equals(mode, "VIEW", StringComparison.OrdinalIgnoreCase))
            {
                uvModel.UserMasterId = int.Parse(id);
            }
            if (string.Equals(mode, "VIEW", StringComparison.OrdinalIgnoreCase))
            {
                uvModel.DisabledClass = "disabledPlace";
            }
            if (mode != null && string.Equals(mode, "EDIT", StringComparison.OrdinalIgnoreCase) || string.Equals(mode, "VIEW", StringComparison.OrdinalIgnoreCase))
            {
                //Populate edit data using id passed in URL, if id==null then show error message
                StatusDTO<UserMasterDTO> dto = _userSvc.Select(Convert.ToInt32(id));
                uvModel.UserMasterId = dto.ReturnObj.UserMasterId;
                //uvModel.UserMasterId = dto.ReturnObj.UserMasterId;
                uvModel.FName = dto.ReturnObj.FName;
                uvModel.MName = dto.ReturnObj.MName;
                uvModel.LName = dto.ReturnObj.LName;
                uvModel.Gender = dto.ReturnObj.Gender;
                uvModel.Image = dto.ReturnObj.Image;
                uvModel.DOB = dto.ReturnObj.DOB;
                uvModel.DOBString = uvModel.DOB.HasValue ? uvModel.DOB.Value.ToString("dd-MMM-yyyy") : string.Empty;
                uvModel.EmailId = dto.ReturnObj.EmailId;
                uvModel.ResidentialAddress = dto.ReturnObj.ResidentialAddress;
                uvModel.PermanentAddress = dto.ReturnObj.PermanentAddress;
                uvModel.ContactNo = dto.ReturnObj.ContactNo;
                uvModel.AltContactNo = dto.ReturnObj.AltContactNo;
                uvModel.BloodGroup = dto.ReturnObj.BloodGroup;
                uvModel.Location = dto.ReturnObj.Location;
                uvModel.Role = dto.ReturnObj.Role;
                uvModel.UserEntitlementList = _userSvc.GetUserEntitlement(dto.ReturnObj.UserMasterId);
                uvModel.SelectUserEntitlement = _ddlRepo.GetUserRole();
                uvModel.Subject = _ddlRepo.Subject();

                uvModel.Employee = new EmployeeDetailsDTO();
                if (dto.ReturnObj.Employee != null)
                {
                    uvModel.Employee.EmployeeId = dto.ReturnObj.Employee.EmployeeId;
                    uvModel.hdnEmployeeId = dto.ReturnObj.Employee.EmployeeId;
                    uvModel.Employee.EducationalQualification = dto.ReturnObj.Employee.EducationalQualification;
                    uvModel.Employee.DateOfJoining = dto.ReturnObj.Employee.DateOfJoining;

                    uvModel.DOJString = uvModel.Employee.DateOfJoining.HasValue ? uvModel.Employee.DateOfJoining.Value.ToString("dd-MMM-yyyy") : string.Empty;

                    uvModel.Employee.StaffEmployeeId = dto.ReturnObj.Employee.StaffEmployeeId;
                    uvModel.DummyStaffEmployeeId = uvModel.Employee.StaffEmployeeId;
                    uvModel.Employee.Department = dto.ReturnObj.Employee.Department;
                    uvModel.Employee.Designation = dto.ReturnObj.Employee.Designation;

                    uvModel.FacultyCourseList = _userSvc.GetFacultyCourseMap(dto.ReturnObj.Employee.EmployeeId);
                    //get PMS designation Map
                    uvModel.PMSEmpDesignationMapList = _userSvc.GetPMSDesignationMap(dto.ReturnObj.Employee.EmployeeId);
                    if (dto.ReturnObj.Employee.ClassType != null)
                    {
                        uvModel.Employee.ClassType = dto.ReturnObj.Employee.ClassType;
                    }

                    string employeeImageFolder = _configSvc.GetEmployeeImagesFolder();

                    uvModel.employeeimagepath = _configSvc.GetEmployeeImagesRelPath() + "/" + GetImageFileName(uvModel.Employee.StaffEmployeeId, employeeImageFolder) + "?ver=" + DateTime.UtcNow.Ticks;
                    //if(dto.ReturnObj.Employee.ClassType != null)
                    //{
                    //    uvModel.Employee.Subject = dto.ReturnObj.Employee.Subject;
                    //}
                }
            }

            uvModel.GenderList = _uiddlRepo.getGenderDropDown();
            uvModel.LocationList = _uiddlRepo.getLocationDropDown();
            uvModel.RoleList = _uiddlRepo.getRoleDropDown();
            uvModel.DepartmentList = _uiddlRepo.getDepartmentDropDown();
            uvModel.DesignationList = _uiddlRepo.getDesignationDropDown();
            uvModel.PmsDesignationList = _ddlRepo.PmsDesignation();
            uvModel.SelectUserEntitlement = _ddlRepo.GetUserRole();
            uvModel.ClassTypeList = _uiddlRepo.getClassTypeDropDown();
            //uvModel.SubjectList = _uiddlRepo.getSubjectDropDown();

            return View(uvModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult GenerateEmployeeId(string fName, string lName)
        {
            string name = fName.Trim() + " " + lName.Trim();
            int currentEmployeeCounter = _userSvc.GetCurrentEmployeeCounter();
            string employeeId = OpMgr.Utilities.Utility.MakeEmployeeId(currentEmployeeCounter, name, 5, 2);
            return Json(new { data = employeeId, status = true });
        }

        public string GetImageFileName(string staffempid, string folder)
        {
            string fileName = string.Empty;
            string[] similarFiles = Directory.GetFiles(folder, staffempid + ".*");
            if (similarFiles != null && similarFiles.Length > 0)
            {
                fileName = similarFiles[0];
                string[] fileParts = fileName.Split('\\');
                fileName = fileParts[fileParts.Length - 1];
            }
            return fileName;
        }

        private void SaveImageFiles(string directoryPath, string uploadedFileName, string empId)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            string fileName = uploadedFileName;
            string[] arrNameWithExtension = fileName.Split('.');
            string currentExtension = arrNameWithExtension[arrNameWithExtension.Length - 1];
            string filePath = directoryPath + "\\" + empId + "." + currentExtension;
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            Request.Files[0].SaveAs(filePath);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Register(Models.UserViewModel uvModel, HttpPostedFileBase file)
        {
            string folderName = string.Empty;
            //if (file != null)
            //{
            //    if (file.ContentLength > 0)
            //    {
            if (Request.Files != null && Request.Files.Count > 0)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    if (Request.Files[i].ContentLength > 0 && Request.Files[i].FileName.Trim().Length > 0)
                    {
                        string keyName = Request.Files.Keys[i];
                        folderName = _configSvc.GetEmployeeImagesFolder();
                        SaveImageFiles(folderName, Request.Files[i].FileName, uvModel.Employee.StaffEmployeeId);
                    }
                }
            }
            //    }
            //}

            SessionDTO sessionRet = _sessionSvc.GetUserSession();
            uvModel.CreatedBy = sessionRet;

            DateTime dtValidator = new DateTime();
            if (DateTime.TryParse(uvModel.DOBString, out dtValidator))
            {
                uvModel.DOB = dtValidator;
            }

            if (DateTime.TryParse(uvModel.DOJString, out dtValidator))
            {
                if (uvModel.Employee == null)
                {
                    uvModel.Employee = new EmployeeDetailsDTO();
                }
                uvModel.Employee.DateOfJoining = dtValidator;
            }

            if (string.Equals(uvModel.MODE, "EDIT", StringComparison.OrdinalIgnoreCase))
            {
                //Call update
                if (!string.IsNullOrEmpty(uvModel.Password))
                {
                    string pass = encrypt.encryption(uvModel.Password);
                    uvModel.Password = pass;
                }
                uvModel.UpdatedBy = sessionRet;

                StatusDTO<UserMasterDTO> status = _userSvc.Update(uvModel);

                if (status.IsSuccess)
                {
                    if (uvModel.UserEntitlementList != null && uvModel.UserEntitlementList.Count > 0)
                    {
                        for (int i = 0; i < uvModel.UserEntitlementList.Count; i++)
                        {
                            if (uvModel.UserEntitlementList[i].RowId > 0)
                            {
                                _userSvc.UpdateUserEntitlement(uvModel.UserEntitlementList[i]);
                            }
                            else
                            {
                                uvModel.UserEntitlementList[i].UserDetails = new UserMasterDTO();
                                uvModel.UserEntitlementList[i].UserDetails.UserMasterId = uvModel.UserMasterId;
                                _userSvc.InsertUserEntitlement(uvModel.UserEntitlementList[i]);
                            }
                        }
                    }

                    // update PMS Emp Designation
                    if (uvModel.PMSEmpDesignationMapList != null && uvModel.PMSEmpDesignationMapList.Count > 0)
                    {
                        for (int i = 0; i < uvModel.PMSEmpDesignationMapList.Count; i++)
                        {
                            if (uvModel.PMSEmpDesignationMapList[i].PmsEmpDesmapId > 0)
                            {
                                uvModel.PMSEmpDesignationMapList[i].UpdatedBy = new UserMasterDTO();
                                uvModel.PMSEmpDesignationMapList[i].UpdatedBy.UserMasterId = Convert.ToInt32(_sessionSvc.GetUserSession().UserMasterId);
                                _userSvc.UpdatePMSDesignationMap(uvModel.PMSEmpDesignationMapList[i]);
                            }
                            else
                            {
                                uvModel.PMSEmpDesignationMapList[i].CreatedBy = new UserMasterDTO();
                                uvModel.PMSEmpDesignationMapList[i].CreatedBy.UserMasterId = Convert.ToInt32(_sessionSvc.GetUserSession().UserMasterId);
                                uvModel.PMSEmpDesignationMapList[i].Employee = new EmployeeDetailsDTO();
                                uvModel.PMSEmpDesignationMapList[i].Employee.EmployeeId = uvModel.hdnEmployeeId;
                                _userSvc.InsertPMSDesignationMap(uvModel.PMSEmpDesignationMapList[i]);
                            }
                        }
                    }

                    if (uvModel.FacultyCourseList != null && uvModel.FacultyCourseList.Count > 0)
                    {
                        for (int i = 0; i < uvModel.FacultyCourseList.Count; i++)
                        {
                            if (uvModel.FacultyCourseList[i].FacultyCourseMapId > 0)
                            {
                                _userSvc.UpdateFacultyCourseMap(uvModel.FacultyCourseList[i]);
                            }
                            else
                            {
                                uvModel.FacultyCourseList[i].Employee = new EmployeeDetailsDTO();
                                uvModel.FacultyCourseList[i].Employee.EmployeeId = uvModel.hdnEmployeeId;
                                _userSvc.InsertFacultyCourse(uvModel.FacultyCourseList[i]);
                            }
                        }
                    }
                }

                return RedirectToAction("Search", "User", new { area = "User" });
            }
            else
            {
                //Call insert

                string pass = encrypt.encryption(uvModel.Password);
                uvModel.Password = pass;

                uvModel.CreatedBy = sessionRet;
                StatusDTO<UserMasterDTO> status = _userSvc.Insert(uvModel);

                if (status.IsSuccess)
                {
                    if (uvModel.UserEntitlementList != null && uvModel.UserEntitlementList.Count > 0)
                    {
                        for (int i = 0; i < uvModel.UserEntitlementList.Count; i++)
                        {
                            uvModel.UserEntitlementList[i].UserDetails = new UserMasterDTO();
                            uvModel.UserEntitlementList[i].UserDetails.UserMasterId = status.ReturnObj.UserMasterId;
                            _userSvc.InsertUserEntitlement(uvModel.UserEntitlementList[i]);
                        }
                    }

                    if (uvModel.FacultyCourseList != null && uvModel.FacultyCourseList.Count > 0)
                    {
                        for (int i = 0; i < uvModel.FacultyCourseList.Count; i++)
                        {
                            uvModel.FacultyCourseList[i].Employee = new EmployeeDetailsDTO();
                            uvModel.FacultyCourseList[i].Employee.EmployeeId = status.ReturnObj.Employee.EmployeeId;
                            _userSvc.InsertFacultyCourse(uvModel.FacultyCourseList[i]);
                        }
                    }

                    //Assign Multiple PMS Designation
                    if (uvModel.PMSEmpDesignationMapList != null && uvModel.PMSEmpDesignationMapList.Count > 0)
                    {
                        for (int i = 0; i < uvModel.PMSEmpDesignationMapList.Count; i++)
                        {
                            uvModel.PMSEmpDesignationMapList[i].CreatedBy = new UserMasterDTO();
                            uvModel.PMSEmpDesignationMapList[i].CreatedBy.UserMasterId = Convert.ToInt32(_sessionSvc.GetUserSession().UserMasterId);
                            uvModel.PMSEmpDesignationMapList[i].Employee = new EmployeeDetailsDTO();

                            uvModel.PMSEmpDesignationMapList[i].Employee.EmployeeId = status.ReturnObj.Employee.EmployeeId;
                            _userSvc.InsertPMSDesignationMap(uvModel.PMSEmpDesignationMapList[i]);
                        }
                    }
                }
                uvModel.GenderList = _uiddlRepo.getGenderDropDown();
                uvModel.LocationList = _uiddlRepo.getLocationDropDown();
                uvModel.RoleList = _uiddlRepo.getRoleDropDown();
                uvModel.DepartmentList = _uiddlRepo.getDepartmentDropDown();
                uvModel.DesignationList = _uiddlRepo.getDesignationDropDown();
                //uvModel.PmsDesignationList = _uiddlRepo.getPMSDesignationDropDown();

                //return View(uvModel);           
                return RedirectToAction("Search", "User", new { area = "User" });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Logout()
        {
            _sessionSvc.Logout();
            return RedirectToAction("Login");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            MailVM mailVM = new MailVM();
            return View(mailVM);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ForgotPassword(MailVM mailView)
        {
            string[] mailTo;
            MailDTO mail;
            bool isMailSent = false; ;
            bool isResetPassword = false;
            int UserMasterId;

            string newPassword = null;
            string newPasswordEncrypted = null;
            if (mailView != null)
            {
                UserMasterId = _mail.CheckMail(mailView.To);
                if (UserMasterId != 0)
                {

                    PasswordGenerator.PasswordGenerator passGen = new PasswordGenerator.PasswordGenerator();
                    passGen.InitializePasswordArrays();

                    PasswordVM passVM = new PasswordVM();
                    passVM.CapitalLettersLength = 1;
                    passVM.DigitsLength = 1;
                    passVM.SmallLettersLength = 3;
                    passVM.SpecialCharactersLength = 1;
                    passVM.PasswordLength = 6;

                    newPassword = passGen.GeneratePassword(passVM); // get password

                    if (!string.IsNullOrEmpty(newPassword))
                    {
                        newPasswordEncrypted = encrypt.encryption(newPassword);//Encrypt Password

                        isResetPassword = _resetPassSvc.ResetPassword(newPasswordEncrypted, UserMasterId);
                        if (isResetPassword)
                        {
                            mail = new MailDTO();
                            mail.From = "ruttu04@gmail.com";
                            mail.IsBodyHtml = false;
                            mail.MailSubject = "Reset Password";
                            mail.MailBody = "Your new password which is reset is: " + newPassword;
                            mail.SmtpPort = 587;
                            mail.SmtpServer = "smtp.gmail.com";
                            mail.EnableSSL = true;
                            mail.UseDefaultCredentials = true;
                            //split the ';' separated string To a List
                            mailTo = mailView.To.Split(';');
                            mail.ToList = new List<string>();
                            for (int i = 0; i < mailTo.Length; i++)
                            {
                                mail.ToList.Add(mailTo[i]);
                            }
                            //instantiate CC and Bcc List if possible
                            isMailSent = _mail.SendMail(mail);// call SendMail method to send the mail 
                            if (isMailSent)
                            {
                                mailView.SuccessOrFailureMessage = "Your new password: " + newPassword + " is sent to mail";
                                mailView.MessageColor = "green";
                            }
                            else
                            {
                                mailView.SuccessOrFailureMessage = "Your password was not reset in a proper manner";
                                mailView.MessageColor = "red";
                            }
                        }
                    }

                }
                else
                {
                    mailView.SuccessOrFailureMessage = "Sorry your email was incorect. Please enter again";
                    mailView.MessageColor = "red";

                }
            }
            return View(mailView);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPassword()
        {
            UserViewModel userView = null;
            SessionDTO sessionRet = _sessionSvc.GetUserSession();//Get Data from User Seesion
            if (sessionRet != null)
            {
                userView = new UserViewModel();
                userView.Name = sessionRet.FName;
                if (!string.IsNullOrEmpty(sessionRet.MName))
                {
                    userView.Name = userView.Name + " " + sessionRet.MName;
                }
                if (!string.IsNullOrEmpty(sessionRet.LName))
                {
                    userView.Name = userView.Name + " " + sessionRet.LName;
                }
                userView.UserMasterId = sessionRet.UserMasterId;

                var val = _sessionSvc.GetUserSession().EntitleMentList.Find(x => x.RoleName.Contains("Admin"));
                if (val != null)
                {
                    if (val.RoleName == "Admin")
                    {
                        userView.RoleName = "Admin";
                    }
                }

                if (!string.IsNullOrEmpty(userView.RoleName) && userView.RoleName.Contains("Admin"))
                {
                    userView.Password = _resetPassSvc.GetPasswordForUser(userView.UserMasterId);
                    userView.ConfirmPassword = "default1234";
                    userView.NewPassword = "default1234";
                }
            }
            return View(userView);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ResetPassword(UserViewModel userView)
        {
            bool IsPasswordReset = false;
            string studentOrStaffId = null;
            string roleDesc = null;
            if (userView != null)
            {
                if (!string.Equals(userView.NewPassword, userView.ConfirmPassword))
                {
                    userView.SuccessorFailureMessage = "New Password and Confirm Password does not match!!";
                    userView.MessageColor = "red";
                }
                else if (string.Equals(userView.Password, userView.NewPassword))
                {
                    userView.SuccessorFailureMessage = "Password and New Password should not match!!";
                    userView.MessageColor = "red";
                }
                else if (!string.IsNullOrEmpty(userView.RoleName) && !userView.RoleName.Contains("Admin")
                && !string.Equals(encrypt.encryption(userView.Password), _resetPassSvc.GetPasswordForUser(userView.UserMasterId)))
                {
                    userView.SuccessorFailureMessage = "Password is Incorrect!!";
                    userView.MessageColor = "red";

                }
                else
                {
                    studentOrStaffId = userView.StudentOrStaffId;
                    roleDesc = userView.Role.RoleDescription;
                    IsPasswordReset = _resetPassSvc.ResetPassword(encrypt.encryption(userView.NewPassword), userView.UserMasterId, studentOrStaffId, roleDesc);
                    if (IsPasswordReset)
                    {
                        userView.SuccessorFailureMessage = "Your password has been successfully reset";// Sucess
                        userView.MessageColor = "green";
                    }
                    else
                    {
                        userView.SuccessorFailureMessage = "Your password was not successfully reset due to some issues. Please contact Admin.";// Sucess
                        userView.MessageColor = "red";
                    }
                }
            }
            return View(userView);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetUserEntitlementDDL()
        {
            return Json(_ddlRepo.GetUserRole(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetPMSDesignation()
        {
            return Json(_ddlRepo.PmsDesignation(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult DeletePmsDesignationMap(PMSEmpDesignationMapDTO pmsdes)
        {
            pmsdes.UpdatedBy = new UserMasterDTO();
            pmsdes.UpdatedBy.UserMasterId = _sessionSvc.GetUserSession().UserMasterId;
            if (_userSvc.DeletePMSDesignationMap(pmsdes).IsSuccess)
            {
                return Json(new { status = true, message = "Deleted successfully." }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = false, message = "Delete failed." }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult DeleteUserEntitlement(UserEntitlementDTO uTrans)
        {
            if (_userSvc.DeleteUserEntitlement(uTrans).IsSuccess)
            {
                return Json(new { status = true, message = "Deleted successfully." }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = false, message = "Delete failed." }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetSubjectDDL()
        {
            return Json(_ddlRepo.Subject(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult DeleteFacultyCourseMap(FacultyCourseMapDTO facultyCourseMap)
        {
            if (_userSvc.DeleteFacultyCourseMap(facultyCourseMap).IsSuccess)
            {
                return Json(new { status = true, message = "Deleted successfully." }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = false, message = "Delete failed." }, JsonRequestBehavior.AllowGet);
        }
    }
}
