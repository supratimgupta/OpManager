using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using OpMgr.Common.Contracts;
using System.Data;
using OpMgr.Common.DTOs;
using OpMgr.Common.Contracts.Modules;

namespace OpMgr.DataAccess.Implementations
{
    public class UserSvc : IUserSvc, IDisposable
    {
        public StatusDTO<UserMasterDTO> Delete(UserMasterDTO data)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public StatusDTO<UserMasterDTO> Insert(UserMasterDTO data)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<UserMasterDTO> Login(UserMasterDTO data)
        {
            throw new NotImplementedException();
        }

        
        public StatusDTO<UserMasterDTO> Select(int rowId)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<List<UserMasterDTO>> Select(UserMasterDTO data)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<UserMasterDTO> Update(UserMasterDTO data)
        {
            throw new NotImplementedException();
        }
    }
}
