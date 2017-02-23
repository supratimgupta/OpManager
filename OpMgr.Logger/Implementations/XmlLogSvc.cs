using OpMgr.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Logger.Implementations
{
    public class XmlLogSvc : ILogSvc
    {
        private IConfigSvc _configSvc;
        public XmlLogSvc(IConfigSvc configSvc)
        {
            _configSvc = configSvc;
        }
        public void Log(string message)
        {
            throw new NotImplementedException();
        }

        public void Log(Exception exception)
        {
            throw new NotImplementedException();
        }
    }
}
