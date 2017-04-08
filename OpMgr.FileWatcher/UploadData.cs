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
    public partial class UploadData : ServiceBase
    {
        private IConfigSvc _configSvc;
        private DataTable _dtData;
        private DataSet _dsData;
        private ILogSvc _logger;

        string diResolverPath = System.Configuration.ConfigurationManager.AppSettings["DIResolverPath"];
        //        StandardKernel kernel = new StandardKernel();
        //        kernel.Load(diResolverPath);
        //ITransactionSvc trnsSvc = kernel.Get<ITransactionSvc>();
        //        trnsSvc.AddRegularTransactions();

        public UploadData(IConfigSvc configSvc, ILogSvc logger)
        {
            _configSvc = configSvc;
            _logger = logger;
        }

        string savePath = System.Configuration.ConfigurationManager.AppSettings["StudentBulkUploadExcelFilePath"];
        public UploadData()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            ImportFileToSQL();
        }

        public void DownloadFile(string uri, string savePath)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFile(new Uri(uri), savePath);
        }

        /// <summary>
        ///  Import File to the Database.
        /// </summary>
        public void ImportFileToSQL()
        {
            string path = @"C:\StudentBulkUpload\STUDENT_INFORMATION.xlsx";
            string sexcelconnectionstring = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1;MAXSCANROWS=0\"";
            using (OleDbConnection conn = new OleDbConnection(sexcelconnectionstring))
            {
                conn.Open();
                DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string sheetname = dt.Rows[0]["TABLE_NAME"].ToString();
                string query = "SELECT * FROM [" + sheetname + "]";
                OleDbCommand ocmd = new OleDbCommand(query, conn);
                OleDbDataReader rdr = ocmd.ExecuteReader();

                //*******************************************************
                string conString = System.Configuration.ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(conString))
                {
                    try
                    {
                        con.Open();
                        while (rdr.Read())
                        {
                            string Name = rdr[0].ToString();
                            string Class = rdr[1].ToString();
                            string Stream = rdr[2].ToString();
                            string DateOfBirth = rdr[3].ToString();
                            string RegistrationNo = rdr[4].ToString();
                            string DateOfAdmission = rdr[5].ToString();
                            string ResidentialAddress = rdr[6].ToString();
                            string FatherName = rdr[7].ToString();
                            string Occupation = rdr[8].ToString();
                            string Designation = rdr[9].ToString();
                            string NameOfOrgAdress = rdr[10].ToString();
                            string ContactNo = rdr[11].ToString();

                            using (MySqlCommand cmd = new MySqlCommand("insert into operationsmanager.studentupload values(@Name,@Class,@Stream,@DateOfBirth,@RegistrationNo,@DateOfAdmission,@ResidentialAddress,@FatherName,@Occupation,@Designation,@NameOfOrgAdress,@ContactNo)", con))
                            {
                                cmd.Parameters.Add("@Name", MySqlDbType.VarChar, 100);
                                cmd.Parameters.Add("@Class", MySqlDbType.VarChar, 30);
                                cmd.Parameters.Add("@Stream", MySqlDbType.VarChar, 30);
                                cmd.Parameters.Add("@DateOfBirth", MySqlDbType.Datetime);
                                cmd.Parameters.Add("@RegistrationNo", MySqlDbType.VarChar, 30);
                                cmd.Parameters.Add("@DateOfAdmission", MySqlDbType.Datetime);
                                cmd.Parameters.Add("@ResidentialAddress", MySqlDbType.VarChar, 2000);
                                cmd.Parameters.Add("@FatherName", MySqlDbType.VarChar, 100);
                                cmd.Parameters.Add("@Occupation", MySqlDbType.VarChar, 100);
                                cmd.Parameters.Add("@Designation", MySqlDbType.VarChar, 100);
                                cmd.Parameters.Add("@NameOfOrgAdress", MySqlDbType.VarChar, 500);
                                cmd.Parameters.Add("@ContactNo", MySqlDbType.VarChar, 100);

                                cmd.Parameters["@Name"].Value = Name;
                                cmd.Parameters["@Class"].Value = Class;
                                cmd.Parameters["@Stream"].Value = Stream;

                                cmd.Parameters["@DateOfBirth"].Value = DateOfBirth;
                                cmd.Parameters["@RegistrationNo"].Value = RegistrationNo;
                                cmd.Parameters["@DateOfAdmission"].Value = DateOfAdmission;

                                cmd.Parameters["@ResidentialAddress"].Value = ResidentialAddress;
                                cmd.Parameters["@FatherName"].Value = FatherName;
                                cmd.Parameters["@Occupation"].Value = Occupation;

                                cmd.Parameters["@Designation"].Value = Designation;
                                cmd.Parameters["@NameOfOrgAdress"].Value = NameOfOrgAdress;
                                cmd.Parameters["@ContactNo"].Value = ContactNo;
                                cmd.ExecuteScalar();

                            }
                        }

                    }
                    catch (Exception exp)
                    {
                        throw exp;
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



        protected override void OnStop()
        {
        }
    }
}
