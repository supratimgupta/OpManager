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
        bool UpdateReviewerReview(int apprMasterId, decimal reviewerRating, string reviewerComment);
        void SaveCompetency(int appraisalMasterId, string delimitedImprovements, string delimitedStrengths);
        void GetCompetencies(int appraisalMasterId, out List<KeyValuePair<string, string>> strengths, out List<KeyValuePair<string, string>> improvements);
        StatusDTO<List<EmployeeAppraisalMasterDTO>> SearchAppraisee(EmployeeAppraisalMasterDTO data);
        bool UpdateInitiativeandSupport(int apprMasterId, string IndividualInitiative, string InstitutionalSupport);

        StatusDTO<List<PMSMasterDTO>> GetAppraisalDetails(int pmsMasterId);
        StatusDTO<List<EmployeeAppraisalMasterDTO>> GetAppraiseePMSLIst(int EmployeeId);
        bool UpdateAppraiserFinalRating(int apprMasterId, decimal appraiserRating, string appraiserComment);

        bool UpdatePMSHeadApproval(EmployeeAppraisalMasterDTO employeemasterDTO);

        StatusDTO<List<EmployeeGoalLogDTO>> ExcelDataForPMSHead(EmployeeAppraisalMasterDTO employeemasterDTO);

        StatusDTO<EmployeeAppraisalMasterDTO> InitiateAppraisal(string AppraisalType);
    }
}
