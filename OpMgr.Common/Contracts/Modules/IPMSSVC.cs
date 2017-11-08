using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.Contracts.Modules
{
    public interface IPMSSvc : ICRUDSvc<EmployeeGoalLogDTO, EmployeeGoalLogDTO>
    {
        StatusDTO<EmployeeGoalLogDTO> getSelfRating(int Achievement);
        int GetCurrentStatus(int empApprMasterId);
        bool MoveFwdBckwd(int appraisalMasterId, int currentStatus, bool isBackwd = false);
        string AccessStatus(int appraisalMasterId);
        StatusDTO<EmployeeGoalLogDTO> UpdateAppraiserRating(EmployeeGoalLogDTO data);
        bool UpdateReviewerReview(int apprMasterId, decimal reviewerRating);
    }
}
