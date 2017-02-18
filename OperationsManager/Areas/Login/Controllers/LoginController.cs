using OpMgr.Common.Contracts;
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

        public LoginController(IUserSvc userSvc, ILogSvc logger, ISessionSvc sessionSvc)
        {
            _userSvc = userSvc;
            _logger = logger;
            _sessionSvc = sessionSvc;


        }

        // GET: Login/Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserMasterDTO data)
        {
            StatusDTO<UserMasterDTO> status = _userSvc.Login(data);
            if(status.IsSuccess)
            {
                SessionDTO session = new SessionDTO();
                session.UserName = status.ReturnObj.UserName;
                session.ActionList = new List<ActionDTO>();
                _sessionSvc.SetUserSession(session);
                SessionDTO sessionRet = _sessionSvc.GetUserSession();
            }

            return View();
        }
    }
}