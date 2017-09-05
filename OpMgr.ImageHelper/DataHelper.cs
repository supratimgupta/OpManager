using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace OpMgr.ImageHelper
{
    public class DataHelper : AbsDataHelper
    {

        private string _connString;

        public DataHelper()
        {
            _connString = ConfigurationManager.ConnectionStrings["CONN_STR"].ConnectionString;
        }

        public override DataTable GetDataFromTemp()
        {
            using (DataSet dsStudent = new DataSet())
            {
                using (System.Data.IDbConnection conn = new MySql.Data.MySqlClient.MySqlConnection(_connString))
                {
                    string query = ConfigurationManager.AppSettings["TEMP_TABLE_QUERY"];
                    using(System.Data.IDbCommand command = new MySql.Data.MySqlClient.MySqlCommand(query))
                    {
                        conn.Open();
                        command.Connection = conn;
                        IDataAdapter adap = new MySql.Data.MySqlClient.MySqlDataAdapter(command as MySql.Data.MySqlClient.MySqlCommand);
                        adap.Fill(dsStudent);
                    }
                }
                return dsStudent.Tables.Count>0?dsStudent.Tables[0]:null;
            }            
        }

        public override bool UpdateAction(string query)
        {
            using (System.Data.IDbConnection conn = new MySql.Data.MySqlClient.MySqlConnection(_connString))
            {
                using (System.Data.IDbCommand command = new MySql.Data.MySqlClient.MySqlCommand(query))
                {
                    conn.Open();
                    command.Connection = conn;

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
