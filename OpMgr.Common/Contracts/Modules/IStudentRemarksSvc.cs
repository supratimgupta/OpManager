using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.Contracts.Modules
{
    public interface IStudentRemarksSvc
    {
        List<DTOs.StudentRemarksDTO> GetStudentRemarks(int standardSectionId, string examResultType, DateTime courseFromDate, DateTime courseToDate, int location);

        void InsertStudentRemarks(DTOs.StudentRemarksDTO studentRemarks);

        void UpdateStudentRemarks(DTOs.StudentRemarksDTO studentRemarks);
    }
}
