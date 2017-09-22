using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
   public class GoalDTO
    {
        public int GoalId { get; set; }

        public string GoalDescription { get; set; }
        public bool Active { get; set; }
    }
}
