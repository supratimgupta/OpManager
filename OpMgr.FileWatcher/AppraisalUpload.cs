using System.Data;
using System.Data.OleDb;

namespace OpMgr.FileWatcher
{
    public class AppraisalUpload : AbsFileUpload
    {
        private DataAccess.AbsDataAccess _dataAccess;

        public AppraisalUpload(DataAccess.AbsDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public override void ImportFileToSQL(string savePath)
        {
            string path = savePath;
            string sexcelconnectionstring = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1;MAXSCANROWS=0\"";
            using (OleDbConnection conn = new OleDbConnection(sexcelconnectionstring))
            {
                conn.Open();
                DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                for (int sc = 0; sc < dt.Rows.Count; sc++)
                {
                    string desg = dt.Rows[0]["TABLE_NAME"].ToString();
                    string query = "SELECT * FROM [" + desg + "]";
                    OleDbCommand ocmd = new OleDbCommand(query, conn);
                    OleDbDataAdapter rdr = new OleDbDataAdapter(ocmd);
                    DataTable dtdata = new DataTable();
                    rdr.Fill(dtdata);
                }
            }
        }
    }
}
