using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class CompetencyDTO
    {
        public int CompetencyId { get; set; }

        public string CompetencyDescription { get; set; }

        public bool Active { get; set; }
    }
}
