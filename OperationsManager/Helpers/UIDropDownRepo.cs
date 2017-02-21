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
            return new SelectList(lDto, "LocationId", "LocationDescription");
        }

        public SelectList getRoleDropDown()
        {
            List<RoleDTO> rDto = _ddlRepo.Roles();
            return new SelectList(rDto, "RoleId", "RoleDescription");
        }
    }
}