using OpMgr.Common.Contracts.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpMgr.Common.DTOs;
using OpMgr.Common.Contracts;
using System.Data;
using MySql.Data.MySqlClient;

namespace OpMgr.DataAccess.Implementations
{
    public class PMSSvc : IPMSSvc
    {
        private IConfigSvc _configSvc;
        private DataTable _dtData;
        private DataSet _dsData;
        private ILogSvc _logger;
        private ISessionSvc _sessionSvc;

        public PMSSvc(IConfigSvc configSvc, ILogSvc logger, ISessionSvc sessionSvc)
        {
            _configSvc = configSvc;
            _logger = logger;
            _sessionSvc = sessionSvc;
        }


        public StatusDTO<EmployeeGoalLogDTO> Delete(EmployeeGoalLogDTO data)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<EmployeeGoalLogDTO> Insert(EmployeeGoalLogDTO data)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "insert_AppraisalData";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    command.Parameters.Add("@EmployeeAppraisalMasterId1", MySqlDbType.Int32).Value = data.EmployeeAppraisalMaster.EmployeeAppraisalMasterId;
                    command.Parameters.Add("@GoalAttributeId1", MySqlDbType.Int32).Value = data.GoalAttribute.GoalAttributeId;
                    command.Parameters.Add("@Achievement1", MySqlDbType.Double).Value = data.Achievement;
                    command.Parameters.Add("@CreatedBy1", MySqlDbType.Int32).Value = _sessionSvc.GetUserSession().UserMasterId;
                    // add createdby from session


                    MySqlDataReader rdr = command.ExecuteReader(CommandBehavior.CloseConnection);
                    _dtData = new DataTable();
                    _dtData.Load(rdr);
                    StatusDTO<EmployeeGoalLogDTO> status = new StatusDTO<EmployeeGoalLogDTO>();

                    if (_dtData.Rows.Count > 0)
                    {
                        status.IsSuccess = true;
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
        // to get Level of Rating by passing Percentage of rating input
        public StatusDTO<EmployeeGoalLogDTO> getSelfRating(int rowId)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "get_RatingLevels";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    command.Parameters.Add("@Rating", MySqlDbType.Int32).Value = rowId;

                    MySqlDataAdapter rdr = new MySqlDataAdapter(command);
                    _dsData = new DataSet();
                    rdr.Fill(_dsData);
                    StatusDTO<EmployeeGoalLogDTO> status = new StatusDTO<EmployeeGoalLogDTO>();
                    EmployeeGoalLogDTO empGoalDTO = new EmployeeGoalLogDTO();
                    if (_dsData != null && _dsData.Tables.Count > 0)
                    {
                        if (_dsData.Tables[0].Rows.Count > 0)
                        {
                            empGoalDTO.SelfRating = _dsData.Tables[0].Rows[0]["LevelName"].ToString();

                        }
                    }
                    status.ReturnObj = empGoalDTO;
                    return status;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public StatusDTO<List<EmployeeGoalLogDTO>> Select(EmployeeGoalLogDTO data)
        {
            StatusDTO<List<EmployeeGoalLogDTO>> goalList = new StatusDTO<List<EmployeeGoalLogDTO>>();
            DataSet dsGoalLst = null;
            int empAppraisalMasterId = -1;
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "get_AppraisalData";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    command.Parameters.Add("@DesignationId1", MySqlDbType.Int32).Value = data.EmployeeAppraisalMaster.Employee.Designation.DesignationId;
                    command.Parameters.Add("@EmployeeId1", MySqlDbType.Int32).Value = data.EmployeeAppraisalMaster.Employee.EmployeeId;


                    MySqlDataAdapter da = new MySqlDataAdapter(command);
                    dsGoalLst = new DataSet();
                    da.Fill(dsGoalLst);

                    StatusDTO<EmployeeGoalLogDTO> status = new StatusDTO<EmployeeGoalLogDTO>();
                    EmployeeGoalLogDTO empGoalLogDTO = new EmployeeGoalLogDTO();
                    if (dsGoalLst != null && dsGoalLst.Tables.Count == 2)
                    {
                        goalList.ReturnObj = new List<EmployeeGoalLogDTO>();
                        if (dsGoalLst.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsGoalLst.Tables[0].Rows.Count; i++)
                            {
                                EmployeeGoalLogDTO empgoallog = new EmployeeGoalLogDTO();
                                empgoallog.EmployeeAppraisalMaster = new EmployeeAppraisalMasterDTO();
                                empAppraisalMasterId = Convert.ToInt32(dsGoalLst.Tables[0].Rows[i]["employeeappraisalmasterid"]);
                                empgoallog.GoalAttribute = new GoalAttributeDTO();
                                empgoallog.GoalAttribute.Goal = new GoalDTO();
                                empgoallog.GoalAttribute.GoalAttributeId = Convert.ToInt32(dsGoalLst.Tables[0].Rows[i]["goalattributeid"]);
                                empgoallog.GoalAttribute.Goal.GoalId = Convert.ToInt32(dsGoalLst.Tables[0].Rows[i]["GoalId"]);
                                empgoallog.GoalAttribute.Goal.GoalDescription = dsGoalLst.Tables[0].Rows[i]["GoalDescription"].ToString();
                                empgoallog.GoalAttribute.Target = dsGoalLst.Tables[0].Rows[i]["Target"].ToString();
                                empgoallog.GoalAttribute.KRA = dsGoalLst.Tables[0].Rows[i]["KRA"].ToString();
                                empgoallog.GoalAttribute.KPI = dsGoalLst.Tables[0].Rows[i]["KPI"].ToString();
                                empgoallog.GoalAttribute.WeightAge = Convert.ToInt32(dsGoalLst.Tables[0].Rows[i]["WeightAge"]);
                                empgoallog.NeedsUpdate = "Y";
                                if (!string.IsNullOrEmpty(dsGoalLst.Tables[0].Rows[i]["Achievement"].ToString()))
                                {
                                    empgoallog.Achievement = Convert.ToDecimal(dsGoalLst.Tables[0].Rows[i]["Achievement"]);
                                    empgoallog.AchivementPercent = Convert.ToInt32((empgoallog.Achievement / empgoallog.GoalAttribute.WeightAge) * 100);
                                    empgoallog.SelfRating = this.getSelfRating(empgoallog.AchivementPercent).ReturnObj.SelfRating;
                                }
                                if (!string.IsNullOrEmpty(dsGoalLst.Tables[0].Rows[i]["AppriaserRating"].ToString()))
                                {
                                    empgoallog.AppraiserRating = Convert.ToDecimal(dsGoalLst.Tables[0].Rows[i]["AppriaserRating"]);
                                }
                                if(!string.IsNullOrEmpty(dsGoalLst.Tables[0].Rows[i]["EmployeeGoalLogId"].ToString()))
                                {
                                    empgoallog.EmployeeGoalLogId = Convert.ToInt32(dsGoalLst.Tables[0].Rows[i]["EmployeeGoalLogId"]);
                                    empgoallog.NeedsUpdate = "N";
                                }
                                else
                                {
                                    empgoallog.EmployeeGoalLogId = -1;
                                }
                                goalList.ReturnObj.Add(empgoallog);
                            }
                        }

                        if (dsGoalLst.Tables[1].Rows.Count > 0)
                        {
                            goalList.ReturnObj[0].EmployeeAppraisalMaster = new EmployeeAppraisalMasterDTO();
                            goalList.ReturnObj[0].EmployeeAppraisalMaster.EmployeeAppraisalMasterId = empAppraisalMasterId;
                            EmployeeDetailsDTO empdetails = new EmployeeDetailsDTO();
                            empdetails.UserDetails = new UserMasterDTO();
                            empdetails.UserDetails.Location = new LocationDTO();
                            empdetails.Designation = new DesignationDTO();
                            empdetails.UserDetails.FName = dsGoalLst.Tables[1].Rows[0]["FName"].ToString();
                            empdetails.UserDetails.MName = dsGoalLst.Tables[1].Rows[0]["MName"].ToString();
                            empdetails.UserDetails.LName = dsGoalLst.Tables[1].Rows[0]["LName"].ToString();
                            empdetails.Designation.DesignationDescription = dsGoalLst.Tables[1].Rows[0]["DesignationDescription"].ToString();
                            empdetails.UserDetails.Location.LocationDescription = dsGoalLst.Tables[1].Rows[0]["LocationDescription"].ToString();
                            empdetails.EducationalQualification = dsGoalLst.Tables[1].Rows[0]["EducationQualification"].ToString();

                            goalList.ReturnObj[0].EmployeeAppraisalMaster.Employee = empdetails;

                        }

                    }

                    return goalList;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }


        public bool MoveFwdBckwd(int appraisalMasterId, int currentStatus, bool isBackwd=false)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "update employeeappraisalmaster set AppraisalStatusId=@status, UpdatedBy=@updatedBy, UpdatedDate=CURDATE(), Active=1 where EmployeeAppraisalMasterId=@empApprMaster";
                    command.CommandType = CommandType.Text;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    command.Parameters.Add("@status", MySqlDbType.Double).Value = isBackwd?currentStatus-1:currentStatus+1;
                    command.Parameters.Add("@updatedBy", MySqlDbType.Int32).Value = _sessionSvc.GetUserSession().UserMasterId;
                    command.Parameters.Add("@empApprMaster", MySqlDbType.Int32).Value = appraisalMasterId;
                    // add createdby from session

                    StatusDTO<EmployeeGoalLogDTO> status = new StatusDTO<EmployeeGoalLogDTO>();

                    if (command.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                    return false;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public int GetCurrentStatus(int empApprMasterId)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "SELECT AppraisalStatusId FROM employeeappraisalmaster WHERE EmployeeAppraisalMasterId=@empApprMaster";
                    command.Parameters.Add("@empApprMaster", MySqlDbType.Int32).Value = empApprMasterId;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    MySqlDataAdapter mDa = new MySqlDataAdapter(command);
                    DataTable dtCurrentStatus = new DataTable();
                    mDa.Fill(dtCurrentStatus);
                    return Convert.ToInt32(dtCurrentStatus.Rows[0]["AppraisalStatusId"]);
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public StatusDTO<EmployeeAppraisalMasterDTO> Update(EmployeeAppraisalMasterDTO data)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<EmployeeGoalLogDTO> Update(EmployeeGoalLogDTO data)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "update employeegoallog set Achievement=@achievement, UpdatedBy=@updatedBy, UpdatedDate=CURDATE(), Active=1 where EmployeeGoalLogId=@empGoalLogId";
                    command.CommandType = CommandType.Text;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    command.Parameters.Add("@achievement", MySqlDbType.Double).Value = data.Achievement;
                    command.Parameters.Add("@updatedBy", MySqlDbType.Int32).Value = _sessionSvc.GetUserSession().UserMasterId;
                    command.Parameters.Add("@empGoalLogId", MySqlDbType.Int32).Value = data.EmployeeGoalLogId;
                    // add createdby from session

                    StatusDTO<EmployeeGoalLogDTO> status = new StatusDTO<EmployeeGoalLogDTO>();

                    if (command.ExecuteNonQuery() > 0)
                    {
                        status.IsSuccess = true;
                        status.ReturnObj = data;
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

        public StatusDTO<EmployeeGoalLogDTO> Select(int rowId)
        {
            throw new NotImplementedException();
        }
    }
}
