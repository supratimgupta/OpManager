using MySql.Data.MySqlClient;
using OpMgr.Common.Contracts;
using OpMgr.Common.Contracts.Modules;
using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OpMgr.DataAccess.Implementations
{
    public class ResultSvc : IResultSvc
    {

        private IConfigSvc _configSvc;
        private DataTable _dtData;
        private IStudentRemarksSvc _remarksSvc;
        //private DataSet _dsData;
        //private IExamMarksSvc _examMarksSvc;

        /// <summary>
        /// Needed for exception handling
        /// </summary>
        //private ILogSvc _logger;

        public ResultSvc(IConfigSvc configSvc, IStudentRemarksSvc remarksSvc)
        {
            _configSvc = configSvc;
            _remarksSvc = remarksSvc;
            //_logger = logger;
            //_examMarksSvc = examMarksSvc;
        }

        public System.Data.DataTable GetResultFormat(int standardSectionId, string resultType, int locationId)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                dbSvc.OpenConnection();
                MySqlCommand command = new MySqlCommand();
                command.Connection = dbSvc.GetConnection() as MySqlConnection;
                command.CommandText = "SELECT column_header, value_expression, grade_expression , column_sequence, value_type, fixed_column_name, has_grade, is_allowed_for_grade, is_calc_for_total FROM result_schema WHERE standard_section=@stdSec AND location=@loc AND result_type=@resType ORDER BY column_sequence";
                command.Parameters.Add("@stdSec", MySqlDbType.Int32).Value = standardSectionId;
                command.Parameters.Add("@loc", MySqlDbType.Int32).Value = locationId;
                command.Parameters.Add("@resType", MySqlDbType.String).Value = resultType;
                using (MySqlDataAdapter mDA = new MySqlDataAdapter(command))
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

        public System.Data.DataTable GetGradeConfig(int location)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                dbSvc.OpenConnection();
                MySqlCommand command = new MySqlCommand();
                command.Connection = dbSvc.GetConnection() as MySqlConnection;
                command.CommandText = "SELECT full_marks, from_marks, to_marks, grade_name FROM grade_config WHERE active=1 AND location=@loc";
                command.Parameters.Add("@loc", MySqlDbType.Int32).Value = location;
                using (MySqlDataAdapter mDA = new MySqlDataAdapter(command))
                {
                    _dtData = new DataTable("GRADE");
                    mDA.Fill(_dtData);
                    return _dtData;
                }
            }
        }

        public List<ResultCardDTO> GetResult(int locationId, int standardSectionId, string resultType, DateTime academicSessionStartDate, DateTime academicSessionEndDate)
        {
            List<ResultCardDTO> lstResultCards = new List<ResultCardDTO>();
            DataTable dtResults = this.GetStudentResults(locationId, standardSectionId, academicSessionStartDate, academicSessionEndDate);
            DataTable dtResultFormat = this.GetResultFormat(standardSectionId, resultType, locationId);
            DataTable dtGrades = this.GetGradeConfig(locationId);
            List<StudentRemarksDTO> remarks = _remarksSvc.GetStudentRemarks(standardSectionId, resultType, academicSessionStartDate, academicSessionEndDate, locationId);
            ResultCardDTO rsCard = null;
            List<string> lstDoneWithStudents = new List<string>();
            Dictionary<string, double> dicSubjectHeighest = new Dictionary<string, double>();
            Dictionary<string, double> dicSubjectTotal = new Dictionary<string, double>();
            if (dtResults != null && dtResults.Rows.Count > 0 && dtResultFormat != null && dtResultFormat.Rows.Count > 0)
            {
                for (int st = 0; st < dtResults.Rows.Count; st++)
                {
                    rsCard = new ResultCardDTO();
                    rsCard.StudentInfoId = dtResults.Rows[st]["StudentInfoId"].ToString();
                    rsCard.TotalMarks = 0;
                    if (lstDoneWithStudents.Contains(rsCard.StudentInfoId))
                    {
                        continue;
                    }
                    rsCard.ClassName = dtResults.Rows[st]["StandardName"].ToString();
                    rsCard.SectionName = dtResults.Rows[st]["SectionName"].ToString();
                    rsCard.SessionEnd = academicSessionEndDate.ToString("yyyy");
                    rsCard.SessionStart = academicSessionStartDate.ToString("yyyy");
                    rsCard.StudentName = dtResults.Rows[st]["FName"].ToString() + " " + dtResults.Rows[st]["LName"].ToString();
                    rsCard.StudentRegNo = dtResults.Rows[st]["RegistrationNumber"].ToString();
                    rsCard.StudentRollNo = dtResults.Rows[st]["RollNumber"].ToString();
                    List<string> lstDoneWithSubjects = new List<string>();
                    DataRow[] arrStudentResults = dtResults.Select("StudentInfoId=" + rsCard.StudentInfoId);
                    if (arrStudentResults != null && arrStudentResults.Length > 0)
                    {
                        rsCard.ResultRows = new List<ResultCardRows>();
                        rsCard.GradeResultRows = new List<ResultCardRows>();
                        ResultCardRows rsRows = null;
                        foreach (DataRow drSub in arrStudentResults)
                        {
                            string subjectId = drSub["SubjectId"].ToString();
                            if (lstDoneWithSubjects.Contains(subjectId))
                            {
                                continue;
                            }
                            rsRows = new ResultCardRows();
                            DataRow[] drSubjects = arrStudentResults.Where(dr => string.Equals(dr["SubjectId"].ToString(), subjectId)).ToArray();
                            rsRows.SubjectName = drSub["SubjectName"].ToString();
                            //----Need to modify this part for now skipping all G subjects ---
                            //if(string.Equals(drSub["SubjectExamType"].ToString(), "G", StringComparison.OrdinalIgnoreCase))
                            //{
                            //    lstDoneWithSubjects.Add(subjectId);
                            //    continue;
                            //}
                            //----Need to modify this part for now skipping all G subjects ---
                            rsRows.ResultColumns = new List<ResultCardColumns>();
                            ResultCardColumns resultCol = null;
                            for (int rf = 0; rf < dtResultFormat.Rows.Count; rf++)
                            {
                                resultCol = new ResultCardColumns();
                                resultCol.ColumnName = dtResultFormat.Rows[rf]["column_header"].ToString();
                                resultCol.ColumnSequence = Convert.ToInt32(dtResultFormat.Rows[rf]["column_sequence"]);
                                string expression = dtResultFormat.Rows[rf]["value_expression"].ToString();
                                string gradeExpression = dtResultFormat.Rows[rf]["grade_expression"].ToString();
                                if (string.IsNullOrEmpty(gradeExpression) && string.Equals(drSub["SubjectExamType"].ToString(), "G", StringComparison.OrdinalIgnoreCase))
                                {
                                    resultCol.ColumnValue = string.Empty;
                                    rsRows.ResultColumns.Add(resultCol);
                                    continue;
                                }
                                if (string.IsNullOrEmpty(expression) && !string.Equals(drSub["SubjectExamType"].ToString(), "G", StringComparison.OrdinalIgnoreCase))
                                {
                                    resultCol.ColumnValue = string.Empty;
                                    rsRows.ResultColumns.Add(resultCol);
                                    continue;
                                }
                                string valueType = dtResultFormat.Rows[rf]["value_type"].ToString();
                                string fixedColName = dtResultFormat.Rows[rf]["fixed_column_name"].ToString();
                                string hasGrade = dtResultFormat.Rows[rf]["has_grade"].ToString();
                                string isAllowedForGrade = dtResultFormat.Rows[rf]["is_allowed_for_grade"].ToString();
                                string isCalcForTotal = dtResultFormat.Rows[rf]["is_calc_for_total"].ToString();
                                resultCol.IsAllowedForGrade = false;
                                resultCol.IsUsedForTotal = false;
                                if (string.Equals(isAllowedForGrade, "Y", StringComparison.OrdinalIgnoreCase))
                                {
                                    resultCol.IsAllowedForGrade = true;
                                }
                                if (string.Equals(isCalcForTotal, "Y", StringComparison.OrdinalIgnoreCase))
                                {
                                    resultCol.IsUsedForTotal = true;
                                }
                                if (!resultCol.IsAllowedForGrade && string.Equals(drSub["SubjectExamType"].ToString(), "G", StringComparison.OrdinalIgnoreCase))
                                {
                                    continue;
                                }
                                string marks = string.Empty;
                                if (string.Equals(valueType, "FIXED", StringComparison.OrdinalIgnoreCase))
                                {
                                    marks = this.CreateValueWithExpression(expression, drSubjects, fixedColName, dtGrades, hasGrade);
                                }
                                else
                                {
                                    if (string.Equals(drSub["SubjectExamType"].ToString(), "G", StringComparison.OrdinalIgnoreCase))
                                    {
                                        marks = this.CreateGradeWithExpression(gradeExpression, drSubjects, dtGrades);
                                    }
                                    else
                                    {
                                        marks = this.CreateValueWithExpression(expression, drSubjects, "CalculatedMarks", dtGrades, hasGrade);
                                        if (resultCol.IsUsedForTotal)
                                        {
                                            rsCard.TotalMarks = rsCard.TotalMarks + double.Parse(Regex.Match(marks, @"\d+").Value);

                                            //======= FOR GRAPH ENTRY =========//
                                            //============HEIGHEST=============//
                                            double currentValue = double.Parse(Regex.Match(marks, @"\d+").Value);
                                            rsRows.GraphValue = Math.Ceiling(currentValue);
                                            if (dicSubjectHeighest.ContainsKey(subjectId))
                                            {
                                                double highestValue = dicSubjectHeighest[subjectId];
                                                if (Math.Ceiling(currentValue) > highestValue)
                                                {
                                                    dicSubjectHeighest[subjectId] = Math.Ceiling(currentValue);
                                                }
                                            }
                                            else
                                            {
                                                dicSubjectHeighest.Add(subjectId, Math.Ceiling(currentValue));
                                            }
                                            //============HEIGHEST============//
                                            //==============TOTAL=============//
                                            if (dicSubjectTotal.ContainsKey(subjectId))
                                            {
                                                dicSubjectTotal[subjectId] = dicSubjectTotal[subjectId] + Math.Ceiling(currentValue);
                                            }
                                            else
                                            {
                                                dicSubjectTotal.Add(subjectId, Math.Ceiling(currentValue));
                                            }
                                        }
                                    }
                                }
                                resultCol.ColumnValue = marks;
                                rsRows.ResultColumns.Add(resultCol);
                            }
                            if (string.Equals(drSub["SubjectExamType"].ToString(), "G", StringComparison.OrdinalIgnoreCase))
                            {
                                rsCard.GradeResultRows.Add(rsRows);
                            }
                            else
                            {
                                rsCard.ResultRows.Add(rsRows);
                            }
                            rsRows.SubjectId = subjectId;
                            lstDoneWithSubjects.Add(subjectId);
                        }
                    }

                    StudentRemarksDTO remark = remarks.FirstOrDefault(r => string.Equals(r.Student.StudentInfoId.ToString(), rsCard.StudentInfoId));
                    if (remark != null)
                    {
                        rsCard.CurrentRemarks = remark.Remarks;
                        if (!string.IsNullOrEmpty(remark.AttendancePercent))
                        {
                            rsCard.AttendancePercent = remark.AttendancePercent;
                        }
                    }

                    rsCard.ResultType = resultType;
                    lstResultCards.Add(rsCard);
                    lstDoneWithStudents.Add(rsCard.StudentInfoId);
                }
            }
            lstResultCards = this.CreateGraphRecords(lstResultCards, dicSubjectHeighest, dicSubjectTotal);
            return lstResultCards;
        }

        private List<ResultCardDTO> CreateGraphRecords(List<ResultCardDTO> currentCards, Dictionary<string, double> dicHighests, Dictionary<string, double> dicTotals)
        {
            for (int i = 0; i < currentCards.Count; i++)
            {
                currentCards[i].GraphRecords = new List<GraphRecords>();
                GraphRecords graphRecord = null;
                for (int j = 0; j < currentCards[i].ResultRows.Count; j++)
                {
                    graphRecord = new GraphRecords();
                    graphRecord.ClassHighest = dicHighests[currentCards[i].ResultRows[j].SubjectId];
                    graphRecord.SubjectName = currentCards[i].ResultRows[j].SubjectName;
                    graphRecord.ClassAverage = Math.Ceiling(dicTotals[currentCards[i].ResultRows[j].SubjectId] / currentCards.Count);
                    graphRecord.CurrentMarks = currentCards[i].ResultRows[j].GraphValue;
                    currentCards[i].GraphRecords.Add(graphRecord);
                }
            }
            return currentCards;
        }

        private string CreateGradeWithExpression(string expression, DataRow[] arrValues, DataTable dtGrade)
        {
            string tentativeFullMarks = dtGrade.Rows[0]["full_marks"].ToString();
            while (expression.Contains("{{") && expression.Contains("}}"))
            {
                int indexOfOpenBrace = expression.IndexOf("{{");
                int indexOfCloseBrace = expression.IndexOf("}}");
                string expr = expression.Substring(indexOfOpenBrace + 2, indexOfCloseBrace - indexOfOpenBrace - 2);
                string[] arrExpr = expr.Split(',');
                string etId = string.Empty;
                string estId = string.Empty;
                if (arrExpr[0].Contains("ET-"))
                {
                    etId = arrExpr[0].Split('-')[1];
                }
                else if (arrExpr[0].Contains("EST-"))
                {
                    estId = arrExpr[0].Split('-')[1];
                }
                if (arrExpr[1].Contains("ET-"))
                {
                    etId = arrExpr[1].Split('-')[1];
                }
                else if (arrExpr[1].Contains("EST-"))
                {
                    estId = arrExpr[1].Split('-')[1];
                }

                DataRow drValue = arrValues.FirstOrDefault(dr => string.Equals(dr["ExamTypeId"].ToString(), etId) && string.Equals(dr["ExamSubTypeId"].ToString(), estId));
                string gradeMarks = "0";

                if (drValue != null && !string.IsNullOrEmpty(drValue["DirectGrade"].ToString()))
                {
                    string grade = drValue["DirectGrade"].ToString();
                    DataRow[] grades = dtGrade.Select("grade_name='" + grade + "' AND full_marks=" + tentativeFullMarks);
                    if (grades != null && grades.Length > 0)
                    {
                        gradeMarks = grades[0]["to_marks"].ToString();
                    }
                }
                expression = expression.Replace("{{" + expr + "}}", gradeMarks);
            }

            DataTable dtEvaluator = new DataTable();
            var computedMarks = dtEvaluator.Compute(expression, "");

            string retGrade = string.Empty;

            DataRow[] arrGrade = dtGrade.Select("full_marks=" + tentativeFullMarks + " AND from_marks<=" + computedMarks + " AND to_marks>=" + computedMarks);
            if (arrGrade != null && arrGrade.Length > 0)
            {
                retGrade = arrGrade[0]["grade_name"].ToString();
            }

            return retGrade;
        }

        private string CreateValueWithExpression(string expression, DataRow[] arrValues, string columnName, DataTable dtGrade, string hasGrade)
        {
            string totMarksExpr = expression;
            while (expression.Contains("{{") && expression.Contains("}}") && totMarksExpr.Contains("{{") && totMarksExpr.Contains("}}"))
            {
                int indexOfOpenBrace = expression.IndexOf("{{");
                int indexOfCloseBrace = expression.IndexOf("}}");
                string expr = expression.Substring(indexOfOpenBrace + 2, indexOfCloseBrace - indexOfOpenBrace - 2);
                string[] arrExpr = expr.Split(',');
                string etId = string.Empty;
                string estId = string.Empty;
                if (arrExpr[0].Contains("ET-"))
                {
                    etId = arrExpr[0].Split('-')[1];
                }
                else if (arrExpr[0].Contains("EST-"))
                {
                    estId = arrExpr[0].Split('-')[1];
                }
                if (arrExpr[1].Contains("ET-"))
                {
                    etId = arrExpr[1].Split('-')[1];
                }
                else if (arrExpr[1].Contains("EST-"))
                {
                    estId = arrExpr[1].Split('-')[1];
                }

                DataRow drValue = arrValues.FirstOrDefault(dr => string.Equals(dr["ExamTypeId"].ToString(), etId) && string.Equals(dr["ExamSubTypeId"].ToString(), estId));
                string marks = "0";
                string fullMarks = "0";
                if (drValue != null && !string.IsNullOrEmpty(drValue[columnName].ToString()) && !string.IsNullOrEmpty(drValue["ActualFullMarks"].ToString()))
                {
                    marks = drValue[columnName].ToString();
                    fullMarks = drValue["ActualFullMarks"].ToString();
                }
                expression = expression.Replace("{{" + expr + "}}", marks);
                totMarksExpr = expression.Replace("{{" + totMarksExpr + "}}", fullMarks);
            }
            DataTable dtEvaluator = new DataTable();
            var computedMarks = dtEvaluator.Compute(expression, "");
            computedMarks = (int)Math.Ceiling(Convert.ToDouble(computedMarks));
            dtEvaluator = new DataTable();
            var totalMarks = dtEvaluator.Compute(totMarksExpr, "");
            totalMarks = (int)Math.Ceiling(Convert.ToDouble(totalMarks));
            totalMarks = totalMarks.ToString().Split('.')[0];
            string grade = string.Empty;
            if (string.Equals(hasGrade, "Y", StringComparison.OrdinalIgnoreCase))
            {
                DataRow[] arrGrade = dtGrade.Select("full_marks=" + totalMarks + " AND from_marks<=" + computedMarks + " AND to_marks>=" + computedMarks);
                if (arrGrade != null && arrGrade.Length > 0)
                {
                    grade = arrGrade[0]["grade_name"].ToString();
                }
            }
            if (!string.IsNullOrEmpty(grade))
            {
                computedMarks = computedMarks + " (" + grade + ")";
            }
            return computedMarks.ToString();
        }

        public DataTable GetStudentResults(int locationId, int standardSectionId, DateTime academicStartDate, DateTime academicEndDate)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                dbSvc.OpenConnection();
                MySqlCommand command = new MySqlCommand();
                command.Connection = dbSvc.GetConnection() as MySqlConnection;
                string examTypeWhereClause = string.Empty;

                command.CommandText = "SELECT u.FName, u.MName, u.LName, st.RegistrationNumber, st.StudentInfoId, st.RollNumber, std.StandardName, sc.SectionName, sub.SubjectName, sub.SubjectId, sub.SubjectExamType, " +
                                      "et.ExamTypeDescription, et.ExamTypeId, est.ExamSubTypeDescription, est.ExamSubTypeId, exm.CalculatedMarks, exm.DirectGrade, er.PassMarks, er.ActualFullMarks, rsm.subject_order " +
                                      "FROM exammarks exm LEFT JOIN StudentInfo st ON exm.StudentInfoId=st.StudentInfoId LEFT JOIN usermaster u ON st.UserMasterId=u.UserMasterId " +
                                      "LEFT JOIN StandardSectionMap scm ON exm.StandardSectionId=scm.StandardSectionId LEFT JOIN standard std ON scm.StandardId=std.StandardId " +
                                      "LEFT JOIN Section sc ON scm.SectionId=sc.SectionId LEFT JOIN subject sub ON exm.SubjectId=sub.SubjectId " +
                                      "LEFT JOIN ExamRule er ON exm.ExamRuleId=er.ExamRuleId " +
                                      "LEFT JOIN courseexam cexm ON exm.CourseExamId=cexm.CourseExamId LEFT JOIN coursemapping cmpng ON cexm.CourseMappingId=cmpng.CourseMappingId " +
                                      "LEFT JOIN ExamTypes et ON cexm.ExamTypeId=et.ExamTypeId LEFT JOIN ExamSubTypes est ON cexm.ExamSubTypeId=est.ExamSubTypeId " +
                                      "LEFT JOIN result_subject_map rsm ON exm.SubjectId=rsm.subject_id " +
                                      "WHERE rsm.standard_id = scm.StandardSectionId AND cmpng.LocationId=@locId AND exm.StandardSectionId=@stdSecId AND exm.CourseFrom=@courseFrom AND exm.CourseTo=@courseTo ORDER BY st.StudentInfoId,rsm.subject_order asc";
                command.Parameters.Add("@locId", MySqlDbType.Int32).Value = locationId;
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
