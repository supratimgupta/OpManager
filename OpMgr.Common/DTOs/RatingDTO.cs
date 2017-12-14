using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class RatingDTO
    {
        public string Id { get; set; }

        public string RatingLevel { get; set; }

        public decimal Minimum { get; set; }

        public decimal Maximum { get; set; }
    }
}
