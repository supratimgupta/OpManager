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

namespace OperationsManager.Areas.Login.Controllers
{
    public class LoginController : Controller
    {
        private IUserSvc _userSvc;

        //private ILogSvc _logger;

        private ISessionSvc _sessionSvc;

        private IDropdownRepo _ddlRepo;
        private PasswordGenerator.PasswordGenerator _passGen;// taken for dependency but not used DOUBT!!!

        private Helpers.UIDropDownRepo _uiddlRepo;

        Encryption encrypt = new Encryption();

        public LoginController(IUserSvc userSvc, IDropdownRepo ddlRepo, ISessionSvc sessionSvc)
        {
            _userSvc = userSvc;
            _ddlRepo = ddlRepo;
            //new OpMgr.DataAccess.Implementations.DropdownRepo(new OpMgr.Configurations.Implementations.ConfigSvc());
            _uiddlRepo = new Helpers.UIDropDownRepo(_ddlRepo);
            //_logger = logger;

            _sessionSvc = sessionSvc;
        }

        // GET: Login/Login
        // GET: Login/Login
        [HttpGet]
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
        public ActionResult Login(UserMasterDTO data)
        {
            List<EntitlementDTO> lstEntitleMent = new List<EntitlementDTO>();
            List<ActionDTO> lstAction = new List<ActionDTO>();
            string unencryptedPass = data.Password;
            string pass = encrypt.encryption(data.Password);
            data.Password = pass;

            StatusDTO<UserMasterDTO> status = _userSvc.Login(data, out lstEntitleMent, out lstAction);
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
                    Response.Cookies.Clear();
                }

                SessionDTO session = new SessionDTO();
                session.UserName = status.ReturnObj.UserName;
                session.ActionList = lstAction;
                session.EntitleMentList = lstEntitleMent;
                _sessionSvc.SetUserSession(session);
                SessionDTO sessionRet = _sessionSvc.GetUserSession();
            }
            else
            {
                data.LoginFailedMsg = status.FailureReason;
                return View(data);
            }

            return RedirectToAction("Landing", "Login", new { area = "Login" }); ;
        }

        [HttpGet]
        public ActionResult Welcome()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Landing()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Register(string mode, string id)
        {
            Models.UserViewModel uvModel = new Models.UserViewModel();
            uvModel.MODE = mode;
            if (string.Equals(mode, "EDIT", StringComparison.OrdinalIgnoreCase))
            {
                uvModel.UserMasterId = int.Parse(id);
            }
            if (mode != null && string.Equals(mode, "EDIT", StringComparison.OrdinalIgnoreCase))
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
                uvModel.EmailId = dto.ReturnObj.EmailId;
                uvModel.ResidentialAddress = dto.ReturnObj.ResidentialAddress;
                uvModel.PermanentAddress = dto.ReturnObj.PermanentAddress;
                uvModel.ContactNo = dto.ReturnObj.ContactNo;
                uvModel.AltContactNo = dto.ReturnObj.AltContactNo;
                uvModel.BloodGroup = dto.ReturnObj.BloodGroup;
                uvModel.Location = dto.ReturnObj.Location;
                uvModel.Role = dto.ReturnObj.Role;
                //if (dto.ReturnObj.Role.RoleId == 1)
                //{
                //    uvModel.Student = new StudentDTO();
                //    uvModel.Student.RollNumber = dto.ReturnObj.Student.RollNumber;
                //    uvModel.Student.RegistrationNumber = dto.ReturnObj.Student.RegistrationNumber;
                //    uvModel.Student.GuardianContact = dto.ReturnObj.Student.GuardianContact;
                //    uvModel.Student.GuardianName = dto.ReturnObj.Student.GuardianName;
                //    uvModel.Student.GuardianEmailId = dto.ReturnObj.Student.GuardianEmailId;
                //    uvModel.Student.HouseType = dto.ReturnObj.Student.HouseType;
                //    uvModel.Student.StandardSectionMap = dto.ReturnObj.Student.StandardSectionMap;
                //}
                //else if (dto.ReturnObj.Role.RoleId > 1)
                //{
                uvModel.Employee = new EmployeeDetailsDTO();
                if (dto.ReturnObj.Employee != null)
                {
                    uvModel.Employee.EducationalQualification = dto.ReturnObj.Employee.EducationalQualification;
                    uvModel.Employee.DateOfJoining = dto.ReturnObj.Employee.DateOfJoining;
                    uvModel.Employee.StaffEmployeeId = dto.ReturnObj.Employee.StaffEmployeeId;
                    uvModel.Employee.Department = dto.ReturnObj.Employee.Department;
                    uvModel.Employee.Designation = dto.ReturnObj.Employee.Designation;
                }
                //}
            }

            uvModel.GenderList = _uiddlRepo.getGenderDropDown();
            uvModel.LocationList = _uiddlRepo.getLocationDropDown();
            uvModel.RoleList = _uiddlRepo.getRoleDropDown();
            // uvModel.ClassTypeList = _uiddlRepo.getClassTypeDropDown();
            //uvModel.SectionList = _uiddlRepo.getSectionDropDown();
            //uvModel.HouseList = _uiddlRepo.getHouseDropDown();
            //uvModel.BookCategoryList = _uiddlRepo.getBookCategoryDropDown();
            uvModel.DepartmentList = _uiddlRepo.getDepartmentDropDown();
            uvModel.DesignationList = _uiddlRepo.getDesignationDropDown();
            //uvModel.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();


            return View(uvModel);
        }

        [HttpPost]
        [OpMgrAuth]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Models.UserViewModel uvModel)
        {
            if (string.Equals(uvModel.MODE, "EDIT", StringComparison.OrdinalIgnoreCase))
            {
                //Call update
                //if (ModelState.IsValid)
                //{

                _userSvc.Update(uvModel);

                //}
            }
            else
            {
                //Call insert

                //if (ModelState.IsValid)
                //{
                string pass = encrypt.encryption(uvModel.Password);
                uvModel.Password = pass;
                _userSvc.Insert(uvModel);

                //}                
            }
            //if(ModelState.IsValid)
            //{
            //}

            uvModel.GenderList = _uiddlRepo.getGenderDropDown();
            uvModel.LocationList = _uiddlRepo.getLocationDropDown();
            uvModel.RoleList = _uiddlRepo.getRoleDropDown();
            //uvModel.ClassTypeList = _uiddlRepo.getClassTypeDropDown();
            //uvModel.SectionList = _uiddlRepo.getSectionDropDown();
            //uvModel.HouseList = _uiddlRepo.getHouseDropDown();
            //uvModel.BookCategoryList = _uiddlRepo.getBookCategoryDropDown();
            uvModel.DepartmentList = _uiddlRepo.getDepartmentDropDown();
            uvModel.DesignationList = _uiddlRepo.getDesignationDropDown();
            //uvModel.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();

            //return View(uvModel);
            return RedirectToAction("Search");
        }

        [HttpGet]
        public ActionResult Logout()
        {
            _sessionSvc.Logout();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            MailVM mailVM = new MailVM();
            return View(mailVM);
        }
        [HttpPost]
        public ActionResult ForgotPassword(MailVM mailView)
        {
            if (mailView != null)
            {
                PasswordGenerator.PasswordGenerator passGen = new PasswordGenerator.PasswordGenerator();
                passGen.InitializePasswordArrays();

                PasswordVM passVM = new PasswordVM();
                passVM.CapitalLettersLength = 1;
                passVM.DigitsLength = 1;
                passVM.SmallLettersLength = 3;
                passVM.SpecialCharactersLength = 1;
                passVM.PasswordLength = 6;

                mailView.Body = passGen.GeneratePassword(passVM);

                string from = "ruttu04@gmail.com";
                using (MailMessage mailMsg = new MailMessage(from, mailView.To))
                {

                    mailMsg.Subject = "Reset Password";
                    mailMsg.Body = mailView.Body;
                    mailMsg.IsBodyHtml = false;

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential network = new NetworkCredential(from, "allcreater04");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = network;
                    smtp.Port = 587;
                    smtp.Send(mailMsg);
                    mailView.SuccessOrFailureMessage = "Your Mail Has been Sent";
                    mailView.MessageColor = Color.Green;
                }
            }
            return View(mailView);
        }

        [HttpGet]
        public ActionResult ResetPassword()
        {
            SessionDTO sessionRet = _sessionSvc.GetUserSession();
            UserViewModel userView = new UserViewModel();
            //if(sessionRet!=null)
            //{
            //    userView.UserName = sessionRet.UserName;

            //}
            return View(userView);

        }
    }

}
