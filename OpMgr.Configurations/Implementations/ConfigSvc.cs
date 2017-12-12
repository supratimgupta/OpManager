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

        public string GetStudentImagesRelPath()
        {
            return System.Configuration.ConfigurationManager.AppSettings["StudentImageRelPath"];
        }

        public string GetFatherImagesRelPath()
        {
            return System.Configuration.ConfigurationManager.AppSettings["FatherImageRelPath"];
        }

        public string GetMotherImagesRelPath()
        {
            return System.Configuration.ConfigurationManager.AppSettings["MotherImageRelPath"];
        }

        public string GetEmployeeImagesFolder()
        {
            return System.Configuration.ConfigurationManager.AppSettings["EmployeeImageFolder"];
        }

        public string GetEmployeeImagesRelPath()
        {
            return System.Configuration.ConfigurationManager.AppSettings["EmployeeImageRelPath"];
        }

        public string StudentDataUploadPath()
        {
            return System.Configuration.ConfigurationManager.AppSettings["StudentBulkUploadExcelFilePath"];
        }

        public string StaffDataUploadPath()
        {
            return System.Configuration.ConfigurationManager.AppSettings["StaffBulkUploadExcelFilePath"];
        }

        public bool EmpAccessToPMSData()
        {
            return System.Configuration.ConfigurationManager.AppSettings["EmpAccessToPMSData"] == "Y" ? true : false;
        }
    }
}
