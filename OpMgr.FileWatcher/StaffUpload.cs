using MySql.Data.MySqlClient;
using OpMgr.Common.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.FileWatcher
{
    partial class StaffUpload
    {
        //string savePath = System.Configuration.ConfigurationManager.AppSettings["StaffBulkUploadExcelFilePath"];
        
        public void StaffImportFileToSQL(string savePath)
        {
            string path = savePath;
            string sexcelconnectionstring = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1;MAXSCANROWS=0\"";
            using (OleDbConnection conn = new OleDbConnection(sexcelconnectionstring))
            {
                conn.Open();
                DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                for (int sc = 0; sc < dt.Rows.Count; sc++)
                {
                    string sheetname = dt.Rows[0]["TABLE_NAME"].ToString();
                    string query = "SELECT * FROM [" + sheetname + "]";
                    OleDbCommand ocmd = new OleDbCommand(query, conn);
                    OleDbDataAdapter rdr = new OleDbDataAdapter(ocmd);
                    DataTable dtdata = new DataTable();
                    rdr.Fill(dtdata);

                    //*******************************************************
                    string conString = System.Configuration.ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
                    using (MySqlConnection con = new MySqlConnection(conString))
                    {
                        try
                        {
                            con.Open();
                            for (int i = 0; i < dtdata.Rows.Count; i++)
                            {
                                string ImageNo = dtdata.Rows[i][0].ToString();
                                string StaffName = dtdata.Rows[i][1].ToString();
                                string ModeOfAppoinrment = dtdata.Rows[i][2].ToString();
                                string DateOfBirth = dtdata.Rows[i][3].ToString();
                                string DateOfJoining = dtdata.Rows[i][4].ToString();
                                string Qualification = dtdata.Rows[i][5].ToString();
                                string LastOrganisation = dtdata.Rows[i][6].ToString();
                                string Address = dtdata.Rows[i][7].ToString();
                                string ContactNo = dtdata.Rows[i][8].ToString();
                                string EmailId = dtdata.Rows[i][9].ToString();
                                string FathersName = dtdata.Rows[i][10].ToString();
                                string ClassX = dtdata.Rows[i][11].ToString();
                                string ClassXII = dtdata.Rows[i][12].ToString();
                                string Graduation = dtdata.Rows[i][13].ToString();
                                string PostGraduation = dtdata.Rows[i][14].ToString();
                                string ProfessionalTraining = dtdata.Rows[i][15].ToString();
                                string SpouseName = dtdata.Rows[i][16].ToString();
                                string NameOf1stChild = dtdata.Rows[i][17].ToString();
                                string ClassType = dtdata.Rows[i][18].ToString();
                                

                                using (MySqlCommand cmd = new MySqlCommand("insert into operationsmanager.staffuploadnew values(@ImageNo,@StaffName,@ModeOfAppoinrment,@DateOfBirth,@DateOfJoining,@Qualification,@LastOrganisation,@Address,@ContactNo,@EmailId,@FathersName,@ClassX,@ClassXII,@Graduation,@PostGraduation,@ProfessionalTraining,@SpouseName,@NameOf1stChild,@ClassType)", con))
                                {
                                    cmd.Parameters.Add("@ImageNo", MySqlDbType.VarChar, 50);
                                    cmd.Parameters.Add("@StaffName", MySqlDbType.VarChar, 100);
                                    cmd.Parameters.Add("@ModeOfAppoinrment", MySqlDbType.VarChar, 100);
                                    cmd.Parameters.Add("@DateOfBirth", MySqlDbType.VarChar, 30);
                                    cmd.Parameters.Add("@DateOfJoining", MySqlDbType.VarChar, 30);
                                    cmd.Parameters.Add("@Qualification", MySqlDbType.VarChar, 200);
                                    cmd.Parameters.Add("@LastOrganisation", MySqlDbType.VarChar, 200);
                                    cmd.Parameters.Add("@Address", MySqlDbType.VarChar, 500);
                                    cmd.Parameters.Add("@ContactNo", MySqlDbType.VarChar, 50);
                                    cmd.Parameters.Add("@EmailId", MySqlDbType.VarChar, 100);
                                    cmd.Parameters.Add("@FathersName", MySqlDbType.VarChar, 100);
                                    cmd.Parameters.Add("@ClassX", MySqlDbType.VarChar, 100);
                                    cmd.Parameters.Add("@ClassXII", MySqlDbType.VarChar, 200);
                                    cmd.Parameters.Add("@Graduation", MySqlDbType.VarChar, 100);
                                    cmd.Parameters.Add("@PostGraduation", MySqlDbType.VarChar, 100);
                                    cmd.Parameters.Add("@ProfessionalTraining", MySqlDbType.VarChar, 200);
                                    cmd.Parameters.Add("@SpouseName", MySqlDbType.VarChar, 100);
                                    cmd.Parameters.Add("@NameOf1stChild", MySqlDbType.VarChar, 100);
                                    cmd.Parameters.Add("@ClassType", MySqlDbType.VarChar, 50);

                                    cmd.Parameters["@ImageNo"].Value = ImageNo;
                                    cmd.Parameters["@StaffName"].Value = StaffName;
                                    cmd.Parameters["@ModeOfAppoinrment"].Value = ModeOfAppoinrment;
                                    cmd.Parameters["@DateOfBirth"].Value = DateOfBirth;
                                    cmd.Parameters["@DateOfJoining"].Value = DateOfJoining;
                                    cmd.Parameters["@Qualification"].Value = Qualification;
                                    cmd.Parameters["@LastOrganisation"].Value = LastOrganisation;
                                    cmd.Parameters["@Address"].Value = Address;
                                    cmd.Parameters["@ContactNo"].Value = ContactNo;
                                    cmd.Parameters["@FathersName"].Value = FathersName;
                                    cmd.Parameters["@ClassX"].Value = ClassX;
                                    cmd.Parameters["@ClassXII"].Value = ClassXII;
                                    cmd.Parameters["@Graduation"].Value = Graduation;
                                    cmd.Parameters["@PostGraduation"].Value = PostGraduation;
                                    cmd.Parameters["@ProfessionalTraining"].Value = ProfessionalTraining;
                                    cmd.Parameters["@SpouseName"].Value = SpouseName;
                                    cmd.Parameters["@NameOf1stChild"].Value = NameOf1stChild;
                                    cmd.Parameters["@ClassType"].Value = ClassType;

                                    cmd.ExecuteScalar();

                                }
                            }

                        }
                        catch (Exception exp)
                        {
                            throw exp;
                        }
                    }
                }

                //*******************************************************
            }
            
        }
    }
}
