using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace OpMgr.FileWatcher.DataAccess
{
    public class MySqlDataAccess : AbsDataAccess
    {
        private static string _CONN_STR = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        public override string GetDeptId(string deptName)
        {
            string deptId = "-1";
            string query = "select deptId from dbo.department where deptName='"+deptName+"'";
            using(MySqlConnection conn = new MySqlConnection(_CONN_STR))
            {
                using(MySqlCommand command = new MySqlCommand(query,conn))
                {
                    using(MySqlDataAdapter da = new MySqlDataAdapter(command))
                    {
                        using(DataTable dt = new DataTable())
                        {
                            da.Fill(dt);
                            if(dt!=null && dt.Rows.Count>0)
                            {
                                deptId = dt.Rows[0]["deptId"].ToString();
                            }
                        }
                    }
                }
            }
            return deptId;
        }
    }
}
