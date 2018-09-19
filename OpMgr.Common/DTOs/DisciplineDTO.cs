using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
   public class DisciplineDTO
    {
        public int DisciplineId { get; set; }

        public string DisciplineName { get; set; }

        public bool IsActive { get; set; }
        public bool IsSelected { get; set; }
    }
}
