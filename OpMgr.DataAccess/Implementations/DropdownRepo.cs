﻿using MySql.Data.MySqlClient;
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

        public List<UserMasterDTO> Users()
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "select UserMasterId,Fname,Mname,Lname from UserMaster where Active=1";
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    _dtData = new DataTable();
                    MySqlDataAdapter msDa = new MySqlDataAdapter(command);
                    msDa.Fill(_dtData);
                    List<UserMasterDTO> lstUsers = new List<UserMasterDTO>();
                    if (_dtData != null && _dtData.Rows.Count > 0)
                    {
                        UserMasterDTO userDTO = null;
                        foreach (DataRow dr in _dtData.Rows)
                        {
                            userDTO = new UserMasterDTO();
                            userDTO.UserMasterId = (int)dr["UserMasterId"];
                            userDTO.FName = dr["Fname"].ToString();
                            userDTO.MName = dr["Mname"].ToString();
                            userDTO.LName = dr["Lname"].ToString();
                            if(!string.IsNullOrEmpty(userDTO.MName.Trim()))
                            {
                                userDTO.FName = userDTO.FName + " " + userDTO.MName;
                            }
                            if(!string.IsNullOrEmpty(userDTO.LName.Trim()))
                            {
                                userDTO.FName = userDTO.FName + " " + userDTO.LName;
                            }
                            lstUsers.Add(userDTO);
                        }
                    }
                    return lstUsers;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public List<TransactionRuleDTO> TransactionRules()
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "select TranRuleId,RuleName from TransactionRule where Active=1";
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    _dtData = new DataTable();
                    MySqlDataAdapter msDa = new MySqlDataAdapter(command);
                    msDa.Fill(_dtData);
                    List<TransactionRuleDTO> lstRules = new List<TransactionRuleDTO>();
                    if (_dtData != null && _dtData.Rows.Count > 0)
                    {
                        TransactionRuleDTO ruleDTO = null;
                        foreach (DataRow dr in _dtData.Rows)
                        {
                            ruleDTO = new TransactionRuleDTO();
                            ruleDTO.TranRuleId = (int)dr["TranRuleId"];
                            ruleDTO.RuleName = dr["RuleName"].ToString();

                            lstRules.Add(ruleDTO);
                        }
                    }
                    return lstRules;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public Dictionary<string, string> TransactionTypes()
        {
            Dictionary<string, string> dicTransactionTypes = new Dictionary<string, string>();
            dicTransactionTypes.Add("D", "Debit");
            dicTransactionTypes.Add("C", "Credit");
            dicTransactionTypes.Add("NA", "Not Applicable");
            return dicTransactionTypes;
        }
    }
}
