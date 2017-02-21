using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpMgr.Common.Contracts.Modules;
using OpMgr.DataAccess.Implementations;
using OpMgr.Common.Contracts;
using System.Data;
using System.Transactions;
using OpMgr.Common.DTOs;

namespace OpMgr.TransactionHandler.Implementations
{
    public class TransactionSvc : ITransactionSvc
    {
        private IUserTransactionSvc _uTransSvc;

        private ILogSvc _logger;

        private ICommonConfigSvc _commonConfig;

        private ITransactionMasterSvc _transMaster;

        private ITransactionRuleSvc _transRule;

        private ITransactionLogSvc _transLog;

        private ILibraryTransactionSvc _libTrans;

        private DataTable _dtTransMaster = null;

        private DataTable _dtTransRule = null;

        private DateTime _runDate;

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

            FillTransDetails();
        }

        public void Dispose()
        {
            if(_dtTransMaster!=null)
            {
                _dtTransMaster.Dispose();
                _dtTransMaster = null;
            }
            if(_dtTransRule!=null)
            {
                _dtTransRule.Dispose();
                _dtTransRule = null;
            }
        }

        private void FillTransDetails()
        {
            if(_dtTransMaster==null)
            {
                _dtTransMaster = _transMaster.GetAllTransactions();
            }
            if(_dtTransRule == null)
            {
                _dtTransRule = _transRule.GetAllRules();
            }

            _commonConfig.PopulateDBConfig();

            string runDate = _commonConfig["TRANS_RUN_DATE"];
            DateTime dtValid = new DateTime();
            if(DateTime.TryParse(runDate, out dtValid))
            {
                _runDate = dtValid;
            }
            else
            {
                _runDate = DateTime.Today.Date;
            }
        }

        private bool IsTransactionRequired(int transMasterId, object nextDayToRunFromDB, object lastRunDate, out DateTime? lastDayOfRun, out DateTime? nextDayToRun, out string isDiffTo)
        {
            bool retValue = false;

            lastDayOfRun = null;
            nextDayToRun = null;

            isDiffTo = string.Empty;

            DataRow[] drTrMasterDet = _dtTransMaster.Select("TranMasterId=" + transMasterId + " AND IsPenalty=0");
            
            
            if(drTrMasterDet != null && drTrMasterDet.Length > 0)
            {
                string trnsFreq = drTrMasterDet[0]["Frequency"].ToString();
                isDiffTo = drTrMasterDet[0]["IsdifferentTo"].ToString();

                if (nextDayToRunFromDB == null)
                {
                    if(string.Equals(trnsFreq, "DAILY"))
                    {
                        retValue = true;
                    }
                    else
                    {
                        string dayToRun = string.Empty;
                        if (string.Equals(trnsFreq, "ONE-TIME", StringComparison.OrdinalIgnoreCase) && lastRunDate==null)
                        {
                            dayToRun = drTrMasterDet[0]["DayToRun"].ToString();
                        }
                        if (string.Equals(trnsFreq, "MONTHLY", StringComparison.OrdinalIgnoreCase))
                        {
                            dayToRun = drTrMasterDet[0]["DayToRun"].ToString() + "-" + DateTime.Today.ToString("MMM-yyyy");
                        }
                        if (string.Equals(trnsFreq, "YEARLY", StringComparison.OrdinalIgnoreCase) || string.Equals(trnsFreq, "YEARLY", StringComparison.OrdinalIgnoreCase) || string.Equals(trnsFreq, "QUARTERLY", StringComparison.OrdinalIgnoreCase) || string.Equals(trnsFreq, "HALF-YEARLY", StringComparison.OrdinalIgnoreCase))
                        {
                            dayToRun = drTrMasterDet[0]["YearlyDayToRun"].ToString() + "-" + DateTime.Today.ToString("yyyy");
                        }

                        if (_runDate >= DateTime.Parse(dayToRun))
                        {
                            retValue = true;
                        }
                    }
                }
                else
                {
                    if (_runDate >= (DateTime)nextDayToRunFromDB)
                    {
                        retValue = true;
                    }
                }
                if(retValue)
                {
                    lastDayOfRun = _runDate;
                    nextDayToRun = new DateTime();
                    switch(trnsFreq.ToUpper())
                    {
                        case "DAILY": nextDayToRun = lastDayOfRun.Value.AddDays(1);
                            break;
                        case "MONTHLY": nextDayToRun = lastDayOfRun.Value.AddMonths(1);
                            break;
                        case "QUARTERLY": nextDayToRun = lastDayOfRun.Value.AddMonths(3);
                            break;
                        case "HALF-YEARLY": nextDayToRun = lastDayOfRun.Value.AddMonths(6);
                            break;
                        case "YEARLY": nextDayToRun = lastDayOfRun.Value.AddYears(1);
                            break;
                        case "ONE-TIME": nextDayToRun = null;
                            break;
                    }
                }
            }

            return retValue;
        }

