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
            throw new NotImplementedException();
        }

        public StatusDTO<TransactionRuleDTO> Update(TransactionRuleDTO data)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public DataTable GetAllRules()
        {
            using(IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "";
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
