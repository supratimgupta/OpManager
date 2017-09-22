using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class GoalAttributeDTO
    {
        public int GoalAttributeId { get; set; }

        public UserMasterDTO CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public UserMasterDTO UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool Active { get; set; }

        public GoalDTO Goal { get; set; }

        public string Target { get; set; }

        public string KRA { get; set; }

        public string KPI { get; set; }

        public int WeightAge { get; set; } 
    }
}
