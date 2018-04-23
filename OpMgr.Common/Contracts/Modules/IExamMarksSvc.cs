﻿using System;
using System.Collections.Generic;
using OpMgr.Common.DTOs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.Contracts.Modules
{
    public interface IExamMarksSvc :ICRUDSvc<ExamMarksDTO,ExamMarksDTO>
    {
        StatusDTO<CourseMappingDTO> GetCourseMappingDetails(CourseMappingDTO coursemappingDTO);
        StatusDTO<ExamRuleDTO> GetExamRuleDetails(CourseExam courseExamDTO);
        StatusDTO<List<ExamMarksDTO>> GetStudentDetailsForMarksEntry(int LocationId, int StandardSectionId, int SubjectId);
        StatusDTO<ExamMarksDTO> InsertMarks(ExamMarksDTO data, int CourseExamId, int StandardSectionId, int SubjectId, DateTime FromDate, DateTime ToDate);
        //StatusDTO<List<ExamMarksDTO>> GetStudentDetailsForMarksEntry(CourseExam courseexamdto);
    }
}
