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
    public class TransactionLogSvc : ITransactionLogSvc
    {
        private IConfigSvc _configSvc;
        private DataTable _dtData;
        private DataSet _dsData;
        private ILogSvc _logger;

        private DataTable _dtResult;

        public void Dispose()
        {
            if (_dtResult != null)
            {
                _dtResult.Dispose();
                _dtResult = null;
            }
        }

        public TransactionLogSvc(IConfigSvc configSvc, ILogSvc logger)
        {
            _configSvc = configSvc;
            _logger = logger;
        }

        public StatusDTO<TransactionLogDTO> Insert(TransactionLogDTO data)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    StatusDTO<TransactionLogDTO> status = new StatusDTO<TransactionLogDTO>();
                    if (data != null)
                    {
                        dbSvc.OpenConnection();
                        MySqlCommand command = new MySqlCommand();
                        command.CommandText = "INSERT INTO TransactionLog (CreatedBy, CreatedDate, UpdatedBy, UpdatedDate, Active, UserMasterId, TransactionDate, " +
                                               "TransactionDueDate, TransactionPreviousDueDate, ParentTransactionLogId, IsCompleted, CompletedOn, AmountImposed, " +
                                               "AmountGiven, DueAmount, TransferMode, locationId, StandardSectionId, TransactionType, HasPenalty, OriginalTransactionLogId, TranRuleId, PenaltyTransactionRule) VALUES " +
                                               "(@createdBy, @createdDate, @updatedBy, @updatedDate, @active, @userMasterId, @transactionDate, " +
                                               "@transactionDueDate, @transPrevDueDate, @parentTransLogId, @isCompleted, @completedOn, @amountImposed, " +
                                               "@amountGiven, @dueAmount, @transferMode, @locationId, @standardSectionId, @transactionType, @hasPenalty, @originalTransLogId, @transRuleId, @penTrnsRuleId); SELECT LAST_INSERT_ID();";

                        command.Connection = dbSvc.GetConnection() as MySqlConnection;

                        if (data.CreatedBy != null)
                        {
                            command.Parameters.Add("@createdBy", MySqlDbType.Int32).Value = data.CreatedBy.UserMasterId;
                        }
                        else
                        {
                            command.Parameters.Add("@createdBy", MySqlDbType.Int32).Value = DBNull.Value;
                        }

                        if (data.CreatedDate != null)
                        {
                            command.Parameters.Add("@createdDate", MySqlDbType.DateTime).Value = data.CreatedDate;
                        }
                        else
                        {
                            command.Parameters.Add("@createdDate", MySqlDbType.DateTime).Value = DBNull.Value;
                        }

                        if (data.UpdatedBy != null)
                        {
                            command.Parameters.Add("@updatedBy", MySqlDbType.Int32).Value = data.UpdatedBy.UserMasterId;
                        }
                        else
                        {
                            command.Parameters.Add("@updatedBy", MySqlDbType.Int32).Value = DBNull.Value;
                        }

                        if (data.UpdatedDate != null)
                        {
                            command.Parameters.Add("@updatedDate", MySqlDbType.DateTime).Value = data.UpdatedDate;
                        }
                        else
                        {
                            command.Parameters.Add("@updatedDate", MySqlDbType.DateTime).Value = DBNull.Value;
                        }

                        if (data.Active != null)
                        {
                            command.Parameters.Add("@active", MySqlDbType.Bit).Value = data.Active;
                        }
                        else
                        {
                            command.Parameters.Add("@active", MySqlDbType.Bit).Value = DBNull.Value;
                        }

                        if (data.User != null)
                        {
                            command.Parameters.Add("@userMasterId", MySqlDbType.Int32).Value = data.User.UserMasterId;
                        }
                        else
                        {
                            command.Parameters.Add("@userMasterId", MySqlDbType.Int32).Value = DBNull.Value;
                        }

                        if (data.TransactionDate != null)
                        {
                            command.Parameters.Add("@transactionDate", MySqlDbType.DateTime).Value = data.TransactionDate;
                        }
                        else
                        {
                            command.Parameters.Add("@transactionDate", MySqlDbType.DateTime).Value = DBNull.Value;
                        }

                        if (data.TransactionDueDate != null)
                        {
                            command.Parameters.Add("@transactionDueDate", MySqlDbType.DateTime).Value = data.TransactionDueDate;
                        }
                        else
                        {
                            command.Parameters.Add("@transactionDueDate", MySqlDbType.DateTime).Value = DBNull.Value;
                        }

                        if (data.TransactionPreviousDueDate != null)
                        {
                            command.Parameters.Add("@transPrevDueDate", MySqlDbType.DateTime).Value = data.TransactionPreviousDueDate;
                        }
                        else
                        {
                            command.Parameters.Add("@transPrevDueDate", MySqlDbType.DateTime).Value = DBNull.Value;
                        }

                        if (data.ParentTransactionLogId != null)
                        {
                            command.Parameters.Add("@parentTransLogId", MySqlDbType.Int32).Value = data.ParentTransactionLogId.TransactionLogId;
                        }
                        else
                        {
                            command.Parameters.Add("@parentTransLogId", MySqlDbType.Int32).Value = DBNull.Value;
                        }

                        if (data.IsCompleted != null)
                        {
                            command.Parameters.Add("@isCompleted", MySqlDbType.Bit).Value = data.IsCompleted;
                        }
                        else
                        {
                            command.Parameters.Add("@isCompleted", MySqlDbType.Bit).Value = DBNull.Value;
                        }

                        if (data.CompletedOn != null)
                        {
                            command.Parameters.Add("@completedOn", MySqlDbType.DateTime).Value = data.CompletedOn;
                        }
                        else
                        {
                            command.Parameters.Add("@completedOn", MySqlDbType.DateTime).Value = DBNull.Value;
                        }

                        if (data.AmountImposed != null)
                        {
                            command.Parameters.Add("@amountImposed", MySqlDbType.Double).Value = data.AmountImposed;
                        }
                        else
                        {
                            command.Parameters.Add("@amountImposed", MySqlDbType.Double).Value = DBNull.Value;
                        }

                        if (data.AmountGiven != null)
                        {
                            command.Parameters.Add("@amountGiven", MySqlDbType.Double).Value = data.AmountGiven;
                        }
                        else
                        {
                            command.Parameters.Add("@amountGiven", MySqlDbType.Double).Value = DBNull.Value;
                        }

                        if (data.DueAmount != null)
                        {
                            command.Parameters.Add("@dueAmount", MySqlDbType.Double).Value = data.DueAmount;
                        }
                        else
                        {
                            command.Parameters.Add("@dueAmount", MySqlDbType.Double).Value = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(data.TransferMode))
                        {
                            command.Parameters.Add("@transferMode", MySqlDbType.String).Value = data.TransferMode;
                        }
                        else
                        {
                            command.Parameters.Add("@transferMode", MySqlDbType.String).Value = DBNull.Value;
                        }

                        if (data.Location != null)
                        {
                            command.Parameters.Add("@locationId", MySqlDbType.Int32).Value = data.Location.LocationId;
                        }
                        else
                        {
                            command.Parameters.Add("@locationId", MySqlDbType.Int32).Value = DBNull.Value;
                        }

                        if (data.StandardSectionMap != null)
                        {
                            command.Parameters.Add("@standardSectionId", MySqlDbType.Int32).Value = data.StandardSectionMap.StandardSectionId;
                        }
                        else
                        {
                            command.Parameters.Add("@standardSectionId", MySqlDbType.Int32).Value = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(data.TransactionType))
                        {
                            command.Parameters.Add("@transactionType", MySqlDbType.String).Value = data.TransactionType;
                        }
                        else
                        {
                            command.Parameters.Add("@transactionType", MySqlDbType.String).Value = DBNull.Value;
                        }

                        if (data.HasPenalty != null)
                        {
                            command.Parameters.Add("@hasPenalty", MySqlDbType.Bit).Value = data.HasPenalty;
                        }
                        else
                        {
                            command.Parameters.Add("@hasPenalty", MySqlDbType.Bit).Value = DBNull.Value;
                        }

                        if (data.OriginalTransLog != null)
                        {
                            command.Parameters.Add("@originalTransLogId", MySqlDbType.Int32).Value = data.OriginalTransLog.TransactionLogId;
                        }
                        else
                        {
                            command.Parameters.Add("@originalTransLogId", MySqlDbType.Int32).Value = DBNull.Value;
                        }

                        if (data.TransactionRule != null)
                        {
                            command.Parameters.Add("@transRuleId", MySqlDbType.Int32).Value = data.TransactionRule.TranRuleId;
                        }
                        else
                        {
                            command.Parameters.Add("@transRuleId", MySqlDbType.Int32).Value = DBNull.Value;
                        }

                        if (data.PenaltyTransactionRule != null)
                        {
                            command.Parameters.Add("@penTrnsRuleId", MySqlDbType.Int32).Value = data.PenaltyTransactionRule.TranRuleId;
                        }
                        else
                        {
                            command.Parameters.Add("@penTrnsRuleId", MySqlDbType.Int32).Value = DBNull.Value;
                        }

                        var retData = command.ExecuteScalar();
                        if (retData != null)
                        {
                            data.TransactionLogId = int.Parse(retData.ToString());
                            status.IsSuccess = true;
                            status.IsException = false;
                            status.ReturnObj = data;
                        }

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

        public StatusDTO<TransactionLogDTO> Update(TransactionLogDTO data)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    StatusDTO<TransactionLogDTO> status = new StatusDTO<TransactionLogDTO>();
                    if (data != null)
                    {
                        dbSvc.OpenConnection();
                        MySqlCommand command = new MySqlCommand();
                        command.CommandText = "UPDATE TransactionLog SET IsCompleted=@completed, CompletedOn=@completedOn" +
                                              ", DueAmount=@due, AmountGiven=@given WHERE TransactionLogId=@trLogId";
                        if (data.IsCompleted != null)
                        {
                            command.Parameters.Add("@completed", MySqlDbType.Bit).Value = data.IsCompleted.Value;
                        }
                        else
                        {
                            command.Parameters.Add("@completed", MySqlDbType.Bit).Value = DBNull.Value;
                        }
                        if (data.CompletedOn != null)
                        {
                            command.Parameters.Add("@completedOn", MySqlDbType.DateTime).Value = data.CompletedOn.Value;
                        }
                        else
                        {
                            command.Parameters.Add("@completedOn", MySqlDbType.DateTime).Value = DBNull.Value;
                        }
                        if (data.DueAmount != null)
                        {
                            command.Parameters.Add("@due", MySqlDbType.Double).Value = data.DueAmount.Value;
                        }
                        else
                        {
                            command.Parameters.Add("@due", MySqlDbType.Double).Value = DBNull.Value;
                        }
                        if (data.AmountGiven != null)
                        {
                            command.Parameters.Add("@given", MySqlDbType.Double).Value = data.AmountGiven.Value;
                        }
                        else
                        {
                            command.Parameters.Add("@given", MySqlDbType.Double).Value = DBNull.Value;
                        }
                        command.Parameters.Add("@trLogId", MySqlDbType.Int32).Value = data.TransactionLogId;
                        command.Connection = dbSvc.GetConnection() as MySqlConnection;
                        if (command.ExecuteNonQuery() > 0)
                        {
                            status.IsSuccess = true;
                            status.IsException = false;
                        }
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

        public StatusDTO<TransactionLogDTO> Delete(TransactionLogDTO data)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<List<TransactionLogDTO>> GetPendingPrincipalApprovals(TransactionLogDTO data)
        {
            StatusDTO<List<TransactionLogDTO>> status = new StatusDTO<List<TransactionLogDTO>>();
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "SELECT DISTINCT um.FName, um.MName, um.LName, tl.TransactionLogId" +
                                          ", tl.TransactionDate, tl.TransactionDueDate, tl.ParentTransactionLogId, tl.IsCompleted, tm.TransactionName, tl.CompletedOn, tl.AmountImposed, tl.AmountGiven, tl.DueAmount, tl.AdjustedAmount" +
                                          ", tl.TransferMode, l.LocationDescription, tl.TransactionType, tl.HasPenalty, tl.OriginalTransactionLogId, tr.RuleName FROM transactionlog tl LEFT JOIN usermaster um" +
                                          " ON um.UserMasterId=tl.UserMasterId LEFT JOIN transactionmaster tm ON tm.TranMasterId=tl.TranMasterId LEFT JOIN transactionrule tr ON tr.TranRuleId=tl.TranRuleId" +
                                          " LEFT JOIN studentinfo si ON tl.UserMasterId=si.UserMasterId LEFT JOIN employeedetails ed ON ed.UserMasterId=tl.UserMasterId LEFT JOIN location l ON l.LocationId=tl.LocationId WHERE tl.Active=1 AND tl.IsPrincipalApproved=0 AND tl.AdjustedAmount IS NOT NULL";

                    if (data.SearchFrom != null)
                    {
                        command.CommandText = command.CommandText + " AND tl.TransactionDate>=@fromDate";
                        command.Parameters.Add("@fromDate", MySqlDbType.DateTime).Value = data.SearchFrom.Value;
                    }
                    if (data.SearchUpto != null)
                    {
                        command.CommandText = command.CommandText + " AND tl.TransactionDate<=@toDate";
                        command.Parameters.Add("@toDate", MySqlDbType.DateTime).Value = data.SearchUpto.Value;
                    }

                    if (data.User != null && data.User.UserMasterId > 0)
                    {
                        command.CommandText = command.CommandText + " AND tl.UserMasterId=@umId";
                        command.Parameters.Add("@umId", MySqlDbType.Int32).Value = data.User.UserMasterId;
                    }
                    else
                    {
                        if (data.User != null && !string.IsNullOrEmpty(data.User.FName))
                        {
                            command.CommandText = command.CommandText + " AND um.FName like @fName";
                            command.Parameters.Add("@fName", MySqlDbType.String).Value = "%" + data.User.FName + "%";
                        }
                        if (data.User != null && !string.IsNullOrEmpty(data.User.LName))
                        {
                            command.CommandText = command.CommandText + " AND um.LName like @lName";
                            command.Parameters.Add("@lName", MySqlDbType.String).Value = "%" + data.User.LName + "%";
                        }
                    }
                    if (!string.IsNullOrEmpty(data.SearchStudRegId))
                    {
                        command.CommandText = command.CommandText + " AND si.RegistrationNumber=@regNo";
                        command.Parameters.Add("@regNo", MySqlDbType.String).Value = data.SearchStudRegId;
                    }
                    if (!string.IsNullOrEmpty(data.SearchEmployeeId))
                    {
                        command.CommandText = command.CommandText + " AND ed.StaffEmployeeId=@empId";
                        command.Parameters.Add("@empId", MySqlDbType.String).Value = data.SearchEmployeeId;
                    }
                    if (data.SearchStdSectionId != null && data.SearchStdSectionId.Value > 0)
                    {
                        command.CommandText = command.CommandText + " AND tl.StandardSectionId=@stdSecId";
                        command.Parameters.Add("@stdSecId", MySqlDbType.Int32).Value = data.SearchStdSectionId;
                    }
                    if (!string.IsNullOrEmpty(data.TransactionType) && !string.Equals(data.TransactionType, "-1"))
                    {
                        command.CommandText = command.CommandText + " AND tl.TransactionType=@trType";
                        command.Parameters.Add("@trType", MySqlDbType.String).Value = data.TransactionType;
                    }

                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    MySqlDataAdapter mDA = new MySqlDataAdapter(command);
                    DataTable dtData = new DataTable("TR_DATA");
                    mDA.Fill(dtData);

                    status.IsSuccess = true;
                    status.ReturnObj = null;

                    if (dtData != null && dtData.Rows.Count > 0)
                    {
                        status.ReturnObj = new List<TransactionLogDTO>();
                        TransactionLogDTO trLog = null;
                        foreach (DataRow dr in dtData.Rows)
                        {
                            trLog = new TransactionLogDTO();
                            trLog.User = new UserMasterDTO();
                            trLog.User.FName = dr["FName"].ToString();
                            trLog.User.MName = dr["MName"].ToString();
                            trLog.User.LName = dr["LName"].ToString();
                            trLog.TransactionLogId = (int)dr["TransactionLogId"];
                            trLog.TransactionDate = (DateTime)dr["TransactionDate"];
                            trLog.TransactionDueDate = (DateTime)dr["TransactionDueDate"];
                            trLog.ParentTransactionLogId = new TransactionLogDTO();
                            trLog.ParentTransactionLogId.TransactionLogId = string.IsNullOrEmpty(dr["ParentTransactionLogId"].ToString()) ? -1 : (int)dr["ParentTransactionLogId"];
                            trLog.IsCompleted = string.Equals(dr["IsCompleted"].ToString(), "1") ? true : false;
                            trLog.CompletedOn = string.IsNullOrEmpty(dr["CompletedOn"].ToString()) ? null : (DateTime?)dr["CompletedOn"];
                            trLog.AmountImposed = double.Parse(dr["AmountImposed"].ToString());
                            trLog.AmountGiven = string.IsNullOrEmpty(dr["AmountGiven"].ToString()) ? 0.0 : double.Parse(dr["AmountGiven"].ToString());
                            trLog.DueAmount = string.IsNullOrEmpty(dr["DueAmount"].ToString()) ? 0.0 : double.Parse(dr["DueAmount"].ToString());
                            trLog.TransferMode = dr["TransferMode"].ToString();
                            trLog.Location = new LocationDTO();
                            trLog.Location.LocationDescription = dr["LocationDescription"].ToString();
                            trLog.TransactionType = dr["TransactionType"].ToString();
                            trLog.HasPenalty = string.Equals(dr["HasPenalty"].ToString(), "1") ? true : false;
                            trLog.OriginalTransLog = new TransactionLogDTO();
                            trLog.OriginalTransLog.TransactionLogId = string.IsNullOrEmpty(dr["OriginalTransactionLogId"].ToString()) ? -1 : (int)dr["OriginalTransactionLogId"];
                            trLog.TransactionRule = new TransactionRuleDTO();
                            trLog.TransactionRule.RuleName = dr["RuleName"].ToString();
                            trLog.AdjustedAmount = string.IsNullOrEmpty(dr["AdjustedAmount"].ToString()) ? 0.0 : double.Parse(dr["AdjustedAmount"].ToString());
                            status.ReturnObj.Add(trLog);
                        }
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

        public StatusDTO<List<TransactionLogDTO>> Select(TransactionLogDTO data)
        {
            StatusDTO<List<TransactionLogDTO>> status = new StatusDTO<List<TransactionLogDTO>>();
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "SELECT DISTINCT um.FName, um.MName, um.LName, tl.TransactionLogId" +
                                          ", tl.TransactionDate, tl.TransactionDueDate, tl.ParentTransactionLogId, tl.IsCompleted, tm.TransactionName, tl.CompletedOn, tl.AmountImposed, tl.AmountGiven, tl.DueAmount" +
                                          ", tl.TransferMode, l.LocationDescription, tl.TransactionType, tl.HasPenalty, tl.OriginalTransactionLogId, tr.RuleName FROM transactionlog tl LEFT JOIN usermaster um" +
                                          " ON um.UserMasterId=tl.UserMasterId LEFT JOIN transactionmaster tm ON tm.TranMasterId=tl.TranMasterId LEFT JOIN transactionrule tr ON tr.TranRuleId=tl.TranRuleId" +
                                          " LEFT JOIN studentinfo si ON tl.UserMasterId=si.UserMasterId LEFT JOIN employeedetails ed ON ed.UserMasterId=tl.UserMasterId LEFT JOIN location l ON l.LocationId=tl.LocationId WHERE tl.Active=1";

                    if (data.SearchFrom != null)
                    {
                        command.CommandText = command.CommandText + " AND tl.TransactionDate>=@fromDate";
                        command.Parameters.Add("@fromDate", MySqlDbType.DateTime).Value = data.SearchFrom.Value;
                    }
                    if (data.SearchUpto != null)
                    {
                        command.CommandText = command.CommandText + " AND tl.TransactionDate<=@toDate";
                        command.Parameters.Add("@toDate", MySqlDbType.DateTime).Value = data.SearchUpto.Value;
                    }

                    if (data.User != null && data.User.UserMasterId > 0)
                    {
                        command.CommandText = command.CommandText + " AND tl.UserMasterId=@umId";
                        command.Parameters.Add("@umId", MySqlDbType.Int32).Value = data.User.UserMasterId;
                    }
                    else
                    {
                        if (data.User != null && !string.IsNullOrEmpty(data.User.FName))
                        {
                            command.CommandText = command.CommandText + " AND um.FName like @fName";
                            command.Parameters.Add("@fName", MySqlDbType.String).Value = "%" + data.User.FName + "%";
                        }
                        if (data.User != null && !string.IsNullOrEmpty(data.User.LName))
                        {
                            command.CommandText = command.CommandText + " AND um.LName like @lName";
                            command.Parameters.Add("@lName", MySqlDbType.String).Value = "%" + data.User.LName + "%";
                        }
                    }
                    if (!string.IsNullOrEmpty(data.SearchStudRegId))
                    {
                        command.CommandText = command.CommandText + " AND si.RegistrationNumber=@regNo";
                        command.Parameters.Add("@regNo", MySqlDbType.String).Value = data.SearchStudRegId;
                    }
                    if (!string.IsNullOrEmpty(data.SearchEmployeeId))
                    {
                        command.CommandText = command.CommandText + " AND ed.StaffEmployeeId=@empId";
                        command.Parameters.Add("@empId", MySqlDbType.String).Value = data.SearchEmployeeId;
                    }
                    if (data.SearchStdSectionId != null && data.SearchStdSectionId.Value > 0)
                    {
                        command.CommandText = command.CommandText + " AND tl.StandardSectionId=@stdSecId";
                        command.Parameters.Add("@stdSecId", MySqlDbType.Int32).Value = data.SearchStdSectionId;
                    }
                    if (!string.IsNullOrEmpty(data.TransactionType) && !string.Equals(data.TransactionType, "-1"))
                    {
                        command.CommandText = command.CommandText + " AND tl.TransactionType=@trType";
                        command.Parameters.Add("@trType", MySqlDbType.String).Value = data.TransactionType;
                    }

                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    MySqlDataAdapter mDA = new MySqlDataAdapter(command);
                    DataTable dtData = new DataTable("TR_DATA");
                    mDA.Fill(dtData);

                    status.IsSuccess = true;
                    status.ReturnObj = null;

                    if (dtData != null && dtData.Rows.Count > 0)
                    {
                        status.ReturnObj = new List<TransactionLogDTO>();
                        TransactionLogDTO trLog = null;
                        foreach (DataRow dr in dtData.Rows)
                        {
                            trLog = new TransactionLogDTO();
                            trLog.User = new UserMasterDTO();
                            trLog.User.FName = dr["FName"].ToString();
                            trLog.User.MName = dr["MName"].ToString();
                            trLog.User.LName = dr["LName"].ToString();
                            trLog.TransactionLogId = (int)dr["TransactionLogId"];
                            trLog.TransactionDate = (DateTime)dr["TransactionDate"];
                            trLog.TransactionDueDate = (DateTime)dr["TransactionDueDate"];
                            trLog.ParentTransactionLogId = new TransactionLogDTO();
                            trLog.ParentTransactionLogId.TransactionLogId = string.IsNullOrEmpty(dr["ParentTransactionLogId"].ToString()) ? -1 : (int)dr["ParentTransactionLogId"];
                            trLog.IsCompleted = string.Equals(dr["IsCompleted"].ToString(), "1") ? true : false;
                            trLog.CompletedOn = string.IsNullOrEmpty(dr["CompletedOn"].ToString()) ? null : (DateTime?)dr["CompletedOn"];
                            trLog.AmountImposed = double.Parse(dr["AmountImposed"].ToString());
                            trLog.AmountGiven = string.IsNullOrEmpty(dr["AmountGiven"].ToString()) ? 0.0 : double.Parse(dr["AmountGiven"].ToString());
                            trLog.DueAmount = string.IsNullOrEmpty(dr["DueAmount"].ToString()) ? 0.0 : double.Parse(dr["DueAmount"].ToString());
                            trLog.TransferMode = dr["TransferMode"].ToString();
                            trLog.Location = new LocationDTO();
                            trLog.Location.LocationDescription = dr["LocationDescription"].ToString();
                            trLog.TransactionType = dr["TransactionType"].ToString();
                            trLog.HasPenalty = string.Equals(dr["HasPenalty"].ToString(), "1") ? true : false;
                            trLog.OriginalTransLog = new TransactionLogDTO();
                            trLog.OriginalTransLog.TransactionLogId = string.IsNullOrEmpty(dr["OriginalTransactionLogId"].ToString()) ? -1 : (int)dr["OriginalTransactionLogId"];
                            trLog.TransactionRule = new TransactionRuleDTO();
                            trLog.TransactionRule.RuleName = dr["RuleName"].ToString();
                            status.ReturnObj.Add(trLog);
                        }
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

        public StatusDTO<TransactionLogDTO> Select(int rowId)
        {
            StatusDTO<TransactionLogDTO> status = new StatusDTO<TransactionLogDTO>();
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "SELECT um.FName, um.MName, um.LName, tl.TransactionLogId" +
                                          ", tl.TransactionDate, tl.TransactionDueDate, tl.ParentTransactionLogId, tl.IsCompleted, tm.TransactionName, tl.CompletedOn, tl.AmountImposed, tl.AmountGiven, tl.DueAmount" +
                                          ", tl.TransferMode, l.LocationDescription, tl.TransactionType, tl.HasPenalty, tl.OriginalTransactionLogId, tr.RuleName FROM transactionlog tl LEFT JOIN usermaster um" +
                                          " ON um.UserMasterId=tl.UserMasterId LEFT JOIN transactionmaster tm ON tm.TranMasterId=tl.TranMasterId LEFT JOIN transactionrule tr ON tr.TranRuleId=tl.TranRuleId" +
                                          " LEFT JOIN studentinfo si ON tl.UserMasterId=si.UserMasterId LEFT JOIN employeedetails ed ON ed.UserMasterId=tl.UserMasterId LEFT JOIN location l ON l.LocationId=tl.LocationId WHERE tl.TransactionLogId=@trLogId";

                    command.Parameters.Add("@trLogId", MySqlDbType.Int32).Value = rowId;

                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    MySqlDataAdapter mDA = new MySqlDataAdapter(command);
                    DataTable dtData = new DataTable("TR_DATA");
                    mDA.Fill(dtData);

                    status.IsSuccess = true;
                    status.ReturnObj = null;

                    if (dtData != null && dtData.Rows.Count > 0)
                    {
                        //status.ReturnObj = new TransactionLogDTO();
                        TransactionLogDTO trLog = null;
                        trLog = new TransactionLogDTO();
                        trLog.User = new UserMasterDTO();
                        trLog.User.FName = dtData.Rows[0]["FName"].ToString();
                        trLog.User.MName = dtData.Rows[0]["MName"].ToString();
                        trLog.User.LName = dtData.Rows[0]["LName"].ToString();
                        trLog.TransactionLogId = (int)dtData.Rows[0]["TransactionLogId"];
                        trLog.TransactionDate = (DateTime)dtData.Rows[0]["TransactionDate"];
                        trLog.TransactionDueDate = (DateTime)dtData.Rows[0]["TransactionDueDate"];
                        trLog.ParentTransactionLogId = new TransactionLogDTO();
                        trLog.ParentTransactionLogId.TransactionLogId = string.IsNullOrEmpty(dtData.Rows[0]["ParentTransactionLogId"].ToString()) ? -1 : (int)dtData.Rows[0]["ParentTransactionLogId"];
                        trLog.IsCompleted = string.Equals(dtData.Rows[0]["IsCompleted"].ToString(), "1") ? true : false;
                        trLog.CompletedOn = string.IsNullOrEmpty(dtData.Rows[0]["CompletedOn"].ToString()) ? null : (DateTime?)dtData.Rows[0]["CompletedOn"];
                        trLog.AmountImposed = double.Parse(dtData.Rows[0]["AmountImposed"].ToString());
                        trLog.AmountGiven = string.IsNullOrEmpty(dtData.Rows[0]["AmountGiven"].ToString()) ? 0.0 : double.Parse(dtData.Rows[0]["AmountGiven"].ToString());
                        trLog.DueAmount = string.IsNullOrEmpty(dtData.Rows[0]["DueAmount"].ToString()) ? 0.0 : double.Parse(dtData.Rows[0]["DueAmount"].ToString());
                        trLog.TransferMode = dtData.Rows[0]["TransferMode"].ToString();
                        trLog.Location = new LocationDTO();
                        trLog.Location.LocationDescription = dtData.Rows[0]["LocationDescription"].ToString();
                        trLog.TransactionType = dtData.Rows[0]["TransactionType"].ToString();
                        trLog.HasPenalty = string.Equals(dtData.Rows[0]["HasPenalty"].ToString(), "1") ? true : false;
                        trLog.OriginalTransLog = new TransactionLogDTO();
                        trLog.OriginalTransLog.TransactionLogId = string.IsNullOrEmpty(dtData.Rows[0]["OriginalTransactionLogId"].ToString()) ? -1 : (int)dtData.Rows[0]["OriginalTransactionLogId"];
                        trLog.TransactionRule = new TransactionRuleDTO();
                        trLog.TransactionRule.RuleName = dtData.Rows[0]["RuleName"].ToString();
                        status.ReturnObj = trLog;
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

        public DataTable GetPendingTransactions(DateTime? runDate)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "SELECT TransactionLogId, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate, UserMasterId, TransactionDate, TransactionDueDate, " +
                                           "TransactionPreviousDueDate, ParentTransactionLogId, IsCompleted, CompletedOn, AmountImposed, AmountGiven, DueAmount, TransferMode, " +
                                           "locationId, StandardSectionId, TransactionType, HasPenalty, OriginalTransactionLogId, TranRuleId, PenaltyTransactionRule FROM TransactionLog WHERE Active=1 AND IsCompleted<>1 AND OriginalTransactionLogId IS NULL AND ParentTransactionLogId IS NULL AND TransactionDueDate IS NOT NULL AND TransactionDueDate<@runDate";
                    command.Parameters.Add("@runDate", MySqlDbType.DateTime).Value = runDate.Value.Date;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    MySqlDataAdapter mDA = new MySqlDataAdapter(command);
                    _dtResult = new DataTable();
                    mDA.Fill(_dtResult);
                    return _dtResult;
                }
                catch (Exception exp)
                {
                    _logger.Log(exp);
                    throw exp;
                }
            }
        }

        public bool UpdateHasPenaltyFlag(int trnsLogId, bool? hasPenalty, DateTime dueDate, int penaltyTransactionRule)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "UPDATE TransactionLog SET HasPenalty=@hasPenalty, TransactionDueDate=@dueDate, PenaltyTransactionRule=@penaltyRule WHERE TransactionLogId=@trnsLogId";
                    command.Parameters.Add("@hasPenalty", MySqlDbType.Bit).Value = hasPenalty.Value;
                    command.Parameters.Add("@dueDate", MySqlDbType.DateTime).Value = dueDate;
                    command.Parameters.Add("@penaltyRule", MySqlDbType.Int32).Value = penaltyTransactionRule;
                    command.Parameters.Add("@trnsLogId", MySqlDbType.Int32).Value = trnsLogId;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    return command.ExecuteNonQuery() > 0;
                }
                catch (Exception exp)
                {
                    _logger.Log(exp);
                    throw exp;
                }
            }
        }

        public StatusDTO<List<TransactionLogDTO>> SelectPayment(StudentDTO student)
        {
            StatusDTO<List<TransactionLogDTO>> status = new StatusDTO<List<TransactionLogDTO>>();
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "get_Payment_details";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    if (!String.IsNullOrEmpty(student.RegistrationNumber))
                    {
                        command.Parameters.Add("@RegistrationNo1", MySqlDbType.VarChar).Value = student.RegistrationNumber;
                    }
                    else
                    {
                        command.Parameters.Add("@RegistrationNo1", MySqlDbType.VarChar).Value = DBNull.Value;
                    }
                    if (student.UserDetails != null)
                    {
                        student.UserDetails = new UserMasterDTO();
                        student.UserDetails.Employee = new EmployeeDetailsDTO();
                        if (!String.IsNullOrEmpty(student.UserDetails.Employee.StaffEmployeeId))
                        {
                            command.Parameters.Add("@StaffEmployeeId1", MySqlDbType.VarChar).Value = student.UserDetails.Employee.StaffEmployeeId;
                        }
                    }
                    else
                    {
                        command.Parameters.Add("@StaffEmployeeId1", MySqlDbType.VarChar).Value = DBNull.Value;
                    }
                    MySqlParameter error = new MySqlParameter("@ErrorMsg", MySqlDbType.VarChar);
                    error.Direction = ParameterDirection.Output;
                    command.Parameters.Add(error);

                    MySqlDataAdapter rdr = new MySqlDataAdapter(command);
                    _dsData = new DataSet();
                    rdr.Fill(_dsData);

                    if (_dsData != null && _dsData.Tables.Count > 0 && _dsData.Tables[0].Rows.Count > 0)
                    {
                        status.ReturnObj = new List<TransactionLogDTO>();
                        for (int i = 0; i < _dsData.Tables[0].Rows.Count; i++)
                        {
                            TransactionLogDTO tranlog = new TransactionLogDTO();
                            tranlog.TransactionLogId = Convert.ToInt32(_dsData.Tables[0].Rows[i]["TransactionLogId"]);
                            tranlog.User = new UserMasterDTO();
                            tranlog.User.FName = _dsData.Tables[0].Rows[i]["FName"].ToString();
                            tranlog.User.LName = _dsData.Tables[0].Rows[i]["Lname"].ToString();
                            tranlog.TransactionDate = Convert.ToDateTime(_dsData.Tables[0].Rows[i]["TransactionDate"]);
                            tranlog.TransactionDueDate = Convert.ToDateTime(_dsData.Tables[0].Rows[i]["TransactionDueDate"]);
                            if (!string.IsNullOrEmpty(_dsData.Tables[0].Rows[i]["TransactionPreviousDueDate"].ToString()))
                            {
                                tranlog.TransactionPreviousDueDate = Convert.ToDateTime(_dsData.Tables[0].Rows[i]["TransactionPreviousDueDate"]);
                            }
                            if (!String.IsNullOrEmpty(_dsData.Tables[0].Rows[i]["ParentTransactionLogId"].ToString()))
                            {
                                if (Convert.ToInt32(_dsData.Tables[0].Rows[i]["ParentTransactionLogId"]) > 0 )
                                {
                                    tranlog.ParentTransactionLogId = new TransactionLogDTO();
                                    tranlog.ParentTransactionLogId.TransactionLogId = Convert.ToInt32(_dsData.Tables[0].Rows[i]["ParentTransactionLogId"]);
                                }
                            }
                            tranlog.IsCompleted = Convert.ToBoolean(_dsData.Tables[0].Rows[i]["IsCompleted"]);
                            if (!String.IsNullOrEmpty(_dsData.Tables[0].Rows[i]["CompletedOn"].ToString()))
                            {
                                tranlog.CompletedOn = Convert.ToDateTime(_dsData.Tables[0].Rows[i]["CompletedOn"]);
                            }
                            tranlog.AmountImposed = Convert.ToDouble(_dsData.Tables[0].Rows[i]["AmountImposed"]);
                            tranlog.AmountGiven = Convert.ToDouble(_dsData.Tables[0].Rows[i]["AmountGiven"]);
                            tranlog.DueAmount = Convert.ToDouble(_dsData.Tables[0].Rows[i]["DueAmount"]);
                            tranlog.TransferMode = _dsData.Tables[0].Rows[i]["TransferMode"].ToString();
                            if (!String.IsNullOrEmpty(_dsData.Tables[0].Rows[i]["HasPenalty"].ToString()))
                            {
                                tranlog.HasPenalty = Convert.ToBoolean(_dsData.Tables[0].Rows[i]["HasPenalty"]);
                            }
                            if (!String.IsNullOrEmpty(_dsData.Tables[0].Rows[i]["AdjustedAmount"].ToString()))
                            {
                                tranlog.AdjustedAmount = Convert.ToDouble(_dsData.Tables[0].Rows[i]["AdjustedAmount"]);
                            }
                            if (!String.IsNullOrEmpty(_dsData.Tables[0].Rows[i]["IsPrincipalApproved"].ToString()))
                            {
                                tranlog.IsPrincipalApproved = Convert.ToInt32(_dsData.Tables[0].Rows[i]["IsPrincipalApproved"]);
                            }
                            if (!String.IsNullOrEmpty(_dsData.Tables[0].Rows[i]["OriginalTransactionLogId"].ToString()))
                            {
                                if (Convert.ToInt32(_dsData.Tables[0].Rows[i]["OriginalTransactionLogId"]) > 0)
                                {
                                    tranlog.OriginalTransLog = new TransactionLogDTO();
                                    tranlog.OriginalTransLog.TransactionLogId = Convert.ToInt32(_dsData.Tables[0].Rows[i]["OriginalTransactionLogId"]);
                                }
                            }
                            if (!string.IsNullOrEmpty(_dsData.Tables[0].Rows[i]["TransactionName"].ToString()))
                            {
                                tranlog.TransactionRule = new TransactionRuleDTO();
                                tranlog.TransactionRule.TranMaster = new TransactionMasterDTO();
                                tranlog.TransactionRule.TranMaster.TransactionName = _dsData.Tables[0].Rows[i]["TransactionName"].ToString();
                            }
                            status.ReturnObj.Add(tranlog);
                            status.IsSuccess = true;
                        }
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

        public StatusDTO<TransactionLogDTO> UpdatePayment(TransactionLogDTO data)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    StatusDTO<TransactionLogDTO> status = new StatusDTO<TransactionLogDTO>();
                    if (data != null)
                    {
                        dbSvc.OpenConnection();
                        MySqlCommand command = new MySqlCommand();
                        command.CommandText = "UPDATE TransactionLog SET  AmountGiven=@given, DueAmount=@due ,AdjustedAmount=@adjusted WHERE TransactionLogId=@trLogId";
                        
                        if (data.AmountGiven != null)
                        {
                            command.Parameters.Add("@given", MySqlDbType.Double).Value = data.AmountGiven.Value;
                        }
                        else
                        {
                            command.Parameters.Add("@given", MySqlDbType.Double).Value = DBNull.Value;
                        }

                        if (data.DueAmount != null)
                        {
                            command.Parameters.Add("@due", MySqlDbType.Double).Value = data.DueAmount.Value;
                        }
                        else
                        {
                            command.Parameters.Add("@due", MySqlDbType.Double).Value = DBNull.Value;
                        }


                        if (data.DueAmount != null)
                        {
                            command.Parameters.Add("@adjusted", MySqlDbType.Double).Value = data.AdjustedAmount.Value;
                        }
                        else
                        {
                            command.Parameters.Add("@adjusted", MySqlDbType.Double).Value = DBNull.Value;

                        }
                         command.Parameters.Add("@trLogId", MySqlDbType.Int32).Value = data.TransactionLogId;
                        command.Connection = dbSvc.GetConnection() as MySqlConnection;
                        if (command.ExecuteNonQuery() > 0)
                        {
                            status.IsSuccess = true;
                            status.IsException = false;
                        }
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

        public bool ApproveCancelAdjustedAmt(List<TransactionLogDTO> lstTransaction)
        {
            bool retValue = false;
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    if(lstTransaction.Count>0)
                    {
                        foreach(TransactionLogDTO trLog in lstTransaction)
                        {
                            MySqlCommand command = new MySqlCommand();
                            command.CommandText = "UPDATE TransactionLog SET IsPrincipalApproved=@isPrincipalApproved, IsCompleted=@isCompleted WHERE TransactionLogId=@trnsLogId";
                            command.Parameters.Add("@isPrincipalApproved", MySqlDbType.Int32).Value = trLog.IsPrincipalApprroved;
                            command.Parameters.Add("@isCompleted", MySqlDbType.Bit).Value = trLog.IsCompleted;
                            command.Parameters.Add("@trnsLogId", MySqlDbType.Int32).Value = trLog.TransactionLogId;
                            command.Connection = dbSvc.GetConnection() as MySqlConnection;

                            command.ExecuteNonQuery();
                        }
                        retValue = true;
                    }
                    return retValue;
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
