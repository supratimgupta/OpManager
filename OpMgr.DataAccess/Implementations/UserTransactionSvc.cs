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

        public IDataReader GetUserTransactions()
        {
            using(IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "";
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