        private DataRow[] GetRuleRow(int trnsMasterId, string isDiffTo, object standardId, object sectionId, object uMasterId, object classTypeId)
        {
            DataRow[] rule = null;

            if(string.Equals(isDiffTo, "USER", StringComparison.OrdinalIgnoreCase))
            {
                rule = _dtTransRule.Select("TranMasterId=" + trnsMasterId +" AND UserMasterId=" + (int)uMasterId);
            }
            if(string.Equals(isDiffTo, "CLASS-TYPE", StringComparison.OrdinalIgnoreCase))
            {
                rule = _dtTransRule.Select("TranMasterId=" + trnsMasterId + " AND ClassTypeId=" + (int)classTypeId);
            }
            if(string.Equals(isDiffTo, "STANDARD", StringComparison.OrdinalIgnoreCase))
            {
                rule = _dtTransRule.Select("TranMasterId=" + trnsMasterId + " AND StandardId=" + (int)standardId);
            }
            if(string.Equals(isDiffTo, "SECTION", StringComparison.OrdinalIgnoreCase))
            {
                rule = _dtTransRule.Select("TranMasterId=" + trnsMasterId + " AND StandardId=" + (int)standardId + " AND SectionId="+(int)sectionId);
            }

            if(rule.Length>1)
            {
                throw new Exception("Multiple rules are defined with same criteria for Transaction Master id: " + trnsMasterId);
            }

            return rule;
        }

        private double CalculateAmount(object graceAmtOn, object graceAmt, double actualAmt)
        {
            double calculatedAmt = actualAmt;

            if (graceAmtOn != null)
            {
                if (graceAmt!=null)
                {
                    if (string.Equals("PERCENT", graceAmtOn.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        calculatedAmt = actualAmt - ((actualAmt * (double)graceAmt) / 100);
                    }
                    else if (string.Equals("ACTUAL", graceAmtOn.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        calculatedAmt = actualAmt - (double)graceAmt;
                    }
                }
            }
            return calculatedAmt;
        }

        public void AddRegularTransactions()
        {
            IDataReader reader = _uTransSvc.GetUserTransactions();
            FillTransDetails();
            if(reader!=null)
            {
                while(reader.NextResult())
                {
                    try
                    {
                       using(TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                       {
                           DateTime? lastDayOfRun = null;
                           DateTime? nextDayToRun = null;
                           string isDiffTo = string.Empty;
                           int transMasterId = (int)reader["TranMasterId"];
                           if (IsTransactionRequired(transMasterId, reader["NextAutoTransactionOn"], reader["LastAutoTransactionOn"], out lastDayOfRun, out nextDayToRun, out isDiffTo))
                           {
                               DataRow[] rules = GetRuleRow(transMasterId, isDiffTo, reader["StandardId"], reader["SectionId"], reader["UserMasterId"], reader["ClassTypeId"]);

                               if(rules!=null)
                               {
                                   //Insert records as pending in trans log
                                   TransactionLogDTO trnsLogDto = new TransactionLogDTO();
                                   trnsLogDto.Active = true;
                                   trnsLogDto.User = new UserMasterDTO();
                                   trnsLogDto.User.UserMasterId = (int)reader["UserMasterId"];
                                   trnsLogDto.TransactionDate = _runDate;

                                   if (rules[0]["FirstDuedateAfterdays"] != null || !string.IsNullOrEmpty(rules[0]["FirstDuedateAfterdays"].ToString()))
                                   {
                                       trnsLogDto.TransactionDueDate = _runDate.AddDays((int)rules[0]["FirstDuedateAfterdays"]);
                                   }
                                   else
                                   {
                                       trnsLogDto.TransactionDueDate = null;
                                   }

                                   trnsLogDto.TransactionPreviousDueDate = null;
                                   trnsLogDto.ParentTransactionLogId = null;
                                   trnsLogDto.IsCompleted = false;
                                   trnsLogDto.CompletedOn = null;
                                   trnsLogDto.AmountImposed = (double)rules[0]["ActualAmount"];
                                   trnsLogDto.AmountGiven = null;
                                   trnsLogDto.DueAmount = trnsLogDto.AmountImposed;
                                   trnsLogDto.TransferMode = null;
                                   trnsLogDto.Location = null;
                                   if(string.Equals(reader["RoleId"].ToString(), _commonConfig["STUD_ROLE_ID"]))
                                   {
                                       trnsLogDto.StandardSectionMap = new StandardSectionMapDTO();
                                       trnsLogDto.StandardSectionMap.StandardSectionId = (int)reader["StandardSectionId"];
                                   }
                                   else
                                   {
                                       trnsLogDto.StandardSectionMap = null;
                                   }
                                   trnsLogDto.TransactionType = _commonConfig["TR_CREDIT_CODE"];
                                   trnsLogDto.HasPenalty = false;
                                   trnsLogDto.TransactionRule = new TransactionRuleDTO();
                                   trnsLogDto.TransactionRule.TranRuleId = (int)rules[0]["TranRuleId"];

                                   StatusDTO<TransactionLogDTO> status = _transLog.Insert(trnsLogDto);
                                   if(status.IsSuccess)
                                   {
                                       UserTransactionDTO uTrns = new UserTransactionDTO();
                                       uTrns.LastAutoTransactionOn = lastDayOfRun;
                                       uTrns.NextAutoTransactionOn = nextDayToRun;
                                       uTrns.UserTransactionId = (int)reader["UserTransactionId"];
                                       StatusDTO uTrnsStatus = _uTransSvc.UpdateTransLastRunNextRun(uTrns);
                                       if (uTrnsStatus.IsSuccess)
                                       {
                                           ts.Complete();
                                       }
                                   }
                               }
                           }
                       }
                    }
                    catch(Exception exp)
                    {
                        _logger.Log(exp);
                    }
                }
            }
        }

        public void CheckDuesAndAddFine()
        {
            throw new NotImplementedException();
        }

        public void CheckLibraryDueAndAddFine()
        {
            throw new NotImplementedException();
        }
    }
}
