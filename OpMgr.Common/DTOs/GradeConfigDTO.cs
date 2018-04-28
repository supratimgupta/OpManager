using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class GradeConfigDTO
    {
        public int GradeConfigId { get; set; }

        public string GreadeName { get; set; }

        public double FullMarks { get; set; }

        public double FromMarks { get; set; }

        public double ToMarks { get; set; }

        public bool Active { get; set; }

        public int LocationId { get; set; }
    }
}
