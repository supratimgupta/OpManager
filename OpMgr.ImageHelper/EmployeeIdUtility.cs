using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.ImageHelper
{
    public class EmployeeIdUtility : AbsUtility
    {
        private AbsDataHelper _dataHelper;
        private OpMgr.Common.Contracts.Modules.IUserSvc _userSvc;
        public EmployeeIdUtility(AbsDataHelper dataHelper, OpMgr.Common.Contracts.Modules.IUserSvc userSvc)
        {
            _dataHelper = dataHelper;
            _userSvc = userSvc;
        }

        public override void DoAction()
        {
            using(DataTable dtStaffData = _dataHelper.GetDataFromTemp())
            {
                string nameColumn = Configuration.TEMPLATE_COLUMN;
                string updateQuery = Configuration.TARGET_QUERY;
                List<string> targetColumns = Configuration.TARGET_COLUMNS;
                List<string> indexColumns = Configuration.TARGET_INDEX_COLUMNS;
                int currentCount = _userSvc.GetCurrentEmployeeCounter();
                for(int i=0;i<dtStaffData.Rows.Count;i++)
                {
                    string localUpdateQry = string.Empty;
                    string name = dtStaffData.Rows[i][nameColumn].ToString();
                    string employeeId = OpMgr.Utilities.Utility.MakeEmployeeId(currentCount, name, 5, 2);
                    localUpdateQry = updateQuery.Replace(targetColumns[0] + "=@" + targetColumns[0], targetColumns[0] + "='" + employeeId + "'");
                    localUpdateQry = localUpdateQry.Replace(indexColumns[0] + "=@" + indexColumns[0], indexColumns[0] + "='" + name + "'");
                    _dataHelper.UpdateAction(localUpdateQry);
                    currentCount++;
                }
            }
        }
    }
}
