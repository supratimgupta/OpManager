using MySql.Data.MySqlClient;
using OpMgr.Common.Contracts;
using OpMgr.Common.Contracts.Modules;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.DataAccess.Implementations
{
    public class TransactionBacklog : ITransactionBacklog
    {
        private IConfigSvc _configSvc;

        private DataTable _dtResult;

        public void Dispose()
        {
            if (_dtResult != null)
            {
                _dtResult.Dispose();
                _dtResult = null;
            }
        }

        public TransactionBacklog(IConfigSvc configSvc)
        {
            _configSvc = configSvc;
        }

        public DataTable GetTransactionBacklogs()
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "SELECT tbl.REGNO, st.UserMasterId, tbl.MONTHLY, tbl.YEARLY, tbl.BUS, tbl.LATEFINE FROM tmpfeesbacklog tbl, studentinfo st WHERE tbl.REGNO=st.RegistrationNumber";

                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    MySqlDataAdapter mDA = new MySqlDataAdapter(command);
                    _dtResult = new DataTable("TR_DATA");
                    mDA.Fill(_dtResult);

                    return _dtResult;
                }
                catch (Exception exp)
                {
                    throw exp;
                }

            }
        }
    }
}
