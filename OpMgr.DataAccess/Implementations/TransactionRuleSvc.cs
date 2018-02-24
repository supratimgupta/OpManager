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
    public class TransactionRuleSvc : ITransactionRuleSvc, IDisposable
    {
        private IConfigSvc _configSvc;

        private ILogSvc _logger;

        private DataTable _dtResult;

        public void Dispose()
        {
            if(_dtResult!=null)
            {
                _dtResult.Dispose();
                _dtResult = null;
            }
        }

        public TransactionRuleSvc(IConfigSvc configSvc, ILogSvc logger)
        {
            _configSvc = configSvc;
            _logger = logger;
        }

        public StatusDTO<TransactionRuleDTO> Insert(TransactionRuleDTO data)
        {
            StatusDTO<TransactionRuleDTO> status = new StatusDTO<TransactionRuleDTO>();
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    command.CommandText = "INSERT INTO transactionrule(Active, TranMasterId, UserMasterId, StandardId, SectionId, FirstDueAfterDays, DueDateIncreasesBy, PenaltyCalculatedIn, PenaltyAmount, ActualAmount, ClassTypeId, RuleName, PenaltyTransactionType, PenaltyTranRuleId, LocationId)" + 
                                          " VALUES (@active, @tmId,@umId,@stdId,@secId,@frstDueAfter,@dueDTIncreasesBy,@penaltyCalcIn,@penaltyAmt,@actualAmt,@ctId,@ruleName,@penaltyTrType,@penaltyTrRuleId,@locationId)";

                    command.Parameters.Add("@active", MySqlDbType.Bit).Value = data.Active;
                    if (data.TranMaster!=null && data.TranMaster.TranMasterId>0)
                    {
                        command.Parameters.Add("@tmId", MySqlDbType.Int32).Value = data.TranMaster.TranMasterId;
                    }
                    else
                    {
                        command.Parameters.Add("@tmId", MySqlDbType.Int32).Value = DBNull.Value;
                    }
                    if (data.UserDTO != null && data.UserDTO.UserMasterId > 0)
                    {
                        command.Parameters.Add("@umId", MySqlDbType.Int32).Value = data.UserDTO.UserMasterId;
                    }
                    else
                    {
                        command.Parameters.Add("@umId", MySqlDbType.Int32).Value = DBNull.Value;
                    }
                    if (data.Standard != null && data.Standard.StandardId > 0)
                    {
                        command.Parameters.Add("@stdId", MySqlDbType.Int32).Value = data.Standard.StandardId;
                    }
                    else
                    {
                        command.Parameters.Add("@stdId", MySqlDbType.Int32).Value = DBNull.Value;
                    }
                    if (data.Section != null && data.Section.SectionId > 0)
                    {
                        command.Parameters.Add("@secId", MySqlDbType.Int32).Value = data.Section.SectionId;
                    }
                    else
                    {
                        command.Parameters.Add("@secId", MySqlDbType.Int32).Value = DBNull.Value;
                    }
                    command.Parameters.Add("@frstDueAfter", MySqlDbType.Int32).Value = data.FirstDueAfterDays;
                    command.Parameters.Add("@dueDTIncreasesBy", MySqlDbType.Int32).Value = data.DueDateIncreasesBy;
                    if (!string.IsNullOrEmpty(data.PenaltyCalculatedIn) && !string.Equals("-1",data.PenaltyCalculatedIn))
                    {
                        command.Parameters.Add("@penaltyCalcIn", MySqlDbType.String).Value = data.PenaltyCalculatedIn;
                    }
                    else
                    {
                        command.Parameters.Add("@penaltyCalcIn", MySqlDbType.String).Value = DBNull.Value;
                    }
                    if(data.PenaltyAmount!=null)
                    {
                        command.Parameters.Add("@penaltyAmt", MySqlDbType.Double).Value = data.PenaltyAmount.Value;
                    }
                    else
                    {
                        command.Parameters.Add("@penaltyAmt", MySqlDbType.Double).Value = DBNull.Value;
                    }
                    if (data.ActualAmount != null)
                    {
                        command.Parameters.Add("@actualAmt", MySqlDbType.Double).Value = data.ActualAmount.Value;
                    }
                    else
                    {
                        command.Parameters.Add("@actualAmt", MySqlDbType.Double).Value = DBNull.Value;
                    }
                    if(data.ClassType!=null && data.ClassType.ClassTypeId>0)
                    {
                        command.Parameters.Add("@ctId", MySqlDbType.Int32).Value = data.ClassType.ClassTypeId;
                    }
                    else
                    {
                        command.Parameters.Add("@ctId", MySqlDbType.Int32).Value = DBNull.Value;
                    }
                    if(!string.IsNullOrEmpty(data.RuleName))
                    {
                        command.Parameters.Add("@ruleName", MySqlDbType.String).Value = data.RuleName;
                    }
                    else
                    {
                        command.Parameters.Add("@ruleName", MySqlDbType.String).Value = DBNull.Value;
                    }
                    if(!string.IsNullOrEmpty(data.PenaltyTransactionType) && !string.Equals("-1", data.PenaltyTransactionType))
                    {
                        command.Parameters.Add("@penaltyTrType", MySqlDbType.String).Value = data.PenaltyTransactionType;
                    }
                    else
                    {
                        command.Parameters.Add("@penaltyTrType", MySqlDbType.String).Value = DBNull.Value;
                    }
                    if(data.PenaltyTransactionRule!=null && data.PenaltyTransactionRule.TranRuleId>0)
                    {
                        command.Parameters.Add("@penaltyTrRuleId", MySqlDbType.Int32).Value = data.PenaltyTransactionRule.TranRuleId;
                    }
                    else
                    {
                        command.Parameters.Add("@penaltyTrRuleId", MySqlDbType.Int32).Value = DBNull.Value;
                    }
                    if(data.Location!=null && data.Location.LocationId>0)
                    {
                        command.Parameters.Add("@locationId", MySqlDbType.Int32).Value = data.Location.LocationId;
                    }
                    else
                    {
                        command.Parameters.Add("@locationId", MySqlDbType.Int32).Value = DBNull.Value;
                    }
                    if(command.ExecuteNonQuery()>0)
                    {
                        status.IsSuccess = true;
                        status.IsException = false;
                        status.ReturnObj = data;
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

        public StatusDTO<TransactionRuleDTO> Update(TransactionRuleDTO data)
        {
            StatusDTO<TransactionRuleDTO> status = new StatusDTO<TransactionRuleDTO>();
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    command.CommandText = "UPDATE transactionrule SET Active=@active, TranMasterId=@tmId, UserMasterId=@umId, StandardId=@stdId, SectionId=@secId, FirstDueAfterDays=@frstDueAfter, DueDateIncreasesBy=@dueDTIncreasesBy, PenaltyCalculatedIn=@penaltyCalcIn, PenaltyAmount=@penaltyAmt, ActualAmount=@actualAmt, ClassTypeId=@ctId, RuleName=@ruleName, PenaltyTransactionType=@penaltyTrType, PenaltyTranRuleId=@penaltyTrRuleId, LocationId=@locationId WHERE TranRuleId=@trRuleId";
                    command.Parameters.Add("@active", MySqlDbType.Bit).Value = data.Active;
                    if (data.TranMaster != null && data.TranMaster.TranMasterId > 0)
                    {
                        command.Parameters.Add("@tmId", MySqlDbType.Int32).Value = data.TranMaster.TranMasterId;
                    }
                    else
                    {
                        command.Parameters.Add("@tmId", MySqlDbType.Int32).Value = DBNull.Value;
                    }
                    if (data.UserDTO != null && data.UserDTO.UserMasterId > 0)
                    {
                        command.Parameters.Add("@umId", MySqlDbType.Int32).Value = data.UserDTO.UserMasterId;
                    }
                    else
                    {
                        command.Parameters.Add("@umId", MySqlDbType.Int32).Value = DBNull.Value;
                    }
                    if (data.Standard != null && data.Standard.StandardId > 0)
                    {
                        command.Parameters.Add("@stdId", MySqlDbType.Int32).Value = data.Standard.StandardId;
                    }
                    else
                    {
                        command.Parameters.Add("@stdId", MySqlDbType.Int32).Value = DBNull.Value;
                    }
                    if (data.Section != null && data.Section.SectionId > 0)
                    {
                        command.Parameters.Add("@secId", MySqlDbType.Int32).Value = data.Section.SectionId;
                    }
                    else
                    {
                        command.Parameters.Add("@secId", MySqlDbType.Int32).Value = DBNull.Value;
                    }
                    command.Parameters.Add("@frstDueAfter", MySqlDbType.Int32).Value = data.FirstDueAfterDays;
                    command.Parameters.Add("@dueDTIncreasesBy", MySqlDbType.Int32).Value = data.DueDateIncreasesBy;
                    if (!string.IsNullOrEmpty(data.PenaltyCalculatedIn) && !string.Equals("-1",data.PenaltyCalculatedIn))
                    {
                        command.Parameters.Add("@penaltyCalcIn", MySqlDbType.String).Value = data.PenaltyCalculatedIn;
                    }
                    else
                    {
                        command.Parameters.Add("@penaltyCalcIn", MySqlDbType.String).Value = DBNull.Value;
                    }
                    if (data.PenaltyAmount != null)
                    {
                        command.Parameters.Add("@penaltyAmt", MySqlDbType.Double).Value = data.PenaltyAmount.Value;
                    }
                    else
                    {
                        command.Parameters.Add("@penaltyAmt", MySqlDbType.Double).Value = DBNull.Value;
                    }
                    if (data.ActualAmount != null)
                    {
                        command.Parameters.Add("@actualAmt", MySqlDbType.Double).Value = data.ActualAmount.Value;
                    }
                    else
                    {
                        command.Parameters.Add("@actualAmt", MySqlDbType.Double).Value = DBNull.Value;
                    }
                    if (data.ClassType != null && data.ClassType.ClassTypeId > 0)
                    {
                        command.Parameters.Add("@ctId", MySqlDbType.Int32).Value = data.ClassType.ClassTypeId;
                    }
                    else
                    {
                        command.Parameters.Add("@ctId", MySqlDbType.Int32).Value = DBNull.Value;
                    }
                    if (!string.IsNullOrEmpty(data.RuleName))
                    {
                        command.Parameters.Add("@ruleName", MySqlDbType.String).Value = data.RuleName;
                    }
                    else
                    {
                        command.Parameters.Add("@ruleName", MySqlDbType.String).Value = DBNull.Value;
                    }
                    if (!string.IsNullOrEmpty(data.PenaltyTransactionType) && !string.Equals("-1", data.PenaltyTransactionType))
                    {
                        command.Parameters.Add("@penaltyTrType", MySqlDbType.String).Value = data.PenaltyTransactionType;
                    }
                    else
                    {
                        command.Parameters.Add("@penaltyTrType", MySqlDbType.String).Value = DBNull.Value;
                    }
                    if (data.PenaltyTransactionRule != null && data.PenaltyTransactionRule.TranRuleId > 0)
                    {
                        command.Parameters.Add("@penaltyTrRuleId", MySqlDbType.Int32).Value = data.PenaltyTransactionRule.TranRuleId;
                    }
                    else
                    {
                        command.Parameters.Add("@penaltyTrRuleId", MySqlDbType.Int32).Value = DBNull.Value;
                    }
                    if (data.Location != null && data.Location.LocationId > 0)
                    {
                        command.Parameters.Add("@locationId", MySqlDbType.Int32).Value = data.Location.LocationId;
                    }
                    else
                    {
                        command.Parameters.Add("@locationId", MySqlDbType.Int32).Value = DBNull.Value;
                    }
                    command.Parameters.Add("@trRuleId", MySqlDbType.Int32).Value = data.TranRuleId;
                    if (command.ExecuteNonQuery() > 0)
                    {
                        status.IsSuccess = true;
                        status.IsException = false;
                        status.ReturnObj = data;
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

        public StatusDTO<TransactionRuleDTO> Delete(TransactionRuleDTO data)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<List<TransactionRuleDTO>> Select(TransactionRuleDTO data)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<TransactionRuleDTO> Select(int rowId)
        {
            StatusDTO<TransactionRuleDTO> status = new StatusDTO<TransactionRuleDTO>();
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    command.CommandText = "SELECT TranRuleId, Active, TranMasterId, UserMasterId, StandardId, SectionId, FirstDueAfterDays, DueDateIncreasesBy, PenaltyCalculatedIn, PenaltyAmount, ActualAmount, ClassTypeId, RuleName, PenaltyTransactionType, PenaltyTranRuleId, LocationId from transactionrule where TranRuleId=@trRuleId";
                    command.Parameters.Add("@trRuleId", MySqlDbType.Int32).Value = rowId;
                    DataTable dtResult = new DataTable();
                    MySqlDataAdapter mDa = new MySqlDataAdapter(command);
                    mDa.Fill(dtResult);
                    if(dtResult!=null && dtResult.Rows.Count>0)
                    {
                        status.ReturnObj = new TransactionRuleDTO();
                        status.ReturnObj.TranRuleId = (int)dtResult.Rows[0]["TranRuleId"];
                        status.ReturnObj.Active = string.Equals(dtResult.Rows[0]["Active"].ToString(), "1") ? true : false;
                        status.ReturnObj.TranMaster = new TransactionMasterDTO();
                        status.ReturnObj.TranMaster.TranMasterId = string.IsNullOrEmpty(dtResult.Rows[0]["TranMasterId"].ToString()) ? -1 : int.Parse(dtResult.Rows[0]["TranMasterId"].ToString());
                        status.ReturnObj.UserDTO = new UserMasterDTO();
                        status.ReturnObj.UserDTO.UserMasterId = string.IsNullOrEmpty(dtResult.Rows[0]["UserMasterId"].ToString()) ? -1 : int.Parse(dtResult.Rows[0]["UserMasterId"].ToString());
                        status.ReturnObj.Standard = new StandardDTO();
                        status.ReturnObj.Standard.StandardId = string.IsNullOrEmpty(dtResult.Rows[0]["StandardId"].ToString()) ? -1 : int.Parse(dtResult.Rows[0]["StandardId"].ToString());
                        status.ReturnObj.Section = new SectionDTO();
                        status.ReturnObj.Section.SectionId = string.IsNullOrEmpty(dtResult.Rows[0]["SectionId"].ToString()) ? -1 : int.Parse(dtResult.Rows[0]["SectionId"].ToString());
                        status.ReturnObj.FirstDueAfterDays = string.IsNullOrEmpty(dtResult.Rows[0]["FirstDueAfterDays"].ToString()) ? -1 : int.Parse(dtResult.Rows[0]["FirstDueAfterDays"].ToString());
                        status.ReturnObj.DueDateIncreasesBy = string.IsNullOrEmpty(dtResult.Rows[0]["DueDateIncreasesBy"].ToString()) ? -1 : int.Parse(dtResult.Rows[0]["DueDateIncreasesBy"].ToString());
                        status.ReturnObj.PenaltyCalculatedIn = dtResult.Rows[0]["PenaltyCalculatedIn"].ToString();
                        status.ReturnObj.PenaltyAmount = string.IsNullOrEmpty(dtResult.Rows[0]["PenaltyAmount"].ToString()) ? 0.0 : double.Parse(dtResult.Rows[0]["PenaltyAmount"].ToString());
                        status.ReturnObj.ActualAmount = string.IsNullOrEmpty(dtResult.Rows[0]["ActualAmount"].ToString()) ? 0.0 : double.Parse(dtResult.Rows[0]["ActualAmount"].ToString());
                        status.ReturnObj.ClassType = new ClassTypeDTO();
                        status.ReturnObj.ClassType.ClassTypeId = string.IsNullOrEmpty(dtResult.Rows[0]["ClassTypeId"].ToString()) ? -1 : int.Parse(dtResult.Rows[0]["ClassTypeId"].ToString());
                        status.ReturnObj.RuleName = dtResult.Rows[0]["RuleName"].ToString();
                        status.ReturnObj.PenaltyTransactionType = dtResult.Rows[0]["PenaltyTransactionType"].ToString();
                        status.ReturnObj.PenaltyTransactionRule = new TransactionRuleDTO();
                        status.ReturnObj.PenaltyTransactionRule.TranRuleId = string.IsNullOrEmpty(dtResult.Rows[0]["PenaltyTranRuleId"].ToString()) ? -1 : int.Parse(dtResult.Rows[0]["PenaltyTranRuleId"].ToString());
                        status.ReturnObj.Location = new LocationDTO();
                        status.ReturnObj.Location.LocationId = string.IsNullOrEmpty(dtResult.Rows[0]["LocationId"].ToString()) ? -1 : int.Parse(dtResult.Rows[0]["LocationId"].ToString());
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
          
        public DataTable GetAllRules()
        {
            using(IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "SELECT TranRuleId, TranMasterId, StandardId, SectionId, UserMasterId, ClassTypeId, FirstDueAfterDays, DueDateincreasesBy, PenaltyCalculatedIn, PenaltyAmount, ActualAmount, PenaltyTransactionType, PenaltyTranRuleId, LocationId FROM TransactionRule WHERE Active=1";
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    MySqlDataAdapter dataAdap = new MySqlDataAdapter(command);
                    _dtResult = new DataTable("TRANS_RULE");
                    dataAdap.Fill(_dtResult);
                    return _dtResult;
                }
                catch(Exception exp)
                {
                    _logger.Log(exp);
                    throw exp;
                }
            }
        }


        public List<TransactionRuleDTO> GetAllRulesWithInactive()
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "SELECT TranRuleId, RuleName, TranMasterId, StandardId, SectionId, UserMasterId, ClassTypeId, FirstDueAfterDays, DueDateincreasesBy, PenaltyCalculatedIn, PenaltyAmount, ActualAmount, PenaltyTransactionType, PenaltyTranRuleId FROM TransactionRule";
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    MySqlDataAdapter dataAdap = new MySqlDataAdapter(command);
                    _dtResult = new DataTable("TRANS_RULE");
                    dataAdap.Fill(_dtResult);
                    List<TransactionRuleDTO> lstTranRule = new List<TransactionRuleDTO>();
                    TransactionRuleDTO trRule = null;
                    if(_dtResult!=null && _dtResult.Rows.Count>0)
                    {
                        foreach(DataRow dr in _dtResult.Rows)
                        {
                            trRule = new TransactionRuleDTO();
                            trRule.TranRuleId = (int)dr["TranRuleId"];
                            trRule.TranMaster = new TransactionMasterDTO();
                            trRule.TranMaster.TranMasterId = string.IsNullOrEmpty(dr["TranMasterId"].ToString()) ? -1 : int.Parse(dr["TranMasterId"].ToString());
                            trRule.UserDTO = new UserMasterDTO();
                            trRule.UserDTO.UserMasterId = string.IsNullOrEmpty(dr["UserMasterId"].ToString()) ? -1 : int.Parse(dr["UserMasterId"].ToString());
                            trRule.Standard = new StandardDTO();
                            trRule.Standard.StandardId = string.IsNullOrEmpty(dr["StandardId"].ToString()) ? -1 : int.Parse(dr["StandardId"].ToString());
                            trRule.Section = new SectionDTO();
                            trRule.Section.SectionId = string.IsNullOrEmpty(dr["SectionId"].ToString()) ? -1 : int.Parse(dr["SectionId"].ToString());
                            trRule.FirstDueAfterDays = string.IsNullOrEmpty(dr["FirstDueAfterDays"].ToString()) ? -1 : int.Parse(dr["FirstDueAfterDays"].ToString());
                            trRule.DueDateIncreasesBy = string.IsNullOrEmpty(dr["DueDateIncreasesBy"].ToString()) ? -1 : int.Parse(dr["DueDateIncreasesBy"].ToString());
                            trRule.PenaltyCalculatedIn = dr["PenaltyCalculatedIn"].ToString();
                            trRule.PenaltyAmount = string.IsNullOrEmpty(dr["PenaltyAmount"].ToString()) ? 0.0 : double.Parse(dr["PenaltyAmount"].ToString());
                            trRule.ActualAmount = string.IsNullOrEmpty(dr["ActualAmount"].ToString()) ? 0.0 : double.Parse(dr["ActualAmount"].ToString());
                            trRule.ClassType = new ClassTypeDTO();
                            trRule.ClassType.ClassTypeId = string.IsNullOrEmpty(dr["ClassTypeId"].ToString()) ? -1 : int.Parse(dr["ClassTypeId"].ToString());
                            trRule.RuleName = dr["RuleName"].ToString();
                            trRule.PenaltyTransactionType = dr["PenaltyTransactionType"].ToString();
                            trRule.PenaltyTransactionRule = new TransactionRuleDTO();
                            trRule.PenaltyTransactionRule.TranRuleId = string.IsNullOrEmpty(dr["PenaltyTranRuleId"].ToString()) ? -1 : int.Parse(dr["PenaltyTranRuleId"].ToString());
                            lstTranRule.Add(trRule);
                        }
                    }
                    return lstTranRule;
                }
                catch (Exception exp)
                {
                    _logger.Log(exp);
                    throw exp;
                }
            }
        }

        public bool IsDuplicate(int location, int trnsMasterId, int standardId, int sectionId, int classTypeId, int userMasterId, string isDiffTo, string mode, int ruleId)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "SELECT TranRuleId FROM TransactionRule WHERE TranMasterId=@tranMaster AND LocationId=@locationId";
                    command.Parameters.Add("@tranMaster", MySqlDbType.Int32).Value = trnsMasterId;
                    command.Parameters.Add("@locationId", MySqlDbType.Int32).Value = location;
                    if(string.Equals(isDiffTo, "CLASS-TYPE", StringComparison.OrdinalIgnoreCase))
                    {
                        command.CommandText = command.CommandText + " AND ClassTypeId=@classType";
                        command.Parameters.Add("@classType", MySqlDbType.Int32).Value = classTypeId;
                    }
                    else if (string.Equals(isDiffTo, "STANDARD", StringComparison.OrdinalIgnoreCase))
                    {
                        command.CommandText = command.CommandText + " AND StandardId=@standardId";
                        command.Parameters.Add("@standardId", MySqlDbType.Int32).Value = standardId;
                    }
                    else if (string.Equals(isDiffTo, "SECTION", StringComparison.OrdinalIgnoreCase))
                    {
                        command.CommandText = command.CommandText + " AND StandardId=@standardId AND SectionId=@sectionId";
                        command.Parameters.Add("@standardId", MySqlDbType.Int32).Value = standardId;
                        command.Parameters.Add("@sectionId", MySqlDbType.Int32).Value = sectionId;
                    }
                    else if (string.Equals(isDiffTo, "USER", StringComparison.OrdinalIgnoreCase))
                    {
                        command.CommandText = command.CommandText + " AND UserMasterId=@userMasterId";
                        command.Parameters.Add("@userMasterId", MySqlDbType.Int32).Value = userMasterId;
                    }
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    MySqlDataAdapter dataAdap = new MySqlDataAdapter(command);
                    _dtResult = new DataTable("TRANS_RULE");
                    dataAdap.Fill(_dtResult);
                    if(string.Equals(mode, "ADD", StringComparison.OrdinalIgnoreCase))
                    {
                        if(_dtResult!=null && _dtResult.Rows.Count>0)
                        {
                            return true;
                        }
                        return false;
                    }
                    if((_dtResult==null) || (_dtResult.Rows.Count==0) || (_dtResult.Rows.Count==1 && string.Equals(_dtResult.Rows[0]["TranRuleId"].ToString(), ruleId.ToString(), StringComparison.OrdinalIgnoreCase)))
                    {
                        return false;
                    }
                    return true;                    
                }
                catch (Exception exp)
                {
                    _logger.Log(exp);
                    throw exp;
                }
            }
        }

        public List<TransactionRuleDTO> GetUserLevelRules(int location, int transactionMasterId, int userRowId)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "SELECT * FROM (SELECT tr.TranRuleId, u.UserMasterId, u.FName, u.MName, u.LName, (select TransactionName from transactionmaster where tranmasterid=@nameSelect) as TransactionName, tr.RuleName, tr.ActualAmount, u.Active, tr.TranMasterId, tm.IsDifferentTo FROM usermaster u LEFT OUTER JOIN transactionrule tr ON u.UserMasterId=tr.UserMasterId LEFT OUTER JOIN transactionmaster tm ON tr.TranMasterId=tm.TranMasterId) res WHERE (res.TranMasterId=@tranMaster OR res.TranMasterId IS NULL) AND res.UserMasterId=@userMaster AND (res.IsDifferentTo='USER' OR res.IsDifferentTo IS NULL)";
                    command.Parameters.Add("@nameSelect", MySqlDbType.Int32).Value = transactionMasterId;
                    command.Parameters.Add("@tranMaster", MySqlDbType.Int32).Value = transactionMasterId;
                    command.Parameters.Add("@userMaster", MySqlDbType.Int32).Value = userRowId;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    MySqlDataAdapter dataAdap = new MySqlDataAdapter(command);
                    _dtResult = new DataTable("TRANS_RULE");
                    dataAdap.Fill(_dtResult);
                    List<TransactionRuleDTO> lstRules = null;
                    if(_dtResult!=null && _dtResult.Rows.Count>0)
                    {
                        lstRules = new List<TransactionRuleDTO>();
                        TransactionRuleDTO rule = null;
                        foreach(DataRow dr in _dtResult.Rows)
                        {
                            rule = new TransactionRuleDTO();
                            if(!string.IsNullOrEmpty(dr["TranRuleId"].ToString()))
                            {
                                rule.TranRuleId = (int)dr["TranRuleId"];
                            }
                            else
                            {
                                rule.TranRuleId = -1;
                            }
                            rule.UserDTO = new UserMasterDTO();
                            rule.UserDTO.UserMasterId = (int)dr["UserMasterId"];
                            rule.UserDTO.FName = dr["FName"].ToString();
                            rule.UserDTO.MName = dr["MName"].ToString();
                            rule.UserDTO.LName = dr["LName"].ToString();
                            rule.TranMaster = new TransactionMasterDTO();
                            rule.TranMaster.TransactionName = dr["TransactionName"].ToString();
                            rule.RuleName = dr["RuleName"].ToString();
                            rule.ActualAmount = string.IsNullOrEmpty(dr["ActualAmount"].ToString()) ? 0.0 : double.Parse(dr["ActualAmount"].ToString());
                            lstRules.Add(rule);
                        }
                    }
                    return lstRules;
                }
                catch (Exception exp)
                {
                    _logger.Log(exp);
                    throw exp;
                }
            }
        }

        public List<TransactionRuleDTO> GetClassTypeLevelRules(int location, int transactionMasterId, int? classTypeRowId = null)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "SELECT * FROM (SELECT trans.TranRuleId, ct.ClassTypeId, ct.ClassTypeName, (select TransactionName from transactionmaster where tranmasterid=@nameSelect) as TransactionName, trans.RuleName, trans.ActualAmount, trans.TranMasterId, ct.Active, trans.IsDifferentTo FROM classtype ct LEFT OUTER JOIN (SELECT tr.TranRuleId, tr.RuleName, tr.ActualAmount, tr.TranMasterId, tm.IsDifferentTo, tr.ClassTypeId FROM transactionrule tr LEFT JOIN transactionmaster tm ON tr.TranMasterId=tm.TranMasterId AND tm.TranMasterId=@tranMaster AND tm.IsdifferentTo='CLASS-TYPE' AND tr.LocationId=@locationId) trans ON trans.ClassTypeId=ct.ClassTypeId AND ct.Active=1) res";
                    command.Parameters.Add("@nameSelect", MySqlDbType.Int32).Value = transactionMasterId;
                    command.Parameters.Add("@tranMaster", MySqlDbType.Int32).Value = transactionMasterId;
                    command.Parameters.Add("@locationId", MySqlDbType.Int32).Value = location;
                    if(classTypeRowId!=null && classTypeRowId.Value>0)
                    {
                        command.CommandText += " WHERE res.ClassTypeId=@classType";
                        command.Parameters.Add("@classType", MySqlDbType.Int32).Value = classTypeRowId.Value;
                    }
                    
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    MySqlDataAdapter dataAdap = new MySqlDataAdapter(command);
                    _dtResult = new DataTable("TRANS_RULE");
                    dataAdap.Fill(_dtResult);
                    List<TransactionRuleDTO> lstRules = null;
                    if (_dtResult != null && _dtResult.Rows.Count > 0)
                    {
                        lstRules = new List<TransactionRuleDTO>();
                        TransactionRuleDTO rule = null;
                        foreach (DataRow dr in _dtResult.Rows)
                        {
                            rule = new TransactionRuleDTO();
                            if (!string.IsNullOrEmpty(dr["TranRuleId"].ToString()))
                            {
                                rule.TranRuleId = (int)dr["TranRuleId"];
                            }
                            else
                            {
                                rule.TranRuleId = -1;
                            }
                            rule.ClassType = new ClassTypeDTO();
                            rule.ClassType.ClassTypeId = (int)dr["ClassTypeId"];
                            rule.ClassType.ClassTypeName = dr["ClassTypeName"].ToString();
                            rule.TranMaster = new TransactionMasterDTO();
                            rule.TranMaster.TransactionName = dr["TransactionName"].ToString();
                            rule.RuleName = dr["RuleName"].ToString();
                            rule.ActualAmount = string.IsNullOrEmpty(dr["ActualAmount"].ToString()) ? 0.0 : double.Parse(dr["ActualAmount"].ToString());
                            lstRules.Add(rule);
                        }
                    }
                    return lstRules;
                }
                catch (Exception exp)
                {
                    _logger.Log(exp);
                    throw exp;
                }
            }
        }

        public List<TransactionRuleDTO> GetStandardLevelRules(int location, int transactionMasterId, int? standardRowId = null)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "SELECT * FROM (SELECT trans.TranRuleId, std.StandardId, std.StandardName, (select TransactionName from transactionmaster where tranmasterid=@nameSelect) as TransactionName, trans.RuleName, trans.ActualAmount, trans.TranMasterId, std.Active, trans.IsDifferentTo FROM standard std LEFT OUTER JOIN (SELECT tr.TranRuleId, tr.RuleName, tr.ActualAmount, tr.TranMasterId, tm.IsDifferentTo, tr.StandardId from transactionrule tr LEFT JOIN transactionmaster tm ON tr.TranMasterId=tm.TranMasterId where tr.TranMasterId=@tranMaster and tm.IsdifferentTo='STANDARD' and tr.LocationId=@trLocationId) trans ON trans.StandardId=std.StandardId AND std.Active=1) res";
                    command.Parameters.Add("@nameSelect", MySqlDbType.Int32).Value = transactionMasterId;
                    command.Parameters.Add("@tranMaster", MySqlDbType.Int32).Value = transactionMasterId;
                    command.Parameters.Add("@trLocationId", MySqlDbType.Int32).Value = location;
                    if (standardRowId != null && standardRowId.Value > 0)
                    {
                        command.CommandText += " WHERE res.StandardId=@stdId";
                        command.Parameters.Add("@stdId", MySqlDbType.Int32).Value = standardRowId.Value;
                    }

                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    MySqlDataAdapter dataAdap = new MySqlDataAdapter(command);
                    _dtResult = new DataTable("TRANS_RULE");
                    dataAdap.Fill(_dtResult);
                    List<TransactionRuleDTO> lstRules = null;
                    if (_dtResult != null && _dtResult.Rows.Count > 0)
                    {
                        lstRules = new List<TransactionRuleDTO>();
                        TransactionRuleDTO rule = null;
                        foreach (DataRow dr in _dtResult.Rows)
                        {
                            rule = new TransactionRuleDTO();
                            if (!string.IsNullOrEmpty(dr["TranRuleId"].ToString()))
                            {
                                rule.TranRuleId = (int)dr["TranRuleId"];
                            }
                            else
                            {
                                rule.TranRuleId = -1;
                            }
                            rule.Standard = new StandardDTO();
                            rule.Standard.StandardId = (int)dr["StandardId"];
                            rule.Standard.StandardName = dr["StandardName"].ToString();
                            rule.TranMaster = new TransactionMasterDTO();
                            rule.TranMaster.TransactionName = dr["TransactionName"].ToString();
                            rule.RuleName = dr["RuleName"].ToString();
                            rule.ActualAmount = string.IsNullOrEmpty(dr["ActualAmount"].ToString()) ? 0.0 : double.Parse(dr["ActualAmount"].ToString());
                            lstRules.Add(rule);
                        }
                    }
                    return lstRules;
                }
                catch (Exception exp)
                {
                    _logger.Log(exp);
                    throw exp;
                }
            }
        }

        public List<TransactionRuleDTO> GetStandardSectionLevelRules(int location, int transactionMasterId, int standardId, int? sectionId = null)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "SELECT * FROM (SELECT trans.TranRuleId, std.StandardId, std.StandardName, sec.SectionId, sec.SectionName, (select TransactionName from transactionmaster where tranmasterid=@nameSelect) as TransactionName, trans.RuleName, trans.ActualAmount, trans.TranMasterId, stdsec.Active as stdSecActive, std.Active as stdActive, sec.Active as secActive, trans.IsDifferentTo FROM standardsectionmap stdsec LEFT OUTER JOIN standard std ON stdsec.StandardId=std.StandardId LEFT OUTER JOIN section sec ON stdsec.SectionId=sec.SectionId LEFT OUTER JOIN (SELECT tr.TranRuleId, tr.RuleName, tr.ActualAmount, tr.TranMasterId, tm.IsDifferentTo,tr.StandardId,tr.SectionId FROM transactionrule tr LEFT JOIN transactionmaster tm ON tr.TranMasterId=tm.TranMasterId AND tr.TranMasterId=@tranMaster AND tm.IsdifferentTo='SECTION' AND tr.LocationId=@trLocationId) trans ON (trans.StandardId=stdsec.StandardId AND trans.SectionId=stdsec.SectionId) WHERE std.Active=1 AND stdsec.Active=1 AND sec.Active=1 "+ (standardId>0? "AND stdsec.StandardId=@stdId)":")")+" res";
                    command.Parameters.Add("@nameSelect", MySqlDbType.Int32).Value = transactionMasterId;
                    command.Parameters.Add("@tranMaster", MySqlDbType.Int32).Value = transactionMasterId;
                    command.Parameters.Add("@trLocationId", MySqlDbType.Int32).Value = location;
                    if(standardId>0)
                    {
                        command.Parameters.Add("@stdId", MySqlDbType.Int32).Value = standardId;
                    }
                    if (sectionId != null && sectionId.Value > 0)
                    {
                        command.CommandText += " WHERE res.SectionId=@secId";
                        command.Parameters.Add("@secId", MySqlDbType.Int32).Value = sectionId.Value;
                    }
                    command.CommandText = command.CommandText + " order by res.StandardId, res.SectionId";
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    MySqlDataAdapter dataAdap = new MySqlDataAdapter(command);
                    _dtResult = new DataTable("TRANS_RULE");
                    dataAdap.Fill(_dtResult);
                    List<TransactionRuleDTO> lstRules = null;
                    if (_dtResult != null && _dtResult.Rows.Count > 0)
                    {
                        lstRules = new List<TransactionRuleDTO>();
                        TransactionRuleDTO rule = null;
                        foreach (DataRow dr in _dtResult.Rows)
                        {
                            rule = new TransactionRuleDTO();
                            if (!string.IsNullOrEmpty(dr["TranRuleId"].ToString()))
                            {
                                rule.TranRuleId = (int)dr["TranRuleId"];
                            }
                            else
                            {
                                rule.TranRuleId = -1;
                            }
                            rule.Standard = new StandardDTO();
                            rule.Standard.StandardName = dr["StandardName"].ToString();
                            rule.Section = new SectionDTO();
                            rule.Section.SectionName = dr["SectionName"].ToString();
                            rule.TranMaster = new TransactionMasterDTO();
                            rule.TranMaster.TransactionName = dr["TransactionName"].ToString();
                            rule.RuleName = dr["RuleName"].ToString();
                            rule.ActualAmount = string.IsNullOrEmpty(dr["ActualAmount"].ToString()) ? 0.0 : double.Parse(dr["ActualAmount"].ToString());
                            lstRules.Add(rule);
                        }
                    }
                    return lstRules;
                }
                catch (Exception exp)
                {
                    _logger.Log(exp);
                    throw exp;
                }
            }
        }

        public List<TransactionRuleDTO> GetNoneLevelRules(int location, int? transactionMasterId = null)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "SELECT tr.TranRuleId, tm.TranMasterId, tm.TransactionName, tr.RuleName, tr.ActualAmount FROM transactionmaster tm LEFT OUTER JOIN transactionrule tr ON tm.TranMasterId=tr.TranMasterId WHERE tm.Active=1 AND (tm.IsDifferentTo='NONE' OR tm.IsDifferentTo IS NULL) AND tr.LocationId=@locationId";
                    command.Parameters.Add("@locationId", MySqlDbType.Int32).Value = location;
                    if (transactionMasterId != null && transactionMasterId.Value > 0)
                    {
                        command.CommandText += " AND tm.TranMasterId=@tmId";
                        command.Parameters.Add("@tmId", MySqlDbType.Int32).Value = transactionMasterId.Value;
                    }

                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    MySqlDataAdapter dataAdap = new MySqlDataAdapter(command);
                    _dtResult = new DataTable("TRANS_RULE");
                    dataAdap.Fill(_dtResult);
                    List<TransactionRuleDTO> lstRules = null;
                    if (_dtResult != null && _dtResult.Rows.Count > 0)
                    {
                        lstRules = new List<TransactionRuleDTO>();
                        TransactionRuleDTO rule = null;
                        foreach (DataRow dr in _dtResult.Rows)
                        {
                            rule = new TransactionRuleDTO();
                            if (!string.IsNullOrEmpty(dr["TranRuleId"].ToString()))
                            {
                                rule.TranRuleId = (int)dr["TranRuleId"];
                            }
                            else
                            {
                                rule.TranRuleId = -1;
                            }
                            rule.TranMaster = new TransactionMasterDTO();
                            rule.TranMaster.TransactionName = dr["TransactionName"].ToString();
                            rule.RuleName = dr["RuleName"].ToString();
                            rule.ActualAmount = string.IsNullOrEmpty(dr["ActualAmount"].ToString()) ? 0.0 : double.Parse(dr["ActualAmount"].ToString());
                            lstRules.Add(rule);
                        }
                    }
                    return lstRules;
                }
                catch (Exception exp)
                {
                    _logger.Log(exp);
                    throw exp;
                }
            }
        }


        public int? GetFirstDueAfterDays(int trRuleId)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                int? retValue = null;
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "SELECT FirstDueAfterDays FROM transactionrule WHERE TranRuleId=@ruleId";
                    command.Parameters.Add("@ruleId", MySqlDbType.Int32).Value = trRuleId;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    _dtResult = new DataTable();
                    MySqlDataAdapter mDa = new MySqlDataAdapter(command);
                    mDa.Fill(_dtResult);
                    int validator = 0;
                    if(_dtResult!=null && _dtResult.Rows.Count>0 && Int32.TryParse(_dtResult.Rows[0]["FirstDueAfterDays"].ToString(), out validator))
                    {
                        retValue = validator;
                    }
                    return retValue;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }
    }
}
