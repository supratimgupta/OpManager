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

namespace OpMgr.DataAccess.Implementations
{
    public class StudentSvc : IStudentSvc
    {
        /// <summary>
        /// Required for getting connection related configs
        /// </summary>
        private IConfigSvc _configSvc;

        /// <summary>
        /// Needed for exception handling
        /// </summary>
        private ILogSvc _logger;

        public StudentSvc(IConfigSvc configSvc, ILogSvc logger)
        {
            _configSvc = configSvc;
            _logger = logger;
        }

        /// <summary>
        /// Delete a student record
        /// </summary>
        /// <param name="data">Data of student DTO</param>
        /// <returns>Status with deleted record</returns>
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
            StatusDTO<List<StudentDTO>> studLst = new StatusDTO<List<StudentDTO>>();
            string whereClause=null;
            string selectClause = null;
            DataSet dsStudentLst = null;

            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();//openning the connection

                    MySqlCommand command = new MySqlCommand();// creating my sql command for queries

                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    selectClause = "SELECT users.UserMasterId,users.FName, users.MName,users.LName," +
                                   "stnd.StandardName,sec.SectionName, student.RollNumber, student.RegistrationNumber,"+
                                   "student.GuardianContactNo "+
                                   "FROM studentinfo student "+
                                   " INNER JOIN UserMaster users ON student.UserMasterId = users.UserMasterId"+
                                   " INNER JOIN StandardSectionMap stdSecMap ON student.StandardSectionId = stdSecMap.StandardSectionId"+
                                   " INNER JOIN Standard stnd ON stdSecMap.StandardId = stnd.StandardId"+
                                   " INNER JOIN Section sec ON stdSecMap.StandardId = sec.SectionId ";

                    //Select All students who are ACTIVE
                    whereClause = "WHERE student.Active=1 ";


                    if (data != null)
                    {
                        //Name Search
                        //data.UserDetails = new UserMasterDTO();

                        if (!string.IsNullOrEmpty(data.UserDetails.FName))
                        {
                            whereClause = whereClause+ " AND users.FName=@FName ";
                            command.Parameters.Add("@FName", MySqlDbType.String).Value = data.UserDetails.FName;
                        }
                        if(!string.IsNullOrEmpty(data.UserDetails.MName))
                        {
                            whereClause = whereClause + " AND users.MName=@MName ";
                            command.Parameters.Add("@MName", MySqlDbType.String).Value = data.UserDetails.MName;
                        }

                        if(!string.IsNullOrEmpty(data.UserDetails.LName))
                        {
                            whereClause = whereClause + " AND users.LName=@LName ";
                            command.Parameters.Add("@LName", MySqlDbType.String).Value = data.UserDetails.LName;
                        }

                        //Class Search

                        //data.StandardSectionMap = new StandardSectionMapDTO();
                        //data.StandardSectionMap.Standard = new StandardDTO();
                        //data.StandardSectionMap.Section = new SectionDTO();

                        if(data.StandardSectionMap.StandardSectionId!=-1)
                        {
                            whereClause = whereClause + " AND stdSecMap.StandardSectionId=@StandardSectionId ";
                            command.Parameters.Add("@StandardSectionId", MySqlDbType.String).Value=data.StandardSectionMap.StandardSectionId;
                        }

                        
                        // Roll Number and Registration Search

                        if(!string.IsNullOrEmpty(data.RegistrationNumber))
                        {
                            whereClause = whereClause + " AND student.RegistrationNumber=@RegistrationNumber ";
                            command.Parameters.Add("@RegistrationNumber", MySqlDbType.String).Value = data.RegistrationNumber;
                        }

                        if (!string.IsNullOrEmpty(data.RollNumber))
                        {
                            whereClause = whereClause + " AND student.RollNumber=@RollNumber ";
                            command.Parameters.Add("@RollNumber", MySqlDbType.String).Value = data.RollNumber;
                        }


                    }

                       //command.CommandText = "select * from studentinfo";

                    command.CommandText = selectClause + whereClause;

                    MySqlDataAdapter da = new MySqlDataAdapter(command);
                    dsStudentLst = new DataSet();
                    da.Fill(dsStudentLst);


