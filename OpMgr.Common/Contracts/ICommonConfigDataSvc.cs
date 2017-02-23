using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.Contracts
{
    public interface ICommonConfigDataSvc : IDisposable
    {
        Dictionary<string, string> GetCommonConfigData();
    }
}
