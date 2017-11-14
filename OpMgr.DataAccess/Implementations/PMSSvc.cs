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
using System.Transactions;

namespace OpMgr.DataAccess.Implementations
{
    public class PMSSvc : IPMSSvc
    {
        private IConfigSvc _configSvc;
        private DataTable _dtData;
        private DataSet _dsData;
        private ILogSvc _logger;
        private ISessionSvc _sessionSvc;
        private DataTable _dtLevels;

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

        private void FillLevelTable()
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "select StartingRange,EndRange,LevelName,LevelDescription from weightage_levels";
                    command.CommandType = CommandType.Text;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    MySqlDataAdapter mDa = new MySqlDataAdapter(command);
                    _dtLevels = new DataTable();
                    mDa.Fill(_dtLevels);
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
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

        public string getRatingLevel(int percent)
        {
            string ratingLevel = string.Empty;
            if(_dtLevels==null)
            {
                this.FillLevelTable();
            }
            DataRow[] levels = _dtLevels.Select("StartingRange<=" + percent + " AND EndRange>=" + percent);
            if(levels!=null && levels.Length>0)
            {
                ratingLevel = levels[0]["LevelName"].ToString();
            }
            return ratingLevel;
        }

        public string AccessStatus(int appraisalMasterId)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                string accessStatus = string.Empty;
                try
                {                    
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "check_AppraisalStatus";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    command.Parameters.Add("@AppraisalMasterId", MySqlDbType.Int32).Value = appraisalMasterId;
                    command.Parameters.Add("@CurrentEmpId", MySqlDbType.Int32).Value = _sessionSvc.GetUserSession().UniqueEmployeeId;
                    MySqlDataAdapter rdr = new MySqlDataAdapter(command);
                    _dsData = new DataSet();
                    rdr.Fill(_dsData);
                    if (_dsData != null && _dsData.Tables.Count > 0)
                    {
                        if (_dsData.Tables[0].Rows.Count > 0)
                        {
                            accessStatus = _dsData.Tables[0].Rows[0]["AccessStatus"].ToString();

                        }
                    }
                    
                    return accessStatus;
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
                    if(data.EmployeeAppraisalMaster.EmployeeAppraisalMasterId==-1)
                    {
                        command.Parameters.Add("@ApprMasterIdParam", MySqlDbType.Int32).Value = DBNull.Value;
                    }
                    else
                    {
                        command.Parameters.Add("@ApprMasterIdParam", MySqlDbType.Int32).Value = data.EmployeeAppraisalMaster.EmployeeAppraisalMasterId;
                    }

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
                                if (!string.IsNullOrEmpty(dsGoalLst.Tables[0].Rows[i]["EmployeeGoalLogId"].ToString()))
                                {
                                    empgoallog.EmployeeGoalLogId = Convert.ToInt32(dsGoalLst.Tables[0].Rows[i]["EmployeeGoalLogId"]);
                                    empgoallog.NeedsUpdate = "N";
                                }
                                else
                                {
                                    empgoallog.EmployeeGoalLogId = -1;
                                }
                                if (!string.IsNullOrEmpty(dsGoalLst.Tables[0].Rows[i]["Achievement"].ToString()))
                                {
                                    empgoallog.Achievement = Convert.ToDecimal(dsGoalLst.Tables[0].Rows[i]["Achievement"]);
                                    empgoallog.AchivementPercent = Convert.ToInt32((empgoallog.Achievement / empgoallog.GoalAttribute.WeightAge) * 100);
                                    empgoallog.SelfRating = this.getRatingLevel(empgoallog.AchivementPercent);
                                    
                                }
                                if (!string.IsNullOrEmpty(dsGoalLst.Tables[0].Rows[i]["AppriaserRating"].ToString()))
                                {
                                    empgoallog.AppraiserRating = Convert.ToDecimal(dsGoalLst.Tables[0].Rows[i]["AppriaserRating"]);
                                    empgoallog.AppraiserRatingPercent = Convert.ToInt32((empgoallog.AppraiserRating / empgoallog.GoalAttribute.WeightAge) * 100);
                                    empgoallog.AppraiserRatingLevel = this.getRatingLevel(empgoallog.AppraiserRatingPercent);
                                    empgoallog.NeedsAppraiserUpdate = "N";
                                }
                                else
                                {
                                    empgoallog.NeedsAppraiserUpdate = "Y";
                                }
                                if (!string.IsNullOrEmpty(dsGoalLst.Tables[0].Rows[i]["FinalRating"].ToString()))
                                {
                                    empgoallog.EmployeeAppraisalMaster.ReviewerFinalRating = Convert.ToDecimal(dsGoalLst.Tables[0].Rows[i]["FinalRating"]);
                                }
                                goalList.ReturnObj.Add(empgoallog);
                            }
                        }