                    if(dsStudentLst!=null && dsStudentLst.Tables.Count>0)
                    {
                        studLst.ReturnObj = new List<StudentDTO>();
                        for (int i=0;i<dsStudentLst.Tables[0].Rows.Count;i++)
                        {
                            StudentDTO student = new StudentDTO();
                            student.Active = true;
                            student.GuardianContact = dsStudentLst.Tables[0].Rows[i]["GuardianContactNo"].ToString();

                            student.StandardSectionMap = new StandardSectionMapDTO();

                            student.StandardSectionMap.Standard = new StandardDTO();

                            student.StandardSectionMap.Section = new SectionDTO();

                            student.StandardSectionMap.Section.SectionName = dsStudentLst.Tables[0].Rows[i]["SectionName"].ToString();
                            student.StandardSectionMap.Standard.StandardName = dsStudentLst.Tables[0].Rows[i]["StandardName"].ToString();
                            student.RegistrationNumber = dsStudentLst.Tables[0].Rows[i]["RegistrationNumber"].ToString();

                            student.RollNumber = dsStudentLst.Tables[0].Rows[i]["RollNumber"].ToString();

                            student.UserDetails = new UserMasterDTO();
                            student.UserDetails.FName = dsStudentLst.Tables[0].Rows[i]["FName"].ToString();
                            student.UserDetails.MName = dsStudentLst.Tables[0].Rows[i]["MName"].ToString();
                            student.UserDetails.LName = dsStudentLst.Tables[0].Rows[i]["LName"].ToString();
                            student.UserDetails.UserMasterId = Convert.ToInt32(dsStudentLst.Tables[0].Rows[i]["UserMasterId"]);

                           
                            studLst.ReturnObj.Add(student);

                            studLst.IsSuccess = true;
                            
                        }
                    }

                }
                catch(Exception exp)
                {
                    _logger.Log(exp);
                    studLst.IsSuccess = false;
                    studLst.IsException = true;
                    studLst.ReturnObj = null;
                    studLst.ExceptionMessage = exp.Message;
                    studLst.StackTrace = exp.StackTrace;
                   // return studLst;
                }
               

            }

            return studLst;

        }

        public StatusDTO<StudentDTO> Update(StudentDTO data)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<List<StudentDTO>> PromoteToNewClass(List<StudentDTO> studentList, string Command, int StandardSectionId)
        {
            StatusDTO<List<StudentDTO>> studLst = new StatusDTO<List<StudentDTO>>();
            string whereClause = null;
            string selectClause = null;
            string updateClause = null;
            DataSet dsStudentLst = null;
            int noOfUpdateSuccessful = 0;

            if (studentList != null && !string.IsNullOrEmpty(Command) && StandardSectionId!=0)
                {
                   
                    using (IDbSvc dbSvc = new DbSvc(_configSvc))
                    {
                        try
                        {
                            dbSvc.OpenConnection();//openning the connection

                            MySqlCommand command = new MySqlCommand();// creating my sql command for queries

                            command.Connection = dbSvc.GetConnection() as MySqlConnection;

                            if (string.Equals(Command, "Promote"))
                           {

                            foreach (StudentDTO stud in studentList)
                            {
                                updateClause = "UPDATE studentinfo SET Status=@Status,NewStandardSectionId=@NewStandardSectionId" +
                                                                      " WHERE StudentInfoId=@StudentInfoId AND Active=1 ";
                                if (stud != null && stud.Status != null && stud.NewStandardSectionId != 0 && stud.StudentInfoId != 0)
                                {
                                    command.Parameters.Add("@Status", MySqlDbType.String).Value = stud.Status;
                                    command.Parameters.Add("@NewStandardSectionId", MySqlDbType.Int32).Value = stud.NewStandardSectionId;
                                    command.Parameters.Add("@StudentInfoId", MySqlDbType.Int32).Value = stud.StudentInfoId;
                                }

                                command.CommandText = updateClause;

                                if (command.ExecuteNonQuery() > 0)
                                {
                                    
                                    noOfUpdateSuccessful++;
                                }
                            }
                        }
                            selectClause = "SELECT student.StudentInfoId,users.UserMasterId,users.FName, users.MName,users.LName,student.StandardSectionId,stdSecMap.StandardId," +
                                       "student.RollNumber,student.NewStandardSectionId,student.Status,stnd.StandardName,sec.SectionName " +
                                       "FROM studentinfo student " +
                                       "INNER JOIN UserMaster users ON student.UserMasterId = users.UserMasterId " +
                                       "INNER JOIN StandardSectionMap stdSecMap ON student.StandardSectionId = stdSecMap.StandardSectionId " +
                                       "INNER JOIN Standard stnd ON stdSecMap.StandardId = stnd.StandardId " +
                                       "INNER JOIN Section sec ON stdSecMap.StandardId = sec.SectionId ";

                        //Select All students who are ACTIVE
                        whereClause = "WHERE student.Active=1 ";

                        //Select students of that particular class
                        if (StandardSectionId != -1)
                        {
                            whereClause = whereClause + " AND student.StandardSectionId=@StandardSectionId";
                            command.Parameters.Add("@StandardSectionId", MySqlDbType.Int32).Value = StandardSectionId;
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
                                if (string.Equals(Command, "Promote"))
                                {
                                    student.Status = dsStudentLst.Tables[0].Rows[i]["Status"].ToString();
                                    student.NewStandardSectionId = (int)dsStudentLst.Tables[0].Rows[i]["NewStandardSectionId"];
                                }

                                student.RollNumber = dsStudentLst.Tables[0].Rows[i]["RollNumber"].ToString();
                                    

                                    student.UserDetails = new UserMasterDTO();
                                    student.UserDetails.FName = dsStudentLst.Tables[0].Rows[i]["FName"].ToString();
                                    student.UserDetails.MName = dsStudentLst.Tables[0].Rows[i]["MName"].ToString();
                                    student.UserDetails.LName = dsStudentLst.Tables[0].Rows[i]["LName"].ToString();
                                    student.UserDetails.UserMasterId = Convert.ToInt32(dsStudentLst.Tables[0].Rows[i]["UserMasterId"]);

                                    student.StandardSectionMap = new StandardSectionMapDTO();
                                    student.StandardSectionMap.Standard = new StandardDTO();
                                    student.StandardSectionMap.Section = new SectionDTO();
                                    student.StandardSectionMap.StandardSectionId= (int)dsStudentLst.Tables[0].Rows[i]["StandardSectionId"];
                                    student.StandardSectionMap.Standard.StandardId = (int)dsStudentLst.Tables[0].Rows[i]["StandardId"];
                                    student.StandardSectionMap.Standard.StandardName =dsStudentLst.Tables[0].Rows[i]["StandardName"].ToString();
                                    student.StandardSectionMap.Section.SectionName = dsStudentLst.Tables[0].Rows[i]["SectionName"].ToString();


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
    }
}
