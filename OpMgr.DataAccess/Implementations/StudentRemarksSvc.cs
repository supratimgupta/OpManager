using MySql.Data.MySqlClient;
using OpMgr.Common.Contracts;
using OpMgr.Common.Contracts.Modules;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.DataAccess.Implementations
{
    public class StudentRemarksSvc : IStudentRemarksSvc
    {

        /// <summary>
        /// Required for getting connection related configs
        /// </summary>
        private IConfigSvc _configSvc;
        private DataTable _dtData;
        private DataSet _dsData;

        /// <summary>
        /// Needed for exception handling
        /// </summary>
        private ILogSvc _logger;

        public StudentRemarksSvc(IConfigSvc configSvc, ILogSvc logger)
        {
            _configSvc = configSvc;
            _logger = logger;
        }

        public void InsertStudentRemarks(Common.DTOs.StudentRemarksDTO studentRemarks)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "INSERT INTO student_remarks(student_id, course_from, course_to, remarks_text, exam_result_type, Attendance)" +
                                          " VALUES (@student_id, @course_from, @course_to, @remarks_text, @exam_result_type, @Attendance)";
                    command.CommandType = CommandType.Text;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    //data.UserDetails = new UserMasterDTO();
                    command.Parameters.Add("@student_id", MySqlDbType.Int32).Value = studentRemarks.Student.StudentInfoId;
                    command.Parameters.Add("@course_from", MySqlDbType.Date).Value = studentRemarks.CourseFromDate.Value.Date;
                    command.Parameters.Add("@course_to", MySqlDbType.Date).Value = studentRemarks.CourseToDate.Value.Date;
                    command.Parameters.Add("@remarks_text", MySqlDbType.String).Value = studentRemarks.Remarks;                    
                    command.Parameters.Add("@exam_result_type", MySqlDbType.String).Value = studentRemarks.ExamResultType;
                    command.Parameters.Add("@Attendance", MySqlDbType.String).Value = studentRemarks.AttendancePercent;

                    command.ExecuteNonQuery();
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public void UpdateStudentRemarks(Common.DTOs.StudentRemarksDTO studentRemarks)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "UPDATE student_remarks SET remarks_text=@remarks_text, Attendance=@Attendance " +
                                          "WHERE student_remarks_id=@student_remarks_id";
                    command.CommandType = CommandType.Text;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    //data.UserDetails = new UserMasterDTO();
                    command.Parameters.Add("@remarks_text", MySqlDbType.String).Value = studentRemarks.Remarks;
                    command.Parameters.Add("@Attendance", MySqlDbType.String).Value = studentRemarks.AttendancePercent;
                    command.Parameters.Add("@student_remarks_id", MySqlDbType.Int32).Value = studentRemarks.StudentRemarksId;

                    command.ExecuteNonQuery();
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public List<Common.DTOs.StudentRemarksDTO> GetStudentRemarks(int standardSectionId, string examResultType, DateTime courseFromDate, DateTime courseToDate, int location)
        {
            List<Common.DTOs.StudentRemarksDTO> lstRemarks = null;
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                dbSvc.OpenConnection();
                MySqlCommand command = new MySqlCommand();
                command.Connection = dbSvc.GetConnection() as MySqlConnection;
                string examTypeWhereClause = string.Empty;

                command.CommandText = "SELECT sr.student_remarks_id, st.StudentInfoId, st.RegistrationNumber, u.FName, u.LName, u.MName, sr.remarks_text, sr.Attendance FROM studentinfo st " +
                                      "INNER JOIN usermaster u ON (st.UserMasterId = u.UserMasterId AND u.LocationId=@location_id) " +
                                      "LEFT OUTER JOIN student_remarks sr ON (st.StudentInfoId=sr.student_id AND sr.exam_result_type = @exam_result_type AND sr.course_from = @course_from AND sr.course_to = @course_to)" +
                                      "WHERE st.StandardSectionId=@standard_sec_id";
                command.Parameters.Add("@location_id", MySqlDbType.Int32).Value = location;
                command.Parameters.Add("@exam_result_type", MySqlDbType.String).Value = examResultType;
                command.Parameters.Add("@course_from", MySqlDbType.DateTime).Value = courseFromDate.Date;
                command.Parameters.Add("@course_to", MySqlDbType.DateTime).Value = courseToDate.Date;
                command.Parameters.Add("@standard_sec_id", MySqlDbType.Int32).Value = standardSectionId;
                using (MySqlDataAdapter mDA = new MySqlDataAdapter(command))
                {
                    _dtData = new DataTable("REMARKS");
                    mDA.Fill(_dtData);
                    if(_dtData!=null && _dtData.Rows.Count>0)
                    {
                        lstRemarks = new List<Common.DTOs.StudentRemarksDTO>();
                        Common.DTOs.StudentRemarksDTO remarks = null;
                        foreach(DataRow dr in _dtData.Rows)
                        {
                            remarks = new Common.DTOs.StudentRemarksDTO();
                            if(string.IsNullOrEmpty(dr["student_remarks_id"].ToString()))
                            {
                                remarks.StudentRemarksId = -1;
                            }
                            else
                            {
                                remarks.StudentRemarksId = int.Parse(dr["student_remarks_id"].ToString());
                            }
                            remarks.Student = new Common.DTOs.StudentDTO();
                            remarks.Student.StudentInfoId = int.Parse(dr["StudentInfoId"].ToString());
                            remarks.Student.RegistrationNumber = dr["RegistrationNumber"].ToString();
                            remarks.Student.UserDetails = new Common.DTOs.UserMasterDTO();
                            remarks.Student.UserDetails.FName = dr["FName"].ToString();
                            remarks.Student.UserDetails.MName = dr["MName"].ToString();
                            remarks.Student.UserDetails.LName = dr["LName"].ToString();
                            remarks.Remarks = dr["remarks_text"].ToString();
                            remarks.AttendancePercent = dr["Attendance"].ToString();
                            lstRemarks.Add(remarks);
                        }
                    }
                }
            }
            return lstRemarks;
        }
    }
}
