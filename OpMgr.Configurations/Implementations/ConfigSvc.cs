using OpMgr.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Configurations.Implementations
{
    public class ConfigSvc : IConfigSvc
    {
        public string GetConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        }

        public string GetLogPath()
        {
            return System.Configuration.ConfigurationManager.AppSettings["LogPath"];
        }

        public string GetStudentImagesFolder()
        {
            return System.Configuration.ConfigurationManager.AppSettings["StudentImageFolder"];
        }

        public string GetFatherImagesFolder()
        {
            return System.Configuration.ConfigurationManager.AppSettings["FatherImageFolder"];
        }

        public string GetMotherImagesFolder()
        {
            return System.Configuration.ConfigurationManager.AppSettings["MotherImageFolder"];
        }
    }
}
