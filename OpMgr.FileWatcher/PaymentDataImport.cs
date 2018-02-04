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

        private DataTable _dtRules;

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

        private string GetTransactionMaster(string feesType)
        {
            string transactionMaster = string.Empty;
            feesType = feesType.ToUpper();
            if(feesType.Contains("MONTHLY"))
            {
                transactionMaster = "21";
            }
            else if(feesType.Contains("YEARLY"))
            {
                transactionMaster = "26";
            }
            else if(feesType.Contains("BUS"))
            {
                transactionMaster = "25";
            }
            return transactionMaster;
        }

        private void FillRuleData()
        {
            if(_dtRules==null)
            {
                _dtRules = _dataAccess.GetAllRules();
            }
        }

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

                            foreach(DataRow dr in dtdata.Rows)
                            {
                                try
                                {
                                    string transactionMaster = this.GetTransactionMaster(dr["TYPE"].ToString());
                                    if (!string.IsNullOrEmpty(transactionMaster))
                                    {
                                        string standard = dr["CLASS"].ToString();
                                        string section = string.Empty;
                                        if (_dtRules.Columns.Contains("SECTION"))
                                        {
                                            section = dr["SECTION"].ToString();
                                        }
                                        if (_dtRules == null)
                                        {
                                            this.FillRuleData();
                                        }
                                        if (_dtRules != null && _dtRules.Rows.Count > 0)
                                        {
                                            string whereClause = "StandardName='" + standard + "' AND TranMasterId=" + transactionMaster + (string.IsNullOrEmpty(section) ? string.Empty : " AND SectionName='" + section + "'");
                                            DataRow[] arrRules = _dtRules.Select(whereClause);
                                            if (arrRules != null && arrRules.Length == 1)
                                            {
                                                string ruleId = arrRules[0]["TranRuleId"].ToString();

                                                DateTime dueDate = (DateTime)dr["DUE_DATE"];

                                                DateTime transactionDate = DateTime.Parse("03-" + dr["FOR_THE_MONTH"].ToString()+"-"+ DateTime.Today.Year);

                                                string amountImposed = "";

                                                if (dr["TYPE"].ToString().ToUpper().Contains("MONTHLY"))
                                                {
                                                    amountImposed = dr["MONTHLY_DUE"].ToString();
                                                }
                                                else if (dr["TYPE"].ToString().ToUpper().Contains("YEARLY"))
                                                {
                                                    amountImposed = dr["YEARLY_DUE"].ToString();
                                                }
                                                else if (dr["TYPE"].ToString().ToUpper().Contains("BUS"))
                                                {
                                                    amountImposed = dr["BUS_DUE"].ToString();
                                                }

                                                if (!string.IsNullOrEmpty(amountImposed))
                                                {
                                                    string userMasterId = _dataAccess.GetUserMasterId(dr["REGNO"].ToString());

                                                    if (!string.IsNullOrEmpty(userMasterId))
                                                    {
                                                        string query1 = "INSERT INTO transactionlog(Active,TransactionDate,TransactionDueDate,IsCompleted,TranMasterId" +
                                                            "AmountImposed,AmountGiven,DueAmount,TransactionType,HasPenalty,UserMasterId,TranRuleId,PenaltyTransactionRule) VALUES (1," +
                                                            "'" + transactionDate.ToString("yyyy-MM-dd") + "','" + dueDate.ToString("yyyy-MM-dd") + "',0,'" + transactionMaster + "'," +
                                                            "'" + amountImposed + "','0','" + amountImposed + "','D',0,'" + userMasterId + "','" + ruleId + "','" + ruleId + "')";
                                                        _dataAccess.InsertTransactionLog(query1);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch { }
                            }
                        }
                    }
                }
            }
        }
    }
}
