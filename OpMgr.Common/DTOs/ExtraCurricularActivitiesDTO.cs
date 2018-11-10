using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class ExtraCurricularActivitiesDTO
    {
        public int ExtraCurricularId { get; set; }

        public string ActivityName { get; set; }

        public string ActivityType { get; set; }

        public bool IsActive { get; set; }

        public bool IsSelected { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public int SelectedActivityId {get;set;}
    }
}
