using OperationsManager.Controllers;
using OpMgr.Common.Contracts.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationsManager.Areas.PMS.Controllers
{
    public class PMSController : BaseController
    {
        private IUserSvc _userSvc;
        //private ILogSvc _logSvc;
        private IDropdownRepo _dropDownRepo;
        private Helpers.UIDropDownRepo _uiddlRepo;

        [HttpGet]
        public ActionResult GoalSheetForAll()
        {
            return View();
        }
    }
}