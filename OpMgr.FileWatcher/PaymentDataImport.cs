using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.FileWatcher
{
    public class PaymentDataImport : AbsFileUpload
    {
        private DataAccess.AbsDataAccess _dataAccess;

        public PaymentDataImport(DataAccess.AbsDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        /*
         * Columns to enter data:
         * 1. Active
         * 2. TransactionDate = CURRENT DATE
         * 3. TransactionDueDate = DUE DATE
         * 4. TransactionPreviousDueDate = DUE DATE
         * 5. IsCompleted = 0
         * 6. TranMasterId - Hardcode for monthly and yearly
         * 7. AmountImposed
         * 8. AmountGiven = 0
         * 9. DueAmount = AmountImposed
         * 10. LocationId - Hardcode shyamnagar or barrackpore
         * 11. TransactionType - D
         * 12. HasPenalty = 0
         * 13. StandardSectionId - ?
         * 14. UserMasterId
         * 15. TranRuleId - Hardcode for rule for monthly transaction rule
         */
        public override void ImportFileToSQL(string savePath)
        {
            string path = savePath;
            string sexcelconnectionstring = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1;MAXSCANROWS=0\"";
            using (OleDbConnection conn = new OleDbConnection(sexcelconnectionstring))
            {
                conn.Open();
                DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                using(DataTable dtGoal = _dataAccess.GetGoals())
                {
                    using(DataTable dtGoalAttributes = _dataAccess.GetGoalAttributes())
                    {
                        for (int sc = 0; sc < dt.Rows.Count; sc++)
                        {
                            string desg = dt.Rows[0]["TABLE_NAME"].ToString();
                            string desgRowId = _dataAccess.GetDeptId(desg);
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


        //public IEnumerable<OpMgr.Common.DTOs.TransactionLogDTO> CovertToTransactionLog(DataTable dtData)
        //{
            
        //}
    }
}
