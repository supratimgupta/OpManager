using OpMgr.Common.Contracts.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpMgr.Common.DTOs;
using OpMgr.Common.Contracts;
using System.Data;
using MySql.Data.MySqlClient;

namespace OpMgr.DataAccess.Implementations
{
    public class EntitlementActionSvc : IEntitlementActionSvc, IDisposable
    {
        private IConfigSvc _configSvc;
        private DataTable _dtData;
        private DataSet _dsData;
        private ILogSvc _logger;

        public EntitlementActionSvc(IConfigSvc configSvc, ILogSvc logger)
        {
            _configSvc = configSvc;
            _logger = logger;
        }

       
        public void Dispose()
        {
            throw new NotImplementedException();
        }
                
        public StatusDTO<EntitlementActionDTO> Insert(EntitlementActionDTO data)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "User_Action_Role_mapping";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    command.Parameters.Add("@ActionId", MySqlDbType.Int32).Value = data.ActionDetails.RowId;
                    command.Parameters.Add("@UserRoleId", MySqlDbType.Int32).Value = data.RoleDetails.UserRoleId;
                    if(data.UserMaster.CreatedBy != null)
                    {
                        command.Parameters.Add("@CreatedBy", MySqlDbType.Int32).Value = data.UserMaster.CreatedBy;
                    }
                    else
                    {
                        command.Parameters.Add("@CreatedBy", MySqlDbType.Int32).Value = DBNull.Value;
                    }                    

                    MySqlDataReader rdr = command.ExecuteReader(CommandBehavior.CloseConnection);
                    _dtData = new DataTable();
                    _dtData.Load(rdr);
                    StatusDTO<EntitlementActionDTO> status = new StatusDTO<EntitlementActionDTO>();
                    if(rdr.RecordsAffected > 0)
                    {
                        status.IsSuccess = true;
                    }
                    else
                    {
                        status.IsSuccess = false;
                    }
                    return status;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public StatusDTO<EntitlementActionDTO> Update(EntitlementActionDTO data)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<EntitlementActionDTO> Delete(EntitlementActionDTO data)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<List<EntitlementActionDTO>> Select(EntitlementActionDTO data)
        {
            StatusDTO<List<EntitlementActionDTO>> entLst = new StatusDTO<List<EntitlementActionDTO>>();
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "get_User_Action_Role_mapping";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    if(data.ActionDetails.RowId > 0)
                    {
                        command.Parameters.Add("@ActionId", MySqlDbType.Int32).Value = data.ActionDetails.RowId;
                    }
                    else
                    {
                        command.Parameters.Add("@ActionId", MySqlDbType.Int32).Value = DBNull.Value;
                    }
                    if (data.ActionDetails.RowId > 0)
                    {
                        command.Parameters.Add("@UserRoleId", MySqlDbType.Int32).Value = data.RoleDetails.UserRoleId;
                    }
                    else
                    {
                        command.Parameters.Add("@UserRoleId", MySqlDbType.Int32).Value = DBNull.Value; ;
                    }
                    

                    MySqlDataAdapter rdr = new MySqlDataAdapter(command);
                    _dsData = new DataSet();
                    rdr.Fill(_dsData);
                    //List<EntitlementActionDTO> entitlementList = new List<EntitlementActionDTO>();

                    if (_dsData != null)
                    {
                        entLst.ReturnObj = new List<EntitlementActionDTO>();
                        for (int i = 0; i < _dsData.Tables[0].Rows.Count; i++)
                        {
                            EntitlementActionDTO entActionDTO = new EntitlementActionDTO();
                            entActionDTO.RowId= Convert.ToInt32(_dsData.Tables[0].Rows[i]["EntitlementActionId"]);
                            entActionDTO.ActionDetails = new ActionDTO();
                            entActionDTO.ActionDetails.ActionName = _dsData.Tables[0].Rows[i]["ActionName"].ToString();
                            entActionDTO.RoleDetails = new EntitlementDTO();
                            entActionDTO.RoleDetails.RoleName = _dsData.Tables[0].Rows[i]["RoleName"].ToString();
                            
                            entLst.ReturnObj.Add(entActionDTO);

                            entLst.IsSuccess = true;
                        }
                    }
                    return entLst;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        StatusDTO<EntitlementActionDTO> ICRUDSvc<EntitlementActionDTO, EntitlementActionDTO>.Select(int rowId)
        {
            throw new NotImplementedException();
        }
    }
}
