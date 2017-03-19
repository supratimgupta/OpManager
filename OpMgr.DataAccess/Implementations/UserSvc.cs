using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using OpMgr.Common.Contracts;
using System.Data;
using OpMgr.Common.DTOs;
using OpMgr.Common.Contracts.Modules;



namespace OpMgr.DataAccess.Implementations
{
    public class UserSvc : IUserSvc, IDisposable
    {
        private IConfigSvc _configSvc;
        private DataTable _dtData;
        private DataSet _dsData;

        public UserSvc(IConfigSvc configSvc)
        {
            _configSvc = configSvc;
        }

        public StatusDTO<UserMasterDTO> Delete(UserMasterDTO data)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public StatusDTO<UserMasterDTO> Insert(UserMasterDTO data)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "ins_UserDetails";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    command.Parameters.Add("@UserMasterId1", MySqlDbType.String).Value = data.UserMasterId;
                    command.Parameters.Add("@FName", MySqlDbType.String).Value = data.FName;
                    command.Parameters.Add("@MName", MySqlDbType.String).Value = data.MName;
                    command.Parameters.Add("@LName", MySqlDbType.String).Value = data.LName;
                    command.Parameters.Add("@Gender", MySqlDbType.String).Value = data.Gender;
                    command.Parameters.Add("@Image", MySqlDbType.String).Value = data.Image;
                    command.Parameters.Add("@DOB", MySqlDbType.DateTime).Value = data.DOB;
                    command.Parameters.Add("@EmailId", MySqlDbType.String).Value = data.EmailId;
                    command.Parameters.Add("@ResidentialAddress", MySqlDbType.String).Value = data.ResidentialAddress;
                    command.Parameters.Add("@PermanentAddress", MySqlDbType.String).Value = data.PermanentAddress;
                    command.Parameters.Add("@ContactNo", MySqlDbType.String).Value = data.ContactNo;
                    command.Parameters.Add("@AlContactNo", MySqlDbType.String).Value = data.AltContactNo;
                    command.Parameters.Add("@BloodGroup", MySqlDbType.String).Value = data.BloodGroup;
                    command.Parameters.Add("@UserName", MySqlDbType.String).Value = data.UserName;
                    command.Parameters.Add("@UserPassword", MySqlDbType.String).Value = data.Password;
                    command.Parameters.Add("@RoleId", MySqlDbType.Int32).Value = data.Role.RoleId;
                    command.Parameters.Add("@LocationId", MySqlDbType.Int32).Value = data.Location.LocationId;

                    if (data.Student != null)
                    {
                        command.Parameters.Add("@RollNumber", MySqlDbType.String).Value = data.Student.RollNumber;
                        command.Parameters.Add("@RegistrationNumber", MySqlDbType.String).Value = data.Student.RegistrationNumber;
                        command.Parameters.Add("@AdmissionDate", MySqlDbType.DateTime).Value = data.Student.AdmissionDate;
                        command.Parameters.Add("@GuardianContactNo", MySqlDbType.String).Value = data.Student.GuardianContact;
                        command.Parameters.Add("@GuardianName", MySqlDbType.String).Value = data.Student.GuardianName;
                        command.Parameters.Add("@GuardianEmailId", MySqlDbType.String).Value = data.Student.GuardianEmailId;
                        command.Parameters.Add("@StandardSectionId", MySqlDbType.Int32).Value = data.Student.StandardSectionMap.StandardSectionId;
                        command.Parameters.Add("@HouseTypeId", MySqlDbType.Int32).Value = data.Student.HouseType.HouseTypeId;
                    }
                    else
                    {
                        command.Parameters.Add("@RollNumber", MySqlDbType.String).Value = DBNull.Value;
                        command.Parameters.Add("@RegistrationNumber", MySqlDbType.String).Value = DBNull.Value;
                        command.Parameters.Add("@AdmissionDate", MySqlDbType.DateTime).Value = DBNull.Value;
                        command.Parameters.Add("@GuardianContactNo", MySqlDbType.String).Value = DBNull.Value;
                        command.Parameters.Add("@GuardianName", MySqlDbType.String).Value = DBNull.Value;
                        command.Parameters.Add("@GuardianEmailId", MySqlDbType.String).Value = DBNull.Value;
                        command.Parameters.Add("@StandardSectionId", MySqlDbType.Int32).Value = DBNull.Value;
                        command.Parameters.Add("@HouseTypeId", MySqlDbType.Int32).Value = DBNull.Value;
                    }

                    if (data.Employee != null)
                    {
                        command.Parameters.Add("@StaffEmployeeId", MySqlDbType.String).Value = data.Employee.StaffEmployeeId;
                        command.Parameters.Add("@EducationQualification", MySqlDbType.String).Value = data.Employee.EducationalQualification;
                        command.Parameters.Add("@DateOfJoining", MySqlDbType.DateTime).Value = data.Employee.DateOfJoining;
                        command.Parameters.Add("@DepartmentId", MySqlDbType.Int32).Value = data.Employee.Department.DepartmentId;
                        command.Parameters.Add("@DesignationId", MySqlDbType.Int32).Value = data.Employee.Designation.DesignationId;
                    }
                    else
                    {
                        command.Parameters.Add("@StaffEmployeeId", MySqlDbType.String).Value = DBNull.Value;
                        command.Parameters.Add("@EducationQualification", MySqlDbType.String).Value = DBNull.Value;
                        command.Parameters.Add("@DateOfJoining", MySqlDbType.DateTime).Value = DBNull.Value;
                        command.Parameters.Add("@DepartmentId", MySqlDbType.Int32).Value = DBNull.Value;
                        command.Parameters.Add("@DesignationId", MySqlDbType.Int32).Value = DBNull.Value;
                    }
                    MySqlDataReader rdr = command.ExecuteReader(CommandBehavior.CloseConnection);
                    _dtData = new DataTable();
                    _dtData.Load(rdr);
                    StatusDTO<UserMasterDTO> status = new StatusDTO<UserMasterDTO>();
                    return status;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public StatusDTO<UserMasterDTO> Login(UserMasterDTO data, out List<EntitlementDTO> roleList, out List<ActionDTO> actionList)
        {
            StatusDTO<UserMasterDTO> status = new StatusDTO<UserMasterDTO>();
            roleList = null;
            actionList = null;
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "UserLogin";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    command.Parameters.Add("@UserName", MySqlDbType.String).Value = data.UserName;
                    command.Parameters.Add("@UserPassword", MySqlDbType.String).Value = data.Password;

                    MySqlParameter error = new MySqlParameter("@ErrorMsg", MySqlDbType.VarChar);
                    error.Direction = ParameterDirection.Output;
                    command.Parameters.Add(error);

                    MySqlDataAdapter rdr = new MySqlDataAdapter(command);
                    _dsData = new DataSet();
                    rdr.Fill(_dsData);
                    List<EntitlementDTO> entitlementList = new List<EntitlementDTO>();
                    List<ActionDTO> useractionList = new List<ActionDTO>();
                    UserMasterDTO userMaster = new UserMasterDTO();
                    if (_dsData != null && _dsData.Tables.Count == 3)
                    {
                        if (_dsData.Tables[0].Rows.Count > 0)
                        {
                            userMaster.UserMasterId = Convert.ToInt32(_dsData.Tables[0].Rows[0]["UserMasterId"]);
                        }

                        if (_dsData.Tables[1].Rows.Count > 0)
                        {
                            EntitlementDTO userEntitle = new EntitlementDTO();
                            for (int i = 0; i < _dsData.Tables[1].Rows.Count; i++)
                            {
                                userEntitle.UserRoleId = Convert.ToInt32(_dsData.Tables[1].Rows[i]["UserRoleId"]);
                                userEntitle.RoleName = _dsData.Tables[1].Rows[i]["RoleName"].ToString();
                                entitlementList.Add(userEntitle);
                            }
                            roleList = entitlementList;
                        }

                        if (_dsData.Tables[2].Rows.Count > 0)
                        {
                            ActionDTO useractionDTO = new ActionDTO();
                            for (int i = 0; i < _dsData.Tables[2].Rows.Count; i++)
                            {
                                useractionDTO.ActionName = _dsData.Tables[2].Rows[i]["ActionName"].ToString();
                                useractionDTO.ActionLink = _dsData.Tables[2].Rows[0]["ActionLink"].ToString();
                                useractionDTO.IsChildAction = Convert.ToBoolean(_dsData.Tables[2].Rows[0]["IsChildAction"]);
                                useractionDTO.MenuText = _dsData.Tables[2].Rows[0]["MenuText"].ToString();
                                useractionDTO.DisabledControlId = _dsData.Tables[2].Rows[0]["DisabledControlId"].ToString();
                                useractionDTO.HiddenControlId = _dsData.Tables[2].Rows[0]["HiddenControlId"].ToString();
                                useractionDTO.GroupName = _dsData.Tables[2].Rows[0]["GroupName"].ToString();
                                useractionList.Add(useractionDTO);
                            }
                            actionList = useractionList;
                        }
                        status.IsException = false;
                        status.IsSuccess = true;
                        status.ReturnObj = userMaster;

                    }
                    else
                    {
                        status.IsSuccess = false;
                        status.FailureReason = error.Value.ToString();
                    }
                    return status;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }


        public StatusDTO<UserMasterDTO> Select(int rowId)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "get_UserDetails";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    command.Parameters.Add("@UserMasterId1", MySqlDbType.Int32).Value = rowId;

                    MySqlDataAdapter rdr = new MySqlDataAdapter(command);
                    _dsData = new DataSet();
                    rdr.Fill(_dsData);
                    StatusDTO<UserMasterDTO> status = new StatusDTO<UserMasterDTO>();
                    UserMasterDTO usermasterDTO = new UserMasterDTO();
                    if (_dsData != null && _dsData.Tables.Count > 0)
                    {
                        if (_dsData.Tables[0].Rows.Count > 0)
                        {
                            usermasterDTO.UserMasterId = Convert.ToInt32(_dsData.Tables[0].Rows[0]["UserMasterId"]);
                            usermasterDTO.Role = new RoleDTO();
                            usermasterDTO.Role.RoleId = Convert.ToInt32(_dsData.Tables[0].Rows[0]["RoleId"]);
                            usermasterDTO.Location = new LocationDTO();
                            usermasterDTO.Location.LocationId = Convert.ToInt32(_dsData.Tables[0].Rows[0]["LocationId"]);
                            usermasterDTO.FName = _dsData.Tables[0].Rows[0]["FName"].ToString();
                            usermasterDTO.MName = _dsData.Tables[0].Rows[0]["MName"].ToString();
                            usermasterDTO.LName = _dsData.Tables[0].Rows[0]["LName"].ToString();
                            usermasterDTO.Gender = _dsData.Tables[0].Rows[0]["Gender"].ToString();
                            usermasterDTO.Image = _dsData.Tables[0].Rows[0]["Image"].ToString();
                            if (!String.IsNullOrEmpty(_dsData.Tables[0].Rows[0]["DOB"].ToString()))
                            {
                                usermasterDTO.DOB = Convert.ToDateTime(_dsData.Tables[0].Rows[0]["DOB"]);
                            }
                            else
                            {
                                usermasterDTO.DOB = null;
                            }
                            usermasterDTO.EmailId = _dsData.Tables[0].Rows[0]["EmailId"].ToString();
                            usermasterDTO.ResidentialAddress = _dsData.Tables[0].Rows[0]["ResidentialAddress"].ToString();
                            usermasterDTO.PermanentAddress = _dsData.Tables[0].Rows[0]["PermanentAddress"].ToString();
                            usermasterDTO.ContactNo = _dsData.Tables[0].Rows[0]["ContactNo"].ToString();
                            usermasterDTO.AltContactNo = _dsData.Tables[0].Rows[0]["AltContactNo"].ToString();
                            usermasterDTO.BloodGroup = _dsData.Tables[0].Rows[0]["BloodGroup"].ToString();
                            
                            if (Convert.ToInt32(_dsData.Tables[0].Rows[0]["RoleId"]) == 1)
                            {
                                usermasterDTO.Student = new StudentDTO();
                                usermasterDTO.Student.RollNumber = _dsData.Tables[0].Rows[0]["RollNumber"].ToString();
                                usermasterDTO.Student.RegistrationNumber = _dsData.Tables[0].Rows[0]["RegistrationNumber"].ToString();
                                if (!String.IsNullOrEmpty(_dsData.Tables[0].Rows[0]["AdmissionDate"].ToString()))
                                {
                                    usermasterDTO.Student.AdmissionDate = Convert.ToDateTime(_dsData.Tables[0].Rows[0]["AdmissionDate"]);
                                }
                                else
                                {
                                    usermasterDTO.Student.AdmissionDate = null;
                                }
                                usermasterDTO.Student.GuardianContact = _dsData.Tables[0].Rows[0]["GuardianContactNo"].ToString();
                                usermasterDTO.Student.GuardianName = _dsData.Tables[0].Rows[0]["GuardianName"].ToString();
                                usermasterDTO.Student.GuardianEmailId = _dsData.Tables[0].Rows[0]["GuardianEmailId"].ToString();
                                usermasterDTO.Student.StandardSectionMap = new StandardSectionMapDTO();
                                usermasterDTO.Student.StandardSectionMap.StandardSectionId = Convert.ToInt32(_dsData.Tables[0].Rows[0]["StandardSectionId"]);
                                usermasterDTO.Student.HouseType = new HouseTypeDTO();
                                usermasterDTO.Student.HouseType.HouseTypeId = Convert.ToInt32(_dsData.Tables[0].Rows[0]["HouseTypeId"]);
                            }
                            else if (Convert.ToInt32(_dsData.Tables[0].Rows[0]["RoleId"]) > 1)
                            {
                                usermasterDTO.Employee = new EmployeeDetailsDTO();
                                usermasterDTO.Employee.EducationalQualification = _dsData.Tables[0].Rows[0]["EducationQualification"].ToString();
                                if (!String.IsNullOrEmpty(_dsData.Tables[0].Rows[0]["DateOfJoining"].ToString()))
                                {
                                    usermasterDTO.Employee.DateOfJoining = Convert.ToDateTime(_dsData.Tables[0].Rows[0]["DateOfJoining"]);
                                }
                                else
                                {
                                    usermasterDTO.Employee.DateOfJoining = null;
                                }
                                usermasterDTO.Employee.Department = new DepartmentDTO();
                                usermasterDTO.Employee.Department.DepartmentId = Convert.ToInt32(_dsData.Tables[0].Rows[0]["DepartmentId"]);
                                usermasterDTO.Employee.StaffEmployeeId = _dsData.Tables[0].Rows[0]["StaffEmployeeId"].ToString();
                                usermasterDTO.Employee.Designation = new DesignationDTO();
                                usermasterDTO.Employee.Designation.DesignationId = Convert.ToInt32(_dsData.Tables[0].Rows[0]["DepartmentId"]);
                            }
                        }
                    }
                    status.ReturnObj = usermasterDTO;
                    return status;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }

        }

        public StatusDTO<List<UserMasterDTO>> Select(UserMasterDTO data)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<UserMasterDTO> Update(UserMasterDTO data)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "update_UserDetails";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    command.Parameters.Add("@UserMasterId1", MySqlDbType.String).Value = data.UserMasterId;
                    command.Parameters.Add("@FName", MySqlDbType.String).Value = data.FName;
                    command.Parameters.Add("@MName", MySqlDbType.String).Value = data.MName;
                    command.Parameters.Add("@LName", MySqlDbType.String).Value = data.LName;
                    command.Parameters.Add("@Gender", MySqlDbType.String).Value = data.Gender;
                    command.Parameters.Add("@Image", MySqlDbType.String).Value = data.Image;
                    command.Parameters.Add("@DOB", MySqlDbType.DateTime).Value = data.DOB;
                    command.Parameters.Add("@EmailId", MySqlDbType.String).Value = data.EmailId;
                    command.Parameters.Add("@ResidentialAddress", MySqlDbType.String).Value = data.ResidentialAddress;
                    command.Parameters.Add("@PermanentAddress", MySqlDbType.String).Value = data.PermanentAddress;
                    command.Parameters.Add("@ContactNo", MySqlDbType.String).Value = data.ContactNo;
                    command.Parameters.Add("@AltContactNo", MySqlDbType.String).Value = data.AltContactNo;
                    command.Parameters.Add("@BloodGroup", MySqlDbType.String).Value = data.BloodGroup;
                    command.Parameters.Add("@UserName", MySqlDbType.String).Value = data.UserName;
                    command.Parameters.Add("@UserPassword", MySqlDbType.String).Value = data.Password;
                    command.Parameters.Add("@RoleId", MySqlDbType.Int32).Value = data.Role.RoleId;
                    command.Parameters.Add("@LocationId", MySqlDbType.Int32).Value = data.Location.LocationId;

                    if (data.Student != null)
                    {
                        command.Parameters.Add("@RollNumber", MySqlDbType.String).Value = data.Student.RollNumber;
                        command.Parameters.Add("@RegistrationNumber", MySqlDbType.String).Value = data.Student.RegistrationNumber;
                        command.Parameters.Add("@AdmissionDate", MySqlDbType.DateTime).Value = data.Student.AdmissionDate;
                        command.Parameters.Add("@GuardianContactNo", MySqlDbType.String).Value = data.Student.GuardianContact;
                        command.Parameters.Add("@GuardianName", MySqlDbType.String).Value = data.Student.GuardianName;
                        command.Parameters.Add("@GuardianEmailId", MySqlDbType.String).Value = data.Student.GuardianEmailId;
                        command.Parameters.Add("@StandardSectionId", MySqlDbType.Int32).Value = data.Student.StandardSectionMap.StandardSectionId;
                        command.Parameters.Add("@HouseTypeId", MySqlDbType.Int32).Value = data.Student.HouseType.HouseTypeId;
                    }
                    else
                    {
                        command.Parameters.Add("@RollNumber", MySqlDbType.String).Value = DBNull.Value;
                        command.Parameters.Add("@RegistrationNumber", MySqlDbType.String).Value = DBNull.Value;
                        command.Parameters.Add("@AdmissionDate", MySqlDbType.DateTime).Value = DBNull.Value;
                        command.Parameters.Add("@GuardianContactNo", MySqlDbType.String).Value = DBNull.Value;
                        command.Parameters.Add("@GuardianName", MySqlDbType.String).Value = DBNull.Value;
                        command.Parameters.Add("@GuardianEmailId", MySqlDbType.String).Value = DBNull.Value;
                        command.Parameters.Add("@StandardSectionId", MySqlDbType.Int32).Value = DBNull.Value;
                        command.Parameters.Add("@HouseTypeId", MySqlDbType.Int32).Value = DBNull.Value;
                    }

                    if (data.Employee != null)
                    {
                        command.Parameters.Add("@StaffEmployeeId", MySqlDbType.String).Value = data.Employee.StaffEmployeeId;
                        command.Parameters.Add("@EducationQualification", MySqlDbType.String).Value = data.Employee.EducationalQualification;
                        command.Parameters.Add("@DateOfJoining", MySqlDbType.DateTime).Value = data.Employee.DateOfJoining;
                        command.Parameters.Add("@DepartmentId", MySqlDbType.Int32).Value = data.Employee.Department.DepartmentId;
                        command.Parameters.Add("@DesignationId", MySqlDbType.Int32).Value = data.Employee.Designation.DesignationId;
                    }
                    else
                    {
                        command.Parameters.Add("@StaffEmployeeId", MySqlDbType.String).Value = DBNull.Value;
                        command.Parameters.Add("@EducationQualification", MySqlDbType.String).Value = DBNull.Value;
                        command.Parameters.Add("@DateOfJoining", MySqlDbType.DateTime).Value = DBNull.Value;
                        command.Parameters.Add("@DepartmentId", MySqlDbType.Int32).Value = DBNull.Value;
                        command.Parameters.Add("@DesignationId", MySqlDbType.Int32).Value = DBNull.Value;
                    }
                    command.ExecuteNonQuery();
                    StatusDTO<UserMasterDTO> status = new StatusDTO<UserMasterDTO>();
                    return status;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }
    }
}
