using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.FileWatcher.DataAccess
{
    public abstract class AbsDataAccess
    {
        public abstract string GetDeptId(string deptName);
    }
}
