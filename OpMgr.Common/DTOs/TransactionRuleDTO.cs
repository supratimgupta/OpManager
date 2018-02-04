using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class TransactionRuleDTO
    {
        public int TranRuleId { get; set; }

        public string RuleName { get; set; }

        public UserMasterDTO CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public UserMasterDTO UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool Active { get; set; }

        public TransactionMasterDTO TranMaster { get; set; }

        /// <summary>
        /// Class Type/ Standard/ Standard + Section/ User
        /// </summary>
        public string IsdifferentTo { get; set; }

        public ClassTypeDTO ClassType { get; set; }

        public StandardDTO Standard { get; set; }

        public SectionDTO Section { get; set; }

        public UserMasterDTO UserDTO { get; set; }

        public int? FirstDueAfterDays { get; set; }

        public int? DueDateIncreasesBy { get; set; }

        //Percent/ Absolute
        public string PenaltyCalculatedIn { get; set; }

        public double? PenaltyAmount { get; set; }

        public double? ActualAmount { get; set; }

        public string PenaltyTransactionType { get; set; }

        public TransactionRuleDTO PenaltyTransactionRule { get; set; }

        public LocationDTO Location { get; set; }
    }
}
