using OpMgr.Common.Contracts.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpMgr.Common.DTOs;

namespace OpMgr.DataAccess.Implementations
{
    public class RoleSvc : IRoleSvc
    {
        public StatusDTO<RoleDTO> Delete(RoleDTO data)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<RoleDTO> Insert(RoleDTO data)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<RoleDTO> Select(int rowId)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<List<RoleDTO>> Select(RoleDTO data)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<RoleDTO> Update(RoleDTO data)
        {
            throw new NotImplementedException();
        }
    }
}
