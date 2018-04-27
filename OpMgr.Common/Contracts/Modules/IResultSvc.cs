using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.Contracts.Modules
{
    public interface IResultSvc
    {
        DataTable GetResultFormat(int standardSectionId, int examTypeId);

        DataTable GetResultRule(int standardSectionId);

        DataTable GetGradeConfig();

        DataTable GetStudentResults(int standardSectionId, List<int> examTypes, DateTime academicStartDate, DateTime academicEndDate);
    }
}
