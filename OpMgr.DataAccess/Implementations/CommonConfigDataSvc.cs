using MySql.Data.MySqlClient;
using OpMgr.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.DataAccess.Implementations
{
    public class CommonConfigDataSvc : ICommonConfigDataSvc
    {

        private IDbSvc _dbSvc;

        private DataTable _dtConfig;

        private Dictionary<string, string> _dbConfigData;

        public CommonConfigDataSvc(IDbSvc dbSvc)
        {
            _dbSvc = dbSvc;
        }

        public void Dispose()
        {
            if(_dtConfig!=null)
            {
                _dtConfig.Dispose();
                _dtConfig = null;
            }
            if(_dbConfigData!=null)
            {
                _dbConfigData = null;
            }
        }

        public Dictionary<string, string> GetCommonConfigData()
        {
            try
            {
                MySqlCommand configSelectCommand = new MySqlCommand("SELECT CONF_KEY, CONF_VALUE FROM dbo.OpMgrConfig WHERE CONF_ACTIVE=1");
                _dbSvc.OpenConnection();
                configSelectCommand.Connection = _dbSvc.GetConnection() as MySqlConnection;
                MySqlDataAdapter msDataAdap = new MySqlDataAdapter(configSelectCommand);
                _dtConfig = new DataTable();
                msDataAdap.Fill(_dtConfig);
                if (_dtConfig != null && _dtConfig.Rows.Count > 0)
                {
                    _dbConfigData = new Dictionary<string, string>();
                    foreach (DataRow dr in _dtConfig.Rows)
                    {
                        _dbConfigData.Add(dr["CONF_KEY"].ToString(), dr["CONF_VALUE"].ToString());
                    }
                }
                return _dbConfigData;
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
    }
}
