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
    public class TransactionMasterSvc : ITransactionMasterSvc, IDisposable
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

        public TransactionMasterSvc(IConfigSvc configSvc, ILogSvc logger)
        {
            _configSvc = configSvc;
            _logger = logger;
        }

        public StatusDTO<TransactionMasterDTO> Insert(TransactionMasterDTO data)
        {
            StatusDTO<TransactionMasterDTO> status = new StatusDTO<TransactionMasterDTO>();
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();

                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "INSERT INTO transactionmaster (TransactionType, TransactionName, IsPenalty, Frequency, DayToRun, YearlyDayToRun, IsdifferentTo, Active)"+
                                          " VALUES(@transactionType, @trName, @isPenalty, @frequency, @dayToRun, @yearlyDayToRun, @isdifferentTo, @active)";
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    command.Parameters.Add("@transactionType", MySqlDbType.String).Value = data.TransactionType;
                    command.Parameters.Add("@trName", MySqlDbType.String).Value = data.TransactionName;
                    command.Parameters.Add("@isPenalty", MySqlDbType.Bit).Value = data.IsPenalty;
                    command.Parameters.Add("@frequency", MySqlDbType.String).Value = data.Frequency;
                    command.Parameters.Add("@dayToRun", MySqlDbType.String).Value = data.DayToRun;
                    command.Parameters.Add("@yearlyDayToRun", MySqlDbType.String).Value = data.YearlyDayToRun;
                    command.Parameters.Add("@isdifferentTo", MySqlDbType.String).Value = data.IsDiffTo;
                    command.Parameters.Add("@active", MySqlDbType.Bit).Value = data.Active;

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

        public StatusDTO<TransactionMasterDTO> Update(TransactionMasterDTO data)
        {
            StatusDTO<TransactionMasterDTO> status = new StatusDTO<TransactionMasterDTO>();
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();

                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "UPDATE transactionmaster SET TransactionType=@transactionType, TransactionName=@trName, IsPenalty=@isPenalty, Frequency=@frequency, DayToRun=@dayToRun, YearlyDayToRun=@yearlyDayToRun, IsdifferentTo=@isdifferentTo, Active=@active WHERE TranMasterId=@trMasterId";
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    command.Parameters.Add("@transactionType", MySqlDbType.String).Value = data.TransactionType;
                    command.Parameters.Add("@trName", MySqlDbType.String).Value = data.TransactionName;
                    command.Parameters.Add("@isPenalty", MySqlDbType.Bit).Value = data.IsPenalty;
                    command.Parameters.Add("@frequency", MySqlDbType.String).Value = data.Frequency;
                    command.Parameters.Add("@dayToRun", MySqlDbType.String).Value = data.DayToRun;
                    command.Parameters.Add("@yearlyDayToRun", MySqlDbType.String).Value = data.YearlyDayToRun;
                    command.Parameters.Add("@isdifferentTo", MySqlDbType.String).Value = data.IsDiffTo;
                    command.Parameters.Add("@active", MySqlDbType.Bit).Value = data.Active;
                    command.Parameters.Add("@trMasterId", MySqlDbType.String).Value = data.TranMasterId;
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

        public StatusDTO<TransactionMasterDTO> Delete(TransactionMasterDTO data)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<List<TransactionMasterDTO>> Select(TransactionMasterDTO data)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<TransactionMasterDTO> Select(int rowId)
        {
            throw new NotImplementedException();
        }

        public DataTable GetAllTransactions()
        {
            using(IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "SELECT TranMasterId, TransactionType, IsPenalty, Frequency, DayToRun, YearlyDayToRun, IsdifferentTo FROM transactionmaster WHERE Active=1";
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    MySqlDataAdapter dataAdap = new MySqlDataAdapter(command);
                    _dtResult = new DataTable("TRANS_MASTER");
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

        public string GetFreq(int trnsMasterId)
        {
            string returnValue = "-1";
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "SELECT Frequency FROM transactionmaster WHERE TranMasterId=@trnsMaster";
                    command.Parameters.Add("@trnsMaster", MySqlDbType.Int32).Value = trnsMasterId;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    MySqlDataAdapter dataAdap = new MySqlDataAdapter(command);
                    _dtResult = new DataTable("TRANS_MASTER");
                    dataAdap.Fill(_dtResult);

                    if (_dtResult != null && _dtResult.Rows.Count > 0)
                    {
                        returnValue = _dtResult.Rows[0]["Frequency"].ToString();
                    }

                    return returnValue;
                }
                catch (Exception exp)
                {
                    _logger.Log(exp);
                    throw exp;
                }
            }
        }

        public string GetIsDifferentTo(int transMasterId)
        {
            string returnValue = "-1";
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "SELECT IsdifferentTo FROM transactionmaster WHERE TranMasterId=@trnsMaster";
                    command.Parameters.Add("@trnsMaster", MySqlDbType.Int32).Value = transMasterId;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    MySqlDataAdapter dataAdap = new MySqlDataAdapter(command);
                    _dtResult = new DataTable("TRANS_MASTER");
                    dataAdap.Fill(_dtResult);
                    
                    if (_dtResult != null && _dtResult.Rows.Count > 0)
                    {
                        returnValue = _dtResult.Rows[0]["IsdifferentTo"].ToString();
                    }

                    return returnValue;
                }
                catch (Exception exp)
                {
                    _logger.Log(exp);
                    throw exp;
                }
            }
        }

        public StatusDTO<List<TransactionMasterDTO>> GetAllTransactioMasters()
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "SELECT TranMasterId, TransactionName, TransactionType, IsPenalty, Frequency, DayToRun, YearlyDayToRun, IsdifferentTo, Active FROM transactionmaster";
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    MySqlDataAdapter dataAdap = new MySqlDataAdapter(command);
                    _dtResult = new DataTable("TRANS_MASTER");
                    dataAdap.Fill(_dtResult);
                    StatusDTO<List<TransactionMasterDTO>> status = new StatusDTO<List<TransactionMasterDTO>>();
                    if(_dtResult!=null && _dtResult.Rows.Count>0)
                    {
                        status.ReturnObj = new List<TransactionMasterDTO>();
                        TransactionMasterDTO trMaster;
                        foreach(DataRow dr in _dtResult.Rows)
                        {
                            trMaster = new TransactionMasterDTO();
                            trMaster.TranMasterId = (int)dr["TranMasterId"];
                            trMaster.TransactionName = dr["TransactionName"].ToString();
                            trMaster.TransactionType = dr["TransactionType"].ToString();
                            trMaster.IsPenalty = string.Equals(dr["IsPenalty"].ToString(), "1") ? true : false;
                            trMaster.Frequency = dr["Frequency"].ToString();
                            trMaster.DayToRun = dr["DayToRun"].ToString();
                            trMaster.YearlyDayToRun = dr["YearlyDayToRun"].ToString();
                            trMaster.IsDiffTo = dr["IsdifferentTo"].ToString();
                            trMaster.Active = string.Equals(dr["Active"].ToString(), "1") ? true : false;

                            status.ReturnObj.Add(trMaster);
                        }
                    }
                    status.IsSuccess = true;
                    status.IsException = false;
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
