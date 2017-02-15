using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpMgr.Common.Contracts;
using OpMgr.DataAccess.Implementations;
using MySql.Data.MySqlClient;
using System.Data;

namespace OpMgr.Configurations.Implementations
{
    public class CommonConfigSvc : ICommonConfigSvc
    {

        private IDbSvc _dbSvc;

        public CommonConfigSvc(IDbSvc dbSvc)
        {
            _dbSvc = dbSvc;
        }

        public void PopulateDBConfig()
        {
            try
            {
                if(System.Web.HttpContext.Current.Application["CONFIG"]==null)
                {
                    MySqlCommand configSelectCommand = new MySqlCommand("SELECT CONF_KEY, CONF_VALUE FROM dbo.OpMgrConfig WHERE CONF_ACTIVE=1");
                    _dbSvc.OpenConnection();
                    configSelectCommand.Connection = _dbSvc.GetConnection() as MySqlConnection;
                    MySqlDataAdapter msDataAdap = new MySqlDataAdapter(configSelectCommand);
                    DataTable dtConfig = new DataTable();
                    msDataAdap.Fill(dtConfig);
                    System.Web.HttpContext.Current.Application["CONFIG"] = dtConfig;
                }
            }
            catch(Exception exp)
            {
                throw exp;
            }
            finally
            {
                _dbSvc.Dispose();
            }
        }

        public string GetConfigValue(string key)
        {
            string value = string.Empty;
            if(System.Web.HttpContext.Current.Application["CONFIG"]==null)
            {
                this.PopulateDBConfig();
            }
            if (System.Web.HttpContext.Current.Application["CONFIG"]!=null)
            {
                DataTable dtConfig = (DataTable)System.Web.HttpContext.Current.Application["CONFIG"];
                if(dtConfig.Rows.Count>0)
                {
                    value = dtConfig.Select("CONF_KEY='" + key + "'")[0]["CONF_VALUE"].ToString();
                }
            }
            return value;
        }
    }
}
