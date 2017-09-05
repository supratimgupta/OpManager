using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.ImageHelper
{
    public static class UtilityFactory
    {
        public static AbsUtility GetUtilityClass(string action, AbsDataHelper dataHelper)
        {
            if(string.Equals(action, "IMAGE_UPDATE", StringComparison.OrdinalIgnoreCase))
            {
                return new ImageUtility(dataHelper);
            }
            if(string.Equals(action, "CREATE_EMPLOYEE_ID", StringComparison.OrdinalIgnoreCase))
            {
                return new EmployeeIdUtility(dataHelper);
            }
            return null;
        }

        public static AbsDataHelper GetDataHelper(string action)
        {
            return new DataHelper();
        }
    }
}
