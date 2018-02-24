using OpMgr.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Configurations.Implementations
{
    public class AppCommonConfigSvc: ICommonConfigSvc
    {

        private ICommonConfigDataSvc _dataSvc;

        private static Dictionary<string, string> _dicConfig;

        public AppCommonConfigSvc(ICommonConfigDataSvc dataSvc)
        {
            _dataSvc = dataSvc;
        }

        public void PopulateDBConfig()
        {
            try
            {
                if (_dicConfig == null)
                {
                    _dicConfig = _dataSvc.GetCommonConfigData();
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
                if (_dicConfig == null)
                {
                    this.PopulateDBConfig();
                }
                if (_dicConfig != null)
                {
                    if (_dicConfig.ContainsKey(key))
                    {
                        return _dicConfig[key];
                    }
                }
                return value;
            }
        }

        public string GetConfigValue(string key)
        {
            string value = string.Empty;
            if (_dicConfig == null)
            {
                this.PopulateDBConfig();
            }
            if (_dicConfig != null)
            {
                if (_dicConfig.ContainsKey(key))
                {
                    return _dicConfig[key];
                }
            }
            return value;
        }


        public List<DayOfWeek> WeekOffDays
        {
            get {
                List<DayOfWeek> lstWeekOffDays = new List<DayOfWeek>();
                string weekOffDays = this["WEEK_OFF_DAYS"];
                if(!string.IsNullOrEmpty(weekOffDays))
                {
                    string[] arrWeekOffDays = weekOffDays.Split(',');
                    foreach(string weekOffDay in arrWeekOffDays)
                    {
                        if(!string.IsNullOrEmpty(weekOffDay))
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
