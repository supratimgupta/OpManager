using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.Contracts
{
    public interface IBatchProcess<O>
    {
        StatusDTO<O> BatchCommandProcess(List<IDbCommand> commands);
    }
}
