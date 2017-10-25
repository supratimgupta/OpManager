using OpMgr.Common.Contracts.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpMgr.Common.DTOs;
using OpMgr.Common.Contracts;
using System.Data;
using MySql.Data.MySqlClient;

namespace OpMgr.DataAccess.Implementations
{
    public class PMSSvc : IPMSSvc
    {
        private IConfigSvc _configSvc;
        private DataTable _dtData;
        private DataSet _dsData;
        private ILogSvc _logger;

        public PMSSvc(IConfigSvc configSvc, ILogSvc logger)
        {
            _configSvc = configSvc;
            _logger = logger;
        }


        public StatusDTO<EmployeeGoalLogDTO> Delete(EmployeeGoalLogDTO data)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<EmployeeGoalLogDTO> Insert(EmployeeGoalLogDTO data)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<EmployeeGoalLogDTO> Select(int rowId)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<List<EmployeeGoalLogDTO>> Select(EmployeeGoalLogDTO data)
        {
            StatusDTO<List<EmployeeGoalLogDTO>> goalList = new StatusDTO<List<EmployeeGoalLogDTO>>();
            DataSet dsGoalLst = null;
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "get_AppraisalData";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    
                    command.Parameters.Add("@DesignationId1", MySqlDbType.Int32).Value = data.EmployeeAppraisalMaster.Employee.Designation.DesignationId;
                    command.Parameters.Add("@EmployeeId1", MySqlDbType.Int32).Value = data.EmployeeAppraisalMaster.Employee.EmployeeId;


                    MySqlDataAdapter da = new MySqlDataAdapter(command);
                    dsGoalLst = new DataSet();
                    da.Fill(dsGoalLst);

                    StatusDTO<EmployeeGoalLogDTO> status = new StatusDTO<EmployeeGoalLogDTO>();
                    EmployeeGoalLogDTO empGoalLogDTO = new EmployeeGoalLogDTO();
                    if (dsGoalLst != null && dsGoalLst.Tables.Count > 0)
                    {
                        goalList.ReturnObj = new List<EmployeeGoalLogDTO>();
                        if (dsGoalLst.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsGoalLst.Tables[0].Rows.Count; i++)
                            {
                                EmployeeGoalLogDTO empgoallog = new EmployeeGoalLogDTO();
                                empgoallog.GoalAttribute = new GoalAttributeDTO();
                                empgoallog.GoalAttribute.Goal = new GoalDTO();
                                empgoallog.GoalAttribute.GoalAttributeId = Convert.ToInt32(dsGoalLst.Tables[0].Rows[i]["goalattributeid"]);
                                empgoallog.GoalAttribute.Goal.GoalId = Convert.ToInt32(dsGoalLst.Tables[0].Rows[i]["GoalId"]);
                                empgoallog.GoalAttribute.Goal.GoalDescription = dsGoalLst.Tables[0].Rows[i]["GoalDescription"].ToString();
                                empgoallog.GoalAttribute.Target = dsGoalLst.Tables[0].Rows[i]["Target"].ToString();
                                empgoallog.GoalAttribute.KRA = dsGoalLst.Tables[0].Rows[i]["KRA"].ToString();
                                empgoallog.GoalAttribute.KPI = dsGoalLst.Tables[0].Rows[i]["KPI"].ToString();
                                empgoallog.GoalAttribute.WeightAge = Convert.ToInt32(dsGoalLst.Tables[0].Rows[i]["WeightAge"]);

                                goalList.ReturnObj.Add(empgoallog);
                            }
                        }

                        if (dsGoalLst.Tables[1].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsGoalLst.Tables[1].Rows.Count; i++)
                            {
                                EmployeeGoalLogDTO empgoallog = new EmployeeGoalLogDTO();
                                empgoallog.GoalAttribute = new GoalAttributeDTO();
                                empgoallog.GoalAttribute.Goal = new GoalDTO();
                                empgoallog.GoalAttribute.GoalAttributeId = Convert.ToInt32(dsGoalLst.Tables[1].Rows[i]["goalattributeid"]);
                                empgoallog.GoalAttribute.Goal.GoalId = Convert.ToInt32(dsGoalLst.Tables[1].Rows[i]["GoalId"]);
                                empgoallog.GoalAttribute.Goal.GoalDescription = dsGoalLst.Tables[1].Rows[i]["GoalDescription"].ToString();
                                empgoallog.GoalAttribute.Target = dsGoalLst.Tables[1].Rows[i]["Target"].ToString();
                                empgoallog.GoalAttribute.KRA = dsGoalLst.Tables[1].Rows[i]["KRA"].ToString();
                                empgoallog.GoalAttribute.KPI = dsGoalLst.Tables[1].Rows[i]["KPI"].ToString();
                                empgoallog.GoalAttribute.WeightAge = Convert.ToInt32(dsGoalLst.Tables[1].Rows[i]["WeightAge"]);

                                goalList.ReturnObj.Add(empgoallog);
                            }
                        }

                        if (dsGoalLst.Tables[2].Rows.Count > 0)
                        {
                            goalList.ReturnObj[0].EmployeeAppraisalMaster = new EmployeeAppraisalMasterDTO();
                            
                                EmployeeDetailsDTO empdetails = new EmployeeDetailsDTO();
                                empdetails.UserDetails = new UserMasterDTO();
                                empdetails.UserDetails.Location = new LocationDTO();
                                empdetails.Designation = new DesignationDTO();
                                empdetails.UserDetails.FName = dsGoalLst.Tables[2].Rows[0]["FName"].ToString();
                                empdetails.UserDetails.MName = dsGoalLst.Tables[2].Rows[0]["MName"].ToString();
                                empdetails.UserDetails.LName = dsGoalLst.Tables[2].Rows[0]["LName"].ToString();
                                empdetails.Designation.DesignationDescription = dsGoalLst.Tables[2].Rows[0]["DesignationDescription"].ToString();
                                empdetails.UserDetails.Location.LocationDescription = dsGoalLst.Tables[2].Rows[0]["LocationDescription"].ToString();
                                empdetails.EducationalQualification = dsGoalLst.Tables[2].Rows[0]["EducationQualification"].ToString();

                            goalList.ReturnObj[0].EmployeeAppraisalMaster.Employee = empdetails;
                            
                        }

                    }

                    return goalList;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public StatusDTO<EmployeeAppraisalMasterDTO> Update(EmployeeAppraisalMasterDTO data)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<EmployeeGoalLogDTO> Update(EmployeeGoalLogDTO data)
        {
            throw new NotImplementedException();
        }
    }
}
