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
        private ISessionSvc _sessionSvc;

        public ExamMarksSvc(IConfigSvc configSvc, ILogSvc logger, ISessionSvc sessionSvc)
        {
            _configSvc = configSvc;
            _logger = logger;
            _sessionSvc = sessionSvc;
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
                    examruleDTO.CourseExam = new CourseExamDTO();
                    if (_dsData != null && _dsData.Tables.Count > 0)
                    {
                        if (_dsData.Tables[0].Rows.Count > 0)
                        {
                            status.IsSuccess = true;
                            examruleDTO.ExamRuleId = Convert.ToInt32(_dsData.Tables[0].Rows[0]["ExamRuleId"]);
                            examruleDTO.CourseExam.CourseExamId = Convert.ToInt32(_dsData.Tables[0].Rows[0]["CourseExamId"]);
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

        public StatusDTO<List<ExamMarksDTO>> GetStudentDetailsForMarksEntry(int LocationId, int StandardSectionId, int SubjectId, DateTime fromDate, DateTime toDate, int examTypeId, int examSubTypeId)
        {
            StatusDTO<List<ExamMarksDTO>> examMarksList = new StatusDTO<List<ExamMarksDTO>>();
            examMarksList.IsException = false;
            examMarksList.IsSuccess = false;
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "getStudentDetailsForMarksEntry_1";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    command.Parameters.Add("@LocationId1", MySqlDbType.Int32).Value = LocationId;
                    command.Parameters.Add("@StandardSectionId1", MySqlDbType.Int32).Value = StandardSectionId;
                    command.Parameters.Add("@SubjectId1", MySqlDbType.Int32).Value = SubjectId;
                    command.Parameters.Add("@CourseFrom1", MySqlDbType.Date).Value = fromDate.ToString("yyyy-MM-dd");
                    command.Parameters.Add("@CourseTo1", MySqlDbType.Date).Value = toDate.ToString("yyyy-MM-dd");
                    command.Parameters.Add("@ExamTypeId1", MySqlDbType.Int32).Value = examTypeId;
                    command.Parameters.Add("@ExamSubTypeId1", MySqlDbType.Int32).Value = examSubTypeId;

                    MySqlDataAdapter rdr = new MySqlDataAdapter(command);
                    _dsData = new DataSet();
                    rdr.Fill(_dsData);
                    
                    if (_dsData != null && _dsData.Tables.Count > 0 && _dsData.Tables[0]!=null && _dsData.Tables[0].Rows.Count>0)
                    {
                        if(_dsData.Tables[0].Columns.Count>3)
                        {
                            examMarksList.ReturnObj = new List<ExamMarksDTO>();
                            for (int i = 0; i < _dsData.Tables[0].Rows.Count; i++)
                            {
                                ExamMarksDTO exammarks = new ExamMarksDTO();
                                exammarks.Student = new StudentDTO();
                                exammarks.Student.StandardSectionMap = new StandardSectionMapDTO();
                                exammarks.Student.StandardSectionMap.Standard = new StandardDTO();
                                exammarks.Student.StandardSectionMap.Section = new SectionDTO();
                                exammarks.Student.UserDetails = new UserMasterDTO();
                                exammarks.Student.UserDetails.Location = new LocationDTO();

                                exammarks.Student.StudentInfoId = Convert.ToInt32(_dsData.Tables[0].Rows[i]["StudentInfoId"]);
                                exammarks.Student.RegistrationNumber = _dsData.Tables[0].Rows[i]["RegistrationNumber"].ToString();
                                exammarks.Student.StandardSectionMap.Standard.StandardName = _dsData.Tables[0].Rows[i]["StandardName"].ToString();
                                exammarks.Student.StandardSectionMap.Section.SectionName = _dsData.Tables[0].Rows[i]["SectionName"].ToString();

                                exammarks.Student.RollNumber = _dsData.Tables[0].Rows[i]["RollNumber"].ToString();
                                exammarks.Student.UserDetails.FName = _dsData.Tables[0].Rows[i]["FName"].ToString();
                                exammarks.Student.UserDetails.LName = _dsData.Tables[0].Rows[i]["LName"].ToString();
                                exammarks.Student.UserDetails.Location.LocationDescription = _dsData.Tables[0].Rows[i]["LocationDescription"].ToString();
                                exammarks.StandardSection = new StandardSectionMapDTO();
                                exammarks.StandardSection.StandardSectionId = Convert.ToInt32(_dsData.Tables[0].Rows[i]["StandardSectionId"]);
                                exammarks.CourseExam = new CourseExam();
                                exammarks.CourseExam.CourseExamId = Convert.ToInt32(_dsData.Tables[0].Rows[i]["CourseExamId"]);

                                exammarks.DirectGrade = _dsData.Tables[0].Rows[i]["DirectGrade"].ToString();
                                exammarks.SubjectExamType = _dsData.Tables[0].Rows[i]["SubjectExamType"].ToString();

                                if (!String.IsNullOrEmpty(_dsData.Tables[0].Rows[i]["ExamMarksId"].ToString()))
                                {
                                    if (Convert.ToInt32(_dsData.Tables[0].Rows[i]["ExamMarksId"]) > 0)
                                    {
                                        exammarks.ExamMarksId = Convert.ToInt32(_dsData.Tables[0].Rows[i]["ExamMarksId"]);
                                    }
                                }
                                if (!String.IsNullOrEmpty(_dsData.Tables[0].Rows[i]["MarksObtained"].ToString()))
                                {
                                    exammarks.MarksObtained = Convert.ToDouble(_dsData.Tables[0].Rows[i]["MarksObtained"]);
                                }
                                if (!String.IsNullOrEmpty(_dsData.Tables[0].Rows[i]["CalculatedMarks"].ToString()))
                                {
                                    exammarks.CalculatedMarks = Convert.ToDouble(_dsData.Tables[0].Rows[i]["CalculatedMarks"]);
                                }

                                examMarksList.ReturnObj.Add(exammarks);
                            }
                            examMarksList.IsSuccess = true;
                        }
                        else
                        {
                            examMarksList.IsSuccess = false;
                            examMarksList.FailureReason = _dsData.Tables[0].Rows[0]["MESSAGE"].ToString()+"^"+_dsData.Tables[0].Rows[0]["COURSEEXAMID"].ToString();
                        }
                    }
                    
                    return examMarksList;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }

        }

        public StatusDTO<ExamMarksDTO> InsertMarks(ExamMarksDTO data,int CourseExamId,int StandardSectionId, int SubjectId, DateTime FromDate, DateTime ToDate, string directGrade)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "insertexammarks";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    command.Parameters.Add("@createdBy1", MySqlDbType.Int32).Value = _sessionSvc.GetUserSession().UserMasterId; 
                    command.Parameters.Add("@RollNumber1", MySqlDbType.String).Value = data.Student.RollNumber;
                    command.Parameters.Add("@MarksObtained1", MySqlDbType.Decimal).Value = data.MarksObtained;
                    command.Parameters.Add("@CalculatedMarks1", MySqlDbType.Decimal).Value = data.CalculatedMarks;
                    command.Parameters.Add("@SubjectId1", MySqlDbType.Int32).Value = SubjectId;
                    command.Parameters.Add("@ExamRuleId1", MySqlDbType.Int32).Value = data.ExamRule.ExamRuleId;
                    command.Parameters.Add("@CourseExamId1", MySqlDbType.Int32).Value = CourseExamId;
                    command.Parameters.Add("@StudentInfoId1", MySqlDbType.Int32).Value = data.Student.StudentInfoId;
                    command.Parameters.Add("@StandardSectionId1", MySqlDbType.Int32).Value = StandardSectionId;
                    command.Parameters.Add("@CourseFrom1", MySqlDbType.Date).Value = FromDate;
                    command.Parameters.Add("@CourseTo1", MySqlDbType.Date).Value = ToDate;
                    command.Parameters.Add("@DirectGrade1", MySqlDbType.String).Value = directGrade;

                    MySqlDataReader rdr = command.ExecuteReader(CommandBehavior.CloseConnection);
                    _dtData = new DataTable();
                    _dtData.Load(rdr);
                    StatusDTO<ExamMarksDTO> status = new StatusDTO<ExamMarksDTO>();

                    if (_dtData.Rows.Count > 0 )
                    {
                        status.IsSuccess = true;
                        status.ReturnObj = new ExamMarksDTO();
                        status.ReturnObj.ExamMarksId = (int)_dtData.Rows[0]["ExamMarksId"];                        
                    }
                    else
                    {
                        status.IsSuccess = false;
                        status.FailureReason = "User Insertion Failed";
                    }
                    return status;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
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
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "UPDATE exammarks SET UpdatedBy=@updatedBy, UpdatedDate=@updatedDate, MarksObtained=@marksObtained, CalculatedMarks=@calcMarks, "+
                                          "DirectGrade=@directGrade WHERE ExamMarksId=@examMarksId";
                    command.CommandType = CommandType.Text;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    command.Parameters.Add("@updatedBy", MySqlDbType.Int32).Value = _sessionSvc.GetUserSession().UserMasterId;
                    command.Parameters.Add("@updatedDate", MySqlDbType.DateTime).Value = DateTime.Today.Date;
                    command.Parameters.Add("@marksObtained", MySqlDbType.Double).Value = data.MarksObtained;
                    command.Parameters.Add("@calcMarks", MySqlDbType.Double).Value = data.CalculatedMarks;

                    if (!string.IsNullOrEmpty(data.DirectGrade))
                    {
                        command.Parameters.Add("@directGrade", MySqlDbType.VarChar).Value = data.DirectGrade;
                    }
                    else
                    {
                        command.Parameters.Add("@directGrade", MySqlDbType.VarChar).Value = DBNull.Value;
                    }

                    command.Parameters.Add("@examMarksId", MySqlDbType.Int32).Value = data.ExamMarksId;
                    
                    
                    StatusDTO<ExamMarksDTO> status = new StatusDTO<ExamMarksDTO>();

                    if (command.ExecuteNonQuery() > 0)
                    {
                        status.IsSuccess = true;
                        status.ReturnObj = data;
                    }
                    else
                    {
                       status.IsSuccess = false;
                       status.FailureReason = "User Insertion Failed";
                    }
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



        public string GetCourseExamId(int location, int standardSection, int subject, DateTime courseFrom, DateTime courseTo)
        {
            StatusDTO<List<ExamRuleDTO>> status = new StatusDTO<List<ExamRuleDTO>>();
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "Select CourseExamId from CourseExam where CourseMappingId = (select CourseMappingId from CourseMapping where LocationId = @location and SubjectId=@subject and (CourseFrom<=@courseFrom and CourseTo>=@courseFrom) and (CourseFrom<=@courseTo and CourseTo>=@courseTo))";
                    command.Parameters.Add("@location", MySqlDbType.Int32).Value = location;
                    command.Parameters.Add("@subject", MySqlDbType.Int32).Value = subject;
                    command.Parameters.Add("@courseFrom", MySqlDbType.Date).Value = courseFrom.ToString("yyyy-MM-dd");
                    command.Parameters.Add("@courseTo", MySqlDbType.Date).Value = courseTo.ToString("yyyy-MM-dd");

                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    MySqlDataAdapter mDA = new MySqlDataAdapter(command);
                    DataTable dtData = new DataTable("TR_DATA");
                    mDA.Fill(dtData);
                    
                    if (dtData != null && dtData.Rows.Count > 0)
                    {
                        return dtData.Rows[0]["CourseExamId"].ToString();
                    }

                    return "Course exam not setup properly.";
                }
                catch (Exception exp)
                {
                    _logger.Log(exp);
                    throw exp;
                }

            }
        }

        //get subject list based on Location and StandardSection dropdown
        public StatusDTO<List<SubjectDTO>> GetSubjectDropdownData(int LocationId, int StandardSectionId)
        {
            StatusDTO<List<SubjectDTO>> subjectList = new StatusDTO<List<SubjectDTO>>();
            subjectList.IsException = false;
            subjectList.IsSuccess = false;
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "getSubjectDropdownForMarks";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    command.Parameters.Add("@LocationId1", MySqlDbType.Int32).Value = LocationId;
                    command.Parameters.Add("@StandardSectionId1", MySqlDbType.Int32).Value = StandardSectionId;

                    MySqlDataAdapter rdr = new MySqlDataAdapter(command);
                    _dsData = new DataSet();
                    rdr.Fill(_dsData);

                    if (_dsData != null && _dsData.Tables.Count > 0 && _dsData.Tables[0] != null && _dsData.Tables[0].Rows.Count > 0)
                    {
                        if (_dsData.Tables[0].Columns.Count > 3)
                        {
                            subjectList.ReturnObj = new List<SubjectDTO>();
                            for (int i = 0; i < _dsData.Tables[0].Rows.Count; i++)
                            {
                                SubjectDTO subjectDTO = new SubjectDTO();
                                subjectDTO.SubjectId = Convert.ToInt32(_dsData.Tables[0].Rows[i]["SubjectId"]);
                                subjectDTO.SubjectName = _dsData.Tables[0].Rows[i]["SubjectName"].ToString();

                                subjectList.ReturnObj.Add(subjectDTO);
                            }
                            subjectList.IsSuccess = true;
                        }
                        else
                        {
                            subjectList.IsSuccess = false;
                            subjectList.FailureReason = "Subject Doesn't Mapped for this location and standard Section";
                        }
                    }

                    return subjectList;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }

        }
    }
}
