using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.Contracts
{
    public interface ICommonConfigSvc
    {
        void PopulateDBConfig();

        string GetConfigValue(string key);

        string this[string key]{ get; }

        List<DayOfWeek> WeekOffDays { get; }
    }
}
