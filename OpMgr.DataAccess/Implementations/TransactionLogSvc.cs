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
    public class TransactionLogSvc : ITransactionLogSvc, IDisposable
    {
        private IConfigSvc _configSvc;

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
                    if(data!=null)
                    {
                        MySqlCommand command = new MySqlCommand();
                        command.CommandText = "INSERT INTO TransactionLog (CreatedBy, CreatedDate, UpdatedBy, UpdatedDate, Active, UserMasterId, TransactionDate, " +
                                               "TransactionDueDate, TransactionPreviousDueDate, ParentTransactionLogId, IsCompleted, CompletedOn, AmountImposed, " +
                                               "AmountGiven, DueAmount, TransferMode, locationId, StandardSectionId, TransactionType, HasPenalty, OriginalTransactionLogiId, TranRuleId) VALUES " +
                                               "(@createdBy, @createdDate, @updatedBy, @updatedDate, @active, @userMasterId, @transactionDate, " +
                                               "@transactionDueDate, @transPrevDueDate, @parentTransLogId, @isCompleted, @completedOn, @amountImposed, " +
                                               "@amountGiven, @dueAmount, @transferMode, @locationId, @standardSectionId, @transactionType, @hasPenalty, @originalTransLogId, @transRuleId)";

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

                        if(data.UpdatedDate!=null)
                        {
                            command.Parameters.Add("@updatedDate", MySqlDbType.DateTime).Value = data.UpdatedDate;
                        }
                        else
                        {
                            command.Parameters.Add("@updatedDate", MySqlDbType.DateTime).Value = DBNull.Value;
                        }

                        if(data.Active!=null)
                        {
                            command.Parameters.Add("@active", MySqlDbType.Bit).Value = data.Active;
                        }
                        else
                        {
                            command.Parameters.Add("@active", MySqlDbType.Bit).Value = DBNull.Value;
                        }

                        if(data.User!=null)
                        {
                            command.Parameters.Add("@userMasterId", MySqlDbType.Int32).Value = data.User.UserMasterId;
                        }
                        else
                        {
                            command.Parameters.Add("@userMasterId", MySqlDbType.Int32).Value = DBNull.Value;
                        }

                        if(data.TransactionDate!=null)
                        {
                            command.Parameters.Add("@transactionDate", MySqlDbType.DateTime).Value = data.TransactionDate;
                        }
                        else
                        {
                            command.Parameters.Add("@transactionDate", MySqlDbType.DateTime).Value = DBNull.Value;
                        }

                        if(data.TransactionDueDate!=null)
                        {
                            command.Parameters.Add("@transactionDueDate", MySqlDbType.DateTime).Value = data.TransactionDueDate;
                        }
                        else
                        {
                            command.Parameters.Add("@transactionDueDate", MySqlDbType.DateTime).Value = DBNull.Value;
                        }

                        if(data.TransactionPreviousDueDate!=null)
                        {
                            command.Parameters.Add("@transPrevDueDate", MySqlDbType.DateTime).Value = data.TransactionPreviousDueDate;
                        }
                        else
                        {
                            command.Parameters.Add("@transPrevDueDate", MySqlDbType.DateTime).Value = DBNull.Value;
                        }

                        if(data.ParentTransactionLogId!=null)
                        {
                            command.Parameters.Add("@parentTransLogId", MySqlDbType.Int32).Value = data.ParentTransactionLogId.TransactionLogId;
                        }
                        else
                        {
                            command.Parameters.Add("@parentTransLogId", MySqlDbType.Int32).Value = DBNull.Value;
                        }

                        if(data.IsCompleted!=null)
                        {
                            command.Parameters.Add("@isCompleted", MySqlDbType.Bit).Value = data.IsCompleted;
                        }
                        else
                        {
                            command.Parameters.Add("@isCompleted", MySqlDbType.Bit).Value = DBNull.Value;
                        }

                        if(data.CompletedOn!=null)
                        {
                            command.Parameters.Add("@completedOn", MySqlDbType.DateTime).Value = data.CompletedOn;
                        }
                        else
                        {
                            command.Parameters.Add("@completedOn", MySqlDbType.DateTime).Value = DBNull.Value;
                        }

                        if(data.AmountImposed!=null)
                        {
                            command.Parameters.Add("@amountImposed", MySqlDbType.Double).Value = data.AmountImposed;
                        }
                        else
                        {
                            command.Parameters.Add("@amountImposed", MySqlDbType.Double).Value = DBNull.Value;
                        }

                        if(data.AmountGiven!=null)
                        {
                            command.Parameters.Add("@amountGiven", MySqlDbType.Double).Value = data.AmountGiven;
                        }
                        else
                        {
                            command.Parameters.Add("@amountGiven", MySqlDbType.Double).Value = DBNull.Value;
                        }

                        if(data.DueAmount!=null)
                        {
                            command.Parameters.Add("@dueAmount", MySqlDbType.Double).Value = data.DueAmount;
                        }
                        else
                        {
                            command.Parameters.Add("@dueAmount", MySqlDbType.Double).Value = DBNull.Value;
                        }

                        if(!string.IsNullOrEmpty(data.TransferMode))
                        {
                            command.Parameters.Add("@transferMode", MySqlDbType.String).Value = data.TransferMode;
                        }
                        else
                        {
                            command.Parameters.Add("@transferMode", MySqlDbType.String).Value = DBNull.Value;
                        }

                        if(data.Location!=null)
                        {
                            command.Parameters.Add("@locationId", MySqlDbType.Int32).Value = data.Location.LocationId;
                        }
                        else
                        {
                            command.Parameters.Add("@locationId", MySqlDbType.Int32).Value = DBNull.Value;
                        }

                        if(data.StandardSectionMap!=null)
                        {
                            command.Parameters.Add("@standardSectionId", MySqlDbType.Int32).Value = data.StandardSectionMap.StandardSectionId;
                        }
                        else
                        {
                            command.Parameters.Add("@standardSectionId", MySqlDbType.Int32).Value = DBNull.Value;
                        }

                        if(!string.IsNullOrEmpty(data.TransactionType))
                        {
                            command.Parameters.Add("@transactionType", MySqlDbType.String).Value = data.TransactionType;
                        }
                        else
                        {
                            command.Parameters.Add("@transactionType", MySqlDbType.String).Value = DBNull.Value;
                        }

                        if(data.HasPenalty!=null)
                        {
                            command.Parameters.Add("@hasPenalty", MySqlDbType.Bit).Value = data.HasPenalty;
                        }
                        else
                        {
                            command.Parameters.Add("@hasPenalty", MySqlDbType.Bit).Value = DBNull.Value;
                        }

                        if(data.OriginalTransLog!=null)
                        {
                            command.Parameters.Add("@originalTransLogId", MySqlDbType.Int32).Value = data.OriginalTransLog.TransactionLogId;
                        }
                        else
                        {
                            command.Parameters.Add("@originalTransLogId", MySqlDbType.Int32).Value = DBNull.Value;
                        }

                        if(data.TransactionRule!=null)
                        {
                            command.Parameters.Add("@transRuleId", MySqlDbType.Int32).Value = data.TransactionRule.TranRuleId;
                        }
                        else
                        {
                            command.Parameters.Add("@transRuleId", MySqlDbType.Int32).Value = DBNull.Value;
                        }

                        var retData = command.ExecuteScalar();
                        if(retData!=null)
                        {
                            data.TransactionLogId = (int)retData;
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
            throw new NotImplementedException();
        }

        public StatusDTO<TransactionLogDTO> Delete(TransactionLogDTO data)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<List<TransactionLogDTO>> Select(TransactionLogDTO data)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<TransactionLogDTO> Select(int rowId)
        {
            throw new NotImplementedException();
        }

        public DataTable GetPendingTransactions()
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "";
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    MySqlDataAdapter dataAdap = new MySqlDataAdapter(command);
                    _dtResult = new DataTable("PENDING_TRANS");
                    dataAdap.Fill(_dtResult);
                    return _dtResult;
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
