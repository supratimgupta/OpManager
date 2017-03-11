using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpMgr.Common.DTOs;

namespace OpMgr.Common.Contracts.Modules
{
    public interface IUserSvc : ICRUDSvc<UserMasterDTO, UserMasterDTO>
    {
        StatusDTO<UserMasterDTO> Login(UserMasterDTO data, out List<EntitlementDTO> roleList, out List<ActionDTO> actionList);
    }
}
