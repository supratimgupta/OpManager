﻿using MySql.Data.MySqlClient;
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
            StatusDTO<StudentDTO> status = null;
            try
            {
                if (data != null && data.UserDetails.UserMasterId != 0)
                {
                    using (IDbSvc dbSvc = new DbSvc(_configSvc))
                    {
                        dbSvc.OpenConnection();
                        MySqlCommand command = new MySqlCommand();
                        command.Connection = dbSvc.GetConnection() as MySqlConnection;
                        command.CommandText = "UPDATE studentinfo SET Active=0 WHERE UserMasterId=@UserMasterId";
                        command.Parameters.Add("@UserMasterId", MySqlDbType.Int32).Value = data.UserDetails.UserMasterId;

                        if (command.ExecuteNonQuery() > 0)
                        {
                            command = new MySqlCommand();
                            command.CommandText = "UPDATE usermaster SET Active=0 WHERE UserMasterId=@umId";
                            command.Parameters.Add("@umId", MySqlDbType.Int32).Value = data.UserDetails.UserMasterId;
                            command.Connection = dbSvc.GetConnection() as MySqlConnection;
                            if(command.ExecuteNonQuery()>0)
                            {
                                status = new StatusDTO<StudentDTO>();
                                status.IsSuccess = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                throw ex;
            }
            return status;
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
                    command.Parameters.Add("@FathersName", MySqlDbType.String).Value = data.FatherName;
                    command.Parameters.Add("@FathersContactNo", MySqlDbType.String).Value = data.FatherContact;
                    command.Parameters.Add("@FathersEmailId", MySqlDbType.String).Value = data.FatherEmailId;
                    command.Parameters.Add("@FathersQualification", MySqlDbType.String).Value = data.FatherQualification;
                    command.Parameters.Add("@FathersDesignation", MySqlDbType.String).Value = data.FatherDesignation;
                    command.Parameters.Add("@FathersOccupation", MySqlDbType.String).Value = data.FatherOccupation;
                    command.Parameters.Add("@FathersOrganisationName", MySqlDbType.String).Value = data.FatherOrganisationName;
                    command.Parameters.Add("@FathersAnnualIncome", MySqlDbType.String).Value = data.FatherAnnualIncome;
                    if (Convert.ToInt32(data.StandardSectionMap.StandardSectionId) > 0)
                    {
                        command.Parameters.Add("@StandardSectionId", MySqlDbType.Int32).Value = data.StandardSectionMap.StandardSectionId;
                    }
                    else
                    {
                        command.Parameters.Add("@StandardSectionId", MySqlDbType.Int32).Value = DBNull.Value;
                    }
                    command.Parameters.Add("@HouseTypeId", MySqlDbType.Int32).Value = data.HouseType.HouseTypeId;
                    command.Parameters.Add("@MothersName", MySqlDbType.String).Value = data.MotherName;
                    command.Parameters.Add("@MothersQualification", MySqlDbType.String).Value = data.MotherQualification;
                    command.Parameters.Add("@MothersOccupation", MySqlDbType.String).Value = data.MotherOccupation;
                    command.Parameters.Add("@MothersOrganisationName", MySqlDbType.String).Value = data.MotherOrganisationName;
                    command.Parameters.Add("@MothersAnnualIncome", MySqlDbType.String).Value = data.MotherAnnualIncome;
                    command.Parameters.Add("@SponsorOrGuardianName", MySqlDbType.String).Value = data.GuardianName;
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

                    if (_dtData.Columns.Count > 1)
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
                            if (!String.IsNullOrEmpty(_dsData.Tables[0].Rows[0]["ResidentialAddress"].ToString()))
                            {
                                studentDTO.UserDetails.ResidentialAddress = _dsData.Tables[0].Rows[0]["ResidentialAddress"].ToString();
                            }
                            else
                            {
                                studentDTO.UserDetails.ResidentialAddress = _dsData.Tables[0].Rows[0]["PermanentAddress"].ToString();
                            }
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
                            if(!String.IsNullOrEmpty(_dsData.Tables[0].Rows[0]["SponsorOrGuardianName"].ToString()))
                            {
                                studentDTO.GuardianName = _dsData.Tables[0].Rows[0]["SponsorOrGuardianName"].ToString();
                            }
                            else if(!String.IsNullOrEmpty(_dsData.Tables[0].Rows[0]["FathersName"].ToString()))
                            {
                                studentDTO.GuardianName = _dsData.Tables[0].Rows[0]["FathersName"].ToString();
                            }
                            else
                            {
                                studentDTO.GuardianName = _dsData.Tables[0].Rows[0]["MothersName"].ToString();
                            }
                            //studentDTO.GuardianName = _dsData.Tables[0].Rows[0]["SponsorOrGuardianName"].ToString();
                            studentDTO.FatherEmailId = _dsData.Tables[0].Rows[0]["FathersEmailId"].ToString();
                            studentDTO.StandardSectionMap = new StandardSectionMapDTO();
                            studentDTO.StandardSectionMap.StandardSectionId = Convert.ToInt32(_dsData.Tables[0].Rows[0]["StandardSectionId"]);
                            studentDTO.HouseType = new HouseTypeDTO();
                            if (!String.IsNullOrEmpty(_dsData.Tables[0].Rows[0]["HouseTypeId"].ToString()))
                            {
                                studentDTO.HouseType.HouseTypeId = Convert.ToInt32(_dsData.Tables[0].Rows[0]["HouseTypeId"]);
                            }
                            studentDTO.FatherName = _dsData.Tables[0].Rows[0]["FathersName"].ToString();
                            studentDTO.FatherQualification = _dsData.Tables[0].Rows[0]["FathersQualification"].ToString();
                            studentDTO.FatherOccupation = _dsData.Tables[0].Rows[0]["FathersOccupation"].ToString();
                            studentDTO.FatherDesignation = _dsData.Tables[0].Rows[0]["FathersDesignation"].ToString();
                            studentDTO.FatherOrganisationName = _dsData.Tables[0].Rows[0]["FathersOrgAddress"].ToString();
                            studentDTO.FatherAnnualIncome = _dsData.Tables[0].Rows[0]["FathersAnnualIncome"].ToString();
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
                            studentDTO.AnyAlumuniMember = _dsData.Tables[0].Rows[0]["AnyAlumuniMember"].ToString();
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

            if (data != null)
            {

                using (IDbSvc dbSvc = new DbSvc(_configSvc))
                {
                    try
                    {
                        dbSvc.OpenConnection();//openning the connection

                        MySqlCommand command = new MySqlCommand();// creating my sql command for queries

                        command.Connection = dbSvc.GetConnection() as MySqlConnection;

                        selectClause = "SELECT users.UserMasterId,users.FName, users.MName,users.LName," +
                                       "stnd.StandardName,sec.SectionName, student.RollNumber, student.RegistrationNumber," +
                                       "student.FathersContactNo " +
                                       "FROM studentinfo student " +
                                       " INNER JOIN UserMaster users ON student.UserMasterId = users.UserMasterId" +
                                       " INNER JOIN StandardSectionMap stdSecMap ON student.StandardSectionId = stdSecMap.StandardSectionId" +
                                       " INNER JOIN Standard stnd ON stdSecMap.StandardId = stnd.StandardId" +
                                       " INNER JOIN Section sec ON stdSecMap.SectionId = sec.SectionId ";

                        //Select All students who are ACTIVE
                        whereClause = "WHERE student.Active=1 ";


                        if (data != null)
                        {
                            //Name Search
                            //data.UserDetails = new UserMasterDTO();

                            if (!string.IsNullOrEmpty(data.UserDetails.FName))
                            {
                                data.UserDetails.FName = data.UserDetails.FName + "%";
                                whereClause = whereClause + " AND users.FName LIKE @FName";
                                command.Parameters.Add("@FName", MySqlDbType.String).Value = data.UserDetails.FName;
                            }
                            if (!string.IsNullOrEmpty(data.UserDetails.MName))
                            {
                                data.UserDetails.MName = data.UserDetails.MName + "%";
                                whereClause = whereClause + " AND users.MName LIKE @MName ";
                                command.Parameters.Add("@MName", MySqlDbType.String).Value = data.UserDetails.MName;
                            }

                            if (!string.IsNullOrEmpty(data.UserDetails.LName))
                            {
                                data.UserDetails.LName = data.UserDetails.LName + "%";
                                whereClause = whereClause + " AND users.LName LIKE @LName ";
                                command.Parameters.Add("@LName", MySqlDbType.String).Value = data.UserDetails.LName;
                            }

                            //Class Search

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
                                student.FatherContact = dsStudentLst.Tables[0].Rows[i]["FathersContactNo"].ToString();

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
                    command.Parameters.Add("@StandardSectionId", MySqlDbType.Int32).Value = data.StandardSectionMap.StandardSectionId;
                    command.Parameters.Add("@HouseTypeId", MySqlDbType.Int32).Value = data.HouseType.HouseTypeId;

                    command.Parameters.Add("@SponsorOrGuardianName", MySqlDbType.String).Value = data.GuardianName;
                    command.Parameters.Add("@FathersName", MySqlDbType.String).Value = data.FatherName;
                    command.Parameters.Add("@FathersContactNo", MySqlDbType.String).Value = data.FatherContact;
                    command.Parameters.Add("@FathersEmailId", MySqlDbType.String).Value = data.FatherEmailId;
                    command.Parameters.Add("@FathersQualification", MySqlDbType.String).Value = data.FatherQualification;
                    command.Parameters.Add("@FathersOccupation", MySqlDbType.String).Value = data.FatherOccupation;
                    command.Parameters.Add("@FathersDesignation", MySqlDbType.String).Value = data.FatherDesignation;
                    command.Parameters.Add("@FathersOrganisationName", MySqlDbType.String).Value = data.FatherOrganisationName;
                    command.Parameters.Add("@FathersAnnualIncome", MySqlDbType.String).Value = data.FatherAnnualIncome;
                    command.Parameters.Add("@MothersName", MySqlDbType.String).Value = data.MotherName;
                    command.Parameters.Add("@MothersQualification", MySqlDbType.String).Value = data.MotherQualification;
                    command.Parameters.Add("@MothersOccupation", MySqlDbType.String).Value = data.MotherOccupation;
                    command.Parameters.Add("@MothersOrganisationName", MySqlDbType.String).Value = data.MotherOrganisationName;
                    command.Parameters.Add("@MothersAnnualIncome", MySqlDbType.String).Value = data.MotherAnnualIncome;
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
                                if (!string.IsNullOrEmpty(dsStudentLst.Tables[0].Rows[i]["NewStandardSectionId"].ToString()))
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
        public StatusDTO<List<StudentDTO>> RunPromotionBatch()
        {
            StatusDTO<List<StudentDTO>> studList = null;
            string selectClause = null;
            string whereClause = null;
            DataSet dsStudentList = null;

            try
            {
                using (IDbSvc dbSvc = new DbSvc(_configSvc))
                {
                    dbSvc.OpenConnection();//openning the connection
                    MySqlCommand command = new MySqlCommand();// creating my sql command for queries
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    selectClause = "SELECT stnd.StandardName,sec.SectionName,student.Status,Count(*) AS Students" +
                                  " FROM studentinfo student" +
                                  " INNER JOIN StandardSectionMap stdSecMap ON student.StandardSectionId = stdSecMap.StandardSectionId" +
                                  " INNER JOIN Standard stnd ON stdSecMap.StandardId = stnd.StandardId" +
                                 " INNER JOIN Section sec ON stdSecMap.SectionId = sec.SectionId" +
                                 " WHERE student.Status = 'Promotion Confirmed' AND student.Active=1" +
                                 " GROUP BY student.StandardSectionId";

                    command.CommandText = selectClause;

                    MySqlDataAdapter msda = new MySqlDataAdapter(command);
                    dsStudentList = new DataSet();
                    msda.Fill(dsStudentList);

                    if (dsStudentList != null && dsStudentList.Tables.Count > 0)
                    {
                        if (dsStudentList.Tables[0].Rows.Count > 0)
                        {
                            studList = new StatusDTO<List<StudentDTO>>();
                            studList.ReturnObj = new List<StudentDTO>();
                            for (int i = 0; i < dsStudentList.Tables[0].Rows.Count; i++)
                            {
                                StudentDTO student = new StudentDTO();
                                student.StandardSectionMap = new StandardSectionMapDTO();
                                student.StandardSectionMap.StandardSectionDesc = (dsStudentList.Tables[0].Rows[i]["StandardName"] + " " + dsStudentList.Tables[0].Rows[i]["SectionName"]).ToString();
                                student.Status = dsStudentList.Tables[0].Rows[i]["Status"].ToString();
                                student.NoOfStudents = dsStudentList.Tables[0].Rows[i]["Students"].ToString();

                                studList.ReturnObj.Add(student);
                                studList.IsSuccess = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return studList;
        }
        public bool UpdatePromotedStudents(int loggedInUser, string status = "Promotion Confirmed")
        {
            string selectClause;
            string whereClause;
            string updateClause; // taken for query
            string insertClause;

            int studentInfoId;
            int newStandardSectionId;
            string newStatus = "Promoted"; // taken for update and insert
            int standardSectionId = 0;
            DateTime date;
            DateTime currentYear;
            DateTime previousYear;
            int active;

            bool IsPromoted=false;
            int noOfStudentsPromoted = 0;
            DataSet dsStudentLst = null;
            using (TransactionScope tran = new TransactionScope(TransactionScopeOption.RequiresNew)) // multiple updates hence using transaction scope
            {
                try
                {
                    using (IDbSvc dbSvc = new DbSvc(_configSvc))
                    {
                        dbSvc.OpenConnection();
                        MySqlCommand command = new MySqlCommand();
                        command.Connection = dbSvc.GetConnection() as MySqlConnection;

                        selectClause = "SELECT users.UserMasterId,student.StudentInfoId," +
                        "stnd.StandardName,sec.SectionName,student.Status,student.StandardSectionId,student.NewStandardSectionId,student.RollNumber,student.RegistrationNumber " +
                        "FROM studentinfo student " +
                        "INNER JOIN UserMaster users ON student.UserMasterId = users.UserMasterId " +
                        "INNER JOIN StandardSectionMap stdSecMap ON student.StandardSectionId = stdSecMap.StandardSectionId " +
                        "INNER JOIN Standard stnd ON stdSecMap.StandardId = stnd.StandardId " +
                        "INNER JOIN Section sec ON stdSecMap.SectionId = sec.SectionId ";

                        whereClause = "WHERE student.status=@status AND student.Active=1";

                        command.Parameters.Add("@status", MySqlDbType.String).Value = status;

                        command.CommandText = selectClause + whereClause;

                        dsStudentLst = new DataSet();
                        MySqlDataAdapter msda = new MySqlDataAdapter(command);
                        msda.Fill(dsStudentLst);

                        if (dsStudentLst != null && dsStudentLst.Tables.Count > 0)
                        {
                            if (dsStudentLst.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < dsStudentLst.Tables[0].Rows.Count; i++)
                                {
                                    studentInfoId = (int)dsStudentLst.Tables[0].Rows[i]["StudentInfoId"];
                                    newStandardSectionId = (int)dsStudentLst.Tables[0].Rows[i]["NewStandardSectionId"];
                                    standardSectionId = (int)dsStudentLst.Tables[0].Rows[i]["StandardSectionId"];
                                    if (studentInfoId != 0 && newStandardSectionId!=0 && standardSectionId!=0)
                                    {
                                        updateClause = "UPDATE studentinfo SET Status=@newStatus1,StandardSectionId=@newStandardSectionId1,NewStandardSectionId=null" +
                                                       " WHERE StudentInfoId=@studentInfoId1 AND Active=1 ";

                                        command.Parameters.Add("@newStatus1", MySqlDbType.String).Value = newStatus;
                                        command.Parameters.Add("@newStandardSectionId1", MySqlDbType.Int32).Value = newStandardSectionId;
                                        command.Parameters.Add("@studentInfoId1", MySqlDbType.Int32).Value = studentInfoId;

                                        command.CommandText = updateClause; // update the student ingfo in student info table

                                        int n = command.ExecuteNonQuery();
                                        if (n > 0) // insert into StudentClassMap History Table
                                        {
                                            date = DateTime.Now;
                                            currentYear = date;
                                            previousYear = date.AddYears(-1);
                                            active = 1;

                                            insertClause = "INSERT INTO studentclassmap(StudentInfoId,StandardSectionId,UpdatedBy,UpdatedDate,CourseTo,CourseFrom,Active) values(@studentinfoId1,@standardSectionId,@loggedInUser,@date,@currentYear,@previousYear,@active)";
                                           
                                            if (standardSectionId != 0)
                                                command.Parameters.Add("@standardSectionId", MySqlDbType.Int32).Value = standardSectionId;
                                            if (loggedInUser != 0)
                                                command.Parameters.Add("@loggedInUser", MySqlDbType.Int32).Value = loggedInUser;
                                            command.Parameters.Add("@date", MySqlDbType.DateTime).Value = date;
                                            command.Parameters.Add("@currentYear", MySqlDbType.DateTime).Value = currentYear;
                                            command.Parameters.Add("@previousYear", MySqlDbType.DateTime).Value = previousYear;
                                            command.Parameters.Add("@active", MySqlDbType.Int32).Value = active;

                                            command.CommandText = insertClause;
                                            if (command.ExecuteNonQuery() > 0)
                                            {
                                                noOfStudentsPromoted++;
                                                if (noOfStudentsPromoted == dsStudentLst.Tables[0].Rows.Count)
                                                {
                                                    IsPromoted = true;
                                                }
                                            }                                           
                                        }
                                        command.Parameters.Clear();// to clear the parameters after every iteration
                                    }                                    
                                }
                                tran.Complete();// Transaction is successful, and hence comitting else revert back
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    string excep = ex.Message;
                   tran.Dispose();
                }
            }
            return IsPromoted;
        }

        public StatusDTO<StudentDTO> GetStudentDetails(string registrationNo)
        {
            StatusDTO<StudentDTO> status = new StatusDTO<StudentDTO>();
            status.IsSuccess = false;
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "SELECT usr.UserMasterId, student.StudentInfoId, usr.FName, usr.MName, usr.LName, student.StandardSectionId FROM usermaster usr, studentinfo student WHERE usr.UserMasterId=student.UserMasterId AND student.RegistrationNumber=@regNo";
                    command.Parameters.Add("@regNo", MySqlDbType.String).Value = registrationNo;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    MySqlDataAdapter mDA = new MySqlDataAdapter(command);
                    _dsData = new DataSet();
                    mDA.Fill(_dsData);
                    if(_dsData!=null && _dsData.Tables[0].Rows.Count>0)
                    {
                        status.IsSuccess = true;
                        StudentDTO student = new StudentDTO();
                        student.RegistrationNumber = registrationNo;
                        student.UserDetails = new UserMasterDTO();
                        student.UserDetails.UserMasterId = (int)_dsData.Tables[0].Rows[0]["UserMasterId"];
                        student.StudentInfoId = (int)_dsData.Tables[0].Rows[0]["StudentInfoId"];
                        student.UserDetails.FName = _dsData.Tables[0].Rows[0]["FName"].ToString();
                        student.UserDetails.MName = _dsData.Tables[0].Rows[0]["MName"].ToString();
                        student.UserDetails.LName = _dsData.Tables[0].Rows[0]["LName"].ToString();
                        if(!string.IsNullOrEmpty(_dsData.Tables[0].Rows[0]["StandardSectionId"].ToString()))
                        {
                            student.StandardSectionMap = new StandardSectionMapDTO();
                            student.StandardSectionMap.StandardSectionId = (int)_dsData.Tables[0].Rows[0]["StandardSectionId"];
                        }
                        student.RegistrationNumber = student.UserDetails.FName + " " + student.UserDetails.MName + " " + student.UserDetails.LName + " ("+student.RegistrationNumber+")";
                        status.ReturnObj = student;
                    }
                }
                catch(Exception exp)
                {
                    _logger.Log(exp);
                    status.IsSuccess = false;
                    status.IsException = true;
                    status.StackTrace = exp.StackTrace;
                    status.ExceptionMessage = exp.Message;
                }
            }
            return status;
        }

        public StatusDTO<StudentDTO> GetStudentTransactionInfo(int studentInfoId)
        {
            StatusDTO<StudentDTO> status = new StatusDTO<StudentDTO>();
            status.IsSuccess = false;
            status.IsException = false;
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "SELECT s.UserMasterId, s.StandardSectionId, ssm.StandardId, ssm.SectionId, std.ClassTypeId FROM StudentInfo s, StandardSectionMap ssm, Standard std WHERE s.StandardSectionId=ssm.StandardSectionId AND ssm.StandardId=std.StandardId AND s.StudentInfoId=@studInfoId";
                    command.Parameters.Add("@studInfoId", MySqlDbType.Int32).Value = studentInfoId;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    _dtData = new DataTable();
                    MySqlDataAdapter mDA = new MySqlDataAdapter(command);
                    mDA.Fill(_dtData);
                    if(_dtData!=null && _dtData.Rows.Count>0)
                    {
                        status.IsSuccess = true;
                        status.ReturnObj = new StudentDTO();
                        status.ReturnObj.UserDetails = new UserMasterDTO();
                        status.ReturnObj.UserDetails.UserMasterId = Convert.ToInt32(_dtData.Rows[0]["UserMasterId"]);
                        status.ReturnObj.StandardSectionMap = new StandardSectionMapDTO();
                        status.ReturnObj.StandardSectionMap.StandardSectionId = Convert.ToInt32(_dtData.Rows[0]["StandardSectionId"]);
                        status.ReturnObj.StandardSectionMap.Standard = new StandardDTO();
                        status.ReturnObj.StandardSectionMap.Standard.StandardId = Convert.ToInt32(_dtData.Rows[0]["StandardId"]);
                        status.ReturnObj.StandardSectionMap.Section = new SectionDTO();
                        status.ReturnObj.StandardSectionMap.Section.SectionId = Convert.ToInt32(_dtData.Rows[0]["SectionId"]);
                        status.ReturnObj.StandardSectionMap.Standard.ClassType = new ClassTypeDTO();
                        status.ReturnObj.StandardSectionMap.Standard.ClassType.ClassTypeId = Convert.ToInt32(_dtData.Rows[0]["ClassTypeId"]);
                    }
                }
                catch (Exception exp)
                {
                    status.IsSuccess = false;
                    status.IsException = true;
                    status.ExceptionMessage = exp.Message;
                    status.StackTrace = exp.StackTrace;
                }
            }
            return status;
        }
    }
}

