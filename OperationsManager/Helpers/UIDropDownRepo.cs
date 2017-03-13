using OpMgr.Common.Contracts.Modules;
using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace OperationsManager.Helpers
{
    public class UIDropDownRepo
    { 
        private IDropdownRepo _ddlRepo;
        public UIDropDownRepo(IDropdownRepo ddlRepo)
        {
            _ddlRepo = ddlRepo;
        }

        public  SelectList getLocationDropDown()
        {
            List<LocationDTO> lDto = _ddlRepo.Location();

            LocationDTO locDto = new LocationDTO();
            locDto.LocationId = -1;
            locDto.LocationDescription = string.Empty;

            lDto.Insert(0, locDto);

            return new SelectList(lDto, "LocationId", "LocationDescription");
        }

        public SelectList getRoleDropDown()
        {
            List<RoleDTO> rDto = _ddlRepo.Roles();
            return new SelectList(rDto, "RoleId", "RoleDescription");
        }

        public SelectList getHouseDropDown()
        {
            List<HouseTypeDTO> rDto = _ddlRepo.House();
            return new SelectList(rDto, "HouseTypeId", "HouseTypeDescription");
        }

        public SelectList getClassTypeDropDown()
        {
            List<ClassTypeDTO> rDto = _ddlRepo.ClassType();
            return new SelectList(rDto, "ClassTypeId", "ClassTypeName");
        }

        public SelectList getSectionDropDown()
        {
            List<SectionDTO> rDto = _ddlRepo.Section();
            return new SelectList(rDto, "SectionId", "SectionName");
        }

        public SelectList getBookCategoryDropDown()
        {
            List<BookCategoryDTO> rDto = _ddlRepo.BookCategry();
            return new SelectList(rDto, "BookCategoryId", "BookCategory");
        }

        public SelectList getDepartmentDropDown()
        {
            List<DepartmentDTO> rDto = _ddlRepo.Department();
            return new SelectList(rDto, "DepartmentId", "DepartmentName");
        }

        public SelectList getDesignationDropDown()
        {
            List<DesignationDTO> rDto = _ddlRepo.Designation();
            return new SelectList(rDto, "DesignationId", "DesignationDescription");
        }

        public SelectList getStandardDropDown(ClassTypeDTO classTypeDTO)
        {
            List<StandardDTO> rDto = _ddlRepo.Standard(classTypeDTO);
            return new SelectList(rDto, "StandardId", "StandardName");
        }

        // return standardlist not based on classtype
        public SelectList getStandardDropDown()
        {
            List<StandardDTO> rDto = _ddlRepo.Standard();
            return new SelectList(rDto, "StandardId", "StandardName");
        }

        // return standard Section List
        public SelectList getStandardSectionDropDown()
        {
            List<StandardSectionMapDTO> rDto = _ddlRepo.StandardSection();

            StandardSectionMapDTO ssDto = new StandardSectionMapDTO();
            ssDto.StandardSectionId = -1;
            ssDto.StandardSectionDesc = string.Empty;

            rDto.Insert(0, ssDto);

            return new SelectList(rDto, "StandardSectionId", "StandardSectionDesc");
        }

        public SelectList getGenderDropDown()
        {
            Dictionary<string, string> dicGender = new Dictionary<string, string>();
            dicGender.Add("-1", "");
            dicGender.Add("1", "Male");
            dicGender.Add("2", "Female");
            dicGender.Add("3", "Other");
            return new SelectList(dicGender, "key", "value");
        }

        public SelectList getUserDropDown()
        {
            List<UserMasterDTO> lstUserMaster = _ddlRepo.GetAllActiveUsers();

            UserMasterDTO umDTO = new UserMasterDTO();
            umDTO.UserMasterId = -1;
            umDTO.FName = string.Empty;
            lstUserMaster.Insert(0,umDTO);

            return new SelectList(lstUserMaster, "UserMasterId", "FName");
        }

        public SelectList getTransactionTypes()
        {
            Dictionary<string, string> dicTrTypes = new Dictionary<string, string>();

            dicTrTypes.Add("-1", "");
            dicTrTypes.Add("D", "Debit");
            dicTrTypes.Add("C", "Credit");
            dicTrTypes.Add("NA", "NA");
            return new SelectList(dicTrTypes, "key", "value");
        }

        public SelectList getTransactionRules()
        {
            List<TransactionRuleDTO> lstTrRule = _ddlRepo.GetActiveTrRules();

            TransactionRuleDTO trRule = new TransactionRuleDTO();
            trRule.TranRuleId = -1;
            trRule.RuleName = string.Empty;

            lstTrRule.Insert(0, trRule);

            return new SelectList(lstTrRule, "TranRuleId", "RuleName");
        }
    }
}