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
        StatusDTO<UserMasterDTO> Login(UserMasterDTO data, out List<EntitlementDTO> roleList, out List<ActionDTO> actionList);

        List<DTOs.UserEntitlementDTO> GetUserEntitlement(int userMasterId);

        StatusDTO<UserEntitlementDTO> InsertUserEntitlement(UserEntitlementDTO data);

        StatusDTO<UserEntitlementDTO> DeleteUserEntitlement(UserEntitlementDTO data); 

         StatusDTO<UserEntitlementDTO> UpdateUserEntitlement(UserEntitlementDTO data);

        StatusDTO<FacultyCourseMapDTO> DeleteFacultyCourseMap(FacultyCourseMapDTO data);

        List<DTOs.FacultyCourseMapDTO> GetFacultyCourseMap(int employeeId);

        StatusDTO<FacultyCourseMapDTO> InsertFacultyCourse(FacultyCourseMapDTO data);

        StatusDTO<FacultyCourseMapDTO> UpdateFacultyCourseMap(FacultyCourseMapDTO data);

        int GetCurrentEmployeeCounter();
    }
}
