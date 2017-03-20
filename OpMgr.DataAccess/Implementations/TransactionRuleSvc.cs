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
                    command.CommandText = "INSERT INTO transactionrule(TranMasterId, UserMasterId, StandardId, SectionId, FirstDueAfterdays, DueDateIncreasesBy, PenaltyCalculatedIn, PenaltyAmount, ActualAmount, ClassTypeId, RuleName, PenaltyTransactionType, PenaltyTranRuleId)" + 
                                          " VALUES (@tmId,@umId,@stdId,@secId,@frstDueAfter,@dueDTIncreasesBy,@penaltyCalcIn,@penaltyAmt,@actualAmt,@ctId,@ruleName,@penaltyTrType,@penaltyTrRuleId)";
                    if(data.TranMaster!=null && data.TranMaster.TranMasterId>0)
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
                    if (!string.IsNullOrEmpty(data.PenaltyCalculatedIn))
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
                        command.Parameters.Add("@ruleName", MySqlDbType.String).Value = DBNull.Value;
                    }
                    else
                    {
                        command.Parameters.Add("@ruleName", MySqlDbType.String).Value = DBNull.Value;
                    }
                    if(!string.IsNullOrEmpty(data.PenaltyTransactionType))
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
                    command.CommandText = "UPDATE transactionrule SET TranMasterId=@tmId, UserMasterId=@umId, StandardId=@stdId, SectionId=@secId, FirstDueAfterdays=@frstDueAfter, DueDateIncreasesBy=@dueDTIncreasesBy, PenaltyCalculatedIn=@penaltyCalcIn, PenaltyAmount=@penaltyAmt, ActualAmount=@actualAmt, ClassTypeId=@ctId, RuleName=@ruleName, PenaltyTransactionType=@penaltyTrType, PenaltyTranRuleId=@penaltyTrRuleId WHERE TranRuleId=@trRuleId";
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
                    if (!string.IsNullOrEmpty(data.PenaltyCalculatedIn))
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
                        command.Parameters.Add("@ruleName", MySqlDbType.String).Value = DBNull.Value;
                    }
                    else
                    {
                        command.Parameters.Add("@ruleName", MySqlDbType.String).Value = DBNull.Value;
                    }
                    if (!string.IsNullOrEmpty(data.PenaltyTransactionType))
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
                    command.CommandText = "SELECT TranRuleId, TranMasterId, UserMasterId, StandardId, SectionId, FirstDueAfterdays, DueDateIncreasesBy, PenaltyCalculatedIn, PenaltyAmount, ActualAmount, ClassTypeId, RuleName, PenaltyTransactionType, PenaltyTranRuleId from transactionrule where TranRuleId=@trRuleId";
                    command.Parameters.Add("@trRuleId", MySqlDbType.Int32).Value = rowId;
                    DataTable dtResult = new DataTable();
                    MySqlDataAdapter mDa = new MySqlDataAdapter(command);
                    mDa.Fill(dtResult);
                    if(dtResult!=null && dtResult.Rows.Count>0)
                    {
                        status.ReturnObj = new TransactionRuleDTO();
                        status.ReturnObj.TranRuleId = (int)dtResult.Rows[0]["TranRuleId"];
                        status.ReturnObj.TranMaster = new TransactionMasterDTO();
                        status.ReturnObj.TranMaster.TranMasterId = string.IsNullOrEmpty(dtResult.Rows[0]["TranMasterId"].ToString()) ? -1 : int.Parse(dtResult.Rows[0]["TranMasterId"].ToString());
                        status.ReturnObj.UserDTO = new UserMasterDTO();
                        status.ReturnObj.UserDTO.UserMasterId = string.IsNullOrEmpty(dtResult.Rows[0]["UserMasterId"].ToString()) ? -1 : int.Parse(dtResult.Rows[0]["UserMasterId"].ToString());
                        status.ReturnObj.Standard = new StandardDTO();
                        status.ReturnObj.Standard.StandardId = string.IsNullOrEmpty(dtResult.Rows[0]["StandardId"].ToString()) ? -1 : int.Parse(dtResult.Rows[0]["StandardId"].ToString());
                        status.ReturnObj.Section = new SectionDTO();
                        status.ReturnObj.Section.SectionId = string.IsNullOrEmpty(dtResult.Rows[0]["SectionId"].ToString()) ? -1 : int.Parse(dtResult.Rows[0]["SectionId"].ToString());
                        status.ReturnObj.FirstDueAfterDays = string.IsNullOrEmpty(dtResult.Rows[0]["FirstDueAfterdays"].ToString()) ? -1 : int.Parse(dtResult.Rows[0]["FirstDueAfterdays"].ToString());
                        status.ReturnObj.DueDateIncreasesBy = string.IsNullOrEmpty(dtResult.Rows[0]["DueDateIncreasesBy"].ToString()) ? -1 : int.Parse(dtResult.Rows[0]["DueDateIncreasesBy"].ToString());
                        status.ReturnObj.PenaltyCalculatedIn = dtResult.Rows[0]["PenaltyCalculatedIn"].ToString();
                        status.ReturnObj.PenaltyAmount = string.IsNullOrEmpty(dtResult.Rows[0]["PenaltyAmount"].ToString()) ? 0.0 : double.Parse(dtResult.Rows[0]["PenaltyAmount"].ToString());
                        status.ReturnObj.ActualAmount = string.IsNullOrEmpty(dtResult.Rows[0]["ActualAmount"].ToString()) ? 0.0 : double.Parse(dtResult.Rows[0]["ActualAmount"].ToString());
                        status.ReturnObj.ClassType = new ClassTypeDTO();
                        status.ReturnObj.ClassType.ClassTypeId = string.IsNullOrEmpty(dtResult.Rows[0]["ClassTypeId"].ToString()) ? -1 : int.Parse(dtResult.Rows[0]["ClassTypeId"].ToString());
                        status.ReturnObj.RuleName = dtResult.Rows[0]["ClassTypeId"].ToString();
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
                    command.CommandText = "SELECT TranRuleId, TranMasterId, StandardId, SectionId, UserMasterId, ClassTypeId, FirstDuedateAfterdays, DueDateincreasesBy, PenaltyCalculatedIn, PenaltyAmount, ActualAmount, PenaltyTransactionType, PenaltyTranRuleId FROM TransactionRule WHERE Active=1";
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
    }
}
