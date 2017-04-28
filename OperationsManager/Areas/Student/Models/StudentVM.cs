using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationsManager.Areas.Student.Models
{
    public class StudentVM : StudentDTO
    {
        public string Name { get; set; }

        public List<StudentVM> studentList { get; set;}

        public SelectList StandardSectionList { get; set; }

        public Boolean IsSearchSuccessful { get; set; }

        public SelectList LocationList { get; set; }

        public SelectList GenderList { get; set; }

        public SelectList IsChristianList { get; set; }

        public SelectList IsParentTeacherList { get; set; }

        public SelectList IsParentFromEngMedList { get; set; }

        public SelectList JointOrNuclearFamilyList { get; set; }

        public SelectList SiblingsInStdOrNotList { get; set; }

        public SelectList AnyAlumunimemberList { get; set; }

        public SelectList StudentinPvtTutionList { get; set; }

        public SelectList RoleList { get; set; }

        public SelectList HouseList { get; set; }

        public SelectList ClassTypeList { get; set; }

        public SelectList SectionList { get; set; }

        public string MODE { get; set; }
        public int HdnUserMasterId { get; set; }

        public List<UserTransactionDTO> Transactions { get; set; }

        public SelectList TransactionMasters { get; set; }

        public SelectList GraceAmountOnList { get; set; }

        public List<TransactionMasterDTO> TransactionMasterSelectList { get; set; }

        public List<KeyValueDTO> CalcInSelectList { get; set; }

        public string ErrorMessage { get; set; }

        public SelectList NextStandardSectionList { get; set; }

        public SelectList PromotionStatusList { get; set; }

        public Boolean IsCommandPromote { get; set; }
    }
}