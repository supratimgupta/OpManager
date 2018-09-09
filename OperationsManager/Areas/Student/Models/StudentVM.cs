using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationsManager.Areas.Student.Models
{
    public class StudentVM : StudentDTO
    {
        public string Name { get; set; }

        public string Location { get; set; }

        public List<StudentVM> studentList { get; set;}

        public SelectList StandardSectionList { get; set; }

        public Boolean IsSearchSuccessful { get; set; }

        public string SuccessOrFailureMessage { get; set; }

        public string MsgColor { get; set; }

        public SelectList LocationList { get; set; }

        public SelectList GenderList { get; set; }
        
        public SelectList IsChristianList { get; set; }

        public SelectList IsParentTeacherList { get; set; }

        public SelectList IsParentFromEngMedList { get; set; }

        public SelectList JointOrNuclearFamilyList { get; set; }

        public SelectList SiblingsInStdOrNotList { get; set; }

        public SelectList BrotherSisterInSchoolList { get; set; }

        public SelectList AnyAlumunimemberList { get; set; }

        public SelectList StudentinPvtTutionList { get; set; }

        public SelectList LikeToPartCCAList { get; set; }

        public SelectList LiketoPartGameList { get; set; }

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

        public string StudentImagePath { get; set; }

        public string FatherImagePath { get; set; }

        public string MotherImagePath { get; set; }

        public string DisabledClass { get; set; }

        public string DOBString { get; set; }
        public string xamdate { get; set; }
        public string interviewdate { get; set; }
        public string FName { get; set; }

        public string MName { get; set; }

        public string LName { get; set; }


        public string Height { get; set; }


        public string Weight { get; set; }

        public string BMI { get; set; }

        public string DV { get; set; }
        public string NV { get; set; }
        public string GenHealth { get; set; }
        public string Majorillness { get; set; }
        public string Pulserate { get; set; }
        public string DrRemarks { get; set; }
        public string DrSugg { get; set; }

        public SelectList AdmissionStatusList { get; set; }

        public SelectList CurrentStandardList { get; set; }

        public SelectList AppliedStandardList { get; set; }
    }
}