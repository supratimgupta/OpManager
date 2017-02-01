using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpMgr.Common.Contracts.Modules;
using OpMgr.Common.Contracts;
using OpMgr.Common.DTOs;
using OpMgr.Common.DTOs.Users;
using MySql.Data.MySqlClient;
using System.Data;

namespace OpMgr.DataAccess.Implementations
{
    public class DbSvc : IDbSvc
    {
        private IConfigSvc _configSvc;

        private MySqlConnection _conn;

        public DbSvc(IConfigSvc configSvc)
        {
            _configSvc = configSvc;
            _conn = new MySqlConnection(configSvc.GetConnectionString());
        }

        public void OpenConnection()
        {
            if (_conn != null)
            {
                if (_conn.State != ConnectionState.Open)
                {
                    _conn.Open();
                }
            }
        }

        public void CloseConnection()
        {
            if (_conn != null)
            {
                if (_conn.State != ConnectionState.Closed)
                {
                    _conn.Close();
                }
            }
        }

        public IDbConnection GetConnection()
        {
            return _conn;
        }

        public void Dispose()
        {
            if (_conn != null)
            {
                this.CloseConnection();
                _conn.Dispose();
                _conn = null;
            }
        }
    }
}
