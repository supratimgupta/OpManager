using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpMgr.Common.DTOs;

namespace OpMgr.FileWatcher.DataAccess
{
    public abstract class AbsDataAccess
    {
        public abstract string GetDeptId(string deptName);

        public abstract string GetDesignationId(string designationName);

        public abstract DataTable GetGoals();

        public abstract DataTable GetGoalAttributes();

        public abstract DataTable GetEmployeeGoals();

        public abstract string InsertGoal(GoalDTO goal);

        public abstract string InsertGoalAttribute(GoalAttributeDTO goalAttribute);

        public abstract string InsertEmployeeGoals(EmployeeGoalDTO employeeGoal);

        public abstract DataTable GetAllRules();

        public abstract string GetUserMasterId(string studentRegNo);

        public abstract string InsertTransactionLog(string query);
    }
}
