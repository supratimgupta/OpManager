using OpMgr.Common.DTOs;
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
        List<ResultCardDTO> GetResult(int locationId, int standardSectionId, List<int> examTypes, DateTime academicSessionStartDate, DateTime academicSessionEndDate);
    }
}
