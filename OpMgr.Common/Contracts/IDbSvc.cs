using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace OpMgr.Common.Contracts
{
    public interface IDbSvc : IDisposable
    {
        IDbConnection GetConnection();

        void OpenConnection();

        void CloseConnection();
    }
}
