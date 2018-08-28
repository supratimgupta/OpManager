using OpMgr.Common.Contracts.Modules;
using System;
using System.Collections.Generic;
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

namespace OpMgr.DataAccess.Implementations
{
    public class AttendanceSvc : IAttendanceSvc
    {
        private IConfigSvc _configSvc;
        private DataTable _dtData;
        private DataSet _dsData;

        /// <summary>
        /// Needed for exception handling
        /// </summary>
        private ILogSvc _logger;

        public AttendanceSvc(IConfigSvc configSvc, ILogSvc logger)
        {
            _configSvc = configSvc;
            _logger = logger;
        }

        public StatusDTO<List<StudentDTO>> MarkAttendance(string AbsentStudentInfoIds, string Command, int StandardSectionId, int LocationId)
        {
            StatusDTO<List<StudentDTO>> studLst = new StatusDTO<List<StudentDTO>>();
            string whereClause = null;
            string selectClause = null;
            string updateClause = null;
            DataSet dsStudentLst = null;
            int noOfUpdateSuccessful = 0;

            if (!string.IsNullOrEmpty(Command))
            {
                using (IDbSvc dbSvc = new DbSvc(_configSvc))
                {
                    try
                    {
                        dbSvc.OpenConnection();//openning the connection
                        MySqlCommand command = new MySqlCommand();// creating my sql command for queries
                        command.Connection = dbSvc.GetConnection() as MySqlConnection;
                        if (string.Equals(Command, "Promotion Confirmed"))
                        {
                            updateClause = "UPDATE studentinfo SET StandardSectionId=NewStandardSectionId, NewStandardSectionId=NULL, Status=NULL WHERE Active=1 AND Status='Promotion Confirmed' AND NewStandardSectionId IS NOT NULL";
                            command = new MySqlCommand();
                            command.Connection = dbSvc.GetConnection() as MySqlConnection;
                            command.CommandText = updateClause;
                            if (command.ExecuteNonQuery() > 0)
                            {
                                noOfUpdateSuccessful++;
                            }
                        }
                        if (string.Equals(Command, "Apply Attendance"))
                        {
                            //foreach (StudentDTO stud in studentList)
                            //{
                            //    command = new MySqlCommand();
                            //    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                            //    updateClause = "UPDATE studentinfo SET Status=@Status,NewStandardSectionId=@NewStandardSectionId" +
                            //                                          " WHERE StudentInfoId=@StudentInfoId AND Active=1 ";
                            //    if (stud != null && stud.Status != null && stud.NewStandardSectionId != 0 && stud.StudentInfoId != 0)
                            //    {
                            //        command.Parameters.Add("@Status", MySqlDbType.String).Value = stud.Status;
                            //        command.Parameters.Add("@NewStandardSectionId", MySqlDbType.Int32).Value = stud.NewStandardSectionId;
                            //        command.Parameters.Add("@StudentInfoId", MySqlDbType.Int32).Value = stud.StudentInfoId;
                            //    }
                            //    command.CommandText = updateClause;
                            //    if (command.ExecuteNonQuery() > 0)
                            //    {
                            //        noOfUpdateSuccessful++;
                            //    }
                            //}
                        }
                        selectClause = "SELECT student.StudentInfoId,users.UserMasterId,users.FName, users.MName,users.LName,student.StandardSectionId,stdSecMap.StandardId,stdSecMap.Serial," +
                                   "student.RollNumber,student.NewStandardSectionId,users.LocationId,loc.LocationDescription,student.Status,stnd.StandardName,sec.SectionName,stnd1.StandardName AS NewStandardName,sec1.SectionName AS NewSectionName " +
                                   "FROM studentinfo student " +
                                   "LEFT JOIN UserMaster users ON student.UserMasterId = users.UserMasterId " +
                                   "LEFT JOIN Location loc ON loc.LocationId = users.LocationId " +
                                   "LEFT JOIN StandardSectionMap stdSecMap ON student.StandardSectionId = stdSecMap.StandardSectionId " +
                                   "LEFT JOIN Standard stnd ON stdSecMap.StandardId = stnd.StandardId " +
                                   "LEFT JOIN Section sec ON stdSecMap.SectionId = sec.SectionId " +
                                   "LEFT JOIN StandardSectionMap stdSecMap1 ON student.NewStandardSectionId=stdSecMap1.StandardSectionId " +
                                   "LEFT JOIN Standard stnd1 ON stdSecMap1.StandardId=stnd1.StandardId " +
                                   "LEFT JOIN Section sec1 ON stdSecMap1.SectionId=sec1.SectionId ";

                        //Select All students who are ACTIVE
                        whereClause = "WHERE student.Active=1 ";

                        //Select students of that particular class
                        if (StandardSectionId != -1)
                        {
                            whereClause = whereClause + " AND student.StandardSectionId=@StandardSectionId";
                            command.Parameters.Add("@StandardSectionId", MySqlDbType.Int32).Value = StandardSectionId;
                        }

                        //Select students of that particular location
                        if (LocationId != -1)
                        {
                            whereClause = whereClause + " AND users.LocationId=@LocationId";
                            command.Parameters.Add("@LocationId", MySqlDbType.Int32).Value = LocationId;
                        }

                        command.CommandText = selectClause + whereClause;

                        MySqlDataAdapter da = new MySqlDataAdapter(command);
                        dsStudentLst = new DataSet();
                        da.Fill(dsStudentLst);

                        if (dsStudentLst != null && dsStudentLst.Tables.Count > 0)
                        {
                            studLst.ReturnObj = new List<StudentDTO>();
                            for (int i = 0; i < dsStudentLst.Tables[0].Rows.Count; i++)
                            {
                                StudentDTO student = new StudentDTO();
                                student.Active = true;

                                student.StudentInfoId = (int)dsStudentLst.Tables[0].Rows[i]["StudentInfoId"];
                                student.Status = dsStudentLst.Tables[0].Rows[i]["Status"].ToString();
                                if (!string.IsNullOrEmpty(dsStudentLst.Tables[0].Rows[i]["NewStandardSectionId"].ToString()))
                                    student.NewStandardSectionId = (int)dsStudentLst.Tables[0].Rows[i]["NewStandardSectionId"];

                                student.RollNumber = dsStudentLst.Tables[0].Rows[i]["RollNumber"].ToString();

                                student.UserDetails = new UserMasterDTO();
                                student.UserDetails.FName = dsStudentLst.Tables[0].Rows[i]["FName"].ToString();
                                student.UserDetails.MName = dsStudentLst.Tables[0].Rows[i]["MName"].ToString();
                                student.UserDetails.LName = dsStudentLst.Tables[0].Rows[i]["LName"].ToString();
                                student.UserDetails.UserMasterId = Convert.ToInt32(dsStudentLst.Tables[0].Rows[i]["UserMasterId"]);

                                student.UserDetails.Location = new LocationDTO();
                                student.UserDetails.Location.LocationId = Convert.ToInt32(dsStudentLst.Tables[0].Rows[i]["LocationId"]);
                                student.UserDetails.Location.LocationDescription = dsStudentLst.Tables[0].Rows[i]["LocationDescription"].ToString();

                                student.StandardSectionMap = new StandardSectionMapDTO();
                                student.StandardSectionMap.Standard = new StandardDTO();
                                student.StandardSectionMap.Section = new SectionDTO();
                                student.StandardSectionMap.StandardSectionId = (int)dsStudentLst.Tables[0].Rows[i]["StandardSectionId"];
                                student.StandardSectionMap.Standard.StandardId = (int)dsStudentLst.Tables[0].Rows[i]["StandardId"];
                                student.StandardSectionMap.Standard.StandardName = dsStudentLst.Tables[0].Rows[i]["StandardName"].ToString();
                                student.StandardSectionMap.Section.SectionName = dsStudentLst.Tables[0].Rows[i]["SectionName"].ToString();
                                student.StandardSectionMap.Serial = (int)dsStudentLst.Tables[0].Rows[i]["Serial"];
                                student.NewStandardSectionMap = new StandardSectionMapDTO();
                                student.NewStandardSectionMap.StandardSectionDesc = dsStudentLst.Tables[0].Rows[i]["NewStandardName"].ToString() + " " + dsStudentLst.Tables[0].Rows[i]["NewSectionName"].ToString();

                                studLst.ReturnObj.Add(student);
                                studLst.IsSuccess = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            return studLst;
        }

        public StatusDTO<StudentDTO> Delete(StudentDTO data)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<StudentDTO> Insert(StudentDTO data)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<StudentDTO> Select(int rowId)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<List<StudentDTO>> Select(StudentDTO data)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<StudentDTO> Update(StudentDTO data)
        {
            throw new NotImplementedException();
        }

    }
}