                        if (dsGoalLst.Tables[1].Rows.Count > 0)
                        {
                            if(goalList.ReturnObj!=null && goalList.ReturnObj.Count>0)
                            {
                                //goalList.ReturnObj[0].EmployeeAppraisalMaster = new EmployeeAppraisalMasterDTO();
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
                    command.CommandText = "update employeegoallog set Achievement=@achievement, UpdatedBy=@updatedBy, UpdatedDate=CURDATE(), Active=1, EmployeeRatingDate=CURDATE() where EmployeeGoalLogId=@empGoalLogId";
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



        public StatusDTO<EmployeeGoalLogDTO> UpdateAppraiserRating(EmployeeGoalLogDTO data)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "update employeegoallog set AppriaserRating=@apprRating, UpdatedBy=@updatedBy, UpdatedDate=CURDATE(), Active=1, AppraiserRatingDate=CURDATE() where EmployeeGoalLogId=@empGoalLogId";
                    command.CommandType = CommandType.Text;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    command.Parameters.Add("@apprRating", MySqlDbType.Double).Value = data.AppraiserRating;
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

        public bool UpdateReviewerReview(int apprMasterId, decimal reviewerRating)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "update employeeappraisalmaster set ReviewerFinalRating=@rvwrRating, UpdatedBy=@updatedBy, UpdatedDate=CURDATE(), Active=1 where EmployeeAppraisalMasterId=@apprMaster";
                    command.CommandType = CommandType.Text;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    command.Parameters.Add("@rvwrRating", MySqlDbType.Double).Value = reviewerRating;
                    command.Parameters.Add("@updatedBy", MySqlDbType.Int32).Value = _sessionSvc.GetUserSession().UserMasterId;
                    command.Parameters.Add("@apprMaster", MySqlDbType.Int32).Value = apprMasterId;
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

        public void SaveCompetency(int appraisalMasterId, string delimitedImprovements, string delimitedStrengths)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    using(TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                    {
                        try
                        {
                            command.CommandText = "delete from employeecompetency where EmployeeAppraisalMasterId=@apprMasterId";
                            command.CommandType = CommandType.Text;
                            command.Connection = dbSvc.GetConnection() as MySqlConnection;

                            command.Parameters.Add("@apprMasterId", MySqlDbType.Int32).Value = appraisalMasterId;

                            command.ExecuteNonQuery();

                            string[] arrImprovements = delimitedImprovements.Split(',');
                            if(arrImprovements!=null && arrImprovements.Length>0)
                            {
                                foreach(string improvements in arrImprovements)
                                {
                                    command = new MySqlCommand();
                                    command.CommandText = "insert into employeecompetency(EmployeeAppraisalMasterId,CompetencyId,Type) values (@apprMasterId,@compId,'IMPROVEMENTS')";
                                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                                    command.Parameters.Add("@apprMasterId", MySqlDbType.Int32).Value = appraisalMasterId;
                                    command.Parameters.Add("@compId", MySqlDbType.Int32).Value = Convert.ToInt32(improvements);
                                    command.ExecuteNonQuery();
                                }
                            }

                            string[] arrStrengths = delimitedStrengths.Split(',');
                            if (arrStrengths != null && arrStrengths.Length > 0)
                            {
                                foreach (string strengths in arrStrengths)
                                {
                                    command = new MySqlCommand();
                                    command.CommandText = "insert into employeecompetency(EmployeeAppraisalMasterId,CompetencyId,Type) values (@apprMasterId,@compId,'STRENGTHS')";
                                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                                    command.Parameters.Add("@apprMasterId", MySqlDbType.Int32).Value = appraisalMasterId;
                                    command.Parameters.Add("@compId", MySqlDbType.Int32).Value = Convert.ToInt32(strengths);
                                    command.ExecuteNonQuery();
                                }
                            }
                            ts.Complete();
                        }
                        catch(Exception exp)
                        {
                            throw exp;
                        }
                    }
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public void GetCompetencies(int appraisalMasterId, out List<KeyValuePair<string, string>> strengths, out List<KeyValuePair<string, string>> improvements)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "select ec.CompetencyId, c.CompetencyDescription, ec.Type from employeecompetency ec left join competency c on ec.CompetencyId=c.CompetencyId where EmployeeAppraisalMasterId=@apprMaster";
                    command.CommandType = CommandType.Text;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    command.Parameters.Add("@apprMaster", MySqlDbType.Double).Value = appraisalMasterId;

                    DataTable dt = new DataTable();
                    MySqlDataAdapter mDa = new MySqlDataAdapter(command);
                    mDa.Fill(dt);
                    strengths = new List<KeyValuePair<string,string>>();
                    improvements = new List<KeyValuePair<string,string>>();
                    if(dt!=null && dt.Rows.Count>0)
                    {
                        KeyValuePair<string, string> item = new KeyValuePair<string,string>();
                        for(int i=0;i<dt.Rows.Count;i++)
                        {
                            item = new KeyValuePair<string, string>(dt.Rows[i]["CompetencyId"].ToString(), dt.Rows[i]["CompetencyDescription"].ToString());

                            if(string.Equals(dt.Rows[i]["Type"].ToString(),"IMPROVEMENTS"))
                            {
                                improvements.Add(item);
                            }
                            if (string.Equals(dt.Rows[i]["Type"].ToString(), "STRENGTHS"))
                            {
                                strengths.Add(item);
                            }
                        }
                    }
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

        public StatusDTO<List<EmployeeAppraisalMasterDTO>> SearchAppraisee(EmployeeAppraisalMasterDTO data)
        {
            StatusDTO<List<EmployeeAppraisalMasterDTO>> appraiseeList = new StatusDTO<List<EmployeeAppraisalMasterDTO>>();
            DataSet dsAppraiseeLst = null;
            int empAppraisalMasterId = -1;
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "get_AppraiseeDetails";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    command.Parameters.Add("@Fname", MySqlDbType.String).Value = data.Employee.UserDetails.FName;
                    command.Parameters.Add("@Lname", MySqlDbType.String).Value = data.Employee.UserDetails.LName;
                    command.Parameters.Add("@StaffEmployeeId", MySqlDbType.String).Value = data.Employee.StaffEmployeeId;
                    command.Parameters.Add("@LocationId", MySqlDbType.Int32).Value = data.Employee.UserDetails.Location.LocationId;
                    command.Parameters.Add("@AppraisalStatusId", MySqlDbType.Int32).Value = data.AppraisalStatus.AppraisalStatusId;
                    command.Parameters.Add("@Gender", MySqlDbType.Int32).Value = data.Employee.UserDetails.Gender;
                    command.Parameters.Add("@AppraisalType", MySqlDbType.String).Value = data.AppraisalType;
                    
                    MySqlDataAdapter da = new MySqlDataAdapter(command);
                    dsAppraiseeLst = new DataSet();
                    da.Fill(dsAppraiseeLst);

                    StatusDTO<EmployeeAppraisalMasterDTO> status = new StatusDTO<EmployeeAppraisalMasterDTO>();
                    EmployeeAppraisalMasterDTO appraisalMasterDTO = new EmployeeAppraisalMasterDTO();
                    if (dsAppraiseeLst != null && dsAppraiseeLst.Tables.Count > 0)
                    {
                        appraiseeList.ReturnObj = new List<EmployeeAppraisalMasterDTO>();
                        if (dsAppraiseeLst.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsAppraiseeLst.Tables[0].Rows.Count; i++)
                            {
                                EmployeeAppraisalMasterDTO empAppraisalMaster = new EmployeeAppraisalMasterDTO();
                                empAppraisalMaster.EmployeeAppraisalMasterId = Convert.ToInt32(dsAppraiseeLst.Tables[0].Rows[i]["employeeappraisalmasterid"]);
                                empAppraisalMaster.Employee = new EmployeeDetailsDTO();
                                empAppraisalMaster.Employee.UserDetails = new UserMasterDTO();
                                empAppraisalMaster.Employee.UserDetails.Location = new LocationDTO();
                                empAppraisalMaster.AppraisalStatus = new AppraisalStatusDTO();
                                empAppraisalMaster.Employee.Designation = new DesignationDTO();

                                empAppraisalMaster.Employee.UserDetails.FName = dsAppraiseeLst.Tables[0].Rows[i]["Fname"].ToString();
                                empAppraisalMaster.Employee.UserDetails.LName = dsAppraiseeLst.Tables[0].Rows[i]["Lname"].ToString();
                                empAppraisalMaster.Employee.UserDetails.Gender = dsAppraiseeLst.Tables[0].Rows[i]["Gender"].ToString();
                                empAppraisalMaster.Employee.UserDetails.Location.LocationId = Convert.ToInt32(dsAppraiseeLst.Tables[0].Rows[i]["LocationId"].ToString());
                                empAppraisalMaster.Employee.UserDetails.Location.LocationDescription = dsAppraiseeLst.Tables[0].Rows[i]["LocationDescription"].ToString();
                                empAppraisalMaster.AppraisalType = dsAppraiseeLst.Tables[0].Rows[i]["AppraisalType"].ToString();
                                empAppraisalMaster.Employee.StaffEmployeeId = dsAppraiseeLst.Tables[0].Rows[i]["StaffEmployeeId"].ToString();

                                empAppraisalMaster.AppraisalStatus.AppraisalStatusId= Convert.ToInt32(dsAppraiseeLst.Tables[0].Rows[i]["AppraisalStatusId"].ToString());
                                empAppraisalMaster.AppraisalStatus.AppraisalStatusDescription= dsAppraiseeLst.Tables[0].Rows[i]["AppraisalStatusDescription"].ToString();

                                appraiseeList.ReturnObj.Add(empAppraisalMaster);
                            }
                        }
                        
                    }

                    return appraiseeList;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }
    }
}
