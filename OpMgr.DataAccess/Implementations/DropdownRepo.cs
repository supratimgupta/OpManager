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
    public class DropdownRepo : IDropdownRepo
    {
        private IConfigSvc _configSvc;
        private DataTable _dtData;
                       
        public DropdownRepo(IConfigSvc configSvc)
        {
            _configSvc = configSvc;
        }

        
        public List<LocationDTO> Location()
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "select LocationId,LocationDescription from location where Active=1";
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    _dtData = new DataTable();
                    MySqlDataAdapter msDa = new MySqlDataAdapter(command);
                    msDa.Fill(_dtData);
                    List<LocationDTO> lstLocation = new List<LocationDTO>();
                    if(_dtData!=null && _dtData.Rows.Count>0)
                    {
                        LocationDTO locationDTO = null;
                        foreach (DataRow dr in _dtData.Rows)
                        {
                            locationDTO = new LocationDTO();
                            locationDTO.LocationId = (int)dr["LocationId"];
                            locationDTO.LocationDescription = dr["LocationDescription"].ToString();
                            lstLocation.Add(locationDTO);
                        }
                    }
                    return lstLocation;                    
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public List<RoleDTO> Roles()
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "select RoleId,RoleDescription from roles where Active=1";
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    _dtData = new DataTable();
                    MySqlDataAdapter msDa = new MySqlDataAdapter(command);
                    msDa.Fill(_dtData);
                    List<RoleDTO> lstRole = new List<RoleDTO>();
                    if (_dtData != null && _dtData.Rows.Count > 0)
                    {
                        RoleDTO roleDTO = null;
                        foreach (DataRow dr in _dtData.Rows)
                        {
                            roleDTO = new RoleDTO();
                            roleDTO.RoleId = (int)dr["RoleId"];
                            roleDTO.RoleDescription = dr["RoleDescription"].ToString();
                            lstRole.Add(roleDTO);
                        }
                    }
                    return lstRole;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public List<HouseTypeDTO> House()
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "select HouseTypeId,HouseTypeDescription from housetype where Active=1";
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    _dtData = new DataTable();
                    MySqlDataAdapter msDa = new MySqlDataAdapter(command);
                    msDa.Fill(_dtData);
                    List<HouseTypeDTO> lstHouse = new List<HouseTypeDTO>();
                    if (_dtData != null && _dtData.Rows.Count > 0)
                    {
                        HouseTypeDTO houseDTO = null;
                        foreach (DataRow dr in _dtData.Rows)
                        {
                            houseDTO = new HouseTypeDTO();
                            houseDTO.HouseTypeId = (int)dr["HouseTypeId"];
                            houseDTO.HouseTypeDescription = dr["HouseTypeDescription"].ToString();
                            lstHouse.Add(houseDTO);
                        }
                    }
                    return lstHouse;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public List<ClassTypeDTO> ClassType()
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "select ClassTypeId,ClassTypeName from classtype where Active=1";
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    _dtData = new DataTable();
                    MySqlDataAdapter msDa = new MySqlDataAdapter(command);
                    msDa.Fill(_dtData);
                    List<ClassTypeDTO> lstClassType = new List<ClassTypeDTO>();
                    if (_dtData != null && _dtData.Rows.Count > 0)
                    {
                        ClassTypeDTO classDTO = null;
                        foreach (DataRow dr in _dtData.Rows)
                        {
                            classDTO = new ClassTypeDTO();
                            classDTO.ClassTypeId = (int)dr["ClassTypeId"];
                            classDTO.ClassTypeName = dr["ClassTypeName"].ToString();
                            lstClassType.Add(classDTO);
                        }
                    }
                    return lstClassType;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public List<SubjectDTO> Subject()
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "select SubjectId,SubjectName from Subject where Active=1";
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    _dtData = new DataTable();
                    MySqlDataAdapter msDa = new MySqlDataAdapter(command);
                    msDa.Fill(_dtData);
                    List<SubjectDTO> lstClassType = new List<SubjectDTO>();
                    if (_dtData != null && _dtData.Rows.Count > 0)
                    {
                        SubjectDTO subjectDTO = null;
                        foreach (DataRow dr in _dtData.Rows)
                        {
                            subjectDTO = new SubjectDTO();
                            subjectDTO.SubjectId = (int)dr["SubjectId"];
                            subjectDTO.SubjectName = dr["SubjectName"].ToString();
                            lstClassType.Add(subjectDTO);
                        }
                    }
                    return lstClassType;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public List<SectionDTO> Section()
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "select SectionId,SectionName from section where Active=1";
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    _dtData = new DataTable();
                    MySqlDataAdapter msDa = new MySqlDataAdapter(command);
                    msDa.Fill(_dtData);
                    List<SectionDTO> lstSection = new List<SectionDTO>();
                    if (_dtData != null && _dtData.Rows.Count > 0)
                    {
                        SectionDTO sectionDTO = null;
                        foreach (DataRow dr in _dtData.Rows)
                        {
                            sectionDTO = new SectionDTO();
                            sectionDTO.SectionId = (int)dr["SectionId"];
                            sectionDTO.SectionName = dr["SectionName"].ToString();
                            lstSection.Add(sectionDTO);
                        }
                    }
                    return lstSection;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public List<BookCategoryDTO> BookCategry()
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "select BookCategoryId,BookCategory from bookcategory where Active=1";
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    _dtData = new DataTable();
                    MySqlDataAdapter msDa = new MySqlDataAdapter(command);
                    msDa.Fill(_dtData);
                    List<BookCategoryDTO> lstBookCategory = new List<BookCategoryDTO>();
                    if (_dtData != null && _dtData.Rows.Count > 0)
                    {
                        BookCategoryDTO bookcategoryDTO = null;
                        foreach (DataRow dr in _dtData.Rows)
                        {
                            bookcategoryDTO = new BookCategoryDTO();
                            bookcategoryDTO.BookCategoryId = (int)dr["BookCategoryId"];
                            bookcategoryDTO.BookCategory = dr["BookCategory"].ToString();
                            lstBookCategory.Add(bookcategoryDTO);
                        }
                    }
                    return lstBookCategory;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public List<DepartmentDTO> Department()
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "select DepartmentId,DepartmentName from department where Active=1";
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    _dtData = new DataTable();
                    MySqlDataAdapter msDa = new MySqlDataAdapter(command);
                    msDa.Fill(_dtData);
                    List<DepartmentDTO> lstDepartment = new List<DepartmentDTO>();
                    if (_dtData != null && _dtData.Rows.Count > 0)
                    {
                        DepartmentDTO departmentDTO = null;
                        foreach (DataRow dr in _dtData.Rows)
                        {
                            departmentDTO = new DepartmentDTO();
                            departmentDTO.DepartmentId = (int)dr["DepartmentId"];
                            departmentDTO.DepartmentName = dr["DepartmentName"].ToString();
                            lstDepartment.Add(departmentDTO);
                        }
                    }
                    return lstDepartment;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public List<DesignationDTO> Designation()
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "select DesignationId,DesignationDescription from designation where Active=1";
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    _dtData = new DataTable();
                    MySqlDataAdapter msDa = new MySqlDataAdapter(command);
                    msDa.Fill(_dtData);
                    List<DesignationDTO> lstDesignation = new List<DesignationDTO>();
                    if (_dtData != null && _dtData.Rows.Count > 0)
                    {
                        DesignationDTO designationDTO = null;
                        foreach (DataRow dr in _dtData.Rows)
                        {
                            designationDTO = new DesignationDTO();
                            designationDTO.DesignationId = (int)dr["DesignationId"];
                            designationDTO.DesignationDescription = dr["DesignationDescription"].ToString();
                            lstDesignation.Add(designationDTO);
                        }
                    }
                    return lstDesignation;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public List<StandardDTO> Standard(ClassTypeDTO classTypeDTO)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "select StandardId,StandardName from standard where Active=1 and ClassTypeId = '" + classTypeDTO.ClassTypeId + "'";
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    _dtData = new DataTable();
                    MySqlDataAdapter msDa = new MySqlDataAdapter(command);
                    msDa.Fill(_dtData);
                    List<StandardDTO> lstStandard = new List<StandardDTO>();
                    if (_dtData != null && _dtData.Rows.Count > 0)
                    {
                        StandardDTO standardDTO = null;
                        foreach (DataRow dr in _dtData.Rows)
                        {
                            standardDTO = new StandardDTO();
                            standardDTO.StandardId = (int)dr["StandardId"];
                            standardDTO.StandardName = dr["StandardName"].ToString();
                            lstStandard.Add(standardDTO);
                        }
                    }
                    return lstStandard;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }


        //below code will fetch only Standard LIst not based on classtype
        public List<StandardDTO> Standard()
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "select StandardId,StandardName from standard where Active=1";
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    _dtData = new DataTable();
                    MySqlDataAdapter msDa = new MySqlDataAdapter(command);
                    msDa.Fill(_dtData);
                    List<StandardDTO> lstStandard = new List<StandardDTO>();
                    if (_dtData != null && _dtData.Rows.Count > 0)
                    {
                        StandardDTO standardDTO = null;
                        foreach (DataRow dr in _dtData.Rows)
                        {
                            standardDTO = new StandardDTO();
                            standardDTO.StandardId = (int)dr["StandardId"];
                            standardDTO.StandardName = dr["StandardName"].ToString();
                            lstStandard.Add(standardDTO);
                        }
                    }
                    return lstStandard;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public List<StandardSectionMapDTO> StandardSection()
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "select StandardSectionId,concat(StandardName,' ', SectionName) as StandardSectionDesc from standardsectionmap ssm join standard std on std.StandardId = ssm.StandardId join section sc on sc.SectionId = ssm.SectionId where ssm.Active = 1";
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    _dtData = new DataTable();
                    MySqlDataAdapter msDa = new MySqlDataAdapter(command);
                    msDa.Fill(_dtData);
                    List<StandardSectionMapDTO> lstStandardSection = new List<StandardSectionMapDTO>();
                    if (_dtData != null && _dtData.Rows.Count > 0)
                    {
                        StandardSectionMapDTO standardSectionMapDTO = null;
                        foreach (DataRow dr in _dtData.Rows)
                        {
                            standardSectionMapDTO = new StandardSectionMapDTO();
                            standardSectionMapDTO.StandardSectionId = (int)dr["StandardSectionId"];
                            standardSectionMapDTO.StandardSectionDesc = dr["StandardSectionDesc"].ToString();
                            lstStandardSection.Add(standardSectionMapDTO);
                        }
                    }
                    return lstStandardSection;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }


       public  List<StandardSectionMapDTO> StandardSectionWithSerial()
        {
            List<StandardSectionMapDTO> lstStandardSection = null;
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText="select StandardSectionId,concat(StandardName, ' ', SectionName) as StandardSectionDesc,Serial from standardsectionmap ssm join standard std on std.StandardId = ssm.StandardId join section sc on sc.SectionId = ssm.SectionId where ssm.Active = 1";
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    lstStandardSection = new List<StandardSectionMapDTO>();

                    _dtData = new DataTable();
                    MySqlDataAdapter msDa = new MySqlDataAdapter(command);
                    msDa.Fill(_dtData);

                    if(_dtData!=null && _dtData.Rows.Count>0)
                    {
                        StandardSectionMapDTO standardSectionMapDTO = null;
                        foreach (DataRow dr in _dtData.Rows)
                        {
                            standardSectionMapDTO = new StandardSectionMapDTO();
                            standardSectionMapDTO.StandardSectionId = (int)dr["StandardSectionId"];
                            standardSectionMapDTO.StandardSectionDesc = dr["StandardSectionDesc"].ToString();
                            standardSectionMapDTO.Serial = (int)dr["Serial"];

                            lstStandardSection.Add(standardSectionMapDTO);
                        }
                    }
                    

                }
                catch(Exception ex)
                {
                    
                }
            }
            return lstStandardSection;
        }


        // returns next standard section w.r.t the current standard 
        public List<StandardSectionMapDTO> NextStandardSection(int currentSerial)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "select StandardSectionId,concat(StandardName,' ', SectionName) as StandardSectionDesc from standardsectionmap ssm join standard std on std.StandardId = ssm.StandardId join section sc on sc.SectionId = ssm.SectionId where ssm.Active = 1 AND ssm.Serial=(@currentSerial+1) order by StandardSectionDesc";
                    command.Parameters.Add("@currentSerial", MySqlDbType.Int32).Value = currentSerial;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    _dtData = new DataTable();
                    MySqlDataAdapter msDa = new MySqlDataAdapter(command);
                    msDa.Fill(_dtData);
                    List<StandardSectionMapDTO> lstStandardSection = new List<StandardSectionMapDTO>();
                    if (_dtData != null && _dtData.Rows.Count > 0)
                    {
                        StandardSectionMapDTO standardSectionMapDTO = null;
                        foreach (DataRow dr in _dtData.Rows)
                        {
                            standardSectionMapDTO = new StandardSectionMapDTO();
                            standardSectionMapDTO.StandardSectionId = (int)dr["StandardSectionId"];
                            standardSectionMapDTO.StandardSectionDesc = dr["StandardSectionDesc"].ToString();
                            lstStandardSection.Add(standardSectionMapDTO);
                        }
                    }
                    return lstStandardSection;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public List<UserMasterDTO> GetAllActiveUsers()
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "select UserMasterId,FName,MName,LName,UserName from usermaster where Active=1";
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    _dtData = new DataTable();
                    MySqlDataAdapter msDa = new MySqlDataAdapter(command);
                    msDa.Fill(_dtData);
                    List<UserMasterDTO> lstUserMaster = new List<UserMasterDTO>();
                    if (_dtData != null && _dtData.Rows.Count > 0)
                    {
                        UserMasterDTO userMasterDTO = null;
                        foreach (DataRow dr in _dtData.Rows)
                        {
                            userMasterDTO = new UserMasterDTO();
                            if(!string.IsNullOrEmpty(dr["FName"].ToString()))
                            {
                                userMasterDTO.FName = dr["FName"].ToString();
                            }
                            if (!string.IsNullOrEmpty(dr["MName"].ToString()))
                            {
                                userMasterDTO.FName = userMasterDTO.FName + " " +dr["MName"].ToString();
                            }
                            if (!string.IsNullOrEmpty(dr["LName"].ToString()))
                            {
                                userMasterDTO.FName = userMasterDTO.FName + " " + dr["LName"].ToString();
                            }
                            userMasterDTO.FName = userMasterDTO.FName + " - " + dr["UserName"].ToString();
                            userMasterDTO.UserMasterId = (int)dr["UserMasterId"];
                            lstUserMaster.Add(userMasterDTO);
                        }
                    }
                    return lstUserMaster;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public List<TransactionRuleDTO> GetActiveTrRules()
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "select TranRuleId, RuleName from transactionrule where Active=1";
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    _dtData = new DataTable();
                    MySqlDataAdapter msDa = new MySqlDataAdapter(command);
                    msDa.Fill(_dtData);
                    List<TransactionRuleDTO> lstTrRule = new List<TransactionRuleDTO>();
                    if (_dtData != null && _dtData.Rows.Count > 0)
                    {
                        TransactionRuleDTO trRule = null;
                        foreach (DataRow dr in _dtData.Rows)
                        {
                            trRule = new TransactionRuleDTO();
                            trRule.TranRuleId = (int)dr["TranRuleId"];
                            trRule.RuleName = dr["RuleName"].ToString();
                            lstTrRule.Add(trRule);
                        }
                    }
                    return lstTrRule;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public List<ActionDTO> GetActions()
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "select ActionId,ActionName from dic_action where Active=1";
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    _dtData = new DataTable();
                    MySqlDataAdapter msDa = new MySqlDataAdapter(command);
                    msDa.Fill(_dtData);
                    List<ActionDTO> lstAction = new List<ActionDTO>();
                    if (_dtData != null && _dtData.Rows.Count > 0)
                    {
                        ActionDTO actionDTO = null;
                        foreach (DataRow dr in _dtData.Rows)
                        {
                            actionDTO = new ActionDTO();
                            actionDTO.RowId = (int)dr["ActionId"];
                            actionDTO.ActionName = dr["ActionName"].ToString();
                            lstAction.Add(actionDTO);
                        }
                    }
                    return lstAction;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public List<EntitlementDTO> GetUserRole()
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "select UserRoleId,RoleName from dic_entitlement where RoleIsActive=1";
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    _dtData = new DataTable();
                    MySqlDataAdapter msDa = new MySqlDataAdapter(command);
                    msDa.Fill(_dtData);
                    List<EntitlementDTO> lstEntitlement = new List<EntitlementDTO>();
                    if (_dtData != null && _dtData.Rows.Count > 0)
                    {
                        EntitlementDTO entitlementDTO = null;
                        foreach (DataRow dr in _dtData.Rows)
                        {
                            entitlementDTO = new EntitlementDTO();
                            entitlementDTO.UserRoleId = (int)dr["UserRoleId"];
                            entitlementDTO.RoleName = dr["RoleName"].ToString();
                            lstEntitlement.Add(entitlementDTO);
                        }
                    }
                    return lstEntitlement;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public List<TransactionMasterDTO> GetTransactionMasters(string frequency=null)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "select TranMasterId, TransactionName from transactionmaster where Active=1";
                    if(!string.IsNullOrEmpty(frequency))
                    {
                        command.CommandText = command.CommandText + " AND Frequency=@freq";
                        command.Parameters.Add("@freq", MySqlDbType.VarChar).Value = frequency;
                    }
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    _dtData = new DataTable();
                    MySqlDataAdapter msDa = new MySqlDataAdapter(command);
                    msDa.Fill(_dtData);
                    List<TransactionMasterDTO> lstTrMaster = new List<TransactionMasterDTO>();
                    if (_dtData != null && _dtData.Rows.Count > 0)
                    {
                        TransactionMasterDTO trMaster = null;
                        foreach (DataRow dr in _dtData.Rows)
                        {
                            trMaster = new TransactionMasterDTO();
                            trMaster.TranMasterId = (int)dr["TranMasterId"];
                            trMaster.TransactionName = dr["TransactionName"].ToString();
                            lstTrMaster.Add(trMaster);
                        }
                    }
                    return lstTrMaster;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public List<TransactionRuleDTO> GetTransactionRules()
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "select TranRuleId, RuleName from transactionrule where Active=1";
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    _dtData = new DataTable();
                    MySqlDataAdapter msDa = new MySqlDataAdapter(command);
                    msDa.Fill(_dtData);
                    List<TransactionRuleDTO> lstTrRule = new List<TransactionRuleDTO>();
                    if (_dtData != null && _dtData.Rows.Count > 0)
                    {
                        TransactionRuleDTO trRule = null;
                        foreach (DataRow dr in _dtData.Rows)
                        {
                            trRule = new TransactionRuleDTO();
                            trRule.TranRuleId = (int)dr["TranRuleId"];
                            trRule.RuleName = dr["RuleName"].ToString();
                            lstTrRule.Add(trRule);
                        }
                    }
                    return lstTrRule;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public List<ExamTypeDTO> getExamTypeList()
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "select ExamTypeId, ExamTypeDescription from examtypes where Active=1";
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    _dtData = new DataTable();
                    MySqlDataAdapter msDa = new MySqlDataAdapter(command);
                    msDa.Fill(_dtData);
                    List<ExamTypeDTO> lstexamType = new List<ExamTypeDTO>();
                    if (_dtData != null && _dtData.Rows.Count > 0)
                    {
                        ExamTypeDTO examType = null;
                        foreach (DataRow dr in _dtData.Rows)
                        {
                            examType = new ExamTypeDTO();
                            examType.ExamTypeId = (int)dr["ExamTypeId"];
                            examType.ExamTypeDescription = dr["ExamTypeDescription"].ToString();
                            lstexamType.Add(examType);
                        }
                    }
                    return lstexamType;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public List<ExamSubTypeDTO> getExamSubTypeList()
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "select ExamSubTypeId, ExamSubTypeDescription from examsubtype where Active=1";
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    _dtData = new DataTable();
                    MySqlDataAdapter msDa = new MySqlDataAdapter(command);
                    msDa.Fill(_dtData);
                    List<ExamSubTypeDTO> lstexamsubType = new List<ExamSubTypeDTO>();
                    if (_dtData != null && _dtData.Rows.Count > 0)
                    {
                        ExamSubTypeDTO examsubType = null;
                        foreach (DataRow dr in _dtData.Rows)
                        {
                            examsubType = new ExamSubTypeDTO();
                            examsubType.ExamSubTypeId = (int)dr["ExamSubTypeId"];
                            examsubType.ExamSubTypeDescription = dr["ExamSubTypeDescription"].ToString();
                            lstexamsubType.Add(examsubType);
                        }
                    }
                    return lstexamsubType;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        /// To get the competencies 
        /// 

        //public List<CompetencyDTO> GetCompetency()
        //{
        //    using (IDbSvc dbSvc = new DbSvc(_configSvc))
        //    {
        //        try
        //        {
        //            dbSvc.OpenConnection();
        //            MySqlCommand command = new MySqlCommand();
        //            command.CommandText = "select CompetencyId,CompetencyDescription from competency where Active=1";
        //            command.Connection = dbSvc.GetConnection() as MySqlConnection;
        //            _dtData = new DataTable();
        //            MySqlDataAdapter msDa = new MySqlDataAdapter(command);
        //            msDa.Fill(_dtData);
        //            List<CompetencyDTO> lstCompetency = new List<CompetencyDTO>();
        //            if (_dtData != null && _dtData.Rows.Count > 0)
        //            {
        //                CompetencyDTO competencyDTO = null;
        //                foreach (DataRow dr in _dtData.Rows)
        //                {
        //                    competencyDTO = new CompetencyDTO();
        //                    competencyDTO.CompetencyId = (int)dr["LocationId"];
        //                    competencyDTO.CompetencyDescription = dr["LocationDescription"].ToString();
        //                    lstCompetency.Add(competencyDTO);
        //                }
        //            }
        //            return lstCompetency;
        //        }
        //        catch (Exception exp)
        //        {
        //            throw exp;
        //        }
        //    }
        //}

        public List<CompetencyDTO> Competency()
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "select CompetencyId,CompetencyDescription from competency where Active=1";
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    _dtData = new DataTable();
                    MySqlDataAdapter msDa = new MySqlDataAdapter(command);
                    msDa.Fill(_dtData);
                    List<CompetencyDTO> lstCompetency = new List<CompetencyDTO>();
                    if (_dtData != null && _dtData.Rows.Count > 0)
                    {
                        CompetencyDTO competencyDTO = null;
                        foreach (DataRow dr in _dtData.Rows)
                        {
                            competencyDTO = new CompetencyDTO();
                            competencyDTO.CompetencyId = (int)dr["CompetencyId"];
                            competencyDTO.CompetencyDescription = dr["CompetencyDescription"].ToString();
                            lstCompetency.Add(competencyDTO);
                        }
                    }
                    return lstCompetency;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public List<AppraisalStatusDTO> AppraisalStatus()
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "select AppraisalStatusId,AppraisalStatusDescription from AppraisalStatus";
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    _dtData = new DataTable();
                    MySqlDataAdapter msDa = new MySqlDataAdapter(command);
                    msDa.Fill(_dtData);
                    List<AppraisalStatusDTO> lstAppraisalStatusDTO = new List<AppraisalStatusDTO>();
                    if (_dtData != null && _dtData.Rows.Count > 0)
                    {
                        AppraisalStatusDTO appraisalStatusDTO = null;
                        foreach (DataRow dr in _dtData.Rows)
                        {
                            appraisalStatusDTO = new AppraisalStatusDTO();
                            appraisalStatusDTO.AppraisalStatusId = (int)dr["AppraisalStatusId"];
                            appraisalStatusDTO.AppraisalStatusDescription = dr["AppraisalStatusDescription"].ToString();
                            lstAppraisalStatusDTO.Add(appraisalStatusDTO);
                        }
                    }
                    return lstAppraisalStatusDTO;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public List<RatingDTO> AppraisalRating()
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "select distinct StartingRange,LevelName from weightage_levels";
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    _dtData = new DataTable();
                    MySqlDataAdapter msDa = new MySqlDataAdapter(command);
                    msDa.Fill(_dtData);
                    List<RatingDTO> lstRatingDTO = new List<RatingDTO>();
                    if (_dtData != null && _dtData.Rows.Count > 0)
                    {
                        RatingDTO ratingDTO = null;
                        foreach (DataRow dr in _dtData.Rows)
                        {
                            ratingDTO = new RatingDTO();
                            ratingDTO.Minimum = (int)dr["StartingRange"] + 1;
                            ratingDTO.RatingLevel = dr["LevelName"].ToString();
                            lstRatingDTO.Add(ratingDTO);
                        }
                    }
                    return lstRatingDTO;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }
    }
}
