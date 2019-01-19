using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class EmployeeSubjectDTO
    {
        public int EmployeeSubjectMappingId { get; set; }

        public SubjectDTO Subject { get; set; }

        public EmployeeDetailsDTO Employee { get; set; }
    }
}
