﻿using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.Contracts.Modules
{
    public interface IDropdownRepo
    {
        List<LocationDTO> Location();

        List<RoleDTO> Roles();

        List<HouseTypeDTO> House();

        List<ClassTypeDTO> ClassType();

        List<SectionDTO> Section();

        List<BookCategoryDTO> BookCategry();

        List<DepartmentDTO> Department();

        List<DesignationDTO> Designation();

        List<StandardDTO> Standard(ClassTypeDTO classTypeDTO);

        // below code will return only list of standard not based on classtype
        List<StandardDTO> Standard();

        List<StandardSectionMapDTO> StandardSection();

        List<UserMasterDTO> GetAllActiveUsers();

        List<TransactionRuleDTO> GetActiveTrRules();

        List<ActionDTO> GetActions();

        List<EntitlementDTO> GetUserRole();

        List<TransactionMasterDTO> GetTransactionMasters(string frequency=null);

        List<TransactionRuleDTO> GetTransactionRules();
    }
}
