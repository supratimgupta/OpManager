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
    public class ExamRuleSvc : IExamRuleSvc
    {

        /// <summary>
        /// Required for getting connection related configs
        /// </summary>
        private IConfigSvc _configSvc;
        private DataTable _dtData;
        private DataSet _dsData;

        /// <summary>
        /// Needed for exception handling
        /// </summary>
        private ILogSvc _logger;

        public ExamRuleSvc(IConfigSvc configSvc, ILogSvc logger)
        {
            _configSvc = configSvc;
            _logger = logger;
        }

        public Common.DTOs.StatusDTO<Common.DTOs.ExamRuleDTO> Insert(ExamRuleDTO data)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "INSERT INTO ExamRule(AssesmentMarks, ActualFullMarks, PassMarks, DateTimeLog, CourseExamId, Active, CreatedBy, CreatedDate)" +
                                          " VALUES (@assesmentMarks, @actualFullMarks, @passMarks, @dateTimeLog, @courseExamId, 1, @createdBy, @createdDate)";
                    command.CommandType = CommandType.Text;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    //data.UserDetails = new UserMasterDTO();
                    command.Parameters.Add("@assesmentMarks", MySqlDbType.Double).Value = data.AssesmentMarks.Value;
                    command.Parameters.Add("@actualFullMarks", MySqlDbType.Double).Value = data.ActualFullMarks.Value;
                    command.Parameters.Add("@passMarks", MySqlDbType.Double).Value = data.PassMarks.Value;
                    command.Parameters.Add("@dateTimeLog", MySqlDbType.DateTime).Value = DateTime.Now;
                    command.Parameters.Add("@courseExamId", MySqlDbType.Int32).Value = data.CourseExam.CourseExamId;
                    command.Parameters.Add("@createdBy", MySqlDbType.Int32).Value = data.CreatedBy.UserMasterId;
                    command.Parameters.Add("@createdDate", MySqlDbType.DateTime).Value = DateTime.Today.Date;

                    StatusDTO<ExamRuleDTO> status = new StatusDTO<ExamRuleDTO>();

                    if (command.ExecuteNonQuery() > 0)
                    {
                        status.IsSuccess = true;
                        status.ReturnObj = data;
                    }
                    return status;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public Common.DTOs.StatusDTO<Common.DTOs.ExamRuleDTO> Update(ExamRuleDTO data)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "UPDATE ExamRule SET AssesmentMarks = @assesmentMarks, ActualFullMarks = @actualFullMarks, PassMarks = @passMarks, " +
                                          "DateTimeLog = @dateTimeLog, UpdatedBy = @updatedBy, UpdatedDate = @updatedDate WHERE ExamRuleId=@examRuleId";
                    command.CommandType = CommandType.Text;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    //data.UserDetails = new UserMasterDTO();
                    command.Parameters.Add("@assesmentMarks", MySqlDbType.Double).Value = data.AssesmentMarks.Value;
                    command.Parameters.Add("@actualFullMarks", MySqlDbType.Double).Value = data.ActualFullMarks.Value;
                    command.Parameters.Add("@passMarks", MySqlDbType.Double).Value = data.PassMarks.Value;
                    command.Parameters.Add("@dateTimeLog", MySqlDbType.DateTime).Value = DateTime.Now;
                    command.Parameters.Add("@updatedBy", MySqlDbType.Int32).Value = data.UpdatedBy.UserMasterId;
                    command.Parameters.Add("@updatedDate", MySqlDbType.DateTime).Value = DateTime.Today.Date;
                    command.Parameters.Add("@examRuleId", MySqlDbType.Int32).Value = data.ExamRuleId;
                    StatusDTO<ExamRuleDTO> status = new StatusDTO<ExamRuleDTO>();

                    if (command.ExecuteNonQuery() > 0)
                    {
                        status.IsSuccess = true;
                        status.ReturnObj = data;
                    }
                    return status;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public Common.DTOs.StatusDTO<Common.DTOs.ExamRuleDTO> Delete(Common.DTOs.ExamRuleDTO data)
        {
            throw new NotImplementedException();
        }

        public Common.DTOs.StatusDTO<List<Common.DTOs.ExamRuleDTO>> Select(Common.DTOs.ExamRuleDTO data)
        {
            StatusDTO<List<ExamRuleDTO>> status = new StatusDTO<List<ExamRuleDTO>>();
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "SELECT ExamRuleId, AssesmentMarks, ActualFullMarks, PassMarks, DateTimeLog FROM ExamRule WHERE CourseExamId=@courseExamId AND Active=1";
                    command.Parameters.Add("@courseExamId", MySqlDbType.Int32).Value = data.CourseExam.CourseExamId;

                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    MySqlDataAdapter mDA = new MySqlDataAdapter(command);
                    DataTable dtData = new DataTable("TR_DATA");
                    mDA.Fill(dtData);

                    status.IsSuccess = true;
                    status.ReturnObj = null;

                    if (dtData != null && dtData.Rows.Count > 0)
                    {
                        status.ReturnObj = new List<ExamRuleDTO>();
                        ExamRuleDTO examRuleDTO = null;
                        foreach (DataRow dr in dtData.Rows)
                        {
                            examRuleDTO = new ExamRuleDTO();
                            examRuleDTO.ExamRuleId = int.Parse(dr["ExamRuleId"].ToString());
                            examRuleDTO.AssesmentMarks = double.Parse(dr["AssesmentMarks"].ToString());
                            examRuleDTO.ActualFullMarks = double.Parse(dr["ActualFullMarks"].ToString());
                            if (!string.IsNullOrEmpty(dr["PassMarks"].ToString()))
                            {
                                examRuleDTO.PassMarks = double.Parse(dr["PassMarks"].ToString());
                            }
                            else
                            {
                                examRuleDTO.PassMarks = 0;
                            }
                            if (!string.IsNullOrEmpty(dr["DateTimeLog"].ToString()))
                            {
                                examRuleDTO.DateTimeLog = DateTime.Parse(dr["DateTimeLog"].ToString());
                            }
                            else
                            {
                                examRuleDTO.DateTimeLog = DateTime.Now;
                            }
                            status.ReturnObj.Add(examRuleDTO);
                        }
                    }

                    return status;
                }
                catch (Exception exp)
                {
                    _logger.Log(exp);
                    throw exp;
                }

            }
        }

        public Common.DTOs.StatusDTO<Common.DTOs.ExamRuleDTO> Select(int rowId)
        {
            StatusDTO<ExamRuleDTO> status = new StatusDTO<ExamRuleDTO>();
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "SELECT AssesmentMarks, ActualFullMarks, PassMarks, DateTimeLog, Active FROM ExamRule WHERE ExamRuleId=@examRuleId";
                    command.Parameters.Add("@examRuleId", MySqlDbType.Int32).Value = rowId;

                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    MySqlDataAdapter mDA = new MySqlDataAdapter(command);
                    DataTable dtData = new DataTable("TR_DATA");
                    mDA.Fill(dtData);

                    status.IsSuccess = true;
                    status.ReturnObj = null;

                    if (dtData != null && dtData.Rows.Count > 0)
                    {
                        status.ReturnObj = new ExamRuleDTO();
                        DataRow dr = dtData.Rows[0];
                        status.ReturnObj.AssesmentMarks = double.Parse(string.IsNullOrEmpty(dr["AssesmentMarks"].ToString()) ? "0" : dr["AssesmentMarks"].ToString());
                        status.ReturnObj.ActualFullMarks = double.Parse(string.IsNullOrEmpty(dr["ActualFullMarks"].ToString()) ? "0" : dr["ActualFullMarks"].ToString());
                        status.ReturnObj.PassMarks = double.Parse(string.IsNullOrEmpty(dr["PassMarks"].ToString()) ? "0" : dr["PassMarks"].ToString());
                        if (!string.IsNullOrEmpty(dr["DateTimeLog"].ToString()))
                        {
                            status.ReturnObj.DateTimeLog = DateTime.Parse(dr["DateTimeLog"].ToString());
                        }
                        if (string.Equals(dr["Active"].ToString(), "1"))
                        {
                            status.ReturnObj.Active = true;
                        }
                        else
                        {
                            status.ReturnObj.Active = false;
                        }
                        status.ReturnObj.ExamRuleId = rowId;
                    }

                    return status;
                }
                catch (Exception exp)
                {
                    _logger.Log(exp);
                    throw exp;
                }

            }
        }
    }
}
