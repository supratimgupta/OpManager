using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class TransactionMasterDTO
    {
        public int TranMasterId { get; set; }

        public UserMasterDTO CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public UserMasterDTO UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool Active { get; set; }

        public string TransactionName { get; set; }

        public string TransactionType { get; set; }

        public bool IsPenalty { get; set; }

        public string Frequency { get; set; }

        public string DayToRun { get; set; }

        public string YearlyDayToRun { get; set; }

        public string IsDiffTo { get; set; }
    }
}
