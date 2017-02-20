﻿using OpMgr.Common.Contracts;
using OpMgr.Common.Contracts.Modules;
using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationsManager.Areas.Login.Controllers
{
    public class LoginController : Controller
    {
        private IUserSvc _userSvc;

        private ILogSvc _logger;

        private ISessionSvc _sessionSvc;

        private IDropdownRepo _ddlRepo;

        private Helpers.UIDropDownRepo _uiddlRepo;

        
        public LoginController(IConfigSvc configSvc, IDropdownRepo ddlRepo)
        {
            _userSvc = new OpMgr.DataAccess.Implementations.UserSvc();
            _ddlRepo = new OpMgr.DataAccess.Implementations.DropdownRepo(new OpMgr.Configurations.Implementations.ConfigSvc());
            _uiddlRepo = new Helpers.UIDropDownRepo(_ddlRepo);
            //_logger = logger;
            // _sessionSvc = sessionSvc;
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
            StatusDTO<UserMasterDTO> status = _userSvc.Login(data);
            if (status.IsSuccess)
            {
                SessionDTO session = new SessionDTO();
                session.UserName = status.ReturnObj.UserName;
                session.ActionList = new List<ActionDTO>();
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
            //Models.UserViewModel uvModel = new Models.UserViewModel();
            //uvModel.LocationList = _uiddlRepo.getLocationDropDown();
            return View();
        }
    }
}