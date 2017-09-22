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
        private ILogSvc _logger;

        public UserSvc(IConfigSvc configSvc, ILogSvc logger)
        {
            _configSvc = configSvc;
            _logger = logger;
        }

        //public StatusDTO<UserMasterDTO> Delete(UserMasterDTO data)
        //{
        //    throw new NotImplementedException();
        //}

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
                    //command.Parameters.Add("@RoleId", MySqlDbType.Int32).Value = data.Role.RoleId;
                    command.Parameters.Add("@LocationId", MySqlDbType.Int32).Value = data.Location.LocationId;

                    command.Parameters.Add("@StaffEmployeeId", MySqlDbType.String).Value = data.Employee.StaffEmployeeId;
                    command.Parameters.Add("@EducationQualification", MySqlDbType.String).Value = data.Employee.EducationalQualification;
                    command.Parameters.Add("@DateOfJoining", MySqlDbType.DateTime).Value = data.Employee.DateOfJoining;
                    command.Parameters.Add("@DepartmentId", MySqlDbType.Int32).Value = data.Employee.Department.DepartmentId;
                    if(data.Employee.Department.DepartmentId == 1 && data.Employee.ClassType.ClassTypeId > 0)
                    {
                        command.Parameters.Add("@ClassTypeId", MySqlDbType.Int32).Value = data.Employee.ClassType.ClassTypeId;
                        //if(data.Employee.ClassType.ClassTypeId > 2)
                        //{
                        //    command.Parameters.Add("@SubjectId", MySqlDbType.Int32).Value = data.Employee.Subject.SubjectId;
                        //}
                        //else
                        //{
                        //    command.Parameters.Add("@SubjectId", MySqlDbType.Int32).Value = DBNull.Value;
                        //}
                    }
                    else
                    {
                        command.Parameters.Add("@ClassTypeId", MySqlDbType.Int32).Value = DBNull.Value;
                        //command.Parameters.Add("@SubjectId", MySqlDbType.Int32).Value = DBNull.Value;
                    }
                    command.Parameters.Add("@DesignationId", MySqlDbType.Int32).Value = data.Employee.Designation.DesignationId;
                    
                    MySqlDataReader rdr = command.ExecuteReader(CommandBehavior.CloseConnection);
                    _dtData = new DataTable();
                    _dtData.Load(rdr);
                    StatusDTO<UserMasterDTO> status = new StatusDTO<UserMasterDTO>();

                    if (_dtData.Rows.Count > 0 && !String.IsNullOrEmpty(_dtData.Rows[0]["Message"].ToString()))
                    {
                        status.IsSuccess = true;
                        status.ReturnObj = new UserMasterDTO();
                        status.ReturnObj.UserMasterId = (int)_dtData.Rows[0]["Message"];

                        if(!String.IsNullOrEmpty(_dtData.Rows[0]["Message1"].ToString()))
                        {
                            status.ReturnObj.Employee = new EmployeeDetailsDTO();
                            status.ReturnObj.Employee.EmployeeId = (int)_dtData.Rows[0]["Message1"];
                        }
                    }
                    else
                    {
                        status.IsSuccess = false;
                        status.FailureReason = "User Insertion Failed";
                    }
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
                            userMaster.UserName = _dsData.Tables[0].Rows[0]["UserName"].ToString();
                            userMaster.FName = _dsData.Tables[0].Rows[0]["FName"].ToString();
                            userMaster.MName = _dsData.Tables[0].Rows[0]["MName"].ToString();
                            userMaster.LName = _dsData.Tables[0].Rows[0]["LName"].ToString();
                            userMaster.StudentCount = Convert.ToInt32(_dsData.Tables[0].Rows[0]["studentcount"].ToString());
                            userMaster.StaffCount = Convert.ToInt32(_dsData.Tables[0].Rows[0]["staffcount"].ToString());
                            userMaster.PaidStudentCount = Convert.ToInt32(_dsData.Tables[0].Rows[0]["paidstudentcount"].ToString());
                            userMaster.PendingPaymentCount = Convert.ToInt32(_dsData.Tables[0].Rows[0]["pendingpaymentcount"].ToString());
                            if(string.IsNullOrEmpty(_dsData.Tables[0].Rows[0]["RegistrationNumber"].ToString()) && !string.IsNullOrEmpty(_dsData.Tables[0].Rows[0]["StaffEmployeeId"].ToString()))
                            {
                                userMaster.UniqueId = _dsData.Tables[0].Rows[0]["StaffEmployeeId"].ToString();
                                userMaster.UserType = "STAFF";
                            }
                            if(!string.IsNullOrEmpty(_dsData.Tables[0].Rows[0]["RegistrationNumber"].ToString()) && string.IsNullOrEmpty(_dsData.Tables[0].Rows[0]["StaffEmployeeId"].ToString()))
                            {
                                userMaster.UniqueId = _dsData.Tables[0].Rows[0]["RegistrationNumber"].ToString();
                                userMaster.UserType = "STUDENT";
                            }
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
                            ActionDTO useractionDTO = null;
                            for (int i = 0; i < _dsData.Tables[2].Rows.Count; i++)
                            {
                                useractionDTO = new ActionDTO();
                                useractionDTO.ActionName = _dsData.Tables[2].Rows[i]["ActionName"].ToString();
                                useractionDTO.ActionLink = _dsData.Tables[2].Rows[i]["ActionLink"].ToString();
                                useractionDTO.IsChildAction = Convert.ToBoolean(_dsData.Tables[2].Rows[i]["IsChildAction"]);
                                useractionDTO.MenuText = _dsData.Tables[2].Rows[i]["MenuText"].ToString();
                                useractionDTO.MenuImage = _dsData.Tables[2].Rows[i]["MenuImage"].ToString();
                                useractionDTO.DisabledControlId = _dsData.Tables[2].Rows[i]["DisabledControlId"].ToString();
                                useractionDTO.HiddenControlId = _dsData.Tables[2].Rows[i]["HiddenControlId"].ToString();
                                useractionDTO.GroupName = _dsData.Tables[2].Rows[i]["GroupName"].ToString();

                                if(useractionDTO.IsChildAction || !string.IsNullOrEmpty(_dsData.Tables[2].Rows[i]["ParentActionId"].ToString()))
                                {
                                    useractionDTO.ParentAction = new ActionDTO();
                                    useractionDTO.ParentAction.RowId = Convert.ToInt32(_dsData.Tables[2].Rows[i]["ParentActionId"]);
                                    useractionDTO.ParentAction.ActionLink = _dsData.Tables[2].Select("ActionId=" + useractionDTO.ParentAction.RowId)[0]["ActionLink"].ToString();
                                }

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

                            usermasterDTO.Employee = new EmployeeDetailsDTO();
                            usermasterDTO.Employee.EmployeeId = Convert.ToInt32(_dsData.Tables[0].Rows[0]["EmployeeId"]);
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
                            if(!string.IsNullOrEmpty(_dsData.Tables[0].Rows[0]["DepartmentId"].ToString()))
                            {
                                usermasterDTO.Employee.Department.DepartmentId = Convert.ToInt32(_dsData.Tables[0].Rows[0]["DepartmentId"]);
                            }
                            usermasterDTO.Employee.StaffEmployeeId = _dsData.Tables[0].Rows[0]["StaffEmployeeId"].ToString();
                            usermasterDTO.Employee.Designation = new DesignationDTO();
                            if(!string.IsNullOrEmpty(_dsData.Tables[0].Rows[0]["DepartmentId"].ToString()))
                            {
                                usermasterDTO.Employee.Designation.DesignationId = Convert.ToInt32(_dsData.Tables[0].Rows[0]["DepartmentId"]);
                            }

                            usermasterDTO.Employee.ClassType = new ClassTypeDTO();
                            //usermasterDTO.Employee.Subject = new SubjectDTO();
                            if (!string.IsNullOrEmpty(_dsData.Tables[0].Rows[0]["ClassTypeId"].ToString()) && Convert.ToInt32(_dsData.Tables[0].Rows[0]["ClassTypeId"]) > 0)
                            {
                                usermasterDTO.Employee.ClassType.ClassTypeId = Convert.ToInt32(_dsData.Tables[0].Rows[0]["ClassTypeId"]);
                                //if(Convert.ToInt32(_dsData.Tables[0].Rows[0]["SubjectId"]) > 0)
                                //{
                                //    usermasterDTO.Employee.Subject.SubjectId = Convert.ToInt32(_dsData.Tables[0].Rows[0]["SubjectId"]);
                                //}
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
            StatusDTO<List<UserMasterDTO>> userList = new StatusDTO<List<UserMasterDTO>>();

            string whereClause = null;
            string selectClause = null;
            DataSet dsUserLst = null;

            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();//openning the connection

                    MySqlCommand command = new MySqlCommand();// creating my sql command for queries

                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    selectClause = "SELECT users.UserMasterId,users.FName,users.MName,users.LName,users.Gender,users.EmailId,users.ResidentialAddress,users.PermanentAddress," +
                                  "users.ContactNo,users.AltContactNo,users.BloodGroup,r.RoleDescription" +
                                   " FROM usermaster users" +
                                   " INNER JOIN employeedetails emp ON emp.UserMasterId = users.UserMasterId" +
                                   " INNER JOIN roles r ON users.RoleId = r.RoleId AND users.RoleId>1";

                    whereClause = " WHERE users.Active = 1";

                    if (data != null)
                    {
                        //Name Search
                        if (!string.IsNullOrEmpty(data.FName))
                        {
                            data.FName = data.FName + "%";
                            whereClause = whereClause + " AND users.FName LIKE @FName";
                            command.Parameters.Add("@FName", MySqlDbType.String).Value = data.FName;
                        }

                        if (!string.IsNullOrEmpty(data.MName))
                        {
                            data.MName = data.MName + "%";
                            whereClause = whereClause + " AND users.MName LIKE @MName ";
                            command.Parameters.Add("@MName", MySqlDbType.String).Value = data.MName;
                        }

                        if (!string.IsNullOrEmpty(data.LName))
                        {
                            data.LName = data.LName + "%";
                            whereClause = whereClause + " AND users.LName LIKE @LName ";
                            command.Parameters.Add("@LName", MySqlDbType.String).Value = data.LName;
                        }

                        //Gender Search

                        if (!string.IsNullOrEmpty(data.Gender) && !string.Equals(data.Gender, "-1"))
                        {
                            whereClause = whereClause + " AND users.Gender=@Gender ";
                            command.Parameters.Add("@Gender", MySqlDbType.String).Value = data.Gender;
                        }

                        // Role Search
                        //if (!string.IsNullOrEmpty(data.Role.RoleDescription))
                        //{
                        //    whereClause = whereClause + " AND r.RoleDescription=@RoleDescription";
                        //    command.Parameters.Add("@RoleDescription", MySqlDbType.String).Value = data.Role.RoleDescription;
                        //}

                        //StaffEmpId Search
                        if (!string.IsNullOrEmpty(data.Employee.StaffEmployeeId))
                        {
                            whereClause = whereClause + " AND emp.StaffEmployeeId=@StaffEmployeeId";
                            command.Parameters.Add("@StaffEmployeeId", MySqlDbType.String).Value = data.Employee.StaffEmployeeId;
                        }


                        //BloodGroup Search
                        if (!string.IsNullOrEmpty(data.BloodGroup))
                        {
                            whereClause = whereClause + " AND users.BloodGroup=@BloodGroup";
                            command.Parameters.Add("@BloodGroup", MySqlDbType.String).Value = data.BloodGroup;
                        }
                    }

                    command.CommandText = selectClause + whereClause;

                    MySqlDataAdapter da = new MySqlDataAdapter(command);
                    dsUserLst = new DataSet();
                    da.Fill(dsUserLst);

                    if (dsUserLst != null && dsUserLst.Tables.Count > 0)
                    {
                        userList.ReturnObj = new List<UserMasterDTO>();
                        for (int i = 0; i < dsUserLst.Tables[0].Rows.Count; i++)
                        {
                            UserMasterDTO user = new UserMasterDTO();
                            user.UserMasterId = Convert.ToInt32(dsUserLst.Tables[0].Rows[i]["UserMasterId"]);
                            user.FName = dsUserLst.Tables[0].Rows[i]["FName"].ToString();
                            user.MName = dsUserLst.Tables[0].Rows[i]["MName"].ToString();
                            user.LName = dsUserLst.Tables[0].Rows[i]["LName"].ToString();
                            user.Gender = dsUserLst.Tables[0].Rows[i]["Gender"].ToString();
                            user.EmailId = dsUserLst.Tables[0].Rows[i]["EmailId"].ToString();
                            user.ResidentialAddress = dsUserLst.Tables[0].Rows[i]["ResidentialAddress"].ToString();
                            user.PermanentAddress = dsUserLst.Tables[0].Rows[i]["PermanentAddress"].ToString();
                            user.ContactNo = dsUserLst.Tables[0].Rows[i]["ContactNo"].ToString();
                            user.AltContactNo = dsUserLst.Tables[0].Rows[i]["AltContactNo"].ToString();
                            user.BloodGroup = dsUserLst.Tables[0].Rows[i]["BloodGroup"].ToString();

                            user.Role = new RoleDTO();
                            user.Role.RoleDescription = dsUserLst.Tables[0].Rows[i]["RoleDescription"].ToString();
                            userList.ReturnObj.Add(user);
                            userList.IsSuccess = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Log(ex);
                    userList.IsSuccess = false;
                    userList.IsException = true;
                    userList.ReturnObj = null;
                    userList.ExceptionMessage = ex.Message;
                    userList.StackTrace = ex.StackTrace;
                }
            }
            return userList;
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
                    //command.Parameters.Add("@RoleId", MySqlDbType.Int32).Value = data.Role.RoleId;
                    command.Parameters.Add("@LocationId", MySqlDbType.Int32).Value = data.Location.LocationId;

                    command.Parameters.Add("@StaffEmployeeId", MySqlDbType.String).Value = data.Employee.StaffEmployeeId;
                    command.Parameters.Add("@EducationQualification", MySqlDbType.String).Value = data.Employee.EducationalQualification;
                    command.Parameters.Add("@DateOfJoining", MySqlDbType.DateTime).Value = data.Employee.DateOfJoining;
                    command.Parameters.Add("@DepartmentId", MySqlDbType.Int32).Value = data.Employee.Department.DepartmentId;
                    if (data.Employee.Department.DepartmentId == 1)
                    {
                        if (data.Employee.ClassType.ClassTypeId > 0)
                        {
                            command.Parameters.Add("@ClassTypeId", MySqlDbType.Int32).Value = data.Employee.ClassType.ClassTypeId;
                        }
                        else
                        {
                            command.Parameters.Add("@ClassTypeId", MySqlDbType.Int32).Value = DBNull.Value;
                        }
                        //if (data.Employee.ClassType.ClassTypeId > 2)
                        //{
                        //    command.Parameters.Add("@SubjectId", MySqlDbType.Int32).Value = data.Employee.Subject.SubjectId;
                        //}
                        //else
                        //{
                        //    command.Parameters.Add("@SubjectId", MySqlDbType.Int32).Value = DBNull.Value;
                        //}
                    }
                    else
                    {
                        command.Parameters.Add("@ClassTypeId", MySqlDbType.Int32).Value = DBNull.Value;
                        //command.Parameters.Add("@SubjectId", MySqlDbType.Int32).Value = DBNull.Value;
                    }
                    command.Parameters.Add("@DesignationId", MySqlDbType.Int32).Value = data.Employee.Designation.DesignationId;

                    command.ExecuteNonQuery();
                    StatusDTO<UserMasterDTO> status = new StatusDTO<UserMasterDTO>();
                    status.IsSuccess = true;
                    return status;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        //To get user entitlementlist
        public List<UserEntitlementDTO> GetUserEntitlement(int userMasterId)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "SELECT UserEntitleMentId, UserMasterId, UserRoleId FROM userentitlement WHERE UserMasterId=@umId";
                    command.Parameters.Add("@umId", MySqlDbType.Int32).Value = userMasterId;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    MySqlDataAdapter mDa = new MySqlDataAdapter(command);
                    _dtData = new DataTable();
                    mDa.Fill(_dtData);
                    List<UserEntitlementDTO> lstUEnt = null;
                    if (_dtData != null && _dtData.Rows.Count > 0)
                    {
                        lstUEnt = new List<UserEntitlementDTO>();
                        UserEntitlementDTO uEntDTO = null;
                        foreach (DataRow dr in _dtData.Rows)
                        {
                            uEntDTO = new UserEntitlementDTO();
                            uEntDTO.UserDetails = new UserMasterDTO();
                            uEntDTO.RoleDetails = new EntitlementDTO();

                            uEntDTO.RowId = (int)dr["UserEntitleMentId"];
                            uEntDTO.UserDetails.UserMasterId = (int)dr["UserMasterId"];
                            uEntDTO.RoleDetails.UserRoleId = (int)dr["UserRoleId"];

                            lstUEnt.Add(uEntDTO);
                        }
                    }
                    return lstUEnt;
                }
                catch (Exception exp)
                {
                    _logger.Log(exp);
                    throw exp;
                }
            }
        }

        public StatusDTO<UserEntitlementDTO> InsertUserEntitlement(UserEntitlementDTO data)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "UserEntitlementMap";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    command.Parameters.Add("@UserMasterId1", MySqlDbType.String).Value = data.UserDetails.UserMasterId;
                    command.Parameters.Add("@UserRoleId1", MySqlDbType.String).Value = data.RoleDetails.UserRoleId;

                    MySqlDataReader rdr = command.ExecuteReader(CommandBehavior.CloseConnection);
                    _dtData = new DataTable();
                    _dtData.Load(rdr);
                    StatusDTO<UserEntitlementDTO> status = new StatusDTO<UserEntitlementDTO>();
                    return status;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public StatusDTO<UserEntitlementDTO> DeleteUserEntitlement(UserEntitlementDTO data)
        {
            StatusDTO<UserEntitlementDTO> status = new StatusDTO<UserEntitlementDTO>();
            status.IsSuccess = false;
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "DELETE FROM userentitlement WHERE UserEntitleMentId=@UserEntitleMentId";
                    command.Parameters.Add("@UserEntitleMentId", MySqlDbType.Int32).Value = data.RowId;

                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    if (command.ExecuteNonQuery() > 0)
                    {
                        status.IsSuccess = true;
                    }
                    return status;
                }
                catch (Exception exp)
                {
                    _logger.Log(exp);
                    throw exp;
                }
            }
        }

        public StatusDTO<UserEntitlementDTO> UpdateUserEntitlement(UserEntitlementDTO data)
        {
            StatusDTO<UserEntitlementDTO> status = new StatusDTO<UserEntitlementDTO>();
            status.IsSuccess = false;
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "UPDATE userentitlement set UserRoleId=@UserRoleId1 WHERE UserEntitleMentId=@UserEntitleMentId";
                    command.Parameters.Add("@UserEntitleMentId", MySqlDbType.Int32).Value = data.RowId;
                    command.Parameters.Add("@UserRoleId1", MySqlDbType.Int32).Value = data.RoleDetails.UserRoleId;

                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    if (command.ExecuteNonQuery() > 0)
                    {
                        status.IsSuccess = true;
                    }
                    return status;
                }
                catch (Exception exp)
                {
                    _logger.Log(exp);
                    throw exp;
                }
            }
        }
        public StatusDTO<UserMasterDTO> Delete(UserMasterDTO user)
        {
            StatusDTO<UserMasterDTO> status = null;
            try
            {
                if (user != null && user.UserMasterId != 0)
                {
                    using (IDbSvc dbSvc = new DbSvc(_configSvc))
                    {
                        dbSvc.OpenConnection();
                        MySqlCommand command = new MySqlCommand();
                        command.Connection = dbSvc.GetConnection() as MySqlConnection;
                        command.CommandText = "UPDATE usermaster SET Active=0 WHERE UserMasterId=@UserMasterId";
                        command.Parameters.Add("@UserMasterId", MySqlDbType.Int32).Value = user.UserMasterId;

                        if(command.ExecuteNonQuery()>0)
                        {
                            status = new StatusDTO<UserMasterDTO>();
                            status.IsSuccess = true;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.Log(ex);
                throw ex;
            }
            return status;
        }

        //to get Faculty Course map
        public List<FacultyCourseMapDTO> GetFacultyCourseMap(int employeeId)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "SELECT FacultyCourseMapId, EmployeeId, SubjectId FROM facultycoursemap WHERE EmployeeId=@employeeId";
                    command.Parameters.Add("@employeeId", MySqlDbType.Int32).Value = employeeId;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    MySqlDataAdapter mDa = new MySqlDataAdapter(command);
                    _dtData = new DataTable();
                    mDa.Fill(_dtData);
                    List<FacultyCourseMapDTO> lstFCMap = null;
                    if (_dtData != null && _dtData.Rows.Count > 0)
                    {
                        lstFCMap = new List<FacultyCourseMapDTO>();
                        FacultyCourseMapDTO fcmpDTO = null;
                        foreach (DataRow dr in _dtData.Rows)
                        {
                            fcmpDTO = new FacultyCourseMapDTO();
                            fcmpDTO.Employee = new EmployeeDetailsDTO();
                            fcmpDTO.Subject = new SubjectDTO();

                            fcmpDTO.FacultyCourseMapId = (int)dr["FacultyCourseMapId"];
                            fcmpDTO.Employee.EmployeeId = (int)dr["EmployeeId"];
                            fcmpDTO.Subject.SubjectId = (int)dr["SubjectId"];

                            lstFCMap.Add(fcmpDTO);
                        }
                    }
                    return lstFCMap;
                }
                catch (Exception exp)
                {
                    _logger.Log(exp);
                    throw exp;
                }
            }
        }

        public StatusDTO<FacultyCourseMapDTO> DeleteFacultyCourseMap(FacultyCourseMapDTO data)
        {
            StatusDTO<FacultyCourseMapDTO> status = new StatusDTO<FacultyCourseMapDTO>();
            status.IsSuccess = false;
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "DELETE FROM facultycoursemap WHERE FacultyCourseMapId=@FacultyCourseMapId";
                    command.Parameters.Add("@FacultyCourseMapId", MySqlDbType.Int32).Value = data.FacultyCourseMapId;

                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    if (command.ExecuteNonQuery() > 0)
                    {
                        status.IsSuccess = true;
                    }
                    return status;
                }
                catch (Exception exp)
                {
                    _logger.Log(exp);
                    throw exp;
                }
            }
        }

        public StatusDTO<FacultyCourseMapDTO> InsertFacultyCourse(FacultyCourseMapDTO data)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "Teacher_Course_Map";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    command.Parameters.Add("@EmployeeId", MySqlDbType.String).Value = data.Employee.EmployeeId;
                    command.Parameters.Add("@SubjectId", MySqlDbType.String).Value = data.Subject.SubjectId;
                    //if(Convert.ToInt32(data.CreatedBy.UserMasterId) > 0)
                    //{
                    //    command.Parameters.Add("@CreatedBy", MySqlDbType.String).Value = data.CreatedBy.UserMasterId;
                    //}
                    //else
                    //{
                        command.Parameters.Add("@CreatedBy", MySqlDbType.String).Value = DBNull.Value;
                    //}

                    MySqlDataReader rdr = command.ExecuteReader(CommandBehavior.CloseConnection);
                    _dtData = new DataTable();
                    _dtData.Load(rdr);
                    StatusDTO<FacultyCourseMapDTO> status = new StatusDTO<FacultyCourseMapDTO>();
                    return status;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public StatusDTO<FacultyCourseMapDTO> UpdateFacultyCourseMap(FacultyCourseMapDTO data)
        {
            StatusDTO<FacultyCourseMapDTO> status = new StatusDTO<FacultyCourseMapDTO>();
            status.IsSuccess = false;
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "UPDATE facultycoursemap set SubjectId=@SubjectId WHERE FacultyCourseMapId=@FacultyCourseMapId";
                    command.Parameters.Add("@FacultyCourseMapId", MySqlDbType.Int32).Value = data.FacultyCourseMapId;
                    command.Parameters.Add("@SubjectId", MySqlDbType.Int32).Value = data.Subject.SubjectId;

                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    if (command.ExecuteNonQuery() > 0)
                    {
                        status.IsSuccess = true;
                    }
                    return status;
                }
                catch (Exception exp)
                {
                    _logger.Log(exp);
                    throw exp;
                }
            }
        }
    }
}
