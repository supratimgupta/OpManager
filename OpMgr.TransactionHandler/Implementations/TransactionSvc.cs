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

            DateTime dtValid = new DateTime();
            //if(!DateTime.TryParse(drTrMasterDet[0]["DayToRun"].ToString(), out dtValid) && !DateTime.TryParse(drTrMasterDet[0]["YearlyDayToRun"].ToString(), out dtValid))
            //{
            //    return false;
            //}

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
                        if (DateTime.TryParse(dayToRun, out dtValid) && _runDate >= DateTime.Parse(dayToRun))
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

        private DataRow[] GetRuleRow(int trnsMasterId, string isDiffTo, object standardId, object sectionId, object uMasterId, object classTypeId, int locationId)
        {
            DataRow[] rule = null;

            if (string.Equals(isDiffTo, "NONE", StringComparison.OrdinalIgnoreCase))
            {
                rule = _dtTransRule.Select("TranMasterId=" + trnsMasterId +" AND LocationId=" + locationId);
            }
            if (string.Equals(isDiffTo, "USER", StringComparison.OrdinalIgnoreCase))
            {
                rule = _dtTransRule.Select("TranMasterId=" + trnsMasterId +" AND UserMasterId=" + (int)uMasterId + " AND LocationId="+locationId);
            }
            if(string.Equals(isDiffTo, "CLASS-TYPE", StringComparison.OrdinalIgnoreCase))
            {
                rule = _dtTransRule.Select("TranMasterId=" + trnsMasterId + " AND ClassTypeId=" + (int)classTypeId + " AND LocationId=" + locationId);
            }
            if(string.Equals(isDiffTo, "STANDARD", StringComparison.OrdinalIgnoreCase))
            {
                rule = _dtTransRule.Select("TranMasterId=" + trnsMasterId + " AND StandardId=" + (int)standardId + " AND LocationId=" + locationId);
            }
            if(string.Equals(isDiffTo, "SECTION", StringComparison.OrdinalIgnoreCase))
            {
                rule = _dtTransRule.Select("TranMasterId=" + trnsMasterId + " AND StandardId=" + (int)standardId + " AND SectionId=" + (int)sectionId + " AND LocationId=" + locationId);
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
                        calculatedAmt = actualAmt - ((actualAmt * double.Parse(graceAmt.ToString())) / 100);
                    }
                    else if (string.Equals("ACTUAL", graceAmtOn.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        calculatedAmt = actualAmt - double.Parse(graceAmt.ToString());
                    }
                }
            }
            return calculatedAmt;
        }

        private double CalculateFineAmount(object fineAmountOn, object fineAmount, double actualAmt)
        {
            double calFineAmount = 0.0;

            if (fineAmountOn != null)
            {
                if (fineAmount != null)
                {
                    if (string.Equals("PERCENT", fineAmountOn.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        calFineAmount = ((actualAmt * double.Parse(fineAmount.ToString())) / 100);
                    }
                    else if (string.Equals("ACTUAL", fineAmountOn.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        calFineAmount = double.Parse(fineAmount.ToString());
                    }
                }
            }
            return calFineAmount;
        }

        public void AddRegularTransactions()
        {
            FillTransDetails();
            DataTable dtReader = _uTransSvc.GetUserTransactions(_runDate);
            if(dtReader != null)
            {
                foreach(DataRow reader in dtReader.Rows)
                {
                    try
                    {
                       using(TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                       {
                           DateTime? lastDayOfRun = null;
                           DateTime? nextDayToRun = null;
                           string isDiffTo = string.Empty;
                           int transMasterId = (int)reader["TranMasterId"];
                           if (IsTransactionRequired(transMasterId, string.IsNullOrEmpty(reader["NextAutoTransactionOn"].ToString())?null: reader["NextAutoTransactionOn"], string.IsNullOrEmpty(reader["LastAutoTransactionOn"].ToString())?null: reader["LastAutoTransactionOn"], out lastDayOfRun, out nextDayToRun, out isDiffTo))
                           {
                               int locationId = -1;
                               if(!string.IsNullOrEmpty(reader["LocationId"].ToString()))
                               {
                                   locationId = Convert.ToInt32(reader["LocationId"]);
                               }

                               DataRow[] rules = GetRuleRow(transMasterId, isDiffTo, reader["StandardId"], reader["SectionId"], reader["UserMasterId"], reader["ClassTypeId"], locationId);

                               if(rules!=null)
                               {
                                   //Insert records as pending in trans log
                                   TransactionLogDTO trnsLogDto = new TransactionLogDTO();
                                   trnsLogDto.Active = true;
                                   trnsLogDto.User = new UserMasterDTO();
                                   trnsLogDto.User.UserMasterId = (int)reader["UserMasterId"];
                                   trnsLogDto.TransactionDate = _runDate;

                                   if (rules[0]["FirstDueAfterDays"] != null || !string.IsNullOrEmpty(rules[0]["FirstDueAfterDays"].ToString()))
                                   {
                                       trnsLogDto.TransactionDueDate = _runDate.AddDays((int)rules[0]["FirstDueAfterDays"]);
                                   }
                                   else
                                   {
                                       trnsLogDto.TransactionDueDate = null;
                                   }

                                   trnsLogDto.TransactionPreviousDueDate = null;
                                   trnsLogDto.ParentTransactionLogId = null;
                                   trnsLogDto.IsCompleted = false;
                                   trnsLogDto.CompletedOn = null;
                                   trnsLogDto.AmountImposed = CalculateAmount(string.IsNullOrEmpty(reader["GraceAmountOn"].ToString())?null: reader["GraceAmountOn"], string.IsNullOrEmpty(reader["GraceAmount"].ToString())?null: reader["GraceAmount"], double.Parse(rules[0]["ActualAmount"].ToString()));
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
                                   trnsLogDto.TransactionType = reader["TransactionType"].ToString();
                                   trnsLogDto.HasPenalty = false;
                                   trnsLogDto.TransactionRule = new TransactionRuleDTO();
                                   trnsLogDto.TransactionRule.TranRuleId = (int)rules[0]["TranRuleId"];

                                    if (trnsLogDto.TransactionDueDate!=null)
                                    {
                                        trnsLogDto.PenaltyTransactionRule = new TransactionRuleDTO();
                                        trnsLogDto.PenaltyTransactionRule.TranRuleId = trnsLogDto.TransactionRule.TranRuleId;
                                    }
                                   
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

            //Disposing source data
            _uTransSvc.Dispose();
            if (dtReader!=null)
            {
                dtReader.Dispose();
                dtReader = null;
            }
        }

        public void CheckDuesAndAddFine()
        {
            FillTransDetails();
            //Currently no penalty for penalty calculation, if that's needed need to add a logic
            DataTable dtReader = _transLog.GetPendingTransactions(_runDate);
            if (dtReader != null)
            {
                foreach(DataRow reader in dtReader.Rows)
                {
                    try
                    {
                        using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                        {
                            if(!string.IsNullOrEmpty(reader["PenaltyTransactionRule"].ToString()))
                            {
                                DataRow[] trnsRule = _dtTransRule.Select("TranRuleId=" + reader["PenaltyTransactionRule"].ToString());
                                if (trnsRule != null && trnsRule.Length > 0)
                                {
                                    TransactionLogDTO trnsLog = new TransactionLogDTO();
                                    trnsLog.Active = true;
                                    trnsLog.User = new UserMasterDTO();
                                    trnsLog.User.UserMasterId = (int)reader["UserMasterId"];
                                    trnsLog.TransactionDate = _runDate;
                                    trnsLog.TransactionDueDate = ((DateTime)reader["TransactionDueDate"]).AddDays((int)trnsRule[0]["DueDateincreasesBy"]);
                                    trnsLog.TransactionPreviousDueDate = (DateTime)reader["TransactionDueDate"];
                                    trnsLog.ParentTransactionLogId = new TransactionLogDTO();
                                    trnsLog.ParentTransactionLogId.TransactionLogId = (int)reader["TransactionLogId"];
                                    trnsLog.IsCompleted = false;
                                    trnsLog.CompletedOn = null;
                                    trnsLog.AmountImposed = CalculateFineAmount(string.IsNullOrEmpty(trnsRule[0]["PenaltyCalculatedIn"].ToString()) ? null : trnsRule[0]["PenaltyCalculatedIn"], string.IsNullOrEmpty(trnsRule[0]["penaltyamount"].ToString()) ? null : trnsRule[0]["penaltyamount"], string.IsNullOrEmpty(reader["DueAmount"].ToString()) ? 0 : double.Parse(reader["DueAmount"].ToString()));
                                    trnsLog.AmountGiven = null;
                                    trnsLog.DueAmount = trnsLog.AmountImposed;
                                    trnsLog.TransferMode = null;
                                    trnsLog.Location = null;
                                    if (reader["StandardSectionId"] == null || string.IsNullOrEmpty(reader["StandardSectionId"].ToString()))
                                    {
                                        trnsLog.StandardSectionMap = null;
                                    }
                                    else
                                    {
                                        trnsLog.StandardSectionMap = new StandardSectionMapDTO();
                                        trnsLog.StandardSectionMap.StandardSectionId = (int)reader["StandardSectionId"];
                                    }
                                    trnsLog.TransactionType = trnsRule[0]["PenaltyTransactionType"] == null || string.IsNullOrEmpty(trnsRule[0]["PenaltyTransactionType"].ToString()) ? reader["TransactionType"].ToString() : trnsRule[0]["PenaltyTransactionType"].ToString();
                                    trnsLog.HasPenalty = false;
                                    trnsLog.OriginalTransLog = new TransactionLogDTO();
                                    if (reader["OriginalTransactionLogId"] == null || string.IsNullOrEmpty(reader["OriginalTransactionLogId"].ToString()))
                                    {
                                        trnsLog.OriginalTransLog.TransactionLogId = (int)reader["TransactionLogId"];
                                    }
                                    else
                                    {
                                        trnsLog.OriginalTransLog.TransactionLogId = (int)reader["OriginalTransactionLogId"];
                                    }
                                    trnsLog.TransactionRule = new TransactionRuleDTO();
                                    trnsLog.TransactionRule.TranRuleId = (int)trnsRule[0]["TranRuleId"];
                                    trnsLog.PenaltyTransactionRule = new TransactionRuleDTO();
                                    trnsLog.PenaltyTransactionRule.TranRuleId = trnsRule[0]["PenaltyTranRuleId"] == null || string.IsNullOrEmpty(trnsRule[0]["PenaltyTranRuleId"].ToString()) ? (int)trnsRule[0]["TranRuleId"] : (int)trnsRule[0]["PenaltyTranRuleId"];
                                    StatusDTO<TransactionLogDTO> status = _transLog.Insert(trnsLog);
                                    if (status.IsSuccess)
                                    {
                                        if (_transLog.UpdateHasPenaltyFlag((int)reader["TransactionLogId"], true, trnsLog.TransactionDueDate.Value, trnsLog.PenaltyTransactionRule.TranRuleId))
                                        {
                                            ts.Complete();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception exp)
                    {
                        _logger.Log(exp);
                    }
                }
            }

            //Disposing source data
            _transLog.Dispose();
            if (dtReader != null)
            {
                dtReader.Dispose();
                dtReader = null;
            }
        }

        public void CheckLibraryDueAndAddFine()
        {
            FillTransDetails();
            DataTable dtReader = _libTrans.GetPendingTransactions(_runDate);
            if (dtReader != null)
            {
                foreach(DataRow reader in dtReader.Rows)
                {
                    try
                    {
                        using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                        {
                            DataRow[] trnsRule = _dtTransRule.Select("TranRuleId=" + _commonConfig["TRNS_RULE_FOR_LIB_DUE"]);
                            if (trnsRule != null && trnsRule.Length > 0)
                            {
                                TransactionLogDTO trnsLog = new TransactionLogDTO();
                                trnsLog.Active = true;
                                trnsLog.User = new UserMasterDTO();
                                trnsLog.User.UserMasterId = (int)reader["UserMasterId"];
                                trnsLog.TransactionDate = _runDate;
                                trnsLog.TransactionDueDate = (_runDate.AddDays((int)trnsRule[0]["FirstDueAfterDays"]));
                                trnsLog.TransactionPreviousDueDate = null;
                                trnsLog.ParentTransactionLogId = null;
                                trnsLog.IsCompleted = false;
                                trnsLog.CompletedOn = null;
                                trnsLog.AmountImposed = double.Parse(trnsRule[0]["ActualAmount"].ToString());
                                trnsLog.AmountGiven = null;
                                trnsLog.DueAmount = trnsLog.AmountImposed;
                                trnsLog.TransferMode = null;
                                trnsLog.Location = null;
                                trnsLog.StandardSectionMap = null;
                                trnsLog.TransactionType = _dtTransMaster.Select("TranMasterId=" + trnsRule[0]["TranMasterId"].ToString())[0]["TransactionType"].ToString();
                                trnsLog.HasPenalty = false;
                                trnsLog.OriginalTransLog = null;
                                trnsLog.TransactionRule = new TransactionRuleDTO();
                                trnsLog.TransactionRule.TranRuleId = (int)trnsRule[0]["TranRuleId"];
                                trnsLog.PenaltyTransactionRule = new TransactionRuleDTO();
                                trnsLog.PenaltyTransactionRule.TranRuleId = trnsLog.TransactionRule.TranRuleId;
                                StatusDTO<TransactionLogDTO> status = _transLog.Insert(trnsLog);
                                if (status.IsSuccess)
                                {
                                    if (_libTrans.MoveLibTransToCashTrans((int)reader["LibraryTranId"], true, status.ReturnObj.TransactionLogId))
                                    {
                                        ts.Complete();
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception exp)
                    {
                        _logger.Log(exp);
                    }
                }
            }

            //Disposing the source data
            _libTrans.Dispose();
            if (dtReader != null)
            {
                dtReader.Dispose();
                dtReader = null;
            }
        }
    }
}
