using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpMgr.Common.Contracts.Modules;
using OpMgr.DataAccess.Implementations;
using OpMgr.Common.Contracts;

namespace OpMgr.TransactionHandler.Implementations
{
    public class TransactionSvc //: ITransactionSvc, IDisposable
    {
        private IUserTransactionSvc _uTransSvc;

        private ILogSvc _logger;

        private ICommonConfigSvc _commonConfig;

        private ITransactionMasterSvc _transMaster;

        private ITransactionRuleSvc _transRule;

        private ITransactionLogSvc _transLog;

        private ILibraryTransactionSvc _libTrans;

        public TransactionSvc(IUserTransactionSvc uTransSvc, ILogSvc logger, ICommonConfigSvc commonConfig, 
                                ITransactionMasterSvc transMaster, ITransactionRuleSvc transRule, ITransactionLogSvc transLog,
                                ILibraryTransactionSvc libTrans)
        {
            _uTransSvc = uTransSvc;
            _logger = logger;
            _commonConfig = commonConfig;
            _transMaster = transMaster;
            _transRule = transRule;
            _transLog = transLog;
            _libTrans = libTrans;
        }


    }
}
