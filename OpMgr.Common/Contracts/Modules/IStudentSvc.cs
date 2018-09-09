﻿using OpMgr.Common.Contracts;
using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.Contracts.Modules
{
   public interface IStudentSvc: ICRUDSvc<StudentDTO,StudentDTO>
    {
        StatusDTO<List<StudentDTO>> PromoteToNewClass(List<StudentDTO> studentList,string Command,int StandardSectionId, int LocationId);
        StatusDTO<List<StudentDTO>> RunPromotionBatch();
        bool UpdatePromotedStudents(int loggedInUser, string status = "Promotion Confirmed");
        StatusDTO<StudentDTO> GetStudentDetails(string registrationNo);

        StatusDTO<StudentDTO> GetStudentTransactionInfo(int studentInfoId);

        StatusDTO<StudentDTO> InsertHealthReport(StudentDTO student);
        StatusDTO<StudentDTO> UpdateHealthReport(StudentDTO student);
        StatusDTO<StudentDTO> InsertAdmission(StudentDTO student);
        StatusDTO<StudentDTO> UpdateAdmission(StudentDTO student);
        StatusDTO<List<StudentDTO>> AdmissionSearch(StudentDTO student);
        StatusDTO<StudentDTO> SelectAdmission(int AdmissionId);
        StatusDTO<StudentDTO> DeleteAdmission(int admissionid);
    }
}
