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
            //locDto.LocationId = -1;
            locDto.LocationDescription = string.Empty;

           // lDto.Insert(0, locDto);

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
            ClassTypeDTO blank = new ClassTypeDTO();
            blank.ClassTypeId = -1;
            blank.ClassTypeName = "";
            rDto.Insert(0, blank);
            return new SelectList(rDto, "ClassTypeId", "ClassTypeName");
        }

        public SelectList getSectionDropDown()
        {
            List<SectionDTO> rDto = _ddlRepo.Section();

            SectionDTO blank = new SectionDTO();
            blank.SectionId = -1;
            blank.SectionName = "";
            rDto.Insert(0, blank);

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

            StandardDTO blank = new StandardDTO();
            blank.StandardId = -1;
            blank.StandardName = "";
            rDto.Insert(0, blank);

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

        public SelectList getStandardSectionDropDownWithSerial()
        {
            List<StandardSectionMapDTO> rDto = _ddlRepo.StandardSectionWithSerial();

            StandardSectionMapDTO ssDto = new StandardSectionMapDTO();
            ssDto.StandardSectionId = -1;
            ssDto.StandardSectionDesc = "SELECT";

            rDto.Insert(0, ssDto);

            return new SelectList(rDto, "StandardSectionId", "StandardSectionDesc");
        }

        // return next Standard Section List w.r.t Current Standard
        public SelectList getNextStandardSectionDropDown(int currentStandardId)
        {
            List<StandardSectionMapDTO> rDto = _ddlRepo.NextStandardSection(currentStandardId);

            StandardSectionMapDTO ssDto = new StandardSectionMapDTO();
            ssDto.StandardSectionId = -1;
            ssDto.StandardSectionDesc = string.Empty;

            rDto.Insert(0, ssDto);

            return new SelectList(rDto, "StandardSectionId", "StandardSectionDesc");
        }

        public SelectList getGenderDropDown()
        {
            Dictionary<string, string> dicGender = new Dictionary<string, string>();
            //dicGender.Add("-1", "");
            dicGender.Add("1", "Male");
            dicGender.Add("2", "Female");
            dicGender.Add("3", "Other");
            return new SelectList(dicGender, "key", "value");
        }

        public SelectList getPromotionStatusDropDown()
        {
            Dictionary<string, string> dicPromotionStatus = new Dictionary<string, string>();
            //dicPromotionStatus.Add("-1", "Select");
            dicPromotionStatus.Add("1", "Passed");
            //dicPromotionStatus.Add("2", "Promotion Confirmed");
            dicPromotionStatus.Add("2", "Failed");
            return new SelectList(dicPromotionStatus, "key", "value");

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

        // return Action List
        public SelectList getActionLinkDropDown()
        {
            List<ActionDTO> lactionDto = _ddlRepo.GetActions();

            ActionDTO actDto = new ActionDTO();
            actDto.RowId = -1;
            actDto.ActionName = string.Empty;

            lactionDto.Insert(0, actDto);

            return new SelectList(lactionDto, "RowId", "ActionName");
        }

        // return Entitlement List
        public SelectList getEntitleMentDropDown()
        {
            List<EntitlementDTO> lstentitleDto = _ddlRepo.GetUserRole();

            EntitlementDTO entitleDto = new EntitlementDTO();
            entitleDto.UserRoleId = -1;
            entitleDto.RoleName = string.Empty;

            lstentitleDto.Insert(0, entitleDto);

            return new SelectList(lstentitleDto, "UserRoleId", "RoleName");
        }

        public SelectList getTransactionIsDiffTo()
        {
            Dictionary<string, string> dicTrTypes = new Dictionary<string, string>();

            dicTrTypes.Add("-1", "");
            dicTrTypes.Add("NONE", "NONE");
            dicTrTypes.Add("USER", "USER");
            dicTrTypes.Add("CLASS-TYPE", "CLASS-TYPE");
            dicTrTypes.Add("STANDARD", "STANDARD");
            dicTrTypes.Add("SECTION", "SECTION");
            return new SelectList(dicTrTypes, "key", "value");
        }

        public SelectList getTransactionFrequencies()
        {
            Dictionary<string, string> dicTrTypes = new Dictionary<string, string>();

            dicTrTypes.Add("-1", "");
            dicTrTypes.Add("DAILY", "DAILY");
            dicTrTypes.Add("MONTHLY", "MONTHLY");
            dicTrTypes.Add("YEARLY", "YEARLY");
            dicTrTypes.Add("ONE-TIME", "ONE-TIME");
            return new SelectList(dicTrTypes, "key", "value");
        }

        public SelectList getCalcType()
        {
            Dictionary<string, string> dicTrTypes = new Dictionary<string, string>();

            dicTrTypes.Add("-1", "");
            dicTrTypes.Add("PERCENT", "PERCENT");
            dicTrTypes.Add("ACTUAL", "ACTUAL");
            return new SelectList(dicTrTypes, "key", "value");
        }

        public List<KeyValueDTO> getCalcTypeDic()
        {
            List<KeyValueDTO> dicTrTypes = new List<KeyValueDTO>();

            KeyValueDTO keyVal = new KeyValueDTO();
            keyVal.Key = "-1"; keyVal.Value = "";
            dicTrTypes.Add(keyVal);
            keyVal = new KeyValueDTO();
            keyVal.Key = "PERCENT"; keyVal.Value = "PERCENT";
            dicTrTypes.Add(keyVal);
            keyVal = new KeyValueDTO();
            keyVal.Key = "ACTUAL"; keyVal.Value = "ACTUAL";
            dicTrTypes.Add(keyVal);

            return dicTrTypes;
        }

        public SelectList getTransactionMasters(string frequency=null)
        {
            List<TransactionMasterDTO> lstentitleDto = _ddlRepo.GetTransactionMasters(frequency);

            TransactionMasterDTO trMasterDTO = new TransactionMasterDTO();
            trMasterDTO.TranMasterId = -1;
            trMasterDTO.TransactionName = string.Empty;

            lstentitleDto.Insert(0, trMasterDTO);

            return new SelectList(lstentitleDto, "TranMasterId", "TransactionName");
        }
    }
}