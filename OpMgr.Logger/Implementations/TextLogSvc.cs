using OpMgr.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace OpMgr.Logger.Implementations
{
    public class TextLogSvc : ILogSvc
    {
        private IConfigSvc _configSvc;
        private string _logPath;
        public TextLogSvc(IConfigSvc configSvc)
        {
            _configSvc = configSvc;
            _logPath = configSvc.GetLogPath();
        }

        private void WriteLog(string message)
        {
            if(!string.IsNullOrEmpty(_logPath))
            {
                if(!File.Exists(_logPath))
                {
                    File.WriteAllText(_logPath, message);
                }
                else
                {
                    File.AppendAllText(_logPath, message);
                }
            }
        }

        public void Log(string message)
        {
            string logMessage = "MESSAGE\t|\t"+DateTime.Now+"\t|\t"+message;
            WriteLog(logMessage);
        }

        public void Log(Exception exception)
        {
            string logMessage = "EXCEPTION\t|\t" + DateTime.Now + "\t|\t" + exception.ToString().Replace("\n","-->");
            WriteLog(logMessage);
        }
    }
}
