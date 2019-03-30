using OpMgr.Common.Contracts;
using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.Contracts.Modules
{
   public interface IRoutinesvc 
    {
        List<RoutineTable> generateTable(int id,int location);
        AddEditRoutine GetData(int id);
        AddEditRoutine PostData(AddEditRoutine rtnedit);
        List<subject> getSubject(string term);
        List<Employee> getEmployee(string term,int location);
        List<Employee> GetReplacementTeacherList(int id);
        bool ReplaceTeachers(int id, int replacement_id); 

    }
}
