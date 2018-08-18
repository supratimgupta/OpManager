using OpMgr.Common.DTOs;
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

                using (DataTable dtGoal = _dataAccess.GetGoals())
                {
                    using (DataTable dtGoalAttributes = _dataAccess.GetGoalAttributes())
                    {
                        for (int sc = 0; sc < dt.Rows.Count; sc++)
                        {
                            string desg = dt.Rows[sc]["TABLE_NAME"].ToString();
                            desg = desg.Replace(@"$", string.Empty);
                            desg = desg.Replace(@"'", string.Empty);
                            desg = desg.TrimStart();
                            desg = desg.TrimEnd();
                            string desgRowId = _dataAccess.GetDesignationId(desg);

                            // string query = "SELECT * FROM [" + desg + "]";
                            string query = "SELECT * FROM [" + dt.Rows[sc]["TABLE_NAME"].ToString() + "]";
                            OleDbCommand ocmd = new OleDbCommand(query, conn);
                            OleDbDataAdapter rdr = new OleDbDataAdapter(ocmd);
                            DataTable dtdata = new DataTable();
                            rdr.Fill(dtdata);
                            string goal = string.Empty;
                            DataRow[] dr = null;
                            GoalDTO goalDTO = null;
                            GoalAttributeDTO goalAttrDTO = null;
                            EmployeeGoalDTO empGoalDTO = null;
                            for (int rc = 0; rc < dtdata.Rows.Count; rc++)
                            {
                                goal = dtdata.Rows[rc]["Goals"].ToString();
                               // dr = dtGoal.Select("GoalDescription='" + goal + "'");
                                //if(dr!=null && dr.Length>0)
                                //{
                                //    //Update logic

                                //}
                                //else
                                //{
                                goalDTO = new GoalDTO();
                                goalDTO.GoalDescription = goal;
                                goalDTO.GoalId = int.Parse(_dataAccess.InsertGoal(goalDTO));

                                goalAttrDTO = new GoalAttributeDTO();
                                goalAttrDTO.Goal = goalDTO;
                                goalAttrDTO.Target = dtdata.Rows[rc]["Target"].ToString();
                                goalAttrDTO.KRA = dtdata.Rows[rc]["KRA"].ToString();
                                goalAttrDTO.KPI = dtdata.Rows[rc]["KPI"].ToString();
                                goalAttrDTO.WeightAge = int.Parse(dtdata.Rows[rc]["Weightage"].ToString());
                                goalAttrDTO.GoalAttributeId = int.Parse(_dataAccess.InsertGoalAttribute(goalAttrDTO));

                                empGoalDTO = new EmployeeGoalDTO();
                                empGoalDTO.GoalAttribute = goalAttrDTO;
                                empGoalDTO.Role = new DesignationDTO();
                                empGoalDTO.Role.DesignationId = int.Parse(desgRowId);

                                empGoalDTO.EmployeeGoalId = int.Parse(_dataAccess.InsertEmployeeGoals(empGoalDTO));
                                //}
                            }
                        }
                    }
                }
            }
        }
    }
}
