using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpMgr.Common.DTOs.Users;

namespace OpMgr.Common.Contracts.Modules
{
    public interface IUserSvc : ICRUDSvc<UserDTO, UserDTO>
    {

    }
}
