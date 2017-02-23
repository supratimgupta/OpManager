using MySql.Data.MySqlClient;
using OpMgr.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.TransactionHandler.Implementations
{
    public class CommonConfigSvc : ICommonConfigSvc
    {

        private IDbSvc _dbSvc;

        private DataTable _dtConfig;

        public CommonConfigSvc(IDbSvc dbSvc)
        {
            _dbSvc = dbSvc;
            PopulateDBConfig();
        }

        public void PopulateDBConfig()
        {
            try
            {
                if (_dtConfig == null)
                {
                    MySqlCommand configSelectCommand = new MySqlCommand("SELECT CONF_KEY, CONF_VALUE FROM dbo.OpMgrConfig WHERE CONF_ACTIVE=1");
                    _dbSvc.OpenConnection();
                    configSelectCommand.Connection = _dbSvc.GetConnection() as MySqlConnection;
                    MySqlDataAdapter msDataAdap = new MySqlDataAdapter(configSelectCommand);
                    DataTable dtConfig = new DataTable();
                    msDataAdap.Fill(dtConfig);
                    _dtConfig = dtConfig;
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                _dbSvc.Dispose();
            }
        }

        public string this[string key]
        {
            get
            {
                string value = string.Empty;
                if (_dtConfig == null)
                {
                    this.PopulateDBConfig();
                }
                if (_dtConfig != null)
                {
                    if (_dtConfig.Rows.Count > 0)
                    {
                        value = _dtConfig.Select("CONF_KEY='" + key + "'")[0]["CONF_VALUE"].ToString();
                    }
                }
                return value;
            }
        }

        public string GetConfigValue(string key)
        {
            string value = string.Empty;
            if (_dtConfig == null)
            {
                this.PopulateDBConfig();
            }
            if (_dtConfig != null)
            {
                if (_dtConfig.Rows.Count > 0)
                {
                    value = _dtConfig.Select("CONF_KEY='" + key + "'")[0]["CONF_VALUE"].ToString();
                }
            }
            return value;
        }
    }
}
