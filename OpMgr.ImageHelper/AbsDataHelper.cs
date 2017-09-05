using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.ImageHelper
{
    public abstract class AbsDataHelper
    {
        public abstract DataTable GetDataFromTemp();

        public abstract bool UpdateAction(string command);
    }
}
