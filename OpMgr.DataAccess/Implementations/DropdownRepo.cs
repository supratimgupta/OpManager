using MySql.Data.MySqlClient;
using OpMgr.Common.Contracts;
using OpMgr.Common.Contracts.Modules;
using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.DataAccess.Implementations
{
    public class DropdownRepo : IDropdownRepo
    {
        private IConfigSvc _configSvc;
        private DataTable _dtData;
                       
        public DropdownRepo(IConfigSvc configSvc)
        {
            _configSvc = configSvc;
        }

        
        public List<LocationDTO> Location()
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "select LocationId,LocationDescription from location where Active=1";
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    _dtData = new DataTable();
                    MySqlDataAdapter msDa = new MySqlDataAdapter(command);
                    msDa.Fill(_dtData);
                    List<LocationDTO> lstLocation = new List<LocationDTO>();
                    if(_dtData!=null && _dtData.Rows.Count>0)
                    {
                        LocationDTO locationDTO = null;
                        foreach (DataRow dr in _dtData.Rows)
                        {
                            locationDTO = new LocationDTO();
                            locationDTO.LocationId = (int)dr["LocationId"];
                            locationDTO.LocationDescription = dr["LocationDescription"].ToString();
                            lstLocation.Add(locationDTO);
                        }
                    }
                    return lstLocation;                    
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public List<RoleDTO> Roles()
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "select RoleId,RoleDescription from roles where Active=1";
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    _dtData = new DataTable();
                    MySqlDataAdapter msDa = new MySqlDataAdapter(command);
                    msDa.Fill(_dtData);
                    List<RoleDTO> lstRole = new List<RoleDTO>();
                    if (_dtData != null && _dtData.Rows.Count > 0)
                    {
                        RoleDTO roleDTO = null;
                        foreach (DataRow dr in _dtData.Rows)
                        {
                            roleDTO = new RoleDTO();
                            roleDTO.RoleId = (int)dr["RoleId"];
                            roleDTO.RoleDescription = dr["RoleDescription"].ToString();
                            lstRole.Add(roleDTO);
                        }
                    }
                    return lstRole;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }
    }
}
