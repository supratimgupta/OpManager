using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using OpMgr.Common.Contracts;
using System.Data;

namespace OpMgr.DataAccess.Implementations
{
    public class UserTransactionSvc : OpMgr.Common.Contracts.Modules.IUserTransactionSvc
    {
        private IConfigSvc _configSvc;
        private ILogSvc _logger;
        private DataTable _dtResult;

        public UserTransactionSvc(IConfigSvc configSvc, ILogSvc logger)
        {
            _configSvc = configSvc;
            _logger = logger;
        }

        public void Dispose()
        {
            if(_dtResult!=null)
            {
                _dtResult.Dispose();
                _dtResult = null;
            }
        }

        public StatusDTO<UserTransactionDTO> Insert(UserTransactionDTO data)
        {
            StatusDTO<UserTransactionDTO> status = new StatusDTO<UserTransactionDTO>();
            status.IsSuccess = false;
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "INSERT INTO UserTransaction(UserMasterId, TranMasterId, GraceAmountOn, GraceAmount, Active) VALUES (@userMasterId, @tranMasterId, @graceAmountOn, @graceAmount, 1)";

                    if(data.User!=null && data.User.UserMasterId>0)
                    {
                        command.Parameters.Add("@userMasterId", MySqlDbType.Int32).Value = data.User.UserMasterId;
                    }
                    else
                    {
                        command.Parameters.Add("@userMasterId", MySqlDbType.Int32).Value = DBNull.Value;
                    }
                    if(data.Transaction!=null && data.Transaction.TranMasterId>0)
                    {
                        command.Parameters.Add("@tranMasterId", MySqlDbType.Int32).Value = data.Transaction.TranMasterId;
                    }
                    else
                    {
                        command.Parameters.Add("@tranMasterId", MySqlDbType.Int32).Value = DBNull.Value;
                    }
                    if(!string.IsNullOrEmpty(data.GraceAmountIn) && !string.Equals(data.GraceAmountIn, "-1"))
                    {
                        command.Parameters.Add("@graceAmountOn", MySqlDbType.String).Value = data.GraceAmountIn;
                    }
                    else
                    {
                        command.Parameters.Add("@graceAmountOn", MySqlDbType.String).Value = DBNull.Value;
                    }

                    if (data.GraceAmount!=null)
                    {
                        command.Parameters.Add("@graceAmount", MySqlDbType.String).Value = data.GraceAmount.Value;
                    }
                    else
                    {
                        command.Parameters.Add("@graceAmount", MySqlDbType.String).Value = DBNull.Value;
                    }

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

        public StatusDTO<UserTransactionDTO> Update(UserTransactionDTO data)
        {
            StatusDTO<UserTransactionDTO> status = new StatusDTO<UserTransactionDTO>();
            status.IsSuccess = false;
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "UPDATE UserTransaction SET TranMasterId=@tranMasterId, GraceAmountOn=@graceAmountOn, GraceAmount=@graceAmount WHERE UserTransactionId=@uTranId";

                    if (data.Transaction != null && data.Transaction.TranMasterId > 0)
                    {
                        command.Parameters.Add("@tranMasterId", MySqlDbType.Int32).Value = data.Transaction.TranMasterId;
                    }
                    else
                    {
                        command.Parameters.Add("@tranMasterId", MySqlDbType.Int32).Value = DBNull.Value;
                    }
                    if (!string.IsNullOrEmpty(data.GraceAmountIn) && !string.Equals(data.GraceAmountIn,"-1"))
                    {
                        command.Parameters.Add("@graceAmountOn", MySqlDbType.String).Value = data.GraceAmountIn;
                    }
                    else
                    {
                        command.Parameters.Add("@graceAmountOn", MySqlDbType.String).Value = DBNull.Value;
                    }

                    if (data.GraceAmount != null)
                    {
                        command.Parameters.Add("@graceAmount", MySqlDbType.String).Value = data.GraceAmount.Value;
                    }
                    else
                    {
                        command.Parameters.Add("@graceAmount", MySqlDbType.String).Value = DBNull.Value;
                    }
                    command.Parameters.Add("@uTranId", MySqlDbType.Int32).Value = data.UserTransactionId;

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

        public StatusDTO<UserTransactionDTO> Delete(UserTransactionDTO data)
        {
            StatusDTO<UserTransactionDTO> status = new StatusDTO<UserTransactionDTO>();
            status.IsSuccess = false;
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "DELETE FROM UserTransaction WHERE UserTransactionId=@userTrans";
                    command.Parameters.Add("@userTrans", MySqlDbType.Int32).Value = data.UserTransactionId;

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

        public StatusDTO<List<UserTransactionDTO>> Select(UserTransactionDTO data)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<UserTransactionDTO> Select(int rowId)
        {
            throw new NotImplementedException();
        }

        public StatusDTO UpdateTransLastRunNextRun(UserTransactionDTO userTrans)
        {
            StatusDTO status = new StatusDTO();
            status.IsSuccess = false;
            using(IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "UPDATE UserTransaction SET LastAutoTransactionOn=@lastTransOn, NextAutoTransactionOn=@nextAutoTransOn WHERE UserTransactionId=@userTrans";
                    command.Parameters.Add("@lastTransOn", MySqlDbType.Date).Value = userTrans.LastAutoTransactionOn;
                    command.Parameters.Add("@nextAutoTransOn", MySqlDbType.Date).Value = userTrans.NextAutoTransactionOn;
                    command.Parameters.Add("@userTrans", MySqlDbType.Int32).Value = userTrans.UserTransactionId;

                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    if(command.ExecuteNonQuery()>0)
                    {
                        status.IsSuccess = true;
                    }
                    return status;
                }
                catch(Exception exp)
                {
                    _logger.Log(exp);
                    throw exp;
                }
            }
        }

        public List<UserTransactionDTO> GetUserTransactions(int userMasterId)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "SELECT UserTransactionId, TranMasterId, GraceAmountOn, GraceAmount FROM UserTransaction WHERE UserMasterId=@umId";
                    command.Parameters.Add("@umId", MySqlDbType.Int32).Value = userMasterId;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    MySqlDataAdapter mDa = new MySqlDataAdapter(command);
                    _dtResult = new DataTable();
                    mDa.Fill(_dtResult);
                    List<UserTransactionDTO> lstUTrans = null;
                    if(_dtResult!=null && _dtResult.Rows.Count>0)
                    {
                        lstUTrans = new List<UserTransactionDTO>();
                        UserTransactionDTO uTrans = null;
                        foreach(DataRow dr in _dtResult.Rows)
                        {
                            uTrans = new UserTransactionDTO();
                            uTrans.UserTransactionId = (int)dr["UserTransactionId"];
                            uTrans.Transaction = new TransactionMasterDTO();
                            uTrans.Transaction.TranMasterId = (int)dr["TranMasterId"];
                            uTrans.GraceAmountIn = string.IsNullOrEmpty(dr["GraceAmountOn"].ToString()) ? "-1" : dr["GraceAmountOn"].ToString();
                            if(string.IsNullOrEmpty(dr["GraceAmount"].ToString()))
                            {
                                uTrans.GraceAmount = null;
                            }
                            else
                            {
                                uTrans.GraceAmount = double.Parse(dr["GraceAmount"].ToString());
                            }
                            lstUTrans.Add(uTrans);
                        }
                    }
                    return lstUTrans;
                }
                catch(Exception exp)
                {
                    _logger.Log(exp);
                    throw exp;
                }
            }
        }

