using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class StudentClassMapDTO
    {
        public int StudentClassMapId { get; set; }

        public UserMasterDTO CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public UserMasterDTO UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool Active { get; set; }

        public StudentDTO Student { get; set; }

        //public StandardSectionMapDTO StandardSection { get; set; }

        public HouseTypeDTO HouseType { get; set; }

        public CourseMappingDTO Coursemap { get; set; }
    }
}
