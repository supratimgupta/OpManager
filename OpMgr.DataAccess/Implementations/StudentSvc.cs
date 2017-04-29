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
        private DataTable _dtData;
        private DataSet _dsData;

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

            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "ins_StudentDetails";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    //data.UserDetails = new UserMasterDTO();
                    command.Parameters.Add("@UserMasterId1", MySqlDbType.String).Value = data.UserDetails.UserMasterId;
                    command.Parameters.Add("@FName", MySqlDbType.String).Value = data.UserDetails.FName;
                    command.Parameters.Add("@MName", MySqlDbType.String).Value = data.UserDetails.MName;
                    command.Parameters.Add("@LName", MySqlDbType.String).Value = data.UserDetails.LName;
                    command.Parameters.Add("@Gender", MySqlDbType.String).Value = data.UserDetails.Gender;
                    command.Parameters.Add("@Image", MySqlDbType.String).Value = data.UserDetails.Image;
                    command.Parameters.Add("@DOB", MySqlDbType.DateTime).Value = data.UserDetails.DOB;
                    command.Parameters.Add("@EmailId", MySqlDbType.String).Value = data.UserDetails.EmailId;
                    command.Parameters.Add("@ResidentialAddress", MySqlDbType.String).Value = data.UserDetails.ResidentialAddress;
                    command.Parameters.Add("@PermanentAddress", MySqlDbType.String).Value = data.UserDetails.PermanentAddress;
                    command.Parameters.Add("@ContactNo", MySqlDbType.String).Value = data.UserDetails.ContactNo;
                    command.Parameters.Add("@AlContactNo", MySqlDbType.String).Value = data.UserDetails.AltContactNo;
                    command.Parameters.Add("@BloodGroup", MySqlDbType.String).Value = data.UserDetails.BloodGroup;
                    command.Parameters.Add("@UserName", MySqlDbType.String).Value = data.UserDetails.UserName;
                    command.Parameters.Add("@UserPassword", MySqlDbType.String).Value = data.UserDetails.Password;
                    //command.Parameters.Add("@RoleId", MySqlDbType.Int32).Value = data.Role.RoleId;
                    command.Parameters.Add("@LocationId", MySqlDbType.Int32).Value = data.UserDetails.Location.LocationId;

                    command.Parameters.Add("@RollNumber", MySqlDbType.String).Value = data.RollNumber;
                    command.Parameters.Add("@RegistrationNumber", MySqlDbType.String).Value = data.RegistrationNumber;
                    command.Parameters.Add("@AdmissionDate", MySqlDbType.DateTime).Value = data.AdmissionDate;
                    command.Parameters.Add("@GuardianContactNo", MySqlDbType.String).Value = data.GuardianContact;
                    command.Parameters.Add("@GuardianName", MySqlDbType.String).Value = data.GuardianName;
                    command.Parameters.Add("@GuardianEmailId", MySqlDbType.String).Value = data.GuardianEmailId;
                    command.Parameters.Add("@StandardSectionId", MySqlDbType.Int32).Value = data.StandardSectionMap.StandardSectionId;
                    command.Parameters.Add("@HouseTypeId", MySqlDbType.Int32).Value = data.HouseType.HouseTypeId;
                    command.Parameters.Add("@FatherName", MySqlDbType.String).Value = data.FatherName;
                    command.Parameters.Add("@FatherQualification", MySqlDbType.String).Value = data.FatherQualification;
                    command.Parameters.Add("@FatherOccupation", MySqlDbType.String).Value = data.FatherOccupation;
                    command.Parameters.Add("@FatherOrganisationName", MySqlDbType.String).Value = data.FatherOrganisationName;
                    command.Parameters.Add("@FatherAnnualIncome", MySqlDbType.String).Value = data.FatherAnnualIncome;
                    command.Parameters.Add("@MotherName", MySqlDbType.String).Value = data.MotherName;
                    command.Parameters.Add("@MotherName", MySqlDbType.String).Value = data.MotherQualification;
                    command.Parameters.Add("@MotherOccupation", MySqlDbType.String).Value = data.MotherOccupation;
                    command.Parameters.Add("@MotherOrganisationName", MySqlDbType.String).Value = data.MotherOrganisationName;
                    command.Parameters.Add("@MotherAnnualIncome", MySqlDbType.String).Value = data.MotherAnnualIncome;
                    command.Parameters.Add("@IsChristian", MySqlDbType.String).Value = data.IsChristian;
                    command.Parameters.Add("@IsParentTeacher", MySqlDbType.String).Value = data.IsParentTeacher;
                    command.Parameters.Add("@SubjectNameTheyTeach", MySqlDbType.String).Value = data.SubjectNameTheyTeach;
                    command.Parameters.Add("@IsParentFromEngMedium", MySqlDbType.String).Value = data.IsParentFromEngMedium;
                    command.Parameters.Add("@IsJointOrNuclearFamily", MySqlDbType.String).Value = data.IsJointOrNuclearFamily;
                    command.Parameters.Add("@SiblingsInStadOrNot", MySqlDbType.String).Value = data.SiblingsInStadOrNot;
                    command.Parameters.Add("@AnyAlumuniMember", MySqlDbType.String).Value = data.AnyAlumuniMember;
                    command.Parameters.Add("@StuInPrivateTution", MySqlDbType.String).Value = data.StuInPrivateTution;
                    command.Parameters.Add("@NoOfTution", MySqlDbType.String).Value = data.NoOfTution;
                    command.Parameters.Add("@FeesPaidForTution", MySqlDbType.String).Value = data.FeesPaidForTution;


                    MySqlDataReader rdr = command.ExecuteReader(CommandBehavior.CloseConnection);
                    _dtData = new DataTable();
                    _dtData.Load(rdr);
                    StatusDTO<StudentDTO> status = new StatusDTO<StudentDTO>();
                    
                    if(_dtData.Columns.Count>1)
                    {
                        status.IsSuccess = true;
                        status.ReturnObj = new StudentDTO();
                        status.ReturnObj.UserDetails = new UserMasterDTO();
                        status.ReturnObj.UserDetails.UserMasterId = (int)_dtData.Rows[0][1];
                    }
                    else
                    {
                        status.IsSuccess = false;
                        status.FailureReason = _dtData.Rows[0][0].ToString();
                    }
                    return status;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public StatusDTO<StudentDTO> Select(int rowId)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "get_StudentDetails";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    command.Parameters.Add("@UserMasterId1", MySqlDbType.Int32).Value = rowId;

                    MySqlDataAdapter rdr = new MySqlDataAdapter(command);
                    _dsData = new DataSet();
                    rdr.Fill(_dsData);
                    StatusDTO<StudentDTO> status = new StatusDTO<StudentDTO>();
                    StudentDTO studentDTO = new StudentDTO();
                    if (_dsData != null && _dsData.Tables.Count > 0)
                    {
                        if (_dsData.Tables[0].Rows.Count > 0)
                        {
                            studentDTO.UserDetails = new UserMasterDTO();
                            studentDTO.UserDetails.UserMasterId = Convert.ToInt32(_dsData.Tables[0].Rows[0]["UserMasterId"]);
                            studentDTO.UserDetails.Role = new RoleDTO();
                            studentDTO.UserDetails.Role.RoleId = Convert.ToInt32(_dsData.Tables[0].Rows[0]["RoleId"]);
                            studentDTO.UserDetails.Location = new LocationDTO();
                            studentDTO.UserDetails.Location.LocationId = Convert.ToInt32(_dsData.Tables[0].Rows[0]["LocationId"]);
                            studentDTO.UserDetails.FName = _dsData.Tables[0].Rows[0]["FName"].ToString();
                            studentDTO.UserDetails.MName = _dsData.Tables[0].Rows[0]["MName"].ToString();
                            studentDTO.UserDetails.LName = _dsData.Tables[0].Rows[0]["LName"].ToString();
                            studentDTO.UserDetails.Gender = _dsData.Tables[0].Rows[0]["Gender"].ToString();
                            studentDTO.UserDetails.Image = _dsData.Tables[0].Rows[0]["Image"].ToString();
                            if (!String.IsNullOrEmpty(_dsData.Tables[0].Rows[0]["DOB"].ToString()))
                            {
                                studentDTO.UserDetails.DOB = Convert.ToDateTime(_dsData.Tables[0].Rows[0]["DOB"]);
                            }
                            else
                            {
                                studentDTO.UserDetails.DOB = null;
                            }
                            studentDTO.UserDetails.EmailId = _dsData.Tables[0].Rows[0]["EmailId"].ToString();
                            studentDTO.UserDetails.ResidentialAddress = _dsData.Tables[0].Rows[0]["ResidentialAddress"].ToString();
                            studentDTO.UserDetails.PermanentAddress = _dsData.Tables[0].Rows[0]["PermanentAddress"].ToString();
                            studentDTO.UserDetails.ContactNo = _dsData.Tables[0].Rows[0]["ContactNo"].ToString();
                            studentDTO.UserDetails.AltContactNo = _dsData.Tables[0].Rows[0]["AltContactNo"].ToString();
                            studentDTO.UserDetails.BloodGroup = _dsData.Tables[0].Rows[0]["BloodGroup"].ToString();


                            studentDTO.RollNumber = _dsData.Tables[0].Rows[0]["RollNumber"].ToString();
                            studentDTO.RegistrationNumber = _dsData.Tables[0].Rows[0]["RegistrationNumber"].ToString();
                                if (!String.IsNullOrEmpty(_dsData.Tables[0].Rows[0]["AdmissionDate"].ToString()))
                                {
                                studentDTO.AdmissionDate = Convert.ToDateTime(_dsData.Tables[0].Rows[0]["AdmissionDate"]);
                                }
                                else
                                {
                                studentDTO.AdmissionDate = null;
                                }
                            //studentDTO.GuardianContact = _dsData.Tables[0].Rows[0]["GuardianContactNo"].ToString();
                            studentDTO.GuardianName = _dsData.Tables[0].Rows[0]["GuardianName"].ToString();
                            studentDTO.GuardianEmailId = _dsData.Tables[0].Rows[0]["GuardianEmailId"].ToString();
                            studentDTO.StandardSectionMap = new StandardSectionMapDTO();
                            studentDTO.StandardSectionMap.StandardSectionId = Convert.ToInt32(_dsData.Tables[0].Rows[0]["StandardSectionId"]);
                            studentDTO.HouseType = new HouseTypeDTO();
                            studentDTO.HouseType.HouseTypeId = Convert.ToInt32(_dsData.Tables[0].Rows[0]["HouseTypeId"]);
                            studentDTO.FatherName = _dsData.Tables[0].Rows[0]["FatherName"].ToString();
                            studentDTO.FatherQualification = _dsData.Tables[0].Rows[0]["FathersQualification"].ToString();
                            studentDTO.FatherOccupation = _dsData.Tables[0].Rows[0]["FatherOccupation"].ToString();
                            studentDTO.FatherDesignation = _dsData.Tables[0].Rows[0]["FatherDesignation"].ToString();
                            studentDTO.FatherOrganisationName = _dsData.Tables[0].Rows[0]["FatherOrgAddress"].ToString();
                            studentDTO.FatherAnnualIncome = _dsData.Tables[0].Rows[0]["FatherIncome"].ToString();
                            studentDTO.MotherName = _dsData.Tables[0].Rows[0]["MothersName"].ToString();
                            studentDTO.MotherQualification = _dsData.Tables[0].Rows[0]["MothersQualification"].ToString();
                            studentDTO.MotherOccupation = _dsData.Tables[0].Rows[0]["MothersOccupation"].ToString();
                            studentDTO.MotherOrganisationName = _dsData.Tables[0].Rows[0]["MothersOrgName"].ToString();
                            studentDTO.MotherAnnualIncome = _dsData.Tables[0].Rows[0]["MothersAnnualIncome"].ToString();
                            studentDTO.IsChristian = _dsData.Tables[0].Rows[0]["IsChristian"].ToString();
                            studentDTO.IsParentTeacher = _dsData.Tables[0].Rows[0]["IsParentTeacher"].ToString();
                            studentDTO.SubjectNameTheyTeach = _dsData.Tables[0].Rows[0]["SubjectNameTheyTeach"].ToString();
                            studentDTO.IsParentFromEngMedium = _dsData.Tables[0].Rows[0]["ParentFromEngMed"].ToString();
                            studentDTO.IsJointOrNuclearFamily = _dsData.Tables[0].Rows[0]["JointOrNuclearFamily"].ToString();
                            studentDTO.SiblingsInStadOrNot = _dsData.Tables[0].Rows[0]["SiblingsInStadsOrNot"].ToString();
                            studentDTO.AnyAlumuniMember = _dsData.Tables[0].Rows[0]["AnyAlumniMember"].ToString();
                            studentDTO.StuInPrivateTution = _dsData.Tables[0].Rows[0]["StudentInPvtTution"].ToString();
                            studentDTO.NoOfTution = _dsData.Tables[0].Rows[0]["NoOfTution"].ToString();
                            studentDTO.FeesPaidForTution = _dsData.Tables[0].Rows[0]["FeesPaidForTution"].ToString();
                        }
                    }
                    status.ReturnObj = studentDTO;
                    return status;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }

        }

        public StatusDTO<List<StudentDTO>> Select(StudentDTO data)
        {
            StatusDTO<List<StudentDTO>> studLst = new StatusDTO<List<StudentDTO>>();
            string whereClause = null;
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
                                   "stnd.StandardName,sec.SectionName, student.RollNumber, student.RegistrationNumber," +
                                   "student.GuardianContactNo " +
                                   "FROM studentinfo student " +
                                   " INNER JOIN UserMaster users ON student.UserMasterId = users.UserMasterId" +
                                   " INNER JOIN StandardSectionMap stdSecMap ON student.StandardSectionId = stdSecMap.StandardSectionId" +
                                   " INNER JOIN Standard stnd ON stdSecMap.StandardId = stnd.StandardId" +
                                   " INNER JOIN Section sec ON stdSecMap.StandardId = sec.SectionId ";

                    //Select All students who are ACTIVE
                    whereClause = "WHERE student.Active=1 ";


                    if (data != null)
                    {
                        //Name Search
                        //data.UserDetails = new UserMasterDTO();

                        if (!string.IsNullOrEmpty(data.UserDetails.FName))
                        {
                            whereClause = whereClause + " AND users.FName=@FName ";
                            command.Parameters.Add("@FName", MySqlDbType.String).Value = data.UserDetails.FName;
                        }
                        if (!string.IsNullOrEmpty(data.UserDetails.MName))
                        {
                            whereClause = whereClause + " AND users.MName=@MName ";
                            command.Parameters.Add("@MName", MySqlDbType.String).Value = data.UserDetails.MName;
                        }

                        if (!string.IsNullOrEmpty(data.UserDetails.LName))
                        {
                            whereClause = whereClause + " AND users.LName=@LName ";
                            command.Parameters.Add("@LName", MySqlDbType.String).Value = data.UserDetails.LName;
                        }

                        //Class Search

                        //data.StandardSectionMap = new StandardSectionMapDTO();
                        //data.StandardSectionMap.Standard = new StandardDTO();
                        //data.StandardSectionMap.Section = new SectionDTO();

                        if (data.StandardSectionMap.StandardSectionId != -1)
                        {
                            whereClause = whereClause + " AND stdSecMap.StandardSectionId=@StandardSectionId ";
                            command.Parameters.Add("@StandardSectionId", MySqlDbType.String).Value = data.StandardSectionMap.StandardSectionId;
                        }


                        // Roll Number and Registration Search

                        if (!string.IsNullOrEmpty(data.RegistrationNumber))
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


                    if (dsStudentLst != null && dsStudentLst.Tables.Count > 0)
                    {
                        studLst.ReturnObj = new List<StudentDTO>();
                        for (int i = 0; i < dsStudentLst.Tables[0].Rows.Count; i++)
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
                catch (Exception exp)
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
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "update_StudentDetails";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    command.Parameters.Add("@UserMasterId1", MySqlDbType.String).Value = data.UserDetails.UserMasterId;
                    command.Parameters.Add("@FName", MySqlDbType.String).Value = data.UserDetails.FName;
                    command.Parameters.Add("@MName", MySqlDbType.String).Value = data.UserDetails.MName;
                    command.Parameters.Add("@LName", MySqlDbType.String).Value = data.UserDetails.LName;
                    command.Parameters.Add("@Gender", MySqlDbType.String).Value = data.UserDetails.Gender;
                    command.Parameters.Add("@Image", MySqlDbType.String).Value = data.UserDetails.Image;
                    command.Parameters.Add("@DOB", MySqlDbType.DateTime).Value = data.UserDetails.DOB;
                    command.Parameters.Add("@EmailId", MySqlDbType.String).Value = data.UserDetails.EmailId;
                    command.Parameters.Add("@ResidentialAddress", MySqlDbType.String).Value = data.UserDetails.ResidentialAddress;
                    command.Parameters.Add("@PermanentAddress", MySqlDbType.String).Value = data.UserDetails.PermanentAddress;
                    command.Parameters.Add("@ContactNo", MySqlDbType.String).Value = data.UserDetails.ContactNo;
                    command.Parameters.Add("@AltContactNo", MySqlDbType.String).Value = data.UserDetails.AltContactNo;
                    command.Parameters.Add("@BloodGroup", MySqlDbType.String).Value = data.UserDetails.BloodGroup;
                    //command.Parameters.Add("@UserPassword", MySqlDbType.String).Value = data.UserDetails.Password;
                    //command.Parameters.Add("@RoleId", MySqlDbType.Int32).Value = data.UserDetails.Role.RoleId;
                    command.Parameters.Add("@LocationId", MySqlDbType.Int32).Value = data.UserDetails.Location.LocationId;
                   command.Parameters.Add("@RollNumber", MySqlDbType.String).Value = data.RollNumber;
                    command.Parameters.Add("@RegistrationNumber", MySqlDbType.String).Value = data.RegistrationNumber;
                    command.Parameters.Add("@AdmissionDate", MySqlDbType.DateTime).Value = data.AdmissionDate;
                    command.Parameters.Add("@GuardianContactNo", MySqlDbType.String).Value = data.GuardianContact;
                    command.Parameters.Add("@GuardianName", MySqlDbType.String).Value = data.GuardianName;
                    command.Parameters.Add("@GuardianEmailId", MySqlDbType.String).Value = data.GuardianEmailId;
                    command.Parameters.Add("@StandardSectionId", MySqlDbType.Int32).Value = data.StandardSectionMap.StandardSectionId;
                    command.Parameters.Add("@HouseTypeId", MySqlDbType.Int32).Value = data.HouseType.HouseTypeId;

                    command.Parameters.Add("@FatherName", MySqlDbType.String).Value = data.FatherName;
                    command.Parameters.Add("@FatherQualification", MySqlDbType.String).Value = data.FatherQualification;
                    command.Parameters.Add("@FatherOccupation", MySqlDbType.String).Value = data.FatherOccupation;
                    command.Parameters.Add("@FatherOrganisationName", MySqlDbType.String).Value = data.FatherOrganisationName;
                    command.Parameters.Add("@FatherAnnualIncome", MySqlDbType.String).Value = data.FatherAnnualIncome;
                    command.Parameters.Add("@MotherName", MySqlDbType.String).Value = data.MotherName;
                    command.Parameters.Add("@MotherName", MySqlDbType.String).Value = data.MotherQualification;
                    command.Parameters.Add("@MotherOccupation", MySqlDbType.String).Value = data.MotherOccupation;
                    command.Parameters.Add("@MotherOrganisationName", MySqlDbType.String).Value = data.MotherOrganisationName;
                    command.Parameters.Add("@MotherAnnualIncome", MySqlDbType.String).Value = data.MotherAnnualIncome;
                    command.Parameters.Add("@IsChristian", MySqlDbType.String).Value = data.IsChristian;
                    command.Parameters.Add("@IsParentTeacher", MySqlDbType.String).Value = data.IsParentTeacher;
                    command.Parameters.Add("@SubjectNameTheyTeach", MySqlDbType.String).Value = data.SubjectNameTheyTeach;
                    command.Parameters.Add("@IsParentFromEngMedium", MySqlDbType.String).Value = data.IsParentFromEngMedium;
                    command.Parameters.Add("@IsJointOrNuclearFamily", MySqlDbType.String).Value = data.IsJointOrNuclearFamily;
                    command.Parameters.Add("@SiblingsInStadOrNot", MySqlDbType.String).Value = data.SiblingsInStadOrNot;
                    command.Parameters.Add("@AnyAlumuniMember", MySqlDbType.String).Value = data.AnyAlumuniMember;
                    command.Parameters.Add("@StuInPrivateTution", MySqlDbType.String).Value = data.StuInPrivateTution;
                    command.Parameters.Add("@NoOfTution", MySqlDbType.String).Value = data.NoOfTution;
                    command.Parameters.Add("@FeesPaidForTution", MySqlDbType.String).Value = data.FeesPaidForTution;


                    command.ExecuteNonQuery();
                    StatusDTO<StudentDTO> status = new StatusDTO<StudentDTO>();
                    status.IsSuccess = true;
                    return status;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public StatusDTO<List<StudentDTO>> PromoteToNewClass(List<StudentDTO> studentList, string Command, int StandardSectionId)
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

                            if(string.Equals(Command,"Promotion Confirmed"))
                            {
                                 updateClause = "UPDATE studentinfo SET StandardSectionId=NewStandardSectionId, NewStandardSectionId=NULL, Status='Promoted' WHERE Active=1 AND Status='Promotion Confirmed' AND NewStandardSectionId IS NOT NULL";
                                command = new MySqlCommand();
                                command.Connection = dbSvc.GetConnection() as MySqlConnection;

                                command.CommandText = updateClause;
                               
                                if (command.ExecuteNonQuery() > 0)
                                 {
                                        noOfUpdateSuccessful++;
                                 }

                             }

                            if (string.Equals(Command, "Promote"))
                           {

                            foreach (StudentDTO stud in studentList)
                            {
                                command = new MySqlCommand();
                                command.Connection = dbSvc.GetConnection() as MySqlConnection;

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
                        selectClause = "SELECT student.StudentInfoId,users.UserMasterId,users.FName, users.MName,users.LName,student.StandardSectionId,stdSecMap.StandardId,stdSecMap.Serial," +
                                   "student.RollNumber,student.NewStandardSectionId,student.Status,stnd.StandardName,sec.SectionName,stnd1.StandardName AS NewStandardName,sec1.SectionName AS NewSectionName " +
                                   "FROM studentinfo student " +
                                   "LEFT JOIN UserMaster users ON student.UserMasterId = users.UserMasterId " +
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
                                    if(!string.IsNullOrEmpty(dsStudentLst.Tables[0].Rows[i]["NewStandardSectionId"].ToString()))
                                         student.NewStandardSectionId = (int)dsStudentLst.Tables[0].Rows[i]["NewStandardSectionId"];
                                

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
    }
}