        public List<UserTransactionDTO> GetUserTransactions(int trMasterId, int userMasterId)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "SELECT UserTransactionId, TranMasterId, GraceAmountOn, GraceAmount FROM UserTransaction WHERE UserMasterId=@umId AND TranMasterId=@tranMasterId AND Active=1";
                    command.Parameters.Add("@umId", MySqlDbType.Int32).Value = userMasterId;
                    command.Parameters.Add("@tranMasterId", MySqlDbType.Int32).Value = trMasterId;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    MySqlDataAdapter mDa = new MySqlDataAdapter(command);
                    _dtResult = new DataTable();
                    mDa.Fill(_dtResult);
                    List<UserTransactionDTO> lstUTrans = null;
                    if (_dtResult != null && _dtResult.Rows.Count > 0)
                    {
                        lstUTrans = new List<UserTransactionDTO>();
                        UserTransactionDTO uTrans = null;
                        foreach (DataRow dr in _dtResult.Rows)
                        {
                            uTrans = new UserTransactionDTO();
                            uTrans.UserTransactionId = (int)dr["UserTransactionId"];
                            uTrans.Transaction = new TransactionMasterDTO();
                            uTrans.Transaction.TranMasterId = (int)dr["TranMasterId"];
                            uTrans.GraceAmountIn = string.IsNullOrEmpty(dr["GraceAmountOn"].ToString()) ? "-1" : dr["GraceAmountOn"].ToString();
                            if (string.IsNullOrEmpty(dr["GraceAmount"].ToString()))
                            {
                                uTrans.GraceAmount = null;
                            }
                            else
                            {
                                uTrans.GraceAmount = double.Parse(dr["GraceAmount"].ToString());
                            }
                            lstUTrans.Add(uTrans);
                        }
                    }
                    return lstUTrans;
                }
                catch (Exception exp)
                {
                    _logger.Log(exp);
                    throw exp;
                }
            }
        }

        public DataTable GetUserTransactions(DateTime? runDate)
        {
            using(IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "SELECT UT.UserTransactionId, UT.UserMasterId, UT.TranMasterId, UT.GraceAmountOn, UT.GraceAmount, UT.LastAutoTransactionOn, UT.NextAutoTransactionOn, UM.RoleId, UM.EmailId, SSM.StandardId, SSM.SectionId, SSM.StandardSectionId, S.ClassTypeId, UM.RoleId, TM.TransactionType, UM.LocationId FROM UserTransaction UT" +
                                            " LEFT JOIN UserMaster UM ON UM.UserMasterId = UT.UserMasterId LEFT JOIN StudentInfo SI ON UM.UserMasterId=SI.UserMasterId LEFT JOIN StandardSectionMap SSM ON SI.StandardSectionId = SSM.StandardSectionId" +
                                            " LEFT JOIN Standard S ON SSM.StandardId = S.StandardId LEFT JOIN transactionmaster TM ON UT.TranMasterId=TM.TranMasterId" +
                                            " WHERE UT.Active=1 AND UM.Active=1 AND ((NextAutoTransactionOn IS NULL AND LastAutoTransactionOn IS NULL) OR NextAutoTransactionOn<=@runDate)";
                    command.Parameters.Add("@runDate", MySqlDbType.DateTime).Value = runDate.Value.Date;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    _dtResult = new DataTable();
                    MySqlDataAdapter mDa = new MySqlDataAdapter(command);
                    mDa.Fill(_dtResult);
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
