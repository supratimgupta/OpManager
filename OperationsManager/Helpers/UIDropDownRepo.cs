﻿using OpMgr.Common.Contracts.Modules;
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
            return new SelectList(rDto, "StandardSectionId", "StandardSectionDesc");
        }

        public SelectList getGenderDropDown()
        {
            SelectList gender = new SelectList((new[] 
                                            {
                                              new {ID="1",Name="Male"},
                                              new{ID="2",Name="Female"},
                                              new{ID="3",Name="Others"},
                                            }),
                            "ID", "Name", 1);
            return new SelectList(gender, "ID", "Name");
        }
    }
}