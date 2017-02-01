using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpMgr.Common.Contracts.Modules;
using OpMgr.Common.Contracts;
using OpMgr.Common.DTOs;
using OpMgr.Common.DTOs.Users;
using MySql.Data.MySqlClient;
using System.Data;

namespace OpMgr.DataAccess.Implementations
{
    public class DepartmentSvc : IDepartmentSvc, IDisposable
    {
        IConfigSvc _configSvc;
        ILogSvc _logger;
        DataTable _dbData;
        MySqlDataAdapter _dbAdapter;

        public void Dispose()
        {
            if(_configSvc!=null)
            {
                _configSvc = null;
            }
            if(_logger!=null)
            {
                _logger = null;
            }
            if(_dbData!=null)
            {
                _dbData.Dispose();
                _dbData = null;
            }
            if(_dbAdapter!=null)
            {
                _dbAdapter.Dispose();
                _dbAdapter = null;
            }
        }

        public DepartmentSvc(IConfigSvc configSvc, ILogSvc logger)
        {
            _configSvc = configSvc;
            _logger = logger;
        }

        public StatusDTO<DepartmentDTO> Insert(DepartmentDTO data)
        {
            StatusDTO<DepartmentDTO> status = new StatusDTO<DepartmentDTO>();
            try
            {
                using (IDbSvc dbSvc = new DbSvc(_configSvc))
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "INSERT INTO dbo.DIC_Department(Name,IsActive,Supervisor) VALUES (@name,@is_active,@supervisor)";
                    command.CommandType = CommandType.Text;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    command.Parameters.Add("@name", MySqlDbType.String).Value = data.Name;
                    command.Parameters.Add("@is_active", MySqlDbType.Bit).Value = data.IsActive;
                    command.Parameters.Add("@supervisor", MySqlDbType.Int32).Value = data.SupervisorDetails.RowId;
                    if(command.ExecuteNonQuery()>0)
                    {
                        status.IsSuccess = true;
                        status.ReturnObj = data;
                        status.IsException = false;
                    }
                    else
                    {
                        status.IsSuccess = false;
                        status.IsException = false;
                        status.FailureReason = "Data has not been updated in DB";
                    }
                }
            }
            catch(Exception exp)
            {
                _logger.Log(exp);
                status.ExceptionMessage = exp.Message;
                status.HResult = exp.HResult.ToString();
                status.IsException = true;
                status.IsSuccess = false;
                status.ReturnObj = null;
                status.StackTrace = exp.StackTrace;
            }
            return status;
        }

        public StatusDTO<DepartmentDTO> Update(DepartmentDTO data)
        {
            StatusDTO<DepartmentDTO> status = new StatusDTO<DepartmentDTO>();
            try
            {
                using (IDbSvc dbSvc = new DbSvc(_configSvc))
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "UPDATE dbo.DIC_Department SET Name=@name,IsActive=@is_active,Supervisor=@supervisor WHERE RowId=@row_id";
                    command.CommandType = CommandType.Text;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    command.Parameters.Add("@name", MySqlDbType.String).Value = data.Name;
                    command.Parameters.Add("@is_active", MySqlDbType.Bit).Value = data.IsActive;
                    command.Parameters.Add("@supervisor", MySqlDbType.Int32).Value = data.SupervisorDetails.RowId;
                    command.Parameters.Add("@row_id", MySqlDbType.Int32).Value = data.RowId;
                    if (command.ExecuteNonQuery() > 0)
                    {
                        status.IsSuccess = true;
                        status.ReturnObj = data;
                        status.IsException = false;
                    }
                    else
                    {
                        status.IsSuccess = false;
                        status.IsException = false;
                        status.FailureReason = "Data has not been updated in DB";
                    }
                }
            }
            catch (Exception exp)
            {
                _logger.Log(exp);
                status.ExceptionMessage = exp.Message;
                status.HResult = exp.HResult.ToString();
                status.IsException = true;
                status.IsSuccess = false;
                status.ReturnObj = null;
                status.StackTrace = exp.StackTrace;
            }
            return status;
        }


        public StatusDTO<DepartmentDTO> Delete(DepartmentDTO data)
        {
            StatusDTO<DepartmentDTO> status = new StatusDTO<DepartmentDTO>();
            try
            {
                using (IDbSvc dbSvc = new DbSvc(_configSvc))
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "UPDATE dbo.DIC_Department SET IsActive=0 WHERE RowId=@row_id";
                    command.CommandType = CommandType.Text;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    command.Parameters.Add("@row_id", MySqlDbType.Int32).Value = data.RowId;
                    if (command.ExecuteNonQuery() > 0)
                    {
                        status.IsSuccess = true;
                        status.ReturnObj = data;
                        status.IsException = false;
                    }
                    else
                    {
                        status.IsSuccess = false;
                        status.IsException = false;
                        status.FailureReason = "Data has not been updated in DB";
                    }
                }
            }
            catch (Exception exp)
            {
                _logger.Log(exp);
                status.ExceptionMessage = exp.Message;
                status.HResult = exp.HResult.ToString();
                status.IsException = true;
                status.IsSuccess = false;
                status.ReturnObj = null;
                status.StackTrace = exp.StackTrace;
            }
            return status;
        }

        public StatusDTO<DepartmentDTO> Select(int rowId)
        {
            StatusDTO<DepartmentDTO> status = new StatusDTO<DepartmentDTO>();
            try
            {
                using (IDbSvc dbSvc = new DbSvc(_configSvc))
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "SELECT D.RowId, D.Name, D.IsActive, D.Supervisor, E.FirstName, E.MiddleName, E.LastName FROM dbo.DIC_Department D, dbo.Employee E WHERE D.Supervisor=E.RowId AND D.RowId=@row_id";
                    command.CommandType = CommandType.Text;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    command.Parameters.Add("@row_id", MySqlDbType.Int32).Value = rowId;
                    _dbData = new DataTable();
                    _dbAdapter = new MySqlDataAdapter(command);
                    _dbAdapter.Fill(_dbData);
                    if(_dbData!=null && _dbData.Rows.Count>0)
                    {
                        status.IsSuccess = true;
                        status.IsException = false;
                        status.ReturnObj = new DepartmentDTO();
                        status.ReturnObj.RowId = (int)_dbData.Rows[0]["RowId"];
                        status.ReturnObj.Name = _dbData.Rows[0]["Name"].ToString();
                        status.ReturnObj.IsActive = (bool)_dbData.Rows[0]["IsActive"];
                        status.ReturnObj.SupervisorDetails = new EmployeeDTO();
                        status.ReturnObj.SupervisorDetails.FirstName = _dbData.Rows[0]["FirstName"].ToString();
                        status.ReturnObj.SupervisorDetails.MiddleName = _dbData.Rows[0]["MiddleName"].ToString();
                        status.ReturnObj.SupervisorDetails.LastName = _dbData.Rows[0]["LastName"].ToString();
                    }
                    else
                    {
                        status.IsSuccess = false;
                        status.IsException = false;
                        status.FailureReason = "No data found";
                        status.ReturnObj = null;
                    }
                }
            }
            catch (Exception exp)
            {
                _logger.Log(exp);
                status.ExceptionMessage = exp.Message;
                status.HResult = exp.HResult.ToString();
                status.IsException = true;
                status.IsSuccess = false;
                status.ReturnObj = null;
                status.StackTrace = exp.StackTrace;
            }
            return status;
        }

        public StatusDTO<DepartmentDTO> Select(int rowId)
        {
            StatusDTO<DepartmentDTO> status = new StatusDTO<DepartmentDTO>();
            try
            {
                using (IDbSvc dbSvc = new DbSvc(_configSvc))
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "SELECT D.RowId, D.Name, D.IsActive, D.Supervisor, E.FirstName, E.MiddleName, E.LastName FROM dbo.DIC_Department D, dbo.Employee E WHERE D.Supervisor=E.RowId AND D.RowId=@row_id";
                    command.CommandType = CommandType.Text;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    command.Parameters.Add("@row_id", MySqlDbType.Int32).Value = rowId;
                    _dbData = new DataTable();
                    _dbAdapter = new MySqlDataAdapter(command);
                    _dbAdapter.Fill(_dbData);
                    if (_dbData != null && _dbData.Rows.Count > 0)
                    {
                        status.IsSuccess = true;
                        status.IsException = false;
                        status.ReturnObj = new DepartmentDTO();
                        status.ReturnObj.RowId = (int)_dbData.Rows[0]["RowId"];
                        status.ReturnObj.Name = _dbData.Rows[0]["Name"].ToString();
                        status.ReturnObj.IsActive = (bool)_dbData.Rows[0]["IsActive"];
                        status.ReturnObj.SupervisorDetails = new EmployeeDTO();
                        status.ReturnObj.SupervisorDetails.FirstName = _dbData.Rows[0]["FirstName"].ToString();
                        status.ReturnObj.SupervisorDetails.MiddleName = _dbData.Rows[0]["MiddleName"].ToString();
                        status.ReturnObj.SupervisorDetails.LastName = _dbData.Rows[0]["LastName"].ToString();
                    }
                    else
                    {
                        status.IsSuccess = false;
                        status.IsException = false;
                        status.FailureReason = "No data found";
                        status.ReturnObj = null;
                    }
                }
            }
            catch (Exception exp)
            {
                _logger.Log(exp);
                status.ExceptionMessage = exp.Message;
                status.HResult = exp.HResult.ToString();
                status.IsException = true;
                status.IsSuccess = false;
                status.ReturnObj = null;
                status.StackTrace = exp.StackTrace;
            }
            return status;
        }
    }
}
