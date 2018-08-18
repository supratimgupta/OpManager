using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpMgr.Common.DTOs;

namespace OpMgr.Common.Contracts.Modules
{
    public interface IUserSvc : ICRUDSvc<UserMasterDTO, UserMasterDTO>
    {
        StatusDTO<UserMasterDTO> Login(UserMasterDTO data, out List<EntitlementDTO> roleList, out List<ActionDTO> actionList, out List<LocationDTO> locationList);

        List<DTOs.UserEntitlementDTO> GetUserEntitlement(int userMasterId);

        StatusDTO<UserEntitlementDTO> InsertUserEntitlement(UserEntitlementDTO data);

        StatusDTO<UserEntitlementDTO> DeleteUserEntitlement(UserEntitlementDTO data);

        StatusDTO<UserEntitlementDTO> UpdateUserEntitlement(UserEntitlementDTO data);

        StatusDTO<FacultyCourseMapDTO> DeleteFacultyCourseMap(FacultyCourseMapDTO data);

        List<DTOs.FacultyCourseMapDTO> GetFacultyCourseMap(int employeeId);

        StatusDTO<FacultyCourseMapDTO> InsertFacultyCourse(FacultyCourseMapDTO data);

        StatusDTO<FacultyCourseMapDTO> UpdateFacultyCourseMap(FacultyCourseMapDTO data);

        StatusDTO<PMSEmpDesignationMapDTO> DeletePMSDesignationMap(PMSEmpDesignationMapDTO data);

        List<PMSEmpDesignationMapDTO> GetPMSDesignationMap(int pmsdesId);

        StatusDTO<PMSEmpDesignationMapDTO> InsertPMSDesignationMap(PMSEmpDesignationMapDTO data);

        StatusDTO<PMSEmpDesignationMapDTO> UpdatePMSDesignationMap(PMSEmpDesignationMapDTO data);

        int GetCurrentEmployeeCounter();
    }
}
