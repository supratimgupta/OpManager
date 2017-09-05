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
        public EmployeeIdUtility(AbsDataHelper dataHelper)
        {
            _dataHelper = dataHelper;
        }

        public override void DoAction()
        {
            using(DataTable dtStaffData = _dataHelper.GetDataFromTemp())
            {
                string nameColumn = Configuration.TEMPLATE_COLUMN;
                string updateQuery = Configuration.TARGET_QUERY;
                List<string> targetColumns = Configuration.TARGET_COLUMNS;
                List<string> indexColumns = Configuration.TARGET_INDEX_COLUMNS;
                for(int i=0;i<dtStaffData.Rows.Count;i++)
                {
                    string localUpdateQry = string.Empty;
                    string name = dtStaffData.Rows[i][nameColumn].ToString();
                    string employeeId = OpMgr.Utilities.Utility.MakeEmployeeId(i + 1, name, 5, 2);
                    localUpdateQry = updateQuery.Replace(targetColumns[0] + "=@" + targetColumns[0], targetColumns[0] + "='" + employeeId + "'");
                    localUpdateQry = localUpdateQry.Replace(indexColumns[0] + "=@" + indexColumns[0], indexColumns[0] + "='" + name + "'");
                    _dataHelper.UpdateAction(localUpdateQry);
                }
            }
        }
    }
}
