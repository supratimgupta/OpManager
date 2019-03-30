
using OperationsManager.Areas.Academics.Models;
using OperationsManager.Attributes;
using OpMgr.Common.Contracts;
using OpMgr.Common.Contracts.Modules;
using System.Web.Http;
using OpMgr.Common.DTOs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using OperationsManager.Controllers;

namespace OperationsManager.Areas.Academics.Controllers
{
    public class RoutineController : Controller
    {
        private IRoutinesvc _rtnSvc;
        //private ILogSvc _logSvc;
        private IDropdownRepo _dropDwnRepo;
        private ISessionSvc _sessionSvc;
        private Helpers.UIDropDownRepo _uiddlRepo;
        Encryption encrypt = new Encryption();
        private IUserTransactionSvc _userTrans;

        private IConfigSvc _configSvc;

        public RoutineController(IRoutinesvc rtnSvc, IDropdownRepo dropDwnRepo, IUserTransactionSvc userTrans, IConfigSvc configSvc, ISessionSvc sessionSvc)
        {
            _rtnSvc = rtnSvc;
            _dropDwnRepo = dropDwnRepo;
            _uiddlRepo = new Helpers.UIDropDownRepo(_dropDwnRepo);
            //_logSvc = logSvc;
            _userTrans = userTrans;
            _configSvc = configSvc;
            _sessionSvc = sessionSvc;
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult search()
        {
            RoutineSectionIDVM rtnsction = null;
            rtnsction = new RoutineSectionIDVM();
            //Fetch the StandardSection List
            rtnsction.StandardSectionList = _uiddlRepo.getStandardSectionDropDown();
            rtnsction.LocationList = _uiddlRepo.getLocationDropDown();
            return View(rtnsction);
        }

        [HttpPost]
        public ActionResult search(RoutineSectionIDVM rtnsction)
        {
            return RedirectToAction("RoutineTable", new { location = rtnsction.Location, standardsection = rtnsction.StandardSectionId });
            
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult RoutineTable(int location,int standardsection)
        {
            Models.RoutineTable routinetbl = new Models.RoutineTable();
            List<Models.RoutineTable> listroutinetbl = new List<Models.RoutineTable>();
            List<OpMgr.Common.DTOs.RoutineTable> rtntbl = new List<OpMgr.Common.DTOs.RoutineTable>();
            rtntbl = _rtnSvc.generateTable(standardsection,location);

            for(int i=0;i<rtntbl.Count;i++)
            {
                routinetbl = new Models.RoutineTable();
                routinetbl.locationid = location;
                routinetbl.standardsectionid = standardsection;
                routinetbl.DayOfWeek = rtntbl[i].DayOfWeek;
                routinetbl.firstPeriodName = rtntbl[i].firstPeriodName;
                routinetbl.firstPeriodFaculty = rtntbl[i].firstPeriodFaculty;
                routinetbl.firstperiodoff = rtntbl[i].firstperiodoff;
                routinetbl.firstperiodpractical = rtntbl[i].firstperiodpractical;
                routinetbl.firstperiodmode = rtntbl[i].firstperiodmode;
                routinetbl.firstperiodid = rtntbl[i].firstperiodid;
                routinetbl.firstPeriodReplacedFaculty = rtntbl[i].firstPeriodReplacedFaculty;
                routinetbl.firstperiodstarttime = rtntbl[i].firstperiodstarttime;
                routinetbl.firstperiodendtime = rtntbl[i].firstperiodendtime;
                routinetbl.secondPeriodName = rtntbl[i].secondPeriodName;
                routinetbl.secondPeriodFaculty = rtntbl[i].secondPeriodFaculty;
                routinetbl.secondperiodoff = rtntbl[i].secondperiodoff;
                routinetbl.secondPeriodReplacedFaculty = rtntbl[i].secondPeriodReplacedFaculty;
                routinetbl.secondperiodpractical = rtntbl[i].secondperiodpractical;
                routinetbl.secondperiodmode = rtntbl[i].secondperiodmode;
                routinetbl.secondperiodid = rtntbl[i].secondperiodid;
                routinetbl.secondperiodstarttime = rtntbl[i].secondperiodstarttime;
                routinetbl.secondperiodendtime = rtntbl[i].secondperiodendtime;
                routinetbl.thirdPeriodName = rtntbl[i].thirdPeriodName;
                routinetbl.thirdPeriodFaculty = rtntbl[i].thirdPeriodFaculty;
                routinetbl.thirdPeriodReplacedFaculty = rtntbl[i].thirdPeriodReplacedFaculty;
                routinetbl.thirdperiodoff = rtntbl[i].thirdperiodoff;
                routinetbl.thirdperiodpractical = rtntbl[i].thirdperiodpractical;
                routinetbl.thirdperiodmode = rtntbl[i].thirdperiodmode;
                routinetbl.thirdperiodid = rtntbl[i].thirdperiodid;
                routinetbl.thirdperiodstarttime = rtntbl[i].thirdperiodstarttime;
                routinetbl.thirdperiodendtime = rtntbl[i].thirdperiodendtime;
                routinetbl.fourthPeriodName = rtntbl[i].fourthPeriodName;
                routinetbl.fourthPeriodFaculty = rtntbl[i].fourthPeriodFaculty;
                routinetbl.fourthperiodoff = rtntbl[i].fourthperiodoff;
                routinetbl.fourthperiodpractical = rtntbl[i].fourthperiodpractical;
                routinetbl.fourthperiodmode = rtntbl[i].fourthperiodmode;
                routinetbl.fourthperiodid = rtntbl[i].fourthperiodid;
                routinetbl.fourthPeriodReplacedFaculty = rtntbl[i].fourthPeriodReplacedFaculty;
                routinetbl.fourthperiodstarttime = rtntbl[i].fourthperiodstarttime;
                routinetbl.fourthperiodendtime = rtntbl[i].fourthperiodendtime;
                routinetbl.fifthPeriodName = rtntbl[i].fifthPeriodName;
                routinetbl.fifthPeriodFaculty = rtntbl[i].fifthPeriodFaculty;
                routinetbl.fifthPeriodReplacedFaculty = rtntbl[i].fifthPeriodReplacedFaculty;
                routinetbl.fifthperiodoff = rtntbl[i].fifthperiodoff;
                routinetbl.fifthperiodpractical = rtntbl[i].fifthperiodpractical;
                routinetbl.fifthperiodmode = rtntbl[i].fifthperiodmode;
                routinetbl.fifthperiodid = rtntbl[i].fifthperiodid;
                routinetbl.fifthperiodstarttime = rtntbl[i].fifthperiodstarttime;
                routinetbl.fifthperiodendtime = rtntbl[i].fifthperiodendtime;
                routinetbl.sixthPeriodName = rtntbl[i].sixthPeriodName;
                routinetbl.sixthPeriodFaculty = rtntbl[i].sixthPeriodFaculty;
                routinetbl.sixthperiodoff = rtntbl[i].sixthperiodoff;
                routinetbl.sixthperiodpractical = rtntbl[i].sixthperiodpractical;
                routinetbl.sixthperiodmode = rtntbl[i].sixthperiodmode;
                routinetbl.sixthPeriodReplacedFaculty = rtntbl[i].sixthPeriodReplacedFaculty;
                routinetbl.sixthperiodid = rtntbl[i].sixthperiodid;
                routinetbl.sixthperiodstarttime = rtntbl[i].sixthperiodstarttime;
                routinetbl.sixthperiodendtime = rtntbl[i].sixthperiodendtime;
                routinetbl.seventhPeriodName = rtntbl[i].seventhPeriodName;
                routinetbl.seventhPeriodFaculty = rtntbl[i].seventhPeriodFaculty;
                routinetbl.seventhPeriodReplacedFaculty = rtntbl[i].seventhPeriodReplacedFaculty;
                routinetbl.seventhperiodoff = rtntbl[i].seventhperiodoff;
                routinetbl.seventhperiodpractical = rtntbl[i].seventhperiodpractical;
                routinetbl.seventhperiodmode = rtntbl[i].seventhperiodmode;
                routinetbl.seventhperiodid = rtntbl[i].seventhperiodid;
                routinetbl.seventhperiodstarttime = rtntbl[i].seventhperiodstarttime;
                routinetbl.seventhperiodendtime = rtntbl[i].seventhperiodendtime;
                routinetbl.eigthPeriodName = rtntbl[i].eigthPeriodName;
                routinetbl.eigthPeriodFaculty = rtntbl[i].eigthPeriodFaculty;
                routinetbl.eighthperiodoff = rtntbl[i].eighthperiodoff;
                routinetbl.eigthperiodpractical = rtntbl[i].eigthperiodpractical;
                routinetbl.eigthperiodmode = rtntbl[i].eigthperiodmode;
                routinetbl.eighthPeriodReplacedFaculty = rtntbl[i].eighthPeriodReplacedFaculty;
                routinetbl.eigthperiodid = rtntbl[i].eigthperiodid;
                routinetbl.eighthperiodstarttime = rtntbl[i].eighthperiodstarttime;
                routinetbl.eighthperiodendtime = rtntbl[i].eighthperiodendtime;
                listroutinetbl.Add(routinetbl);

            }
            routinetbl.RoutineTblList = listroutinetbl;


            return View(routinetbl);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Replacement( string id, string replacement_id)
        {
            Models.Employee emp = new Models.Employee();
            List<Models.Employee> emplist= new List<Models.Employee>();
            List<OpMgr.Common.DTOs.Employee> listemp = new List<OpMgr.Common.DTOs.Employee>();

            listemp=_rtnSvc.GetReplacementTeacherList(Convert.ToInt32(id));
            for(int i=0;i< listemp.Count;i++)
            {
                emplist.Add(

                    new Models.Employee()
                    {
                        employeeid = listemp[i].employeeid,
                        employeename = listemp[i].employeename
                    }

                    );
            }
            // Create an empty list to hold result of the operation
            var selectList = new List<SelectListItem>();

            foreach (var element in emplist)
            {
                selectList.Add(new SelectListItem
                {
                    Value = Convert.ToString(element.employeeid),
                    Text = element.employeename
                });
            }
            emp.employeeList = selectList;
            emp.id = id;

            return View(emp);
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult AddEdit(string mode, string id, string dayofweek, string period, string location, string standardsectionid)
        {
            Models.AddEditRoutine Editrtn = new Models.AddEditRoutine();
            OpMgr.Common.DTOs.AddEditRoutine rtntbl = new OpMgr.Common.DTOs.AddEditRoutine();
            
            if (mode == "EDIT")
            {
                rtntbl = _rtnSvc.GetData(Convert.ToInt32(id));
                Editrtn.DayOfWeek = rtntbl.DayOfWeek;
                Editrtn.PeriodName = rtntbl.PeriodName;
                Editrtn.periodid = rtntbl.periodid;
                Editrtn.employeeid = rtntbl.employeeid;
                Editrtn.employeename = rtntbl.employeename;
                Editrtn.PeriodStartTimehour = rtntbl.PeriodStartTime.Substring(0, 2);
                Editrtn.PeriodStartTimeminute = rtntbl.PeriodStartTime.Substring(3, 2);
                Editrtn.PeriodStartTimeMeridiem = rtntbl.PeriodStartTime.Substring(6, 2);
                Editrtn.periodEndTimehour = rtntbl.periodEndTime.Substring(0, 2);
                Editrtn.periodEndTimeminute = rtntbl.periodEndTime.Substring(3, 2);
                Editrtn.periodEndTimeMeridiem = rtntbl.periodEndTime.Substring(6, 2);
                Editrtn.standardsectionid = Convert.ToInt32(standardsectionid);
                Editrtn.locationid = Convert.ToInt32(location);
                Editrtn.offperiod = rtntbl.offperiod;
                Editrtn.practical = rtntbl.practical;
                Editrtn.subjectid = rtntbl.subjectid;
                Editrtn.periodmode = 0;
            }
            else if (mode=="ADD")
            {
                Editrtn.DayOfWeek = dayofweek;
                Editrtn.period = Convert.ToInt32(period);
                Editrtn.standardsectionid = Convert.ToInt32(standardsectionid);
                Editrtn.locationid = Convert.ToInt32(location);
                Editrtn.PeriodStartTimehour = "12";
                Editrtn.PeriodStartTimeminute = "00";
                Editrtn.PeriodStartTimeMeridiem = "AM";
                Editrtn.periodEndTimehour = "12";
                Editrtn.periodEndTimeminute = "00";
                Editrtn.periodEndTimeMeridiem = "AM";
            }
            return View(Editrtn);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult pstedit(string data)
        {
            var Editrtn = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.AddEditRoutine>(data);
            OpMgr.Common.DTOs.AddEditRoutine rtntbl = new OpMgr.Common.DTOs.AddEditRoutine();
            rtntbl.subjectid = Editrtn.subjectid;
            rtntbl.standardsectionid = Editrtn.standardsectionid;
            rtntbl.periodid = Editrtn.periodid;
            rtntbl.DayOfWeek = Editrtn.DayOfWeek;
            rtntbl.periodschedule = Editrtn.periodschedule;
            rtntbl.locationid = Editrtn.locationid;
            rtntbl.PeriodName = Editrtn.PeriodName;
            rtntbl.PeriodStartTime = Editrtn.PeriodStartTime;
            rtntbl.periodEndTime = Editrtn.periodEndTime;
            rtntbl.employeeid = Editrtn.employeeid;
            rtntbl.offperiod = Editrtn.offperiod;
            rtntbl.practical = Editrtn.practical;
            rtntbl = _rtnSvc.PostData(rtntbl);
            return Json(rtntbl, JsonRequestBehavior.AllowGet);
        }
        //[AllowAnonymous]
        //[HttpPost]
        //public ActionResult AddEdit(Models.AddEditRoutine Editrtn)
        //{            
        //    OpMgr.Common.DTOs.AddEditRoutine rtntbl = new OpMgr.Common.DTOs.AddEditRoutine();
        //    rtntbl.subjectid = Editrtn.subjectid;
        //    rtntbl.standardsectionid = Editrtn.standardsectionid;
        //    rtntbl.periodid = Editrtn.periodid;
        //    rtntbl.DayOfWeek = Editrtn.DayOfWeek;
        //    rtntbl.periodschedule = Editrtn.period;
        //    rtntbl.locationid = Editrtn.locationid;
        //    rtntbl.PeriodName = Editrtn.PeriodName;
        //    rtntbl.employeeid = Editrtn.employeeid;
        //    rtntbl.offperiod = Editrtn.offperiod;
        //    rtntbl.practical = Editrtn.practical;
        //    rtntbl = _rtnSvc.PostData(rtntbl);
        //    Editrtn.periodmode = 1;
        //    Response.Write("<script language='javascript'> { self.close() }</script>");

        //    return View();
        //}
        
        [AllowAnonymous]
        
        public ActionResult pstreplacement(string data)
        {
            var Editrtn = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Employee>(data);
            bool result = false;
                      
            result = _rtnSvc.ReplaceTeachers(Convert.ToInt32(Editrtn.id),Editrtn.employeeid);
            return Json(Editrtn,JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        [HttpPost]
        public JsonResult Subjectsearch(string term)
        {
            List<OpMgr.Common.DTOs.subject> Sbjctlist = new List<OpMgr.Common.DTOs.subject>();
            List<Models.subject> listsubject = new List<Models.subject>();
            List<string> subject = new List<string>();
            //Models.subject sbjct = new Models.subject();
            Sbjctlist =_rtnSvc.getSubject(term);
            for (int i=0;i< Sbjctlist.Count;i++)
            {
                Models.subject sbjct = new Models.subject();
                sbjct.subjectid = Sbjctlist[i].subjectid;
                sbjct.subjectname = Sbjctlist[i].subjectname;
                listsubject.Add(sbjct);
                subject.Add(Sbjctlist[i].subjectname);
            }
             return Json(listsubject, JsonRequestBehavior.AllowGet);
           // return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public JsonResult Employeesearch(string term,string location)
        {
            List<OpMgr.Common.DTOs.Employee> Employeelist = new List<OpMgr.Common.DTOs.Employee>();
            List<Models.Employee> listemployee = new List<Models.Employee>();
            List<string> employee = new List<string>();
            //Models.subject sbjct = new Models.subject();
            Employeelist = _rtnSvc.getEmployee(term,Convert.ToInt32(location));
            for (int i = 0; i < Employeelist.Count; i++)
            {
                Models.Employee emp = new Models.Employee();
                emp.employeeid = Employeelist[i].employeeid;
                emp.employeename = Employeelist[i].employeename;
                listemployee.Add(emp);
                
            }
            return Json(listemployee, JsonRequestBehavior.AllowGet);
            // return View();
        }


    }
}