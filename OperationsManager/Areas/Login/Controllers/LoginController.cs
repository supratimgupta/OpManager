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

namespace OperationsManager.Areas.Login.Controllers
{
    public class LoginController : Controller
    {
        private IUserSvc _userSvc;

        private ILogSvc _logger;

        private ISessionSvc _sessionSvc;

        private IDropdownRepo _ddlRepo;

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
        [HttpGet]
        public ActionResult Login()
        {
            UserMasterDTO userDto = null;

            if(Request.Cookies["userDetails"]!=null)
            {
                var userId = Request.Cookies["userDetails"]["uid"];
                var pwd = Request.Cookies["userDetails"]["pwd"];

                if (!string.IsNullOrEmpty(userId) && pwd != null && !string.IsNullOrEmpty(pwd))
                {
                    userDto = new UserMasterDTO();
                    userDto.RememberMe = true;
                    userDto.UserName = userId;
                    userDto.Password = pwd;
                    return View(userDto);
                }
            }
            return View();
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

            if(data.RememberMe)
            {
                HttpCookie cookie = new HttpCookie("userDetails");
                cookie["uid"] = data.UserName;
                cookie["pwd"] = unencryptedPass;
                cookie.Expires = DateTime.Now + new TimeSpan(1, 0, 0, 0);

                if(Request.Cookies["userDetails"] != null)
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

            StatusDTO<UserMasterDTO> status = _userSvc.Login(data, out lstEntitleMent,out lstAction);
            if (status.IsSuccess)
            {
                SessionDTO session = new SessionDTO();
                session.UserName = status.ReturnObj.UserName;
                session.ActionList = lstAction;
                session.EntitleMentList = lstEntitleMent;
                _sessionSvc.SetUserSession(session);
                SessionDTO sessionRet = _sessionSvc.GetUserSession();
            }


            return RedirectToAction("Register");
        }

        [HttpGet]
        public ActionResult Welcome()
        {
            return View();
        }

        [HttpGet]
        [OpMgrAuth]
        public ActionResult Register()
        {
            Models.UserViewModel uvModel = new Models.UserViewModel();
            uvModel.GenderList = _uiddlRepo.getGenderDropDown();
            uvModel.LocationList = _uiddlRepo.getLocationDropDown();
            uvModel.RoleList = _uiddlRepo.getRoleDropDown();
            uvModel.ClassTypeList = _uiddlRepo.getClassTypeDropDown();
            uvModel.SectionList = _uiddlRepo.getSectionDropDown();
            uvModel.HouseList = _uiddlRepo.getHouseDropDown();
            uvModel.BookCategoryList = _uiddlRepo.getBookCategoryDropDown();
            uvModel.DepartmentList = _uiddlRepo.getDepartmentDropDown();
            uvModel.DesignationList = _uiddlRepo.getDesignationDropDown();
            uvModel.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();

            

            return View(uvModel);
        }

        [HttpPost]
        [OpMgrAuth]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Models.UserViewModel uvModel)
        {
            //if(ModelState.IsValid)
            //{
                string pass = encrypt.encryption(uvModel.Password);
                uvModel.Password = pass;
                _userSvc.Insert(uvModel);
            //}

            uvModel.GenderList = _uiddlRepo.getGenderDropDown();
            uvModel.LocationList = _uiddlRepo.getLocationDropDown();
            uvModel.RoleList = _uiddlRepo.getRoleDropDown();
            uvModel.ClassTypeList = _uiddlRepo.getClassTypeDropDown();
            uvModel.SectionList = _uiddlRepo.getSectionDropDown();
            uvModel.HouseList = _uiddlRepo.getHouseDropDown();
            uvModel.BookCategoryList = _uiddlRepo.getBookCategoryDropDown();
            uvModel.DepartmentList = _uiddlRepo.getDepartmentDropDown();
            uvModel.DesignationList = _uiddlRepo.getDesignationDropDown();
            uvModel.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();

           

            return View(uvModel);
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
    }
}