using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class StandardSectionMapDTO
    {
        public int StandardSectionId { get; set; }

        public UserMasterDTO CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public UserMasterDTO UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool Active { get; set; }

        public string StandardSectionDesc { get; set; }

        public SectionDTO Section { get; set; }

        public StandardDTO Standard { get; set; }

        public int Serial { get; set; }

        public EmployeeDetailsDTO ClassTeacher { get; set; }

        //provision for serial if needed
    }
}
