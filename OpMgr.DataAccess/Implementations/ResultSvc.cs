using MySql.Data.MySqlClient;
using OpMgr.Common.Contracts;
using OpMgr.Common.Contracts.Modules;
using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.DataAccess.Implementations
{
    public class ResultSvc : IResultSvc
    {

        private IConfigSvc _configSvc;
        private DataTable _dtData;
        //private DataSet _dsData;
        //private IExamMarksSvc _examMarksSvc;

        /// <summary>
        /// Needed for exception handling
        /// </summary>
        //private ILogSvc _logger;

        public ResultSvc(IConfigSvc configSvc)
        {
            _configSvc = configSvc;
            //_logger = logger;
            //_examMarksSvc = examMarksSvc;
        }

        public System.Data.DataTable GetResultFormat(int standardSectionId, int examTypeId)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                dbSvc.OpenConnection();
                MySqlCommand command = new MySqlCommand();
                command.Connection = dbSvc.GetConnection() as MySqlConnection;
                command.CommandText = "SELECT et.ExamTypeDescription, rf.exam_type_id, rf.exam_sub_type_id, est.ExamSubTypeDescription"+
                                      " FROM result_format rf LEFT JOIN ExamTypes et on rf.exam_type_id=et.ExamTypeId LEFT JOIN ExamSubTypes est ON rf.exam_sub_type_id=est.ExamSubTypeId WHERE rf.active=1 AND rf.standard_section_id=@stdSecId AND rf.exam_type_id=@exmTypeId";
                command.Parameters.Add("@stdSecId", MySqlDbType.Int32).Value = standardSectionId;
                command.Parameters.Add("@exmTypeId", MySqlDbType.Int32).Value = examTypeId;
                using(MySqlDataAdapter mDA = new MySqlDataAdapter(command))
                {
                    _dtData = new DataTable("FORMAT");
                    mDA.Fill(_dtData);
                    return _dtData;
                }
            }
        }

        public System.Data.DataTable GetResultRule(int standardSectionId)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable GetGradeConfig()
        {
            throw new NotImplementedException();
        }

        public List<ResultCardDTO> GetResult(int standardSectionId, List<int> examTypes, DateTime academicSessionStartDate, DateTime academicSessionEndDate)
        {
            //string currentStudentId = string.Empty;
            List<ResultCardDTO> lstResultCards = new List<ResultCardDTO>();
            //Shouldn't use format as this is a extra setup step (Unnecessary)
            //DataSet dsResultFormats = new DataSet();
            //foreach(int examType in examTypes)
            //{
            //    DataTable dtFormat = this.GetResultFormat(standardSectionId, examType);
            //    dtFormat.TableName = "Format_"+examType;
            //    dsResultFormats.Tables.Add(dtFormat);
            //}
            DataTable dtResults = this.GetStudentResults(standardSectionId, examTypes, academicSessionStartDate, academicSessionEndDate);
            List<string> doneWithStudents = new List<string>();
            for (int i = 0; i < dtResults.Rows.Count;i++)
            {
                string studentId = dtResults.Rows[i]["StudentInfoId"].ToString();
                if (!doneWithStudents.Contains(studentId))
                {
                    DataRow[] arrStudentMarks = dtResults.Select("StudentInfoId='" + studentId + "'");
                    if(arrStudentMarks!=null && arrStudentMarks.Length>0)
                    {
                        ResultCardDTO rc = new ResultCardDTO();
                        rc.ClassName = arrStudentMarks[0]["StandardName"].ToString();
                        rc.SectionName = arrStudentMarks[0]["SectionName"].ToString();
                        rc.SessionEnd = academicSessionEndDate.ToString("dd-MMM-yyyy");
                        rc.SessionStart = academicSessionStartDate.ToString("dd-MMM-yyyy");
                        rc.StudentInfoId = arrStudentMarks[0]["StudentInfoId"].ToString();
                        rc.StudentName = arrStudentMarks[0]["FName"].ToString() + " " + arrStudentMarks[0]["LName"].ToString();
                        rc.StudentRegNo = arrStudentMarks[0]["RegistrationNumber"].ToString();
                        rc.StudentRollNo = arrStudentMarks[0]["RollNumber"].ToString();

                        List<string> lstCompletedLineItems = new List<string>();
                        rc.LineItems = new List<ResultLineItemDTO>();
                        for(int li=0;li<arrStudentMarks.Length;li++)
                        {
                            ResultLineItemDTO rli = new ResultLineItemDTO();
                            rli.Subject = arrStudentMarks[li]["SubjectName"].ToString();
                            rli.SubjectId = arrStudentMarks[li]["SubjectId"].ToString();

                            if(lstCompletedLineItems.Contains(rli.SubjectId))
                            {
                                continue;
                            }

                            DataRow[] arrLineItems = arrStudentMarks.Where(dr => dr["SubjectId"].ToString() == rli.SubjectId).ToArray();
                            if(arrLineItems!=null && arrLineItems.Length>0)
                            {
                                rli.ResultItems = new List<ResultItem>();
                                for(int et=0;et<examTypes.Count;et++)
                                {
                                    DataRow[] arrResults = arrLineItems.Where(dr => dr["ExamTypeId"].ToString() == examTypes[et].ToString()).ToArray();
                                    if(arrResults!=null && arrResults.Length>0)
                                    {
                                        ResultItem rsltItem = new ResultItem();
                                        rsltItem.ExamTypeId = arrResults[0]["ExamTypeId"].ToString();
                                        rsltItem.ExamTypeName = arrResults[0]["ExamTypeDescription"].ToString();
                                        rsltItem.ResultSubItems = new List<ResultSubItem>();
                                        foreach(DataRow drSubItems in arrResults)
                                        {
                                            ResultSubItem rsubItem = new ResultSubItem();
                                            rsubItem.ExamSubTypeId = drSubItems["ExamSubTypeId"].ToString();
                                            rsubItem.ExamSubTypeName = drSubItems["ExamSubTypeDescription"].ToString();
                                            rsubItem.FullMarks = drSubItems["ActualFullMarks"].ToString();
                                            rsubItem.ObtainedMarks = drSubItems["CalculatedMarks"].ToString();
                                            rsubItem.PassMarks = drSubItems["PassMarks"].ToString();
                                            rsltItem.ResultSubItems.Add(rsubItem);
                                        }
                                        rli.ResultItems.Add(rsltItem);
                                    }
                                }
                                rc.LineItems.Add(rli);
                            }
                            lstCompletedLineItems.Add(rli.SubjectId);
                        }
                        lstResultCards.Add(rc);
                    }
                    doneWithStudents.Add(studentId);
                }
            }

            return lstResultCards;
        }


        public DataTable GetStudentResults(int standardSectionId, List<int> examTypes, DateTime academicStartDate, DateTime academicEndDate)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                dbSvc.OpenConnection();
                MySqlCommand command = new MySqlCommand();
                command.Connection = dbSvc.GetConnection() as MySqlConnection;
                string examTypeWhereClause = string.Empty;
                if(examTypes.Count>0)
                {
                    examTypeWhereClause = " AND cexm.ExamTypeId IN (";

                    for (int i = 0; i < examTypes.Count;i++)
                    {
                        if(i==0)
                        {
                            examTypeWhereClause = examTypeWhereClause + "'" + examTypes[i] + "'";
                        }
                        else
                        {
                            examTypeWhereClause = examTypeWhereClause + ",'" + examTypes[i] + "'";
                        }
                    }
                    examTypeWhereClause = examTypeWhereClause + ")";
                }
                command.CommandText = "SELECT u.FName, u.MName, u.LName, st.RegistrationNumber, st.StudentInfoId, st.RollNumber, std.StandardName, sc.SectionName, sub.SubjectName, sub.SubjectId, "+
                                      "et.ExamTypeDescription, et.ExamTypeId, est.ExamSubTypeDescription, est.ExamSubTypeId, exm.CalculatedMarks, er.PassMarks, er.ActualFullMarks " +
                                      "FROM exammarks exm LEFT JOIN StudentInfo st ON exm.StudentInfoId=st.StudentInfoId LEFT JOIN usermaster u ON st.UserMasterId=u.UserMasterId "+
                                      "LEFT JOIN StandardSectionMap scm ON exm.StandardSectionId=scm.StandardSectionId LEFT JOIN standard std ON scm.StandardId=std.StandardId "+
                                      "LEFT JOIN Section sc ON scm.SectionId=sc.SectionId LEFT JOIN subject sub ON exm.SubjectId=sub.SubjectId "+
                                      "LEFT JOIN examrule er ON exm.ExamRuleId=er.ExamRuleId LEFT JOIN courseexam cexm ON er.CourseExamId=cexm.CourseExamId " +
                                      "LEFT JOIN ExamTypes et ON cexm.ExamTypeId=et.ExamTypeId LEFT JOIN ExamSubTypes est ON cexm.ExamSubTypeId=est.ExamSubTypeId " +
                                      "WHERE exm.StandardSectionId=@stdSecId" + examTypeWhereClause + " AND exm.CourseFrom=@courseFrom AND exm.CourseTo=@courseTo ORDER BY st.StudentInfoId,sub.SubjectId";
                command.Parameters.Add("@stdSecId", MySqlDbType.Int32).Value = standardSectionId;
                command.Parameters.Add("@courseFrom", MySqlDbType.DateTime).Value = academicStartDate;
                command.Parameters.Add("@courseTo", MySqlDbType.DateTime).Value = academicEndDate;
                using (MySqlDataAdapter mDA = new MySqlDataAdapter(command))
                {
                    _dtData = new DataTable("ALL_MARKS");
                    mDA.Fill(_dtData);
                    return _dtData;
                }
            }
        }
    }
}
