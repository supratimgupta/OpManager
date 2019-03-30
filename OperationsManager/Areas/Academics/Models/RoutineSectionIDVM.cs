using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationsManager.Areas.Academics.Models
{
    public class RoutineSectionIDVM : StandardSectionMapDTO
    {
       

        public List<RoutineSectionIDVM> RoutineList { get; set; }

        public SelectList StandardSectionList { get; set; }

        public string Location { get; set; }

        public SelectList LocationList { get; set; }

        public Boolean IsSearchSuccessful { get; set; }

        public string SuccessOrFailureMessage { get; set; }

        public string MsgColor { get; set; }      

        public string ErrorMessage { get; set; }

        public string DOBString { get; set; }
    }

    public class RoutineTable
    {
            public List<RoutineTable> RoutineTblList { get; set; }
            public int standardsectionid { get; set; }
            public int locationid {get; set;}
            public string DayOfWeek { get; set; }
            public string firstPeriodName { get; set; }
            public string firstPeriodFaculty { get; set; }
        public string firstPeriodReplacedFaculty { get; set; }
        public bool firstperiodpractical { get; set; }
            public bool firstperiodoff { get; set; }
            public int firstperiodmode { get; set; }
            public int firstperiodid { get; set; }
            public string firstperiodstarttime { get; set; }
            public string firstperiodendtime { get; set; }
            public string secondPeriodName { get; set; }
            public string secondperiodstarttime { get; set; }
            public string secondperiodendtime { get; set; }
            public string secondPeriodFaculty { get; set; }
        public string secondPeriodReplacedFaculty { get; set; }
        public bool secondperiodpractical { get; set; }
            public int secondperiodmode { get; set; }
            public bool secondperiodoff { get; set; }
            public int secondperiodid { get; set; }
            public string thirdPeriodName { get; set; }
            public string thirdPeriodFaculty { get; set; }
        public string thirdPeriodReplacedFaculty { get; set; }
        public bool thirdperiodpractical { get; set; }
            public bool thirdperiodoff { get; set; }
            public int thirdperiodmode { get; set; }
            public int thirdperiodid { get; set; }
            public string thirdperiodstarttime { get; set; }
            public string thirdperiodendtime { get; set; }
            public string fourthPeriodName { get; set; }
            public string fourthPeriodFaculty { get; set; }
        public string fourthPeriodReplacedFaculty { get; set; }
        public bool fourthperiodpractical { get; set; }
            public bool fourthperiodoff { get; set; }
            public string fourthperiodstarttime { get; set; }
            public string fourthperiodendtime { get; set; }
            public int fourthperiodmode { get; set; }
            public int fourthperiodid { get; set; }
            public string fifthPeriodName { get; set; }
            public string fifthPeriodFaculty { get; set; }
        public string fifthPeriodReplacedFaculty { get; set; }
        public bool fifthperiodpractical { get; set; }
            public bool fifthperiodoff { get; set; }
            public int fifthperiodmode { get; set; }
            public string fifthperiodstarttime { get; set; }
            public string fifthperiodendtime { get; set; }
            public int fifthperiodid { get; set; }
            public string sixthPeriodName { get; set; }
            public string sixthPeriodFaculty { get; set; }
        public string sixthPeriodReplacedFaculty { get; set; }
        public bool sixthperiodpractical { get; set; }
            public bool sixthperiodoff { get; set; }
            public int sixthperiodmode { get; set; }
            public string sixthperiodstarttime { get; set; }
            public string sixthperiodendtime { get; set; }
            public int sixthperiodid { get; set; }
            public string seventhPeriodName { get; set; }
            public string seventhPeriodFaculty { get; set; }
        public string seventhPeriodReplacedFaculty { get; set; }
        public bool seventhperiodpractical { get; set; }
            public int seventhperiodmode { get; set; }
            public int seventhperiodid { get; set; }
            public string seventhperiodstarttime { get; set; }
            public string seventhperiodendtime { get; set; }
            public bool seventhperiodoff { get; set; }
            public string eigthPeriodName { get; set; }
            public string eigthPeriodFaculty { get; set; }
        public string eighthPeriodReplacedFaculty { get; set; }
        public bool eigthperiodpractical { get; set; }
            public int eigthperiodmode { get; set; }
            public int eigthperiodid { get; set; }
            public bool eighthperiodoff { get; set; }
            public string eighthperiodstarttime { get; set; }
            public string eighthperiodendtime { get; set; }
        
    }

    public class AddEditRoutine
    {
        public string DayOfWeek { get; set; }
        public string  PeriodName { get; set; }
        public string  PeriodStartTime { get; set; }
        public string PeriodStartTimeMeridiem { get; set; }
        public string PeriodStartTimehour { get; set; }
        public string PeriodStartTimeminute { get; set; }
        public string  periodEndTime { get; set; }
        public string periodEndTimehour { get; set; }
        public string periodEndTimeminute { get; set; }
        public string periodEndTimeMeridiem { get; set; }
        public int  periodmode { get; set; }
        public int  periodid { get; set; }
        public int period { get; set; }
        public int subjectid { get; set; }
        public int locationid { get; set; }
        public int standardsectionid { get; set; }
        public int periodschedule { get; set; }
        public int employeeid { get; set; }
        public string employeename { get; set; }
        public bool offperiod { get; set; }
        public bool practical { get; set; }
    }

    public class subject
    {
        public int subjectid { get; set; }
        public string subjectname { get; set; }
    }
    public class Employee
    {
        public List<SelectListItem> employeeList { get; set; }
        public int employeeid { get; set; }
        public string employeename { get; set; }
        public string id { get; set; }
    }

    
}