using MySql.Data.MySqlClient;
using OpMgr.Common.Contracts;
using OpMgr.Common.Contracts.Modules;
using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.DataAccess.Implementations
{
    public class LibraryTransactionSvc : ILibraryTransactionSvc
    {
        private IConfigSvc _configSvc;

        private ILogSvc _logger;

        private DataTable _dtResult;

        public void Dispose()
        {
            if(_dtResult!=null)
            {
                _dtResult.Dispose();
                _dtResult = null;
            }
        }

        public LibraryTransactionSvc(IConfigSvc configSvc, ILogSvc logger)
        {
            _configSvc = configSvc;
            _logger = logger;
        }

        public StatusDTO<LibraryTransactionDTO> Insert(LibraryTransactionDTO data)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<LibraryTransactionDTO> Update(LibraryTransactionDTO data)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<LibraryTransactionDTO> Delete(LibraryTransactionDTO data)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<List<LibraryTransactionDTO>> Select(LibraryTransactionDTO data)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<LibraryTransactionDTO> Select(int rowId)
        {
            throw new NotImplementedException();
        }

        public bool MoveLibTransToCashTrans(int libTrnsId, bool? IsMovedToTransaction, int? cashTrnsId)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "UPDATE LibraryTransaction SET IsMovedToTransaction=@isMoved, TransactionIdForDue=@trnsIdForDue WHERE LibraryTranId=@libTrnsId";

                    command.Parameters.Add("@isMoved", MySqlDbType.Bit).Value = IsMovedToTransaction;
                    command.Parameters.Add("@trnsIdForDue", MySqlDbType.Int32).Value = cashTrnsId;
                    command.Parameters.Add("@libTrnsId", MySqlDbType.Int32).Value = libTrnsId;

                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    return command.ExecuteNonQuery()>0;
                }
                catch (Exception exp)
                {
                    _logger.Log(exp);
                    throw exp;
                }
            }
        }

        public DataTable GetPendingTransactions(DateTime? runDate)
        {
            using(IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();

                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "SELECT LibraryTranId, UserMasterId, DueDate, IsRemindedSubmission, IsReturned, IsMovedToTransaction FROM LibraryTransaction WHERE Active=1 AND IsRemindedSubmission=1 AND IsReturned<>1 AND IsMovedToTransaction AND TransactionIdForDue IS NULL AND DueDate<@runDate";

                    command.Parameters.Add("@runDate", MySqlDbType.DateTime).Value = runDate.Value.Date;

                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    _dtResult = new DataTable();
                    MySqlDataAdapter mDa = new MySqlDataAdapter(command);
                    mDa.Fill(_dtResult);
                    return _dtResult;
                }
                catch(Exception exp)
                {
                    _logger.Log(exp);
                    throw exp;
                }
            }
        }
    }
}
