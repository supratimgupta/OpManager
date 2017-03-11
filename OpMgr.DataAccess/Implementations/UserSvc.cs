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
        private IConfigSvc _configSvc;

        public UserSvc(IConfigSvc configSvc)
        {
            _configSvc = configSvc;
        }

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

        public string GetUserRole(int userId)
        {
            string role = string.Empty;
            using(IDbSvc dbSvc=new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "select RoleId from UserMaster where UserMasterId=@userMasterId";
                    command.Parameters.Add("@userMasterId", MySqlDbType.Int32).Value = userId;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    DataTable dtRole = new DataTable();
                    MySqlDataAdapter msDa = new MySqlDataAdapter(command);
                    msDa.Fill(dtRole);
                    if(dtRole!=null && dtRole.Rows.Count>0)
                    {
                        role = dtRole.Rows[0]["RoleId"].ToString();
                    }
                }
                catch(Exception exp)
                {
                    throw exp;
                }
            }
            return role;
        }
    }
}
