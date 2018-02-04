using OpMgr.Common.Contracts;
using OpMgr.Common.Contracts.Modules;
using OpMgr.Common.DTOs;
using OpMgr.Configurations.Implementations;
using OpMgr.DataAccess.Implementations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Backlog.Handler
{
    public class BacklogHandler
    {
        public void Handlebacklog()
        {
            IConfigSvc configSvc = new ConfigSvc();

            using(ITransactionBacklog trBacklog = new TransactionBacklog(configSvc))
            {
                using(DataTable dtBacklogData = trBacklog.GetTransactionBacklogs())
                {
                    ITransactionLogSvc trLogSvc = new TransactionLogSvc(configSvc, null);
                    if(dtBacklogData!=null && dtBacklogData.Rows.Count>0)
                    {
                        TransactionLogDTO trLog;
                        foreach(DataRow dr in dtBacklogData.Rows)
                        {
                            try
                            {
                                trLog = new TransactionLogDTO();
                                trLog.User = new UserMasterDTO();
                                trLog.User.UserMasterId = Convert.ToInt32(dr["UserMasterId"]);

                                if(dr["REGNO"].ToString().StartsWith("BKP"))
                                {
                                    trLog.BacklogFromDate = DateTime.Parse("1-Apr-2017");
                                    trLog.BacklogToDate = DateTime.Parse("28-Feb-2018");
                                }
                                if(dr["REGNO"].ToString().StartsWith("SHY"))
                                {
                                    trLog.BacklogFromDate = DateTime.Parse("1-Apr-2017");
                                    trLog.BacklogToDate = DateTime.Parse("31-Jan-2018");
                                }

                                if (!string.IsNullOrEmpty(dr["MONTHLY"].ToString()))
                                {
                                    trLog.MonthlyBacklogAmount = Convert.ToDouble(dr["MONTHLY"]);
                                }
                                else
                                {
                                    trLog.MonthlyBacklogAmount = null;
                                }
                                if (!string.IsNullOrEmpty(dr["YEARLY"].ToString()))
                                {
                                    trLog.YearlyBacklogAmount = Convert.ToDouble(dr["YEARLY"]);
                                }
                                else
                                {
                                    trLog.YearlyBacklogAmount = null;
                                }
                                if (!string.IsNullOrEmpty(dr["LATEFINE"].ToString()))
                                {
                                    trLog.LateFineBacklogAmount = Convert.ToDouble(dr["LATEFINE"]);
                                }
                                else
                                {
                                    trLog.LateFineBacklogAmount = null;
                                }
                                if (!string.IsNullOrEmpty(dr["BUS"].ToString()))
                                {
                                    trLog.BusBacklogAmount = Convert.ToDouble(dr["BUS"]);
                                }
                                else
                                {
                                    trLog.BusBacklogAmount = null;
                                }

                                trLogSvc.InsertBacklogAmount(trLog);
                            }
                            catch { }
                        }
                    }
                }
            }

        }
    }
}
