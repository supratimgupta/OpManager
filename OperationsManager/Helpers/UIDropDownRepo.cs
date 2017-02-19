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
            DataTable dtLocation = new DataTable();
            dtLocation = _ddlRepo.Location();
            
            
            List<LocationDTO> lDto = new List<LocationDTO>();
            foreach(DataRow dr in dtLocation.Rows)
            {
                LocationDTO dto = new LocationDTO();
                dto.LocationId = Convert.ToInt32(dr["LocationId"]);
                dto.LocationDescription = dr["LocationDescription"].ToString();
                lDto.Add(dto);
            }
            return new SelectList(lDto, "LocationId", "LocationDescription");
        }
    }
}