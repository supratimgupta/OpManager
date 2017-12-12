using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.Contracts
{
    public interface IConfigSvc
    {
        string GetConnectionString();

        string GetLogPath();

        string GetStudentImagesFolder();

        string GetFatherImagesFolder();

        string GetMotherImagesFolder();

        string GetStudentImagesRelPath();

        string GetFatherImagesRelPath();

        string GetMotherImagesRelPath();

        string GetEmployeeImagesFolder();

        string GetEmployeeImagesRelPath();

        string StudentDataUploadPath();

        string StaffDataUploadPath();

        bool EmpAccessToPMSData();
    }
}
