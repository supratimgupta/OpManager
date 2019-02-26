﻿using System;
using System.Collections.Generic;
using OpMgr.Common.DTOs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace OpMgr.Common.Contracts.Modules
{
    public interface IExamMarksSvc : ICRUDSvc<ExamMarksDTO, ExamMarksDTO>, IBatchProcess<bool>
    {
        StatusDTO<CourseMappingDTO> GetCourseMappingDetails(CourseMappingDTO coursemappingDTO);
        StatusDTO<ExamRuleDTO> GetExamRuleDetails(CourseExam courseExamDTO);
        StatusDTO<List<ExamMarksDTO>> GetStudentDetailsForMarksEntry(int LocationId, int StandardSectionId, int SubjectId, DateTime fromDate, DateTime toDate, int examTypeId, int examSubTypeId);
        StatusDTO<List<ExamMarksDTO>> GetStudentDetailsForMarksEntryExcel(int LocationId, int StandardSectionId, int SubjectId, DateTime fromDate, DateTime toDate, int examTypeId, int examSubTypeId);
        StatusDTO<ExamMarksDTO> InsertMarks(ExamMarksDTO data, int CourseExamId, int StandardSectionId, int SubjectId, DateTime FromDate, DateTime ToDate, string directGrade);
        string GetCourseExamId(int location, int standardSection, int subject, DateTime courseFrom, DateTime courseTo);
        //StatusDTO<List<ExamMarksDTO>> GetStudentDetailsForMarksEntry(CourseExam courseexamdto);
        StatusDTO<List<SubjectDTO>> GetSubjectDropdownData(int LocationId, int StandardSectionId);
        IDbCommand GetInsertMarksCommand(ExamMarksDTO data, int CourseExamId, int StandardSectionId, int SubjectId, DateTime FromDate, DateTime ToDate, string directGrade);
        IDbCommand GetUpdateMarksCommand(ExamMarksDTO data);
        IDbCommand GetDeleteMarksCommand(ExamMarksDTO data);
    }
}
