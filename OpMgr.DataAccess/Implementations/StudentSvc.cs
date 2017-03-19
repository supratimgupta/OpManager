using MySql.Data.MySqlClient;
using OpMgr.Common.Contracts;
using OpMgr.Common.Contracts.Modules;
using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.DataAccess.Implementations
{
    public class StudentSvc : IStudentSvc
    {
        /// <summary>
        /// Required for getting connection related configs
        /// </summary>
        private IConfigSvc _configSvc;

        /// <summary>
        /// Needed for exception handling
        /// </summary>
        private ILogSvc _logger;

        public StudentSvc(IConfigSvc configSvc, ILogSvc logger)
        {
            _configSvc = configSvc;
            _logger = logger;
        }

        /// <summary>
        /// Delete a student record
        /// </summary>
        /// <param name="data">Data of student DTO</param>
        /// <returns>Status with deleted record</returns>
        public StatusDTO<StudentDTO> Delete(StudentDTO data)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<StudentDTO> Insert(StudentDTO data)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<StudentDTO> Select(int rowId)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<List<StudentDTO>> Select(StudentDTO data)
        {
            StatusDTO<List<StudentDTO>> studLst = new StatusDTO<List<StudentDTO>>();
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();//openning the connection

                    MySqlCommand command = new MySqlCommand();// creating my sql command for queries

                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    command.CommandText = "select * from studentinfo";

                    StudentDTO student = new StudentDTO();
                    student.Active = true;
                    student.GuardianContact = "9830641878";

                    student.StandardSectionMap = new StandardSectionMapDTO();

                    student.StandardSectionMap.Standard = new StandardDTO();

                    student.StandardSectionMap.Section = new SectionDTO();

                    student.StandardSectionMap.Section.SectionName = "A";
                    student.StandardSectionMap.Standard.StandardName = "Class 1";
                    student.RegistrationNumber = "163/97";

                    student.RollNumber = "15";

                    student.UserDetails = new UserMasterDTO();
                    student.UserDetails.FName = "Navajit";
                    student.UserDetails.LName = "Maitra";

                    studLst.ReturnObj = new List<StudentDTO>();
                    studLst.ReturnObj.Add(student);

                    studLst.IsSuccess = true;
                    return studLst;


                    



                }
                catch(Exception exp)
                {
                    _logger.Log(exp);
                    studLst.IsSuccess = false;
                    studLst.IsException = true;
                    studLst.ReturnObj = null;
                    studLst.ExceptionMessage = exp.Message;
                    studLst.StackTrace = exp.StackTrace;
                    return studLst;
                }
            }
            
        }

        public StatusDTO<StudentDTO> Update(StudentDTO data)
        {
            throw new NotImplementedException();
        }
    }
}
