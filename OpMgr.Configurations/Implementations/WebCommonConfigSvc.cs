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
    public class WebCommonConfigSvc : ICommonConfigSvc
    {

        private ICommonConfigDataSvc _dataSvc;

        public WebCommonConfigSvc(ICommonConfigDataSvc dataSvc)
        {
            _dataSvc = dataSvc;
        }

        public void PopulateDBConfig()
        {
            try
            {
                if(System.Web.HttpContext.Current.Application["CONFIG"]==null)
                {
                    System.Web.HttpContext.Current.Application["CONFIG"] = _dataSvc.GetCommonConfigData();
                }
            }
            catch(Exception exp)
            {
                throw exp;
            }
            finally
            {
                _dataSvc.Dispose();
            }
        }

        public string this[string key]
        {
            get
            {
                string value = string.Empty;
                if (System.Web.HttpContext.Current.Application["CONFIG"] == null)
                {
                    this.PopulateDBConfig();
                }
                if (System.Web.HttpContext.Current.Application["CONFIG"] != null)
                {
                    Dictionary<string, string> dicConfig = (Dictionary<string, string>)System.Web.HttpContext.Current.Application["CONFIG"];
                    if (dicConfig != null && dicConfig.ContainsKey(key))
                    {
                        return dicConfig[key];
                    }
                }
                return value;
            }
        }

        public string GetConfigValue(string key)
        {
            string value = string.Empty;
            if (System.Web.HttpContext.Current.Application["CONFIG"] == null)
            {
                this.PopulateDBConfig();
            }
            if (System.Web.HttpContext.Current.Application["CONFIG"] != null)
            {
                Dictionary<string, string> dicConfig = (Dictionary<string, string>)System.Web.HttpContext.Current.Application["CONFIG"];
                if (dicConfig != null && dicConfig.ContainsKey(key))
                {
                    return dicConfig[key];
                }
            }
            return value;
        }

        public List<DayOfWeek> WeekOffDays
        {
            get
            {
                List<DayOfWeek> lstWeekOffDays = new List<DayOfWeek>();
                string weekOffDays = this["WEEK_OFF_DAYS"];
                if (!string.IsNullOrEmpty(weekOffDays))
                {
                    string[] arrWeekOffDays = weekOffDays.Split(',');
                    foreach (string weekOffDay in arrWeekOffDays)
                    {
                        if (!string.IsNullOrEmpty(weekOffDay))
                        {
                            lstWeekOffDays.Add((DayOfWeek)Convert.ToInt32(weekOffDay));
                        }
                    }
                }
                return lstWeekOffDays;
            }
        }
    }
}
