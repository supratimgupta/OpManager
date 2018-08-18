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
        //private  string _CONN_STR = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        string _CONN_STR = System.Configuration.ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        public override string GetDeptId(string deptName)
        {
            string deptId = "-1";
            string query = "select deptId from dbo.department where deptName='" + deptName + "'";
            DataTable dt = this.GetData(query);
            deptId = dt.Rows[0]["deptId"].ToString();
            return deptId;
        }

        public override string GetDesignationId(string designationName)
        {
            string designationId = "-1";
            string query = "select PmsDesignationId from PMSdesignation where PmsDesignationDescription='" + designationName + "'";
            DataTable dt = this.GetData(query);
            designationId = dt.Rows[0]["PmsDesignationId"].ToString();
            return designationId;
        }


        private DataTable GetData(string query)
        {
            using (MySqlConnection conn = new MySqlConnection(_CONN_STR))
            {
                using (MySqlCommand command = new MySqlCommand(query, conn))
                {
                    using (MySqlDataAdapter da = new MySqlDataAdapter(command))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            da.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
        }

        private int getInsertGoal(string goaldescription)
        {
            using (MySqlConnection conn = new MySqlConnection(_CONN_STR))
            {
                int returnVal = 0;
                DataTable dtData = new DataTable();
                try
                {
                    conn.Open();
                    MySqlCommand command = new MySqlCommand("get_insertGoalId", conn);
                    //command.CommandText = "get_insertGoalId";
                    command.CommandType = CommandType.StoredProcedure;
                    
                    command.Parameters.Add("@GoalDescription1", MySqlDbType.String).Value = goaldescription;
                    MySqlDataAdapter rdr = new MySqlDataAdapter(command);
                                       
                    rdr.Fill(dtData);

                    returnVal = Convert.ToInt32(dtData.Rows[0]["GoalId1"]);
                }
                catch (Exception exp)
                {
                    throw exp;
                }
                return returnVal;
            }            
        }

        private string InsertUpdateData(string query)
        {
            using (MySqlConnection conn = new MySqlConnection(_CONN_STR))
            {
                int returnVal = 0;
                try
                {
                    conn.Open();
                    using (MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        returnVal = Convert.ToInt32(command.ExecuteScalar());
                        string returnValue = Convert.ToString(returnVal);
                        return returnValue;
                    }
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public override DataTable GetGoals()
        {
            string query = "select GoalId, GoalDescription, Active from goal";
            return this.GetData(query);
        }

        public override DataTable GetGoalAttributes()
        {
            string query = "select GoalAttributeId, GoalId, Target, KRA, KPI, Weightage, Active from goalattribute";
            return this.GetData(query);
        }

        public override DataTable GetEmployeeGoals()
        {
            string query = "select EmployeeGoalId, Active, EmployeeId, RoleId, GoalAttributeId from employeegoal";
            return this.GetData(query);
        }

        public override string InsertGoal(Common.DTOs.GoalDTO goal)
        {
            string goaldesc = goal.GoalDescription.TrimEnd();
            goaldesc = goaldesc.TrimStart();
            //string query = "insert into goal (GoalDescription, Active) values ('" + goaldesc + "',1); select Last_Insert_ID();";
            //return this.InsertUpdateData(query);
            return Convert.ToString(this.getInsertGoal(goaldesc));
        }

        public override string InsertGoalAttribute(Common.DTOs.GoalAttributeDTO goalAttribute)
        {
            string query = "insert into goalAttribute(Active, GoalId, Target, KRA, KPI, Weightage) values (1, " + goalAttribute.Goal.GoalId + ", '" + goalAttribute.Target + "', '" + goalAttribute.KRA + "', '" + goalAttribute.KPI + "', '" + goalAttribute.WeightAge + "'); select Last_Insert_ID();";
            return this.InsertUpdateData(query);
        }

        public override string InsertEmployeeGoals(Common.DTOs.EmployeeGoalDTO employeeGoal)
        {
            string query = "insert into EmployeeGoal(Active, DesignationId, GoalAttributeId) values (1, '" + employeeGoal.Role.DesignationId + "', '" + employeeGoal.GoalAttribute.GoalAttributeId + "'); select Last_Insert_ID();";
            return this.InsertUpdateData(query);
        }

        public override DataTable GetAllRules()
        {
            string query = "select * from transactionrule tr left join standard st on tr.StandardId=st.StandardId left join section sc on tr.SectionId=sc.SectionId";
            return this.GetData(query);
        }

        public override string GetUserMasterId(string studentRegNo)
        {
            string userMasterId = string.Empty;
            string query = "select UserMasterId from student info where RegistrationNumber='" + studentRegNo + "'";
            DataTable dtData = this.GetData(query);
            if (dtData != null && dtData.Rows.Count > 0)
            {
                userMasterId = dtData.Rows[0]["UserMasterId"].ToString();
            }
            return userMasterId;
        }

        public override string InsertTransactionLog(string query)
        {
            return this.InsertUpdateData(query);
        }
    }
}
