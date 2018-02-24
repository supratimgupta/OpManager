using System;
using System.Collections.Generic;
using System.Linq;
using OpMgr.Common.DTOs;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using OpMgr.Common.Contracts;
using System.Data;
using OpMgr.Common.Contracts.Modules;

namespace OpMgr.DataAccess.Implementations
{
    public class ExamMarksSvc : IExamMarksSvc, IDisposable
    {
        private IConfigSvc _configSvc;
        private DataTable _dtData;
        private DataSet _dsData;
        private ILogSvc _logger;

        public ExamMarksSvc(IConfigSvc configSvc, ILogSvc logger)
        {
            _configSvc = configSvc;
            _logger = logger;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public StatusDTO<ExamMarksDTO> Delete(ExamMarksDTO data)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<CourseMappingDTO> GetCourseMappingDetails(CourseMappingDTO coursemappingDTO)
        {
            StatusDTO<CourseMappingDTO> status = new StatusDTO<CourseMappingDTO>();
            status.IsException = false;
            status.IsSuccess = false;
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "getCoursemappingId";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    command.Parameters.Add("@locationId", MySqlDbType.Int32).Value = coursemappingDTO.Location.LocationId;
                    command.Parameters.Add("@standardSectionId", MySqlDbType.Int32).Value = coursemappingDTO.StandardSection.StandardSectionId;
                    command.Parameters.Add("@subjectId", MySqlDbType.Int32).Value = coursemappingDTO.Subject.SubjectId;
                    command.Parameters.Add("@employeeId", MySqlDbType.Int32).Value = coursemappingDTO.Employee.EmployeeId;

                    MySqlDataAdapter rdr = new MySqlDataAdapter(command);
                    _dsData = new DataSet();
                    rdr.Fill(_dsData);
                    //StatusDTO<CourseMappingDTO> status = new StatusDTO<CourseMappingDTO>();
                    CourseMappingDTO courseDTO = new CourseMappingDTO();
                    if (_dsData != null && _dsData.Tables.Count > 0)
                    {
                        if (_dsData.Tables[0].Rows.Count > 0)
                        {
                            status.IsSuccess = true;
                            courseDTO.CourseMappingId = Convert.ToInt32(_dsData.Tables[0].Rows[0]["CourseMappingId"]);
                            courseDTO.CourseFrom = Convert.ToDateTime(_dsData.Tables[0].Rows[0]["CourseFrom"]);
                            courseDTO.CourseTo = Convert.ToDateTime(_dsData.Tables[0].Rows[0]["CourseTo"]);                            
                        }
                    }
                    status.ReturnObj = courseDTO;
                    return status;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }

        }

        public StatusDTO<ExamRuleDTO> GetExamRuleDetails(CourseExam courseexam)
        {
            StatusDTO<ExamRuleDTO> status = new StatusDTO<ExamRuleDTO>();
            status.IsException = false;
            status.IsSuccess = false;
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "get_examRule";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    command.Parameters.Add("@CourseMappingId1", MySqlDbType.Int32).Value = courseexam.CourseMapping.CourseMappingId;
                    command.Parameters.Add("@ExamTypeId1", MySqlDbType.Int32).Value = courseexam.ExamType.ExamTypeId;
                    command.Parameters.Add("@ExamSubTypeId1", MySqlDbType.Int32).Value = courseexam.ExamSubType.ExamSubTypeId;

                    MySqlDataAdapter rdr = new MySqlDataAdapter(command);
                    _dsData = new DataSet();
                    rdr.Fill(_dsData);
                    ExamRuleDTO examruleDTO = new ExamRuleDTO();
                    if (_dsData != null && _dsData.Tables.Count > 0)
                    {
                        if (_dsData.Tables[0].Rows.Count > 0)
                        {
                            status.IsSuccess = true;
                            examruleDTO.ExamRuleId = Convert.ToInt32(_dsData.Tables[0].Rows[0]["ExamRuleId"]);
                        }
                    }
                    status.ReturnObj = examruleDTO;
                    return status;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }

        }

        public StatusDTO<ExamMarksDTO> Insert(ExamMarksDTO data)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<ExamMarksDTO> Select(int rowId)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<List<ExamMarksDTO>> Select(ExamMarksDTO data)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<ExamMarksDTO> Update(ExamMarksDTO data)
        {
            throw new NotImplementedException();
        }
    }
}
