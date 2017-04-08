using OpMgr.Common.Contracts;
using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.Contracts.Modules
{
   public interface IStudentSvc: ICRUDSvc<StudentDTO,StudentDTO>
    {
         StatusDTO<List<StudentDTO>> PromoteToNewClass(List<StudentDTO> studentList,string Command,int StandardSectionId);
        
    }
}
