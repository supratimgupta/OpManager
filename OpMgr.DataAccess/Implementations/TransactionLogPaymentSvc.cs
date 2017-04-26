using OpMgr.Common.Contracts.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpMgr.Common.DTOs;
using OpMgr.Common.Contracts;
using MySql.Data.MySqlClient;

namespace OpMgr.DataAccess.Implementations
{
    public class TransactionLogPaymentSvc : ITransactionLogPaymentSvc
    {
        private IConfigSvc _configSvc;

        private ILogSvc _logger;

        public TransactionLogPaymentSvc(IConfigSvc config, ILogSvc logger)
        {
            _configSvc = config;
            _logger = logger;
        }

        public StatusDTO<TransactionLogPaymentsDTO> Delete(TransactionLogPaymentsDTO data)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<TransactionLogPaymentsDTO> Insert(TransactionLogPaymentsDTO data)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "INSERT INTO transactionlogpayments(transactionlogid, paymentdate, " +
                                          "currentamount, paymentchequeno, paymentmode, currentadjustingamount) VALUES (@transLogId, @paymentDate, " +
                                          "@curramount, @chqNo, @mode, @currentadjusting)";
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    command.Parameters.Add("@transLogId", MySqlDbType.Int32).Value = data.TransactionLog.TransactionLogId;
                    command.Parameters.Add("@paymentDate", MySqlDbType.DateTime).Value = data.PaymentDate.Value;
                    command.Parameters.Add("@curramount", MySqlDbType.Decimal).Value = data.CurrentAmount.Value;
                    if(string.IsNullOrEmpty(data.PaymentChequeNo))
                    {
                        command.Parameters.Add("@chqNo", MySqlDbType.VarChar).Value = DBNull.Value;
                    }
                    else
                    {
                        command.Parameters.Add("@chqNo", MySqlDbType.VarChar).Value = data.PaymentChequeNo;
                    }
                    command.Parameters.Add("@mode", MySqlDbType.VarChar).Value = data.PaymentMode;
                    command.Parameters.Add("@currentadjusting", MySqlDbType.Decimal).Value = data.CurrentAdjustingAmount.Value;

                    StatusDTO<TransactionLogPaymentsDTO> status = new StatusDTO<TransactionLogPaymentsDTO>();
                    if(command.ExecuteNonQuery()>0)
                    {
                        status.IsSuccess = true;
                        status.IsException = false;
                        return status;
                    }
                    status.IsSuccess = false;
                    status.IsException = false;
                    status.FailureReason = "Insert failed";
                    return status;
                    
                }
                catch (Exception exp)
                {
                    _logger.Log(exp);
                    throw exp;
                }
            }
        }

        public StatusDTO<TransactionLogPaymentsDTO> Select(int rowId)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<List<TransactionLogPaymentsDTO>> Select(TransactionLogPaymentsDTO data)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<TransactionLogPaymentsDTO> Update(TransactionLogPaymentsDTO data)
        {
            throw new NotImplementedException();
        }
    }
}
