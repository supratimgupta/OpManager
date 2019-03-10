using MySql.Data.MySqlClient;
using OpMgr.Common.Contracts;
using OpMgr.Common.Contracts.Modules;
using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.IO;
using OpMgr.DataAccess.Implementations;

namespace OpMgr.DataAccess.Implementations
{
   public class Routinesvc : IRoutinesvc
    {
        private IDropdownRepo _dropDwnRepo;
        private ISessionSvc _sessionSvc;
         private IUserTransactionSvc _userTrans;

        private IConfigSvc _configSvc;

        public Routinesvc(IDropdownRepo dropDwnRepo, IUserTransactionSvc userTrans, IConfigSvc configSvc, ISessionSvc sessionSvc)
        {

            _dropDwnRepo = dropDwnRepo;
            
            //_logSvc = logSvc;
            _userTrans = userTrans;
            _configSvc = configSvc;
            _sessionSvc = sessionSvc;

            
        }
       
        public List<RoutineTable> generateTable(int id,int location)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                string selectClause = null;
                string whereClause = null;
                DataSet rtntblset = null;
                List<RoutineTable> listrtntbl= new List<RoutineTable>();
                RoutineTable rtntbl = new RoutineTable();
                try
                {
                    dbSvc.OpenConnection();//openning the connection

                    MySqlCommand command = new MySqlCommand();// creating my sql command for queries

                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    selectClause = "select * from classroutine inner join routineperiod on classroutine.RoutinePeriodId=routineperiod.RoutinePeriodId left join (select routine_id,replaced_employee_id from replacementteacher where isActive = 1 and Date(replacement_date) >= curdate()  order by created_date desc) rt on ClassRoutineId = rt.routine_id where routineperiod.Active=1";
                    whereClause = " AND StandardSectionId=" + id + " AND LocationId=" + location;

                    command.CommandText = selectClause + whereClause;

                    MySqlDataAdapter da = new MySqlDataAdapter(command);
                    rtntblset = new DataSet();
                    da.Fill(rtntblset);
                    var table = rtntblset.Tables[0].AsEnumerable();
                    var rows = table.Where(row => row.Field<string>("DayOfWeek") == "Monday");
                    DataTable tblFilteredMonday = rows.Any() ? rows.CopyToDataTable() : new DataTable();
                    if (tblFilteredMonday.Rows.Count > 0)
                    {
                        for (int i = 0; i < tblFilteredMonday.Rows.Count; i++)
                        {
                            rtntbl.DayOfWeek = "Monday";
                            string subject = string.Empty;
                            if (tblFilteredMonday.Rows[i]["PeriodName"].ToString() != null)
                            {
                                if (tblFilteredMonday.Rows[i]["periodschedule"].ToString() == "1")
                                {
                                    rtntbl.firstPeriodName = tblFilteredMonday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.firstperiodid = Convert.ToInt32(tblFilteredMonday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.firstperiodoff = Convert.ToInt32(tblFilteredMonday.Rows[i]["IsOffPeriod"].ToString())!=0;
                                    rtntbl.firstperiodpractical = Convert.ToInt32(tblFilteredMonday.Rows[i]["IsPractical"].ToString())!=0;
                                    rtntbl.firstPeriodFaculty = getteacher(tblFilteredMonday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.firstperiodstarttime = Convert.ToString(tblFilteredMonday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.firstperiodendtime= Convert.ToString(tblFilteredMonday.Rows[i]["PeriodEndTime"]);
                                    rtntbl.firstperiodmode = 1;
                                    rtntbl.firstPeriodReplacedFaculty = getteacher(tblFilteredMonday.Rows[i]["replaced_employee_id"].ToString());
                                }
                                if (tblFilteredMonday.Rows[i]["periodschedule"].ToString() == "2")
                                {
                                    rtntbl.secondPeriodName = tblFilteredMonday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.secondperiodid = Convert.ToInt32(tblFilteredMonday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.secondperiodoff = Convert.ToInt32(tblFilteredMonday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.secondperiodpractical = Convert.ToInt32(tblFilteredMonday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.secondPeriodFaculty = getteacher(tblFilteredMonday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.secondperiodmode = 1;
                                    rtntbl.secondperiodstarttime = Convert.ToString(tblFilteredMonday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.secondperiodendtime = Convert.ToString(tblFilteredMonday.Rows[i]["PeriodEndTime"]);
                                    rtntbl.secondPeriodReplacedFaculty = getteacher(tblFilteredMonday.Rows[i]["replaced_employee_id"].ToString());
                                }
                                if (tblFilteredMonday.Rows[i]["periodschedule"].ToString() == "3")
                                {
                                    rtntbl.thirdPeriodName = tblFilteredMonday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.thirdperiodid = Convert.ToInt32(tblFilteredMonday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.thirdperiodoff = Convert.ToInt32(tblFilteredMonday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.thirdperiodpractical = Convert.ToInt32(tblFilteredMonday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.thirdPeriodFaculty = getteacher(tblFilteredMonday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.thirdperiodmode = 1;
                                    rtntbl.thirdperiodstarttime = Convert.ToString(tblFilteredMonday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.thirdperiodendtime = Convert.ToString(tblFilteredMonday.Rows[i]["PeriodEndTime"]);
                                    rtntbl.thirdPeriodReplacedFaculty = getteacher(tblFilteredMonday.Rows[i]["replaced_employee_id"].ToString());
                                }
                                if (tblFilteredMonday.Rows[i]["periodschedule"].ToString() == "4")
                                {
                                    rtntbl.fourthPeriodName = tblFilteredMonday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.fourthperiodid = Convert.ToInt32(tblFilteredMonday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.fourthperiodoff = Convert.ToInt32(tblFilteredMonday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.fourthperiodpractical = Convert.ToInt32(tblFilteredMonday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.fourthPeriodFaculty = getteacher(tblFilteredMonday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.fourthperiodmode = 1;
                                    rtntbl.fourthperiodstarttime = Convert.ToString(tblFilteredMonday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.fourthperiodendtime = Convert.ToString(tblFilteredMonday.Rows[i]["PeriodEndTime"]);
                                    rtntbl.fourthPeriodReplacedFaculty = getteacher(tblFilteredMonday.Rows[i]["replaced_employee_id"].ToString());
                                }
                                if (tblFilteredMonday.Rows[i]["periodschedule"].ToString() == "5")
                                {
                                    rtntbl.fifthPeriodName = tblFilteredMonday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.fifthperiodid = Convert.ToInt32(tblFilteredMonday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.fifthperiodoff = Convert.ToInt32(tblFilteredMonday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.fifthperiodpractical = Convert.ToInt32(tblFilteredMonday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.fifthPeriodFaculty = getteacher(tblFilteredMonday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.fifthperiodmode = 1;
                                    rtntbl.fifthperiodstarttime = Convert.ToString(tblFilteredMonday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.fifthperiodendtime = Convert.ToString(tblFilteredMonday.Rows[i]["PeriodEndTime"]);
                                    rtntbl.fifthPeriodReplacedFaculty = getteacher(tblFilteredMonday.Rows[i]["replaced_employee_id"].ToString());
                                }
                                if (tblFilteredMonday.Rows[i]["periodschedule"].ToString() == "6")
                                {
                                    rtntbl.sixthPeriodName = tblFilteredMonday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.sixthperiodid = Convert.ToInt32(tblFilteredMonday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.sixthperiodoff = Convert.ToInt32(tblFilteredMonday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.sixthperiodpractical = Convert.ToInt32(tblFilteredMonday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.sixthPeriodFaculty = getteacher(tblFilteredMonday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.sixthperiodmode = 1;
                                    rtntbl.sixthperiodstarttime = Convert.ToString(tblFilteredMonday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.sixthperiodendtime = Convert.ToString(tblFilteredMonday.Rows[i]["PeriodEndTime"]);
                                    rtntbl.sixthPeriodReplacedFaculty = getteacher(tblFilteredMonday.Rows[i]["replaced_employee_id"].ToString());
                                }
                                if (tblFilteredMonday.Rows[i]["periodschedule"].ToString() == "7")
                                {
                                    rtntbl.seventhPeriodName = tblFilteredMonday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.seventhperiodid = Convert.ToInt32(tblFilteredMonday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.seventhperiodoff = Convert.ToInt32(tblFilteredMonday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.seventhperiodpractical = Convert.ToInt32(tblFilteredMonday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.seventhPeriodFaculty = getteacher(tblFilteredMonday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.seventhperiodmode = 1;
                                    rtntbl.seventhperiodstarttime = Convert.ToString(tblFilteredMonday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.seventhperiodendtime = Convert.ToString(tblFilteredMonday.Rows[i]["PeriodEndTime"]);
                                    rtntbl.seventhPeriodReplacedFaculty = getteacher(tblFilteredMonday.Rows[i]["replaced_employee_id"].ToString());
                                }
                                if (tblFilteredMonday.Rows[i]["periodschedule"].ToString() == "8")
                                {
                                    rtntbl.eigthPeriodName = tblFilteredMonday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.eigthperiodid = Convert.ToInt32(tblFilteredMonday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.eighthperiodoff = Convert.ToInt32(tblFilteredMonday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.eigthperiodpractical = Convert.ToInt32(tblFilteredMonday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.eigthPeriodFaculty = getteacher(tblFilteredMonday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.eigthperiodmode = 1;
                                    rtntbl.eighthPeriodReplacedFaculty = getteacher(tblFilteredMonday.Rows[i]["replaced_employee_id"].ToString());
                                    rtntbl.eighthperiodstarttime = Convert.ToString(tblFilteredMonday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.eighthperiodendtime = Convert.ToString(tblFilteredMonday.Rows[i]["PeriodEndTime"]);
                                }

                            }



                        }
                        listrtntbl.Add(rtntbl);
                    }
                    else
                    {
                        rtntbl.DayOfWeek = "Monday";
                        rtntbl.firstperiodmode = 0;
                        rtntbl.secondperiodmode = 0;
                        rtntbl.thirdperiodmode = 0;
                        rtntbl.fourthperiodmode = 0;
                        rtntbl.fifthperiodmode = 0;
                        rtntbl.sixthperiodmode = 0;
                        rtntbl.seventhperiodmode = 0;
                        rtntbl.eigthperiodmode = 0;
                        listrtntbl.Add(rtntbl);

                    }

                    rows = table.Where(row => row.Field<string>("DayOfWeek") == "Tuesday");
                    DataTable tblFilteredTuesday = rows.Any() ? rows.CopyToDataTable() : new DataTable();
                     rtntbl = new RoutineTable();
                    if (tblFilteredTuesday.Rows.Count > 0)
                    {
                        for (int i = 0; i < tblFilteredTuesday.Rows.Count; i++)
                        {
                            rtntbl.DayOfWeek = "Tuesday";
                            string subject = string.Empty;
                            if (tblFilteredTuesday.Rows[i]["PeriodName"].ToString() != null)
                            {
                                if (tblFilteredTuesday.Rows[i]["periodschedule"].ToString() == "1")
                                {
                                    rtntbl.firstPeriodName = tblFilteredTuesday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.firstperiodid = Convert.ToInt32(tblFilteredTuesday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.firstperiodoff = Convert.ToInt32(tblFilteredTuesday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.firstperiodpractical = Convert.ToInt32(tblFilteredTuesday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.firstPeriodFaculty = getteacher(tblFilteredTuesday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.firstperiodmode = 1;
                                    rtntbl.firstperiodstarttime = Convert.ToString(tblFilteredTuesday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.firstperiodendtime = Convert.ToString(tblFilteredTuesday.Rows[i]["PeriodEndTime"]);
                                    rtntbl.firstPeriodReplacedFaculty = getteacher(tblFilteredTuesday.Rows[i]["replaced_employee_id"].ToString());
                                }
                                if (tblFilteredTuesday.Rows[i]["periodschedule"].ToString() == "2")
                                {
                                    rtntbl.secondPeriodName = tblFilteredTuesday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.secondperiodid = Convert.ToInt32(tblFilteredTuesday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.secondperiodoff = Convert.ToInt32(tblFilteredTuesday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.secondperiodpractical = Convert.ToInt32(tblFilteredTuesday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.secondPeriodFaculty = getteacher(tblFilteredTuesday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.secondperiodmode = 1;
                                    rtntbl.secondPeriodReplacedFaculty = getteacher(tblFilteredTuesday.Rows[i]["replaced_employee_id"].ToString());
                                    rtntbl.secondperiodstarttime = Convert.ToString(tblFilteredTuesday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.secondperiodendtime = Convert.ToString(tblFilteredTuesday.Rows[i]["PeriodEndTime"]);
                                }
                                if (tblFilteredTuesday.Rows[i]["periodschedule"].ToString() == "3")
                                {
                                    rtntbl.thirdPeriodName = tblFilteredTuesday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.thirdperiodid = Convert.ToInt32(tblFilteredTuesday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.thirdperiodoff = Convert.ToInt32(tblFilteredTuesday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.thirdperiodpractical = Convert.ToInt32(tblFilteredTuesday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.thirdPeriodFaculty = getteacher(tblFilteredTuesday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.thirdperiodmode = 1;
                                    rtntbl.thirdPeriodReplacedFaculty = getteacher(tblFilteredTuesday.Rows[i]["replaced_employee_id"].ToString());
                                    rtntbl.thirdperiodstarttime = Convert.ToString(tblFilteredTuesday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.thirdperiodendtime = Convert.ToString(tblFilteredTuesday.Rows[i]["PeriodEndTime"]);
                                }
                                if (tblFilteredTuesday.Rows[i]["periodschedule"].ToString() == "4")
                                {
                                    rtntbl.fourthPeriodName = tblFilteredTuesday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.fourthperiodid = Convert.ToInt32(tblFilteredTuesday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.fourthperiodoff = Convert.ToInt32(tblFilteredTuesday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.fourthperiodpractical = Convert.ToInt32(tblFilteredTuesday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.fourthPeriodFaculty = getteacher(tblFilteredTuesday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.fourthperiodmode = 1;
                                    rtntbl.fourthPeriodReplacedFaculty = getteacher(tblFilteredTuesday.Rows[i]["replaced_employee_id"].ToString());
                                    rtntbl.fourthperiodstarttime = Convert.ToString(tblFilteredTuesday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.fourthperiodendtime = Convert.ToString(tblFilteredTuesday.Rows[i]["PeriodEndTime"]);
                                }
                                if (tblFilteredTuesday.Rows[i]["periodschedule"].ToString() == "5")
                                {
                                    rtntbl.fifthPeriodName = tblFilteredTuesday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.fifthperiodid = Convert.ToInt32(tblFilteredTuesday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.fifthperiodoff = Convert.ToInt32(tblFilteredTuesday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.fifthperiodpractical = Convert.ToInt32(tblFilteredTuesday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.fifthPeriodFaculty = getteacher(tblFilteredTuesday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.fifthperiodmode = 1;
                                    rtntbl.fifthPeriodReplacedFaculty = getteacher(tblFilteredTuesday.Rows[i]["replaced_employee_id"].ToString());
                                    rtntbl.fifthperiodstarttime = Convert.ToString(tblFilteredTuesday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.fifthperiodendtime = Convert.ToString(tblFilteredTuesday.Rows[i]["PeriodEndTime"]);
                                }
                                if (tblFilteredTuesday.Rows[i]["periodschedule"].ToString() == "6")
                                {
                                    rtntbl.sixthPeriodName = tblFilteredTuesday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.sixthperiodid = Convert.ToInt32(tblFilteredTuesday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.sixthperiodoff = Convert.ToInt32(tblFilteredTuesday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.sixthperiodpractical = Convert.ToInt32(tblFilteredTuesday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.sixthPeriodFaculty = getteacher(tblFilteredTuesday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.sixthperiodmode = 1;
                                    rtntbl.sixthPeriodReplacedFaculty = getteacher(tblFilteredTuesday.Rows[i]["replaced_employee_id"].ToString());
                                    rtntbl.sixthperiodstarttime = Convert.ToString(tblFilteredTuesday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.sixthperiodendtime = Convert.ToString(tblFilteredTuesday.Rows[i]["PeriodEndTime"]);
                                }
                                if (tblFilteredTuesday.Rows[i]["periodschedule"].ToString() == "7")
                                {
                                    rtntbl.seventhPeriodName = tblFilteredTuesday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.seventhperiodid = Convert.ToInt32(tblFilteredTuesday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.seventhperiodoff = Convert.ToInt32(tblFilteredTuesday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.seventhperiodpractical = Convert.ToInt32(tblFilteredTuesday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.seventhPeriodFaculty = getteacher(tblFilteredTuesday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.seventhperiodmode = 1;
                                    rtntbl.seventhPeriodReplacedFaculty = getteacher(tblFilteredTuesday.Rows[i]["replaced_employee_id"].ToString());
                                    rtntbl.seventhperiodstarttime = Convert.ToString(tblFilteredTuesday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.seventhperiodendtime = Convert.ToString(tblFilteredTuesday.Rows[i]["PeriodEndTime"]);
                                }
                                if (tblFilteredTuesday.Rows[i]["periodschedule"].ToString() == "8")
                                {
                                    rtntbl.eigthPeriodName = tblFilteredTuesday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.eigthperiodid = Convert.ToInt32(tblFilteredTuesday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.eighthperiodoff = Convert.ToInt32(tblFilteredTuesday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.eigthperiodpractical = Convert.ToInt32(tblFilteredTuesday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.eigthPeriodFaculty = getteacher(tblFilteredTuesday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.eigthperiodmode = 1;
                                    rtntbl.eighthPeriodReplacedFaculty = getteacher(tblFilteredTuesday.Rows[i]["replaced_employee_id"].ToString());
                                    rtntbl.eighthperiodstarttime = Convert.ToString(tblFilteredTuesday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.eighthperiodendtime = Convert.ToString(tblFilteredTuesday.Rows[i]["PeriodEndTime"]);
                                }

                            }


                        }
                        listrtntbl.Add(rtntbl);
                    }

                    else
                    {
                        rtntbl.DayOfWeek = "Tuesday";
                        rtntbl.firstperiodmode = 0;
                        rtntbl.secondperiodmode = 0;
                        rtntbl.thirdperiodmode = 0;
                        rtntbl.fourthperiodmode = 0;
                        rtntbl.fifthperiodmode = 0;
                        rtntbl.sixthperiodmode = 0;
                        rtntbl.seventhperiodmode = 0;
                        rtntbl.eigthperiodmode = 0;
                        listrtntbl.Add(rtntbl);
                    }

                    rows = table.Where(row => row.Field<string>("DayOfWeek") == "Wednesday");
                    DataTable tblFilteredWednesday = rows.Any() ? rows.CopyToDataTable() : new DataTable();
                    rtntbl = new RoutineTable();
                    if (tblFilteredWednesday.Rows.Count > 0)
                    {
                        for (int i = 0; i < tblFilteredWednesday.Rows.Count; i++)
                        {
                            rtntbl.DayOfWeek = "Wednesday";
                            string subject = string.Empty;
                            if (tblFilteredWednesday.Rows[i]["PeriodName"].ToString() != null)
                            {
                                if (tblFilteredWednesday.Rows[i]["periodschedule"].ToString() == "1")
                                {
                                    rtntbl.firstPeriodName = tblFilteredWednesday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.firstperiodid = Convert.ToInt32(tblFilteredWednesday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.firstperiodoff = Convert.ToInt32(tblFilteredWednesday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.firstperiodpractical = Convert.ToInt32(tblFilteredWednesday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.firstPeriodFaculty = getteacher(tblFilteredWednesday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.firstperiodmode = 1;
                                    rtntbl.firstPeriodReplacedFaculty = getteacher(tblFilteredWednesday.Rows[i]["replaced_employee_id"].ToString());
                                    rtntbl.firstperiodstarttime = Convert.ToString(tblFilteredWednesday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.firstperiodendtime = Convert.ToString(tblFilteredWednesday.Rows[i]["PeriodEndTime"]);
                                }
                                if (tblFilteredWednesday.Rows[i]["periodschedule"].ToString() == "2")
                                {
                                    rtntbl.secondPeriodName = tblFilteredWednesday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.secondperiodid = Convert.ToInt32(tblFilteredWednesday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.secondperiodoff = Convert.ToInt32(tblFilteredWednesday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.secondperiodpractical = Convert.ToInt32(tblFilteredWednesday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.secondPeriodFaculty = getteacher(tblFilteredWednesday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.secondperiodmode = 1;
                                    rtntbl.secondPeriodReplacedFaculty = getteacher(tblFilteredWednesday.Rows[i]["replaced_employee_id"].ToString());
                                    rtntbl.secondperiodstarttime = Convert.ToString(tblFilteredWednesday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.secondperiodendtime = Convert.ToString(tblFilteredWednesday.Rows[i]["PeriodEndTime"]);
                                }
                                if (tblFilteredWednesday.Rows[i]["periodschedule"].ToString() == "3")
                                {
                                    rtntbl.thirdPeriodName = tblFilteredWednesday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.thirdperiodid = Convert.ToInt32(tblFilteredWednesday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.thirdperiodoff = Convert.ToInt32(tblFilteredWednesday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.thirdperiodpractical = Convert.ToInt32(tblFilteredWednesday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.thirdPeriodFaculty = getteacher(tblFilteredWednesday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.thirdperiodmode = 1;
                                    rtntbl.thirdPeriodReplacedFaculty = getteacher(tblFilteredWednesday.Rows[i]["replaced_employee_id"].ToString());
                                    rtntbl.thirdperiodstarttime = Convert.ToString(tblFilteredWednesday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.thirdperiodendtime = Convert.ToString(tblFilteredWednesday.Rows[i]["PeriodEndTime"]);
                                }
                                if (tblFilteredWednesday.Rows[i]["periodschedule"].ToString() == "4")
                                {
                                    rtntbl.fourthPeriodName = tblFilteredWednesday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.fourthperiodid = Convert.ToInt32(tblFilteredWednesday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.fourthperiodoff = Convert.ToInt32(tblFilteredWednesday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.fourthperiodpractical = Convert.ToInt32(tblFilteredWednesday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.fourthPeriodFaculty = getteacher(tblFilteredWednesday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.fourthperiodmode = 1;
                                    rtntbl.fourthPeriodReplacedFaculty = getteacher(tblFilteredWednesday.Rows[i]["replaced_employee_id"].ToString());
                                    rtntbl.fourthperiodstarttime = Convert.ToString(tblFilteredWednesday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.fourthperiodendtime = Convert.ToString(tblFilteredWednesday.Rows[i]["PeriodEndTime"]);
                                }
                                if (tblFilteredWednesday.Rows[i]["periodschedule"].ToString() == "5")
                                {
                                    rtntbl.fifthPeriodName = tblFilteredWednesday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.fifthperiodid = Convert.ToInt32(tblFilteredWednesday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.fifthperiodoff = Convert.ToInt32(tblFilteredWednesday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.fifthperiodpractical = Convert.ToInt32(tblFilteredWednesday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.fifthPeriodFaculty = getteacher(tblFilteredWednesday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.fifthperiodmode = 1;
                                    rtntbl.fifthPeriodReplacedFaculty = getteacher(tblFilteredWednesday.Rows[i]["replaced_employee_id"].ToString());
                                    rtntbl.fifthperiodstarttime = Convert.ToString(tblFilteredWednesday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.fifthperiodendtime = Convert.ToString(tblFilteredWednesday.Rows[i]["PeriodEndTime"]);
                                }
                                if (tblFilteredWednesday.Rows[i]["periodschedule"].ToString() == "6")
                                {
                                    rtntbl.sixthPeriodName = tblFilteredWednesday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.sixthperiodid = Convert.ToInt32(tblFilteredWednesday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.sixthperiodoff = Convert.ToInt32(tblFilteredWednesday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.sixthperiodpractical = Convert.ToInt32(tblFilteredWednesday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.sixthPeriodFaculty = getteacher(tblFilteredWednesday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.sixthperiodmode = 1;
                                    rtntbl.sixthPeriodReplacedFaculty = getteacher(tblFilteredWednesday.Rows[i]["replaced_employee_id"].ToString());
                                    rtntbl.sixthperiodstarttime = Convert.ToString(tblFilteredWednesday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.sixthperiodendtime = Convert.ToString(tblFilteredWednesday.Rows[i]["PeriodEndTime"]);
                                }
                                if (tblFilteredWednesday.Rows[i]["periodschedule"].ToString() == "7")
                                {
                                    rtntbl.seventhPeriodName = tblFilteredWednesday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.seventhperiodid = Convert.ToInt32(tblFilteredWednesday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.seventhperiodoff = Convert.ToInt32(tblFilteredWednesday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.seventhperiodpractical = Convert.ToInt32(tblFilteredWednesday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.seventhPeriodFaculty = getteacher(tblFilteredWednesday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.seventhperiodmode = 1;
                                    rtntbl.seventhPeriodReplacedFaculty = getteacher(tblFilteredWednesday.Rows[i]["replaced_employee_id"].ToString());
                                    rtntbl.seventhperiodstarttime = Convert.ToString(tblFilteredWednesday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.seventhperiodendtime = Convert.ToString(tblFilteredWednesday.Rows[i]["PeriodEndTime"]);
                                }
                                if (tblFilteredWednesday.Rows[i]["periodschedule"].ToString() == "8")
                                {
                                    rtntbl.eigthPeriodName = tblFilteredWednesday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.eigthperiodid = Convert.ToInt32(tblFilteredWednesday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.eighthperiodoff = Convert.ToInt32(tblFilteredWednesday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.eigthperiodpractical = Convert.ToInt32(tblFilteredWednesday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.eigthPeriodFaculty = getteacher(tblFilteredWednesday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.eigthperiodmode = 1;
                                    rtntbl.eighthPeriodReplacedFaculty = getteacher(tblFilteredWednesday.Rows[i]["replaced_employee_id"].ToString());
                                    rtntbl.eighthperiodstarttime = Convert.ToString(tblFilteredWednesday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.eighthperiodendtime = Convert.ToString(tblFilteredWednesday.Rows[i]["PeriodEndTime"]);
                                }

                            }


                        }
                        listrtntbl.Add(rtntbl);
                    }

                    else
                    {
                        rtntbl.DayOfWeek = "Wednesday";
                        rtntbl.firstperiodmode = 0;
                        rtntbl.secondperiodmode = 0;
                        rtntbl.thirdperiodmode = 0;
                        rtntbl.fourthperiodmode = 0;
                        rtntbl.fifthperiodmode = 0;
                        rtntbl.sixthperiodmode = 0;
                        rtntbl.seventhperiodmode = 0;
                        rtntbl.eigthperiodmode = 0;
                        listrtntbl.Add(rtntbl);
                    }

                    rows = table.Where(row => row.Field<string>("DayOfWeek") == "Thursday");
                    DataTable tblFilteredThursday = rows.Any() ? rows.CopyToDataTable() : new DataTable();
                    rtntbl = new RoutineTable();
                    if (tblFilteredThursday.Rows.Count > 0)
                    {
                        for (int i = 0; i < tblFilteredThursday.Rows.Count; i++)
                        {
                            rtntbl.DayOfWeek = "Thursday";
                            string subject = string.Empty;
                            if (tblFilteredThursday.Rows[i]["PeriodName"].ToString() != null)
                            {
                                if (tblFilteredThursday.Rows[i]["periodschedule"].ToString() == "1")
                                {
                                    rtntbl.firstPeriodName = tblFilteredThursday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.firstperiodid = Convert.ToInt32(tblFilteredThursday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.firstperiodoff = Convert.ToInt32(tblFilteredThursday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.firstperiodpractical = Convert.ToInt32(tblFilteredThursday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.firstPeriodFaculty = getteacher(tblFilteredThursday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.firstperiodmode = 1;
                                    rtntbl.firstPeriodReplacedFaculty = getteacher(tblFilteredThursday.Rows[i]["replaced_employee_id"].ToString());
                                    rtntbl.firstperiodstarttime = Convert.ToString(tblFilteredThursday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.firstperiodendtime = Convert.ToString(tblFilteredThursday.Rows[i]["PeriodEndTime"]);
                                }
                                if (tblFilteredThursday.Rows[i]["periodschedule"].ToString() == "2")
                                {
                                    rtntbl.secondPeriodName = tblFilteredThursday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.secondperiodid = Convert.ToInt32(tblFilteredThursday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.secondperiodoff = Convert.ToInt32(tblFilteredThursday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.secondperiodpractical = Convert.ToInt32(tblFilteredThursday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.secondPeriodFaculty = getteacher(tblFilteredThursday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.secondperiodmode = 1;
                                    rtntbl.secondPeriodReplacedFaculty = getteacher(tblFilteredThursday.Rows[i]["replaced_employee_id"].ToString());
                                    rtntbl.secondperiodstarttime = Convert.ToString(tblFilteredThursday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.secondperiodendtime = Convert.ToString(tblFilteredThursday.Rows[i]["PeriodEndTime"]);
                                }
                                if (tblFilteredThursday.Rows[i]["periodschedule"].ToString() == "3")
                                {
                                    rtntbl.thirdPeriodName = tblFilteredThursday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.thirdperiodid = Convert.ToInt32(tblFilteredThursday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.thirdperiodoff = Convert.ToInt32(tblFilteredThursday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.thirdperiodpractical = Convert.ToInt32(tblFilteredThursday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.thirdPeriodFaculty = getteacher(tblFilteredThursday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.thirdperiodmode = 1;
                                    rtntbl.thirdPeriodReplacedFaculty = getteacher(tblFilteredThursday.Rows[i]["replaced_employee_id"].ToString());
                                    rtntbl.thirdperiodstarttime = Convert.ToString(tblFilteredThursday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.thirdperiodendtime = Convert.ToString(tblFilteredThursday.Rows[i]["PeriodEndTime"]);
                                }
                                if (tblFilteredThursday.Rows[i]["periodschedule"].ToString() == "4")
                                {
                                    rtntbl.fourthPeriodName = tblFilteredThursday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.fourthperiodid = Convert.ToInt32(tblFilteredThursday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.fourthperiodoff = Convert.ToInt32(tblFilteredThursday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.fourthperiodpractical = Convert.ToInt32(tblFilteredThursday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.fourthPeriodFaculty = getteacher(tblFilteredThursday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.fourthperiodmode = 1;
                                    rtntbl.fourthPeriodReplacedFaculty = getteacher(tblFilteredThursday.Rows[i]["replaced_employee_id"].ToString());
                                    rtntbl.fourthperiodstarttime = Convert.ToString(tblFilteredThursday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.fourthperiodendtime = Convert.ToString(tblFilteredThursday.Rows[i]["PeriodEndTime"]);
                                }
                                if (tblFilteredThursday.Rows[i]["periodschedule"].ToString() == "5")
                                {
                                    rtntbl.fifthPeriodName = tblFilteredThursday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.fifthperiodid = Convert.ToInt32(tblFilteredThursday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.fifthperiodoff = Convert.ToInt32(tblFilteredThursday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.fifthperiodpractical = Convert.ToInt32(tblFilteredThursday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.fifthPeriodFaculty = getteacher(tblFilteredThursday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.fifthperiodmode = 1;
                                    rtntbl.fifthPeriodReplacedFaculty = getteacher(tblFilteredThursday.Rows[i]["replaced_employee_id"].ToString());
                                    rtntbl.fifthperiodstarttime = Convert.ToString(tblFilteredThursday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.fifthperiodendtime = Convert.ToString(tblFilteredThursday.Rows[i]["PeriodEndTime"]);
                                }
                                if (tblFilteredThursday.Rows[i]["periodschedule"].ToString() == "6")
                                {
                                    rtntbl.sixthPeriodName = tblFilteredThursday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.sixthperiodid = Convert.ToInt32(tblFilteredThursday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.sixthperiodoff = Convert.ToInt32(tblFilteredThursday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.sixthperiodpractical = Convert.ToInt32(tblFilteredThursday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.sixthPeriodFaculty = getteacher(tblFilteredThursday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.sixthperiodmode = 1;
                                    rtntbl.sixthPeriodReplacedFaculty = getteacher(tblFilteredThursday.Rows[i]["replaced_employee_id"].ToString());
                                    rtntbl.sixthperiodstarttime = Convert.ToString(tblFilteredThursday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.sixthperiodendtime = Convert.ToString(tblFilteredThursday.Rows[i]["PeriodEndTime"]);
                                }
                                if (tblFilteredThursday.Rows[i]["periodschedule"].ToString() == "7")
                                {
                                    rtntbl.seventhPeriodName = tblFilteredThursday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.seventhperiodid = Convert.ToInt32(tblFilteredThursday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.seventhperiodoff = Convert.ToInt32(tblFilteredThursday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.seventhperiodpractical = Convert.ToInt32(tblFilteredThursday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.seventhPeriodFaculty = getteacher(tblFilteredThursday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.seventhperiodmode = 1;
                                    rtntbl.seventhPeriodReplacedFaculty = getteacher(tblFilteredThursday.Rows[i]["replaced_employee_id"].ToString());
                                    rtntbl.seventhperiodstarttime = Convert.ToString(tblFilteredThursday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.seventhperiodendtime = Convert.ToString(tblFilteredThursday.Rows[i]["PeriodEndTime"]);
                                }
                                if (tblFilteredThursday.Rows[i]["periodschedule"].ToString() == "8")
                                {
                                    rtntbl.eigthPeriodName = tblFilteredThursday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.eigthperiodid = Convert.ToInt32(tblFilteredThursday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.eighthperiodoff = Convert.ToInt32(tblFilteredThursday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.eigthperiodpractical = Convert.ToInt32(tblFilteredThursday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.eigthPeriodFaculty = getteacher(tblFilteredThursday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.eigthperiodmode = 1;
                                    rtntbl.eighthPeriodReplacedFaculty = getteacher(tblFilteredThursday.Rows[i]["replaced_employee_id"].ToString());
                                    rtntbl.eighthperiodstarttime = Convert.ToString(tblFilteredThursday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.eighthperiodendtime = Convert.ToString(tblFilteredThursday.Rows[i]["PeriodEndTime"]);
                                }

                            }


                        }
                        listrtntbl.Add(rtntbl);
                    }

                    else
                    {
                        rtntbl.DayOfWeek = "Thursday";
                        rtntbl.firstperiodmode = 0;
                        rtntbl.secondperiodmode = 0;
                        rtntbl.thirdperiodmode = 0;
                        rtntbl.fourthperiodmode = 0;
                        rtntbl.fifthperiodmode = 0;
                        rtntbl.sixthperiodmode = 0;
                        rtntbl.seventhperiodmode = 0;
                        rtntbl.eigthperiodmode = 0;
                        listrtntbl.Add(rtntbl);
                    }

                    rows = table.Where(row => row.Field<string>("DayOfWeek") == "Friday");
                    DataTable tblFilteredFriday = rows.Any() ? rows.CopyToDataTable() : new DataTable();
                    rtntbl = new RoutineTable();
                    if (tblFilteredFriday.Rows.Count > 0)
                    {
                        for (int i = 0; i < tblFilteredFriday.Rows.Count; i++)
                        {
                            rtntbl.DayOfWeek = "Friday";
                            string subject = string.Empty;
                            if (tblFilteredFriday.Rows[i]["PeriodName"].ToString() != null)
                            {
                                if (tblFilteredFriday.Rows[i]["periodschedule"].ToString() == "1")
                                {
                                    rtntbl.firstPeriodName = tblFilteredFriday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.firstperiodid = Convert.ToInt32(tblFilteredFriday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.firstperiodoff = Convert.ToInt32(tblFilteredFriday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.firstperiodpractical = Convert.ToInt32(tblFilteredFriday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.firstPeriodFaculty = getteacher(tblFilteredFriday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.firstperiodmode = 1;
                                    rtntbl.firstPeriodReplacedFaculty = getteacher(tblFilteredFriday.Rows[i]["replaced_employee_id"].ToString());
                                    rtntbl.firstperiodstarttime = Convert.ToString(tblFilteredFriday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.firstperiodendtime = Convert.ToString(tblFilteredFriday.Rows[i]["PeriodEndTime"]);
                                }
                                if (tblFilteredFriday.Rows[i]["periodschedule"].ToString() == "2")
                                {
                                    rtntbl.secondPeriodName = tblFilteredFriday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.secondperiodid = Convert.ToInt32(tblFilteredFriday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.secondperiodoff = Convert.ToInt32(tblFilteredFriday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.secondperiodpractical = Convert.ToInt32(tblFilteredFriday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.secondPeriodFaculty = getteacher(tblFilteredFriday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.secondperiodmode = 1;
                                    rtntbl.secondPeriodReplacedFaculty = getteacher(tblFilteredFriday.Rows[i]["replaced_employee_id"].ToString());
                                    rtntbl.secondperiodstarttime = Convert.ToString(tblFilteredFriday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.secondperiodendtime = Convert.ToString(tblFilteredFriday.Rows[i]["PeriodEndTime"]);
                                }
                                if (tblFilteredFriday.Rows[i]["periodschedule"].ToString() == "3")
                                {
                                    rtntbl.thirdPeriodName = tblFilteredFriday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.thirdperiodid = Convert.ToInt32(tblFilteredFriday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.thirdperiodoff = Convert.ToInt32(tblFilteredFriday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.thirdperiodpractical = Convert.ToInt32(tblFilteredFriday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.thirdPeriodFaculty = getteacher(tblFilteredFriday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.thirdperiodmode = 1;
                                    rtntbl.thirdPeriodReplacedFaculty = getteacher(tblFilteredFriday.Rows[i]["replaced_employee_id"].ToString());
                                    rtntbl.thirdperiodstarttime = Convert.ToString(tblFilteredFriday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.thirdperiodendtime = Convert.ToString(tblFilteredFriday.Rows[i]["PeriodEndTime"]);
                                }
                                if (tblFilteredFriday.Rows[i]["periodschedule"].ToString() == "4")
                                {
                                    rtntbl.fourthPeriodName = tblFilteredFriday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.fourthperiodid = Convert.ToInt32(tblFilteredFriday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.fourthperiodoff = Convert.ToInt32(tblFilteredFriday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.fourthperiodpractical = Convert.ToInt32(tblFilteredFriday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.fourthPeriodFaculty = getteacher(tblFilteredFriday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.fourthPeriodReplacedFaculty = getteacher(tblFilteredFriday.Rows[i]["replaced_employee_id"].ToString());
                                    rtntbl.fourthperiodmode = 1;
                                    rtntbl.fourthperiodstarttime = Convert.ToString(tblFilteredFriday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.fourthperiodendtime = Convert.ToString(tblFilteredFriday.Rows[i]["PeriodEndTime"]);
                                }
                                if (tblFilteredFriday.Rows[i]["periodschedule"].ToString() == "5")
                                {
                                    rtntbl.fifthPeriodName = tblFilteredFriday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.fifthperiodid = Convert.ToInt32(tblFilteredFriday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.fifthperiodoff = Convert.ToInt32(tblFilteredFriday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.fifthperiodpractical = Convert.ToInt32(tblFilteredFriday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.fifthPeriodFaculty = getteacher(tblFilteredFriday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.fifthperiodmode = 1;
                                    rtntbl.fifthPeriodReplacedFaculty = getteacher(tblFilteredFriday.Rows[i]["replaced_employee_id"].ToString());
                                    rtntbl.fifthperiodstarttime = Convert.ToString(tblFilteredFriday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.fifthperiodendtime = Convert.ToString(tblFilteredFriday.Rows[i]["PeriodEndTime"]);
                                }
                                if (tblFilteredFriday.Rows[i]["periodschedule"].ToString() == "6")
                                {
                                    rtntbl.sixthPeriodName = tblFilteredFriday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.sixthperiodid = Convert.ToInt32(tblFilteredFriday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.sixthperiodoff = Convert.ToInt32(tblFilteredFriday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.sixthperiodpractical = Convert.ToInt32(tblFilteredFriday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.sixthPeriodFaculty = getteacher(tblFilteredFriday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.sixthperiodmode = 1;
                                    rtntbl.sixthPeriodReplacedFaculty = getteacher(tblFilteredFriday.Rows[i]["replaced_employee_id"].ToString());
                                    rtntbl.sixthperiodstarttime = Convert.ToString(tblFilteredFriday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.sixthperiodendtime = Convert.ToString(tblFilteredFriday.Rows[i]["PeriodEndTime"]);
                                }
                                if (tblFilteredFriday.Rows[i]["periodschedule"].ToString() == "7")
                                {
                                    rtntbl.seventhPeriodName = tblFilteredFriday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.seventhperiodid = Convert.ToInt32(tblFilteredFriday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.seventhperiodoff = Convert.ToInt32(tblFilteredFriday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.seventhperiodpractical = Convert.ToInt32(tblFilteredFriday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.seventhPeriodFaculty = getteacher(tblFilteredFriday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.seventhperiodmode = 1;
                                    rtntbl.seventhPeriodReplacedFaculty = getteacher(tblFilteredFriday.Rows[i]["replaced_employee_id"].ToString());
                                    rtntbl.seventhperiodstarttime = Convert.ToString(tblFilteredFriday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.seventhperiodendtime = Convert.ToString(tblFilteredFriday.Rows[i]["PeriodEndTime"]);
                                }
                                if (tblFilteredFriday.Rows[i]["periodschedule"].ToString() == "8")
                                {
                                    rtntbl.eigthPeriodName = tblFilteredFriday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.eigthperiodid = Convert.ToInt32(tblFilteredFriday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.eighthperiodoff = Convert.ToInt32(tblFilteredFriday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.eigthperiodpractical = Convert.ToInt32(tblFilteredFriday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.eigthPeriodFaculty = getteacher(tblFilteredFriday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.eigthperiodmode = 1;
                                    rtntbl.eighthPeriodReplacedFaculty = getteacher(tblFilteredFriday.Rows[i]["replaced_employee_id"].ToString());
                                    rtntbl.eighthperiodstarttime = Convert.ToString(tblFilteredFriday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.eighthperiodendtime = Convert.ToString(tblFilteredFriday.Rows[i]["PeriodEndTime"]);
                                }

                            }


                        }
                        listrtntbl.Add(rtntbl);
                    }

                    else
                    {
                        rtntbl.DayOfWeek = "Friday";
                        rtntbl.firstperiodmode = 0;
                        rtntbl.secondperiodmode = 0;
                        rtntbl.thirdperiodmode = 0;
                        rtntbl.fourthperiodmode = 0;
                        rtntbl.fifthperiodmode = 0;
                        rtntbl.sixthperiodmode = 0;
                        rtntbl.seventhperiodmode = 0;
                        rtntbl.eigthperiodmode = 0;
                        listrtntbl.Add(rtntbl);
                    }

                    rows = table.Where(row => row.Field<string>("DayOfWeek") == "Saturday");
                    DataTable tblFilteredSaturday = rows.Any() ? rows.CopyToDataTable() : new DataTable();
                    rtntbl = new RoutineTable();
                    if (tblFilteredSaturday.Rows.Count > 0)
                    {
                        for (int i = 0; i < tblFilteredSaturday.Rows.Count; i++)
                        {
                            rtntbl.DayOfWeek = "Saturday";
                            string subject = string.Empty;
                            if (tblFilteredSaturday.Rows[i]["PeriodName"].ToString() != null)
                            {
                                if (tblFilteredSaturday.Rows[i]["periodschedule"].ToString() == "1")
                                {
                                    rtntbl.firstPeriodName = tblFilteredSaturday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.firstperiodid = Convert.ToInt32(tblFilteredSaturday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.firstperiodoff = Convert.ToInt32(tblFilteredSaturday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.firstperiodpractical = Convert.ToInt32(tblFilteredSaturday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.firstPeriodFaculty = getteacher(tblFilteredSaturday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.firstperiodmode = 1;
                                    rtntbl.firstPeriodReplacedFaculty = getteacher(tblFilteredSaturday.Rows[i]["replaced_employee_id"].ToString());
                                    rtntbl.firstperiodstarttime = Convert.ToString(tblFilteredSaturday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.firstperiodendtime = Convert.ToString(tblFilteredSaturday.Rows[i]["PeriodEndTime"]);
                                }
                                if (tblFilteredSaturday.Rows[i]["periodschedule"].ToString() == "2")
                                {
                                    rtntbl.secondPeriodName = tblFilteredSaturday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.secondperiodid = Convert.ToInt32(tblFilteredSaturday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.secondperiodoff = Convert.ToInt32(tblFilteredSaturday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.secondperiodpractical = Convert.ToInt32(tblFilteredSaturday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.secondPeriodFaculty = getteacher(tblFilteredSaturday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.secondperiodmode = 1;
                                    rtntbl.secondPeriodReplacedFaculty = getteacher(tblFilteredSaturday.Rows[i]["replaced_employee_id"].ToString());
                                    rtntbl.secondperiodstarttime = Convert.ToString(tblFilteredSaturday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.secondperiodendtime = Convert.ToString(tblFilteredSaturday.Rows[i]["PeriodEndTime"]);
                                }
                                if (tblFilteredSaturday.Rows[i]["periodschedule"].ToString() == "3")
                                {
                                    rtntbl.thirdPeriodName = tblFilteredSaturday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.thirdperiodid = Convert.ToInt32(tblFilteredSaturday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.thirdperiodoff = Convert.ToInt32(tblFilteredSaturday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.thirdperiodpractical = Convert.ToInt32(tblFilteredSaturday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.thirdPeriodFaculty = getteacher(tblFilteredSaturday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.thirdperiodmode = 1;
                                    rtntbl.thirdPeriodReplacedFaculty = getteacher(tblFilteredSaturday.Rows[i]["replaced_employee_id"].ToString());
                                    rtntbl.thirdperiodstarttime = Convert.ToString(tblFilteredSaturday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.thirdperiodendtime = Convert.ToString(tblFilteredSaturday.Rows[i]["PeriodEndTime"]);
                                }
                                if (tblFilteredSaturday.Rows[i]["periodschedule"].ToString() == "4")
                                {
                                    rtntbl.fourthPeriodName = tblFilteredSaturday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.fourthperiodid = Convert.ToInt32(tblFilteredSaturday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.fourthperiodoff = Convert.ToInt32(tblFilteredSaturday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.fourthperiodpractical = Convert.ToInt32(tblFilteredSaturday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.fourthPeriodFaculty = getteacher(tblFilteredSaturday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.fourthperiodmode = 1;
                                    rtntbl.fourthPeriodReplacedFaculty = getteacher(tblFilteredSaturday.Rows[i]["replaced_employee_id"].ToString());
                                    rtntbl.fourthperiodstarttime = Convert.ToString(tblFilteredSaturday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.fourthperiodendtime = Convert.ToString(tblFilteredSaturday.Rows[i]["PeriodEndTime"]);
                                }
                                if (tblFilteredSaturday.Rows[i]["periodschedule"].ToString() == "5")
                                {
                                    rtntbl.fifthPeriodName = tblFilteredSaturday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.fifthperiodid = Convert.ToInt32(tblFilteredSaturday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.fifthperiodoff = Convert.ToInt32(tblFilteredSaturday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.fifthperiodpractical = Convert.ToInt32(tblFilteredSaturday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.fifthPeriodFaculty = getteacher(tblFilteredSaturday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.fifthperiodmode = 1;
                                    rtntbl.fifthPeriodReplacedFaculty = getteacher(tblFilteredSaturday.Rows[i]["replaced_employee_id"].ToString());
                                    rtntbl.fifthperiodstarttime = Convert.ToString(tblFilteredSaturday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.fifthperiodendtime = Convert.ToString(tblFilteredSaturday.Rows[i]["PeriodEndTime"]);
                                }
                                if (tblFilteredSaturday.Rows[i]["periodschedule"].ToString() == "6")
                                {
                                    rtntbl.sixthPeriodName = tblFilteredSaturday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.sixthperiodid = Convert.ToInt32(tblFilteredSaturday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.sixthperiodoff = Convert.ToInt32(tblFilteredSaturday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.sixthperiodpractical = Convert.ToInt32(tblFilteredSaturday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.sixthPeriodFaculty = getteacher(tblFilteredSaturday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.sixthperiodmode = 1;
                                    rtntbl.sixthPeriodReplacedFaculty = getteacher(tblFilteredSaturday.Rows[i]["replaced_employee_id"].ToString());
                                    rtntbl.sixthperiodstarttime = Convert.ToString(tblFilteredSaturday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.sixthperiodendtime = Convert.ToString(tblFilteredSaturday.Rows[i]["PeriodEndTime"]);
                                }
                                if (tblFilteredSaturday.Rows[i]["periodschedule"].ToString() == "7")
                                {
                                    rtntbl.seventhPeriodName = tblFilteredSaturday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.seventhperiodid = Convert.ToInt32(tblFilteredSaturday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.seventhperiodoff = Convert.ToInt32(tblFilteredSaturday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.seventhperiodpractical = Convert.ToInt32(tblFilteredSaturday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.seventhPeriodFaculty = getteacher(tblFilteredSaturday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.seventhperiodmode = 1;
                                    rtntbl.seventhPeriodReplacedFaculty = getteacher(tblFilteredSaturday.Rows[i]["replaced_employee_id"].ToString());
                                    rtntbl.seventhperiodstarttime = Convert.ToString(tblFilteredSaturday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.seventhperiodendtime = Convert.ToString(tblFilteredSaturday.Rows[i]["PeriodEndTime"]);
                                }
                                if (tblFilteredSaturday.Rows[i]["periodschedule"].ToString() == "8")
                                {
                                    rtntbl.eigthPeriodName = tblFilteredSaturday.Rows[i]["PeriodName"].ToString();
                                    rtntbl.eigthperiodid = Convert.ToInt32(tblFilteredSaturday.Rows[i]["ClassRoutineId"].ToString());
                                    rtntbl.eighthperiodoff = Convert.ToInt32(tblFilteredSaturday.Rows[i]["IsOffPeriod"].ToString()) != 0;
                                    rtntbl.eigthperiodpractical = Convert.ToInt32(tblFilteredSaturday.Rows[i]["IsPractical"].ToString()) != 0;
                                    rtntbl.eigthPeriodFaculty = getteacher(tblFilteredSaturday.Rows[i]["EmployeeId"].ToString());
                                    rtntbl.eigthperiodmode = 1;
                                    rtntbl.eighthPeriodReplacedFaculty = getteacher(tblFilteredSaturday.Rows[i]["replaced_employee_id"].ToString());
                                    rtntbl.eighthperiodstarttime = Convert.ToString(tblFilteredSaturday.Rows[i]["PeriodStartTime"]);
                                    rtntbl.eighthperiodendtime = Convert.ToString(tblFilteredSaturday.Rows[i]["PeriodEndTime"]);
                                }

                            }


                        }
                        listrtntbl.Add(rtntbl);
                    }

                    else
                    {
                        rtntbl.DayOfWeek = "Saturday";
                        rtntbl.firstperiodmode = 0;
                        rtntbl.secondperiodmode = 0;
                        rtntbl.thirdperiodmode = 0;
                        rtntbl.fourthperiodmode = 0;
                        rtntbl.fifthperiodmode = 0;
                        rtntbl.sixthperiodmode = 0;
                        rtntbl.seventhperiodmode = 0;
                        rtntbl.eigthperiodmode = 0;
                        listrtntbl.Add(rtntbl);
                    }



                }
                catch (Exception ex)
                {

                }
                return listrtntbl;

            }
        }
        public List<Employee> GetReplacementTeacherList(int id)
        {
            List<Employee> emplist = new List<Employee>();
            DataSet _dsData = new DataSet();
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "teacher_replacement";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    command.Parameters.Add("@classroutineid", MySqlDbType.Int32).Value = id;
                    MySqlDataAdapter rdr = new MySqlDataAdapter(command);
                    _dsData = new DataSet();
                    rdr.Fill(_dsData);
                    for(int i=0;i<_dsData.Tables[1].Rows.Count;i++)
                    {
                        emplist.Add(
                            new Employee()
                            {
                                employeeid = Convert.ToInt32(_dsData.Tables[1].Rows[i]["EmployeeId"].ToString()),
                                employeename = _dsData.Tables[1].Rows[i]["fullname"].ToString()
                            }
                            );
                    }
                }
                catch (Exception ex)
                {

                }
            }
                return emplist;

        }

        public AddEditRoutine PostData(AddEditRoutine rtnedit)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
               
               
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "ins_RoutineDetails";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    command.Parameters.Add("@classroutine", MySqlDbType.Int32).Value = rtnedit.periodid;
                    command.Parameters.Add("@standardsectionid", MySqlDbType.Int32).Value = rtnedit.standardsectionid;
                    command.Parameters.Add("@locationid", MySqlDbType.Int32).Value = rtnedit.locationid;
                    command.Parameters.Add("@subjectid", MySqlDbType.Int32).Value = rtnedit.subjectid;
                    command.Parameters.Add("@EmployeeId", MySqlDbType.Int32).Value = rtnedit.employeeid;
                    command.Parameters.Add("@dayofweek", MySqlDbType.String).Value = rtnedit.DayOfWeek;
                    command.Parameters.Add("@ispractical", MySqlDbType.Bit).Value = rtnedit.practical;
                    command.Parameters.Add("@offperiod", MySqlDbType.Bit).Value = rtnedit.offperiod;
                    command.Parameters.Add("@periodschedule", MySqlDbType.Int32).Value = rtnedit.periodschedule;
                    command.Parameters.Add("@periodstarttime", MySqlDbType.String).Value = rtnedit.PeriodStartTime;
                    command.Parameters.Add("@periodendtime", MySqlDbType.String).Value = rtnedit.periodEndTime;
                    command.Parameters.Add("@periodname", MySqlDbType.String).Value = rtnedit.PeriodName;
                    //command.Parameters.Add("@classroutineid", MySqlDbType.Int32).Value = rtnedit.



                    /*standardsectionid int,
                    Locationid int,
                    employeeid int,
                    ispractical bit,
                    offperiod bit,
                    periodschedule int*/
                    MySqlDataReader rdr = command.ExecuteReader(CommandBehavior.CloseConnection);
                    //_dtData = new DataTable();
                    //_dtData.Load(rdr);
                }
                catch (Exception ex)
                {

                }
                return rtnedit;
            }
        }

        public bool ReplaceTeachers(int id, int replacement_id)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {


                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "ins_replacementdetails";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    command.Parameters.Add("@routineId", MySqlDbType.Int32).Value = id;
                    command.Parameters.Add("@replacement_teacher_id", MySqlDbType.Int32).Value = replacement_id;
                    MySqlDataReader rdr = command.ExecuteReader(CommandBehavior.CloseConnection);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;

                }

            }



                   
        }

        public List<subject> getSubject(string term)
        {
            List<subject> lstsbjct = new List<subject>();
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                string selectClause = null;
                string whereClause = null;
                DataSet rtntblset = null;
                List<RoutineTable> listrtntbl = new List<RoutineTable>();
                RoutineTable rtntbl = new RoutineTable();
                try
                {
                    dbSvc.OpenConnection();//openning the connection

                    MySqlCommand command = new MySqlCommand();// creating my sql command for queries

                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    selectClause = "select SubjectId,SubjectName from subject where Active = 1";
                    whereClause = " AND SubjectName Like '" + term + "%'";

                    command.CommandText = selectClause + whereClause;

                    MySqlDataAdapter da = new MySqlDataAdapter(command);
                    rtntblset = new DataSet();
                    da.Fill(rtntblset);
                    lstsbjct = (from rw in rtntblset.Tables[0].AsEnumerable()
                                         select new subject()
                                         {
                                             subjectid = Convert.ToInt32(rw["SubjectId"]),
                                             subjectname = Convert.ToString(rw["SubjectName"])
                                         }).ToList();
                }
                catch (Exception ex)
                {

                }
            }
                  return lstsbjct;
        }

        public List<Employee> getEmployee(string term, int location)
        {
            List<Employee> lstemp = new List<Employee>();
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                string selectClause = null;
                string whereClause = null;
                DataSet rtntblset = null;
                List<RoutineTable> listrtntbl = new List<RoutineTable>();
                RoutineTable rtntbl = new RoutineTable();
                try
                {
                    dbSvc.OpenConnection();//openning the connection

                    MySqlCommand command = new MySqlCommand();// creating my sql command for queries

                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    selectClause = "select concat(FName,' ',LName) As FullName,EmployeeId from employeedetails inner join usermaster on employeedetails.UserMasterId=usermaster.UserMasterId where LocationId="+ location + " AND employeedetails.Active=1";
                    whereClause = " AND (FName Like '" + term + "%' OR MName Like '" + term + "%' OR LName Like '" + term + "%')";

                    command.CommandText = selectClause + whereClause;

                    MySqlDataAdapter da = new MySqlDataAdapter(command);
                    rtntblset = new DataSet();
                    da.Fill(rtntblset);
                    lstemp = (from rw in rtntblset.Tables[0].AsEnumerable()
                                select new Employee()
                                {
                                    employeeid = Convert.ToInt32(rw["EmployeeId"]),
                                    employeename = Convert.ToString(rw["FullName"])
                                }).ToList();
                }
                catch (Exception ex)
                {

                }
            }
            return lstemp;
        }
        public AddEditRoutine GetData(int id)
        {
            AddEditRoutine addeditrtn = new AddEditRoutine();
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                string selectClause = null;
                string whereClause = null;
                DataSet rtntblset = null;
                try
                {
                    dbSvc.OpenConnection();//openning the connection

                    MySqlCommand command = new MySqlCommand();// creating my sql command for queries

                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    selectClause = "select * from classroutine inner join routineperiod on classroutine.RoutinePeriodId=routineperiod.RoutinePeriodId where routineperiod.Active=1";
                    whereClause = " AND ClassRoutineId=" + id;

                    command.CommandText = selectClause + whereClause;

                    MySqlDataAdapter da = new MySqlDataAdapter(command);
                    rtntblset = new DataSet();
                    da.Fill(rtntblset);
                    addeditrtn.DayOfWeek = rtntblset.Tables[0].Rows[0]["DayOfWeek"].ToString();
                    addeditrtn.PeriodName = rtntblset.Tables[0].Rows[0]["PeriodName"].ToString();
                    addeditrtn.PeriodStartTime= rtntblset.Tables[0].Rows[0]["PeriodStartTime"].ToString();
                    addeditrtn.periodEndTime = rtntblset.Tables[0].Rows[0]["PeriodEndTime"].ToString();
                    addeditrtn.locationid = Convert.ToInt32(rtntblset.Tables[0].Rows[0]["LocationId"].ToString());
                    addeditrtn.employeeid = Convert.ToInt32(rtntblset.Tables[0].Rows[0]["EmployeeId"].ToString());
                    addeditrtn.employeename = getteacher(rtntblset.Tables[0].Rows[0]["EmployeeId"].ToString());
                    addeditrtn.offperiod = Convert.ToInt32(rtntblset.Tables[0].Rows[0]["IsOffPeriod"].ToString()) != 0;
                    addeditrtn.practical= Convert.ToInt32(rtntblset.Tables[0].Rows[0]["IsPractical"].ToString()) != 0;
                    addeditrtn.subjectid= Convert.ToInt32(rtntblset.Tables[0].Rows[0]["SubjectId"].ToString());
                    addeditrtn.periodid = id;
                    addeditrtn.standardsectionid= Convert.ToInt32(rtntblset.Tables[0].Rows[0]["StandardSectionId"].ToString());

                }
                catch (Exception ex)
                {

                }
                return addeditrtn;
            }
        }

        private string getteacher(string id)
        {
            string tchrname = string.Empty;
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                string selectClause = null;
                DataSet teachername = null;
                
                try
                {
                    dbSvc.OpenConnection();//openning the connection

                    MySqlCommand command = new MySqlCommand();// creating my sql command for queries

                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    selectClause = "select concat(FName, ' ', LName) as fullname from usermaster where usermasterid = (select usermasterid from employeedetails where employeeid =" + id + ")";
                    

                    command.CommandText = selectClause ;

                    MySqlDataAdapter da = new MySqlDataAdapter(command);
                    teachername = new DataSet();
                    da.Fill(teachername);
                    tchrname = teachername.Tables[0].Rows[0]["fullname"].ToString();
                    
                }
                catch (Exception ex)
                {

                }
            }
            return tchrname;


        }

       
    }
}
