using OpMgr.Common.Contracts;
using OpMgr.Common.Contracts.Modules;
using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OperationsManager.Attributes;

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
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserMasterDTO data)
        {
            List<EntitlementDTO> lstEntitleMent = new List<EntitlementDTO>();
            List<ActionDTO> lstAction = new List<ActionDTO>();
            string pass = encrypt.encryption(data.Password);
            data.Password = pass;
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


            return View();
        }

        [HttpGet]
        public ActionResult Welcome()
        {
            return View();
        }

        [HttpGet]
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
    }
}