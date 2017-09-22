using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.FileWatcher
{
    public abstract class AbsFileUpload
    {
        public abstract void ImportFileToSQL(string savePath);
    }
}
