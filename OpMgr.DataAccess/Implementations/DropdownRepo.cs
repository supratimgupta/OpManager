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

        public  DataTable Location()
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
                    MySqlDataAdapter mda = new MySqlDataAdapter(command);
                    mda.Fill(_dtData);
                    return _dtData;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }
    }
}
