using MySql.Data.MySqlClient;
using OpMgr.Common.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.FileWatcher
{
    public partial class StudentUpload : AbsFileUpload
    {

        /// <summary>
        ///  Import File to the Database.
        /// </summary>
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
                                string Name = dtdata.Rows[i][0].ToString();
                                string Gender = dtdata.Rows[i][1].ToString();
                                string Standard = dtdata.Rows[i][2].ToString();
                                string Stream = dtdata.Rows[i][3].ToString();                                
                                string ROLL = dtdata.Rows[i][4].ToString();
                                string RegnNo = dtdata.Rows[i][5].ToString();
                                string Religion = dtdata.Rows[i][6].ToString();
                                string Caste = dtdata.Rows[i][7].ToString();
                                string Fathersname = dtdata.Rows[i][8].ToString();
                                string Guardiansname = dtdata.Rows[i][9].ToString();
                                string Address = dtdata.Rows[i][10].ToString();
                                string FATHERSOCCUPATION = dtdata.Rows[i][11].ToString();
                                string ORGANISATIONNAME = dtdata.Rows[i][12].ToString();
                                string TYPEOFBUSINESS = dtdata.Rows[i][13].ToString();
                                string DEPARTMENT = dtdata.Rows[i][14].ToString();
                                string DESIGNATION = dtdata.Rows[i][15].ToString();
                                string OFFICEADDRESS = dtdata.Rows[i][16].ToString();
                                string OFFICEPHONENO = dtdata.Rows[i][17].ToString();
                                string EMAIL = dtdata.Rows[i][18].ToString();
                                string MOTHEROCCUPATION = dtdata.Rows[i][19].ToString();
                                string MNAMEOFORGANISATION = dtdata.Rows[i][20].ToString();
                                string MTYPEOFBUSINESS = dtdata.Rows[i][21].ToString();
                                string MDEPARTMENT = dtdata.Rows[i][22].ToString();
                                string MDESIGNATION = dtdata.Rows[i][23].ToString();
                                string MOFFICEADDRESS = dtdata.Rows[i][24].ToString();
                                string MOFFICEPHONENO = dtdata.Rows[i][25].ToString();
                                string MOBILENO = dtdata.Rows[i][26].ToString();
                                string NAMEOF1STPERSON = dtdata.Rows[i][27].ToString();
                                string RELATIONWITHCHILD1stPerson = dtdata.Rows[i][28].ToString();
                                string NAMEOF2NDPERSON = dtdata.Rows[i][29].ToString();
                                string REATIONWITHCHILD2ndPerson = dtdata.Rows[i][30].ToString();
                                string LIKETOTAKEPARTINCCA = dtdata.Rows[i][31].ToString();
                                string LIKETOTAKEINGAMES = dtdata.Rows[i][32].ToString();
                                string HOUSE = dtdata.Rows[i][33].ToString();
                                string ANYBROTHERSISTERINSCHOOL = dtdata.Rows[i][34].ToString();
                                string MODEOFTRANSPORT = dtdata.Rows[i][35].ToString();
                                string DROPPOINT = dtdata.Rows[i][36].ToString();
                                string TRANSPORTDETAILS = dtdata.Rows[i][37].ToString();
                                string TransportCONTACTNO = dtdata.Rows[i][38].ToString();
                                string ANYSERIOUSAILMENTS = dtdata.Rows[i][39].ToString();
                                string BLOODGROUP = dtdata.Rows[i][40].ToString();
                                string MOTHERTONGUE = dtdata.Rows[i][41].ToString();
                                string FATHERQUALIFICATION = dtdata.Rows[i][42].ToString();
                                string FATHERANNUALINCOME = dtdata.Rows[i][43].ToString();
                                string MOTHERQUALIFICATION = dtdata.Rows[i][44].ToString();
                                string MOTHERANNUALINCOME = dtdata.Rows[i][45].ToString();
                                string INCASEOFCHRISTIAN = dtdata.Rows[i][46].ToString();
                                string PARENTSINTEACHINGPROFESSION = dtdata.Rows[i][47].ToString();
                                string SCHOOLCOLLEGENAME = dtdata.Rows[i][48].ToString();
                                string SUBJECTNAMEPARENT = dtdata.Rows[i][49].ToString();
                                string PARENTSFROMENGMEDIUM = dtdata.Rows[i][50].ToString();
                                string JOINTNUCLEARFAMILY = dtdata.Rows[i][51].ToString();
                                string SIBLINGSINSTADS = dtdata.Rows[i][52].ToString();
                                string NOOFSIBLINGS = dtdata.Rows[i][53].ToString();
                                string ANYALUMNIMEMBERFROMFAMILY = dtdata.Rows[i][54].ToString();
                                string STUDENTTAKINGPVTTUTION = dtdata.Rows[i][55].ToString();
                                string NOOFPVTTUTOR = dtdata.Rows[i][56].ToString();
                                string FEESPAIDPVTTUTOR = dtdata.Rows[i][57].ToString();
                                string DATEOFBIRTH = dtdata.Rows[i][58].ToString();
                                string CONTACTNO1 = dtdata.Rows[i][59].ToString();
                                string CONTACTNO2 = dtdata.Rows[i][60].ToString();
                                string STUDENTIMAGENO = dtdata.Rows[i][61].ToString();
                                string FATHERIMAGENO = dtdata.Rows[i][62].ToString();
                                string MOTHERIMAGENO = dtdata.Rows[i][63].ToString();
                                string Location = dtdata.Rows[i][64].ToString();

                                using (MySqlCommand cmd = new MySqlCommand("insert into operationsmanager.studentuploadshyamnagar values(@Name,@Gender,@Standard,@Stream,@Roll,@RegistrationNo,@Religion,@Caste,@FatherName,@Guardianname,@Address,@FATHERSOCCUPATION,@ORGANISATIONNAME,@TYPEOFBUSINESS,@DEPARTMENT,@DESIGNATION,@OFFICEADDRESS,@OFFICEPHONENO,@EMAIL,@MOTHEROCCUPATION,@MNAMEOFORGANISATION,@MTYPEOFBUSINESS,@MDEPARTMENT,@MDESIGNATION,@MOFFICEADDRESS,@MOFFICEPHONENO,@MOBILENO,@NAMEOF1STPERSON,@RELATIONWITHCHILD1stPerson,@NAMEOF2NDPERSON,@REATIONWITHCHILD2ndPerson,@LIKETOTAKEPARTINCCA,@LIKETOTAKEINGAMES,@HOUSE,@ANYBROTHERSISTERINSCHOOL,@MODEOFTRANSPORT,@DROPPOINT,@TRANSPORTDETAILS,@TransportCONTACTNO,@ANYSERIOUSAILMENTS,@BLOODGROUP,@MOTHERTONGUE,@FATHERQUALIFICATION,@FATHERANNUALINCOME,@MOTHERQUALIFICATION,@MOTHERANNUALINCOME,@INCASEOFCHRISTIAN,@PARENTSINTEACHINGPROFESSION,@SCHOOLCOLLEGENAME,@SUBJECTNAMEPARENT,@PARENTSFROMENGMEDIUM,@JOINTNUCLEARFAMILY,@SIBLINGSINSTADS,@NOOFSIBLINGS,@ANYALUMNIMEMBERFROMFAMILY,@STUDENTTAKINGPVTTUTION,@NOOFPVTTUTOR,@FEESPAIDPVTTUTOR,@DATEOFBIRTH,@CONTACTNO1,@CONTACTNO2,@STUDENTIMAGENO,@FATHERIMAGENO,@MOTHERIMAGENO,@Location)", con))
                                {
                                    cmd.Parameters.Add("@Name", MySqlDbType.VarChar, 100);
                                    cmd.Parameters.Add("@Gender", MySqlDbType.VarChar, 50);
                                    cmd.Parameters.Add("@Standard", MySqlDbType.VarChar, 30);
                                    cmd.Parameters.Add("@Stream", MySqlDbType.VarChar, 30);
                                    cmd.Parameters.Add("@Roll", MySqlDbType.VarChar, 30);
                                    cmd.Parameters.Add("@RegistrationNo", MySqlDbType.VarChar, 30);
                                    cmd.Parameters.Add("@Religion", MySqlDbType.VarChar, 30);
                                    cmd.Parameters.Add("@Caste", MySqlDbType.VarChar, 50);
                                    cmd.Parameters.Add("@FatherName", MySqlDbType.VarChar, 100);
                                    cmd.Parameters.Add("@Guardianname", MySqlDbType.VarChar, 100);
                                    cmd.Parameters.Add("@Address", MySqlDbType.VarChar, 1000);
                                    cmd.Parameters.Add("@FATHERSOCCUPATION", MySqlDbType.VarChar, 100);
                                    cmd.Parameters.Add("@ORGANISATIONNAME", MySqlDbType.VarChar, 200);
                                    cmd.Parameters.Add("@TYPEOFBUSINESS", MySqlDbType.VarChar, 100);
                                    cmd.Parameters.Add("@DEPARTMENT", MySqlDbType.VarChar, 100);
                                    cmd.Parameters.Add("@DESIGNATION", MySqlDbType.VarChar, 100);
                                    cmd.Parameters.Add("@OFFICEADDRESS", MySqlDbType.VarChar, 1000);
                                    cmd.Parameters.Add("@OFFICEPHONENO", MySqlDbType.VarChar, 30);
                                    cmd.Parameters.Add("@EMAIL", MySqlDbType.VarChar, 100);
                                    cmd.Parameters.Add("@MOTHEROCCUPATION", MySqlDbType.VarChar, 100);
                                    cmd.Parameters.Add("@MNAMEOFORGANISATION", MySqlDbType.VarChar, 100);
                                    cmd.Parameters.Add("@MTYPEOFBUSINESS", MySqlDbType.VarChar, 100);
                                    cmd.Parameters.Add("@MDEPARTMENT", MySqlDbType.VarChar, 100);
                                    cmd.Parameters.Add("@MDESIGNATION", MySqlDbType.VarChar, 100);
                                    cmd.Parameters.Add("@MOFFICEADDRESS", MySqlDbType.VarChar, 1000);
                                    cmd.Parameters.Add("@MOFFICEPHONENO", MySqlDbType.VarChar, 100);
                                    cmd.Parameters.Add("@MOBILENO", MySqlDbType.VarChar, 50);
                                    cmd.Parameters.Add("@NAMEOF1STPERSON", MySqlDbType.VarChar, 100);
                                    cmd.Parameters.Add("@RELATIONWITHCHILD1stPerson", MySqlDbType.VarChar, 30);
                                    cmd.Parameters.Add("@NAMEOF2NDPERSON", MySqlDbType.VarChar, 100);
                                    cmd.Parameters.Add("@REATIONWITHCHILD2ndPerson", MySqlDbType.VarChar, 100);
                                    cmd.Parameters.Add("@LIKETOTAKEPARTINCCA", MySqlDbType.VarChar, 50);
                                    cmd.Parameters.Add("@LIKETOTAKEINGAMES", MySqlDbType.VarChar, 100);
                                    cmd.Parameters.Add("@HOUSE", MySqlDbType.VarChar, 100);
                                    cmd.Parameters.Add("@ANYBROTHERSISTERINSCHOOL", MySqlDbType.VarChar, 100);
                                    cmd.Parameters.Add("@MODEOFTRANSPORT", MySqlDbType.VarChar, 100);
                                    cmd.Parameters.Add("@DROPPOINT", MySqlDbType.VarChar, 1000);
                                    cmd.Parameters.Add("@TRANSPORTDETAILS", MySqlDbType.VarChar, 200);
                                    cmd.Parameters.Add("@TransportCONTACTNO", MySqlDbType.VarChar, 50);
                                    cmd.Parameters.Add("@ANYSERIOUSAILMENTS", MySqlDbType.VarChar, 200);
                                    cmd.Parameters.Add("@BLOODGROUP", MySqlDbType.VarChar, 30);
                                    cmd.Parameters.Add("@MOTHERTONGUE", MySqlDbType.VarChar, 50);                                    
                                    cmd.Parameters.Add("@FATHERQUALIFICATION", MySqlDbType.VarChar, 100);
                                    cmd.Parameters.Add("@FATHERANNUALINCOME", MySqlDbType.VarChar, 100);
                                    cmd.Parameters.Add("@MOTHERQUALIFICATION", MySqlDbType.VarChar, 100);
                                    cmd.Parameters.Add("@MOTHERANNUALINCOME", MySqlDbType.VarChar, 50);
                                    cmd.Parameters.Add("@INCASEOFCHRISTIAN", MySqlDbType.VarChar, 50);
                                    cmd.Parameters.Add("@PARENTSINTEACHINGPROFESSION", MySqlDbType.VarChar, 50);
                                    cmd.Parameters.Add("@SCHOOLCOLLEGENAME", MySqlDbType.VarChar, 300);
                                    cmd.Parameters.Add("@SUBJECTNAMEPARENT", MySqlDbType.VarChar, 100);
                                    cmd.Parameters.Add("@PARENTSFROMENGMEDIUM", MySqlDbType.VarChar, 50);
                                    cmd.Parameters.Add("@JOINTNUCLEARFAMILY", MySqlDbType.VarChar, 50);
                                    cmd.Parameters.Add("@SIBLINGSINSTADS", MySqlDbType.VarChar, 100);
                                    cmd.Parameters.Add("@NOOFSIBLINGS", MySqlDbType.VarChar, 20);
                                    cmd.Parameters.Add("@ANYALUMNIMEMBERFROMFAMILY", MySqlDbType.VarChar, 100);
                                    cmd.Parameters.Add("@STUDENTTAKINGPVTTUTION", MySqlDbType.VarChar, 50);
                                    cmd.Parameters.Add("@NOOFPVTTUTOR", MySqlDbType.VarChar, 20);
                                    cmd.Parameters.Add("@FEESPAIDPVTTUTOR", MySqlDbType.VarChar, 50);
                                    cmd.Parameters.Add("@DATEOFBIRTH", MySqlDbType.VarChar, 30);
                                    cmd.Parameters.Add("@CONTACTNO1", MySqlDbType.VarChar, 20);
                                    cmd.Parameters.Add("@CONTACTNO2", MySqlDbType.VarChar, 20);
                                    cmd.Parameters.Add("@STUDENTIMAGENO", MySqlDbType.VarChar, 20);
                                    cmd.Parameters.Add("@FATHERIMAGENO", MySqlDbType.VarChar, 20);
                                    cmd.Parameters.Add("@MOTHERIMAGENO", MySqlDbType.VarChar, 20);
                                    cmd.Parameters.Add("@Location", MySqlDbType.VarChar, 20);


                                    cmd.Parameters["@Name"].Value = Name;
                                    cmd.Parameters["@Gender"].Value = Gender;
                                    cmd.Parameters["@Standard"].Value = Standard;
                                    cmd.Parameters["@Stream"].Value = Stream;
                                    cmd.Parameters["@Roll"].Value = ROLL;
                                    cmd.Parameters["@RegistrationNo"].Value = RegnNo;
                                    cmd.Parameters["@Religion"].Value = Religion;
                                    cmd.Parameters["@Caste"].Value = Caste;
                                    cmd.Parameters["@FatherName"].Value = Fathersname;
                                    cmd.Parameters["@Guardianname"].Value = Guardiansname;
                                    cmd.Parameters["@Address"].Value = Address;
                                    cmd.Parameters["@FATHERSOCCUPATION"].Value = FATHERSOCCUPATION;
                                    cmd.Parameters["@ORGANISATIONNAME"].Value = ORGANISATIONNAME;
                                    cmd.Parameters["@TYPEOFBUSINESS"].Value = TYPEOFBUSINESS;
                                    cmd.Parameters["@DEPARTMENT"].Value = DEPARTMENT;
                                    cmd.Parameters["@DESIGNATION"].Value = DESIGNATION;
                                    cmd.Parameters["@OFFICEADDRESS"].Value = OFFICEADDRESS;
                                    cmd.Parameters["@OFFICEPHONENO"].Value = OFFICEPHONENO;
                                    cmd.Parameters["@EMAIL"].Value = EMAIL;
                                    cmd.Parameters["@MOTHEROCCUPATION"].Value = MOTHEROCCUPATION;
                                    cmd.Parameters["@MNAMEOFORGANISATION"].Value = MNAMEOFORGANISATION;
                                    cmd.Parameters["@MTYPEOFBUSINESS"].Value = MTYPEOFBUSINESS;
                                    cmd.Parameters["@MDEPARTMENT"].Value = MDEPARTMENT;                                    
                                    cmd.Parameters["@MDESIGNATION"].Value = MDESIGNATION;
                                    cmd.Parameters["@MOFFICEADDRESS"].Value = MOFFICEADDRESS;
                                    cmd.Parameters["@MOFFICEPHONENO"].Value = MOFFICEPHONENO;
                                    cmd.Parameters["@MOBILENO"].Value = MOBILENO;
                                    cmd.Parameters["@NAMEOF1STPERSON"].Value = NAMEOF1STPERSON;
                                    cmd.Parameters["@RELATIONWITHCHILD1stPerson"].Value = RELATIONWITHCHILD1stPerson;
                                    cmd.Parameters["@NAMEOF2NDPERSON"].Value = NAMEOF2NDPERSON;
                                    cmd.Parameters["@REATIONWITHCHILD2ndPerson"].Value = REATIONWITHCHILD2ndPerson;
                                    cmd.Parameters["@LIKETOTAKEPARTINCCA"].Value = LIKETOTAKEPARTINCCA;
                                    cmd.Parameters["@LIKETOTAKEINGAMES"].Value = LIKETOTAKEINGAMES;
                                    cmd.Parameters["@HOUSE"].Value = HOUSE;
                                    cmd.Parameters["@ANYBROTHERSISTERINSCHOOL"].Value = ANYBROTHERSISTERINSCHOOL;
                                    cmd.Parameters["@MODEOFTRANSPORT"].Value = MODEOFTRANSPORT;
                                    cmd.Parameters["@DROPPOINT"].Value = DROPPOINT;
                                    cmd.Parameters["@TRANSPORTDETAILS"].Value = TRANSPORTDETAILS;
                                    cmd.Parameters["@TransportCONTACTNO"].Value = TransportCONTACTNO;
                                    cmd.Parameters["@ANYSERIOUSAILMENTS"].Value = ANYSERIOUSAILMENTS;
                                    cmd.Parameters["@BLOODGROUP"].Value = BLOODGROUP;
                                    cmd.Parameters["@MOTHERTONGUE"].Value = MOTHERTONGUE;
                                    cmd.Parameters["@FATHERQUALIFICATION"].Value = FATHERQUALIFICATION;
                                    cmd.Parameters["@FATHERANNUALINCOME"].Value = FATHERANNUALINCOME;
                                    cmd.Parameters["@MOTHERQUALIFICATION"].Value = MOTHERQUALIFICATION;
                                    cmd.Parameters["@MOTHERANNUALINCOME"].Value = MOTHERANNUALINCOME;
                                    cmd.Parameters["@INCASEOFCHRISTIAN"].Value = INCASEOFCHRISTIAN;
                                    cmd.Parameters["@PARENTSINTEACHINGPROFESSION"].Value = PARENTSINTEACHINGPROFESSION;
                                    cmd.Parameters["@SCHOOLCOLLEGENAME"].Value = SCHOOLCOLLEGENAME;
                                    cmd.Parameters["@SUBJECTNAMEPARENT"].Value = SUBJECTNAMEPARENT;
                                    cmd.Parameters["@PARENTSFROMENGMEDIUM"].Value = PARENTSFROMENGMEDIUM;
                                    cmd.Parameters["@JOINTNUCLEARFAMILY"].Value = JOINTNUCLEARFAMILY;
                                    cmd.Parameters["@SIBLINGSINSTADS"].Value = SIBLINGSINSTADS;
                                    cmd.Parameters["@NOOFSIBLINGS"].Value = NOOFSIBLINGS;
                                    cmd.Parameters["@ANYALUMNIMEMBERFROMFAMILY"].Value = ANYALUMNIMEMBERFROMFAMILY;
                                    cmd.Parameters["@STUDENTTAKINGPVTTUTION"].Value = STUDENTTAKINGPVTTUTION;
                                    cmd.Parameters["@NOOFPVTTUTOR"].Value = NOOFPVTTUTOR;
                                    cmd.Parameters["@FEESPAIDPVTTUTOR"].Value = FEESPAIDPVTTUTOR;
                                    cmd.Parameters["@DATEOFBIRTH"].Value = DATEOFBIRTH;
                                    cmd.Parameters["@CONTACTNO1"].Value = CONTACTNO1;
                                    cmd.Parameters["@CONTACTNO2"].Value = CONTACTNO2;
                                    cmd.Parameters["@STUDENTIMAGENO"].Value = STUDENTIMAGENO;
                                    cmd.Parameters["@FATHERIMAGENO"].Value = FATHERIMAGENO;
                                    cmd.Parameters["@MOTHERIMAGENO"].Value = MOTHERIMAGENO;
                                    cmd.Parameters["@Location"].Value = Location;

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

            ////Virtual directory will be mapped in web.Config.
            //string filePathDownload = System.Configuration.ConfigurationManager.AppSettings["AssetBulkFilePathDownload"];

            //Boolean isSuccess = true;
            //string connString = string.Empty;

            //if (FileUpload.FileName.Contains(".xlsx")) ////Read the Excel in the formate of Microsoft Excel 2007 ///
            //{
            //    connString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("~/Upload" + FileUpload.FileName) + ";Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1;MAXSCANROWS=0\"";
            //}
            //else if (FileUpload.FileName.Contains(".xls"))  ///Reads the Excel in the formate of Microsoft Excel 2003 ///
            //{
            //    connString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Server.MapPath("~/Upload" + FileUpload.FileName) + ";Extended Properties=Excel 8.0";
            //}

            //DownloadFile(filePathDownload + FileUpload.FileName, Server.MapPath("~/Upload" + FileUpload.FileName));

            //OleDbConnection conn = new OleDbConnection(connString);
            //try
            //{
            //    //After connecting to the Excelsheet here we are selecting the data 
            //    //using select statement from the Excel sheet

            //    conn.Open();

            //    DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            //    string sheetName = dt.Rows[0]["TABLE_NAME"].ToString();
            //    StringBuilder sb = new StringBuilder();

            //    sb.Append("SELECT [Category Type] AS CategoryType");
            //    sb.Append(",[Asset Type] AS AssetType");
            //    sb.Append(",[AssetId] AS AssetId");
            //    sb.Append(",[Location] AS Location");
            //    sb.Append(",[Building/Tower Type] AS BuildingTowerType");
            //    sb.Append(",[Floor Type] AS FloorType");
            //    sb.Append(",[Wing/Zone Type] AS WingZoneType");
            //    sb.Append(",[Module Name] AS ModuleName");
            //    sb.Append(",[Cubical Seat Number] AS CubicalSeatNumber");

            //    sb.Append(" from [" + sheetName + "]");

            //    OleDbCommand cmd = new OleDbCommand(sb.ToString(), conn);
            //    OleDbDataReader reader = cmd.ExecuteReader();
            //    this.ImportBulkFile(reader);
            //    this.SetAssetInventoryMainImport();
            //    this.InsertAssetInventoryBasicDetails();
            //    this.InsertAssetInventoryAdditionalDetails();
            //    conn.Close();

            //}
            //catch (DataException dex)
            //{
            //    lblErrorMsg.Text = dex.ToString();
            //    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            //    isSuccess = false;

            //}
            //catch (Exception ex)
            //{
            //    lblErrorMsg.Text = ex.ToString();
            //    isSuccess = false;
            //}
            //finally
            //{
            //    if (isSuccess)
            //    {
            //        lblErrorMsg.Text = "Data Inserted Sucessfully";
            //        lblErrorMsg.ForeColor = System.Drawing.Color.Green;
            //    }
            //}
        }

        /// <summary>
        /// Bulk file importing in to the Table by verifying the column names
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>True if inserted correctly</returns>
        //public bool ImportBulkFile(OleDbDataReader reader)
        //{
        //    SqlBulkCopy sbc = new SqlBulkCopy(System.Configuration.ConfigurationManager.ConnectionStrings["connString"].ToString());

        //    //copying Data to the Database

        //    sbc.ColumnMappings.Add("CategoryType", "[CategoryType]");
        //    sbc.ColumnMappings.Add("AssetType", "[AssetType]");
        //    sbc.ColumnMappings.Add("AssetId", "[AssetId]");
        //    sbc.ColumnMappings.Add("Location", "[Location]");
        //    sbc.ColumnMappings.Add("BuildingTowerType", "[BuildingTowerType]");
        //    sbc.ColumnMappings.Add("FloorType", "[FloorType]");
        //    sbc.ColumnMappings.Add("WingZoneType", "[WingZoneType]");
        //    sbc.ColumnMappings.Add("ModuleName", "[ModuleName]");
        //    sbc.ColumnMappings.Add("CubicalSeatNumber", "[CubicalSeatNumber]");

        //    sbc.DestinationTableName = "[Asset].[AssetInventoryMainImport]";
        //    sbc.BulkCopyTimeout = 500;
        //    sbc.WriteToServer(reader);
        //    return true;
        //}
    }
}
