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
        public UserTransactionSvc(IConfigSvc configSvc, ILogSvc logger)
        {
            _configSvc = configSvc;
            _logger = logger;
        }
        public StatusDTO<UserTransactionDTO> Insert(UserTransactionDTO data)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<UserTransactionDTO> Update(UserTransactionDTO data)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<UserTransactionDTO> Delete(UserTransactionDTO data)
        {
            throw new NotImplementedException();
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
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "UPDATE dbo.UserTransaction SET LastAutoTransactionOn=@lastTransOn, NextAutoTransactionOn=@nextAutoTransOn WHERE UserTransactionId=@userTrans";
                    command.Parameters.Add("@lastTransOn", MySqlDbType.Date).Value = userTrans.LastAutoTransactionOn;
                    command.Parameters.Add("@nextAutoTransOn", MySqlDbType.Date).Value = userTrans.NextAutoTransactionOn;
                    command.Parameters.Add("@userTrans", MySqlDbType.Int32).Value = userTrans.UserTransactionId;

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

        public IDataReader GetUserTransactions(DateTime? runDate)
        {
            using(IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "SELECT UT.UserTransactionId, UT.UserMasterId, UT.TranMasterId, UT.GraceAmountOn, UT.GraceAmount, UT.LastAutoTransactionOn, UT.NextAutoTransactionOn, UM.RoleId, UM.EmailId, SSM.StandardId, SSM.SectionId, SSM.StandardSectionId, S.ClassTypeId, UM.RoleId, TM.TransactionType FROM dbo.UserTransaction UT" +
                                            " LEFT JOIN dbo.UserMaster UM ON UM.UserMasterId = UT.UserMasterId LEFT JOIN dbo.StudentInfo SI ON UM.UserMasterId=SI.UserMasterId LEFT JOIN dbo.StandardSectionMap SSM ON SI.StandardSectionId = SSM.StandardSectionId" +
                                            " LEFT JOIN dbo.Standard S ON SCM.StandardId = S.StandardId LEFT JOIN dbo.TransactionMaster TM ON UT.TranMasterId=TM.TranMasterId" +
                                            " WHERE Active=1 AND ((NextAutoTransactionOn IS NULL AND LastAutoTransactionOn IS NULL) OR NextAutoTransactionOn<=@runDate)";
                    command.Parameters.Add("@runDate", MySqlDbType.DateTime).Value = runDate.Value.Date;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    return command.ExecuteReader();
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
