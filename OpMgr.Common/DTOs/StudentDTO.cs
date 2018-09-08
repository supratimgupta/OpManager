using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class StudentDTO
    {
        public int StudentInfoId { get; set; }

        public UserMasterDTO CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public UserMasterDTO UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool Active { get; set; }

        public UserMasterDTO UserDetails { get; set; }

        [Required(ErrorMessage = "Roll Number is required")]
        public string RollNumber { get; set; }

        [Required(ErrorMessage = "Registration Number is required")]
        public string RegistrationNumber { get; set; }

        [Required(ErrorMessage = "Admission Date is required")]
        public DateTime? AdmissionDate { get; set; }

        [Required(ErrorMessage = "Father Contact is required")]
        public string FatherContact { get; set; }

        [Required(ErrorMessage = "Guardian Name is required")]
        public string GuardianName { get; set; }

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


        
        public string Nationality { get; set; }
        public string Category { get; set; }
       
        public string Secondlang { get; set; }
        public int Currclass { get; set; }
        public int classAppld { get; set; }
        public string Prevschool { get; set; }
        public string Prevmedium { get; set; }
        public string Prevstream { get; set; }
        public string sibName { get; set; }
        public string sibclass { get; set; }
        public string sibGender { get; set; }

        public string Religion { get; set; }

        public string Caste { get; set; }

        public string MotherToungue { get; set; }

        public string FatherName { get; set; }

        public string FatherOccupation { get; set; }

        public string FatherDesignation { get; set; }

        public string FatherQualification { get; set; }

        public string FatherOrganisationName { get; set; }

        public string FatherAnnualIncome { get; set; }

        public string FatherContactNo { get; set; }

        public string FatherDepartment { get; set; }

        public string FatherEmailId { get; set; }

        public string FatherOfficeAddress { get; set; }

        public string FatherOfficePhNo { get; set; }

        public string FatherTypeOfBusiness { get; set; }

        public string MotherName { get; set; }

        public string MotherOccupation { get; set; }

        public string MotherOrganisationName { get; set; }

        public string MotherAnnualIncome { get; set; }

        public string MotherQualification { get; set; }

        public string MotherDepartment { get; set; }

        public string MotherDesignation { get; set; }

        public string MotherOfficeAddress { get; set; }

        public string admissionformno { get; set; }
        public string MotherOfficePhNo { get; set; }

        public string MotherTypeOfBusiness { get; set; }

        public string IsChristian { get; set; }

        public string IsParentTeacher { get; set; }

        public string SubjectNameTheyTeach { get; set; }

        public string IsParentFromEngMedium { get; set; }

        public string IsJointOrNuclearFamily { get; set; }

        public string SiblingsInStadOrNot { get; set; }

        public string NoOfSiblings { get; set; }

        public string BrotherSisterInSchool { get; set; }

        public string AnyAlumuniMember { get; set; }

        public string StuInPrivateTution { get; set; }

        public string NoOfTution { get; set; }

        public string FeesPaidForTution { get; set; }
        
        public string Status { get; set; }

        public int NewStandardSectionId { get; set; }

        public StandardSectionMapDTO StandardSectionMap { get; set; }

        public StandardSectionMapDTO NewStandardSectionMap { get; set; }

        public string NoOfStudents { get; set; }

        public HouseTypeDTO HouseType { get; set; }

        public string LikeToPartinCCA { get; set; }

        public string LikeToPartInGames { get; set; }

        public string ModeOfTransport { get; set; }

        public string DropPoint { get; set; }

        public string TransportContactNo { get; set; }

        public string TransportDetails { get; set; }

        public string NameOf1stPerson { get; set; }

        public string NameOf2ndPerson { get; set; }

        public string RelationWithChild1stPerson { get; set; }

        public string RelationWithChild2ndPerson { get; set; }

        //for admission

        public AdmissionStatusDTO AdmissionStatus { get; set; }

        public DateTime? AdmissionExamDate { get; set; }

        public DateTime? AdmissionInterviewDate { get; set; }
    }
}
