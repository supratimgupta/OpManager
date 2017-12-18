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
        private static string _CONN_STR = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        public override string GetDeptId(string deptName)
        {
            string deptId = "-1";
            string query = "select deptId from dbo.department where deptName='"+deptName+"'";
            DataTable dt = this.GetData(query);
            deptId = dt.Rows[0]["deptId"].ToString();
            return deptId;
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

        private string InsertUpdateData(string query)
        {
            using(MySqlConnection conn = new MySqlConnection(_CONN_STR))
            {
                using(MySqlCommand command = new MySqlCommand(query, conn))
                {
                    string returnValue = command.ExecuteScalar().ToString();
                    return returnValue;
                }
            }
        }

        public override DataTable GetGoals()
        {
            string query = "select GoalId, GoalDescription, Active from dbo.Goal";
            return this.GetData(query);
        }

        public override DataTable GetGoalAttributes()
        {
            string query = "select GoalAttributeId, GoalId, Target, KRA, KPI, Weightage, Active from dbo.Goal";
            return this.GetData(query);
        }

        public override DataTable GetEmployeeGoals()
        {
            string query = "select EmployeeGoalId, Active, EmployeeId, RoleId, GoalAttributeId from dbo.EmployeeGoal";
            return this.GetData(query);
        }

        public override string InsertGoal(Common.DTOs.GoalDTO goal)
        {
            string query = "insert into dbo.Goal (GoalDescription, Active) values ('"+goal.GoalDescription+"',1)";
            return this.InsertUpdateData(query);
        }

        public override string InsertGoalAttribute(Common.DTOs.GoalAttributeDTO goalAttribute)
        {
            string query = "insert into dbo.GoalAttribute(Acive, GoalId, Target, KRA, KPI, Weightage) values (1, "+goalAttribute.Goal.GoalId+", '"+goalAttribute.Target+"', '"+goalAttribute.KRA+"', '"+goalAttribute.KPI+"', '"+goalAttribute.WeightAge+"')";
            return this.InsertUpdateData(query);
        }

        public override string InsertEmployeeGoals(Common.DTOs.EmployeeGoalDTO employeeGoal)
        {
            string query = "insert into dbo.EmployeeGoal(Active, RoleId, GoalAttributeId) values (1, '"+employeeGoal.Role.DesignationId+"', '"+employeeGoal.GoalAttribute.GoalAttributeId+"')";
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
            string query = "select UserMasterId from student info where RegistrationNumber='"+studentRegNo+"'";
            DataTable dtData = this.GetData(query);
            if(dtData!=null && dtData.Rows.Count>0)
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
