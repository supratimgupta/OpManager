using MySql.Data.MySqlClient;
using OpMgr.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.DataAccess.Implementations
{
    public class ResetPasswordSvc : IResetPasswordSvc
    {
        /// <summary>
        /// Required for getting connection related configs
        /// </summary>
        private IConfigSvc _configSvc;
        private DataTable _dtPasswordTable;
        private DataSet _dsPasswordSet;

        /// <summary>
        /// Needed for exception handling
        /// </summary>
        private ILogSvc _logger;

        public ResetPasswordSvc(IConfigSvc configSvc, ILogSvc logger)
        {
            _configSvc = configSvc;
            _logger = logger;
        }
       public bool ResetPassword(string newPassword, int UserMasterId,string studentOrStaffEmpId,string roleDescription)
        {
            bool isResetPassword = false;
            string updateClause = null;
            string whereClause = null;
            string selectClause = null;
            try
            {
                if (!string.IsNullOrEmpty(newPassword) && UserMasterId != 0 || !string.IsNullOrEmpty(studentOrStaffEmpId))
                {
                    using (IDbSvc dbSvc = new DbSvc(_configSvc))
                    {
                        dbSvc.OpenConnection();
                        MySqlCommand command = new MySqlCommand();
                        command.Connection = dbSvc.GetConnection() as MySqlConnection;// establish connection
                        if (UserMasterId==0)
                        {
                            if (!string.IsNullOrEmpty(roleDescription) && string.Equals(roleDescription, "Staff"))
                                selectClause = "select usermasterid from employeedetails where staffemployeeid=@studentOrStaffEmpId AND Active=1";
                            else
                                selectClause = "select usermasterid from studentinfo where registrationnumber=@studentOrStaffEmpId AND Active=1";

                            command.Parameters.Add("@studentOrStaffEmpId", MySqlDbType.String).Value = studentOrStaffEmpId;
                            command.CommandText = selectClause;

                            MySqlDataAdapter msda = new MySqlDataAdapter(command);
                            _dsPasswordSet = new DataSet();
                            msda.Fill(_dsPasswordSet);
                            if (_dsPasswordSet != null && _dsPasswordSet.Tables.Count > 0)
                            {
                                if (_dsPasswordSet.Tables[0].Rows.Count > 0)
                                {
                                    UserMasterId = (int)_dsPasswordSet.Tables[0].Rows[0]["usermasterid"];
                                }
                            }
                        }

                        updateClause = "update usermaster set Password=@newPassword ";
                        whereClause = "WHERE UserMasterId=@UserMasterId AND Active=1";
                        command.CommandText = updateClause+whereClause;
                        command.Parameters.Add("@newPassword", MySqlDbType.String).Value = newPassword;
                        command.Parameters.Add("@UserMasterId", MySqlDbType.Int32).Value = UserMasterId;

                        if(command.ExecuteNonQuery()>0)
                        {
                            isResetPassword = true;                           
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return isResetPassword;
        }
        public string GetPasswordForUser(int UserMasterId)
        {
            string encryptedPassword = "";
            string selectClause = null;
            try
            {
                if (UserMasterId != 0)
                {
                    using (IDbSvc dbSvc = new DbSvc(_configSvc))
                    {
                        dbSvc.OpenConnection();
                        MySqlCommand command = new MySqlCommand();
                        command.Connection = dbSvc.GetConnection() as MySqlConnection;// establish connection
                        selectClause = "select Password from usermaster where UserMasterId=@UserMasterId AND Active=1";
                        command.Parameters.Add("@UserMasterId", MySqlDbType.String).Value = UserMasterId;
                        command.CommandText = selectClause;

                        MySqlDataAdapter msda = new MySqlDataAdapter(command);
                        _dsPasswordSet = new DataSet();
                        msda.Fill(_dsPasswordSet);
                        if(_dsPasswordSet!=null && _dsPasswordSet.Tables.Count>0)
                        {
                            if(_dsPasswordSet.Tables[0].Rows.Count>0)
                            {
                                encryptedPassword = _dsPasswordSet.Tables[0].Rows[0]["Password"].ToString();
                            }
                        }                        
                    }
                }
            }
            catch(Exception ex)
            {

            }
            return encryptedPassword;
        }
    }
}
