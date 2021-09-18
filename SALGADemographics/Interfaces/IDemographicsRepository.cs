using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SALGADBLib
{
    public interface IDemographicsRepository
    {
        public Task<List<UserRole>> GetAllUserRoles();
        public Task<IEnumerable<SeniorManager>> GetSeniorManagers();
        public Task<IEnumerable<SeniorManagerPosition>> GetSeniorManagerPositions(MunicipalityDemographics municipalityDemographics);
        public Task<IEnumerable<SeniorManagerPosition>> UpdateSeniorManagerPositions(IEnumerable<SeniorManagerPosition> listPositions);
        public Task<IEnumerable<IntervieweeDetails>> GetAllPersonnelData();
        public Task<Municipality> GetMunicipality(int pkMunicipalityID);
        public Task<HRMDFunctionDemographics> GetHRMDDemographics(Municipality municipality);
        public Task<HRMDFunctionDemographics> GetHRMDDemographics(IdentityUser user);
        public Task<MunicipalityDemographics> GetDemographics(IdentityUser user );
        public Task<MunicipalityDemographics> GetDemographics(Municipality municipality);
        public Task<List<IntervieweeDetails>> GetIntervieweeDetails(Municipality municipality);
        public Task<IntervieweeDetails> GetIntervieweeDetails(IdentityUser user);
        public Task<IntervieweeDetails> GetIntervieweeDetails(int pkIntervieweeID);
        public Task<MunicipalityDemographics> SaveMunicipalityDemographics(MunicipalityDemographics municipalityDemographics);
        public Task<IntervieweeDetails> SaveInterviewDetails(IntervieweeDetails intervieweeDetails);
        public Task<HRMDFunctionDemographics> SaveHRMDDemographics(HRMDFunctionDemographics hRMDFunctionDemographics);
        public Task<IEnumerable<JobTitle>> GetJobTitles();
        public Task<IEnumerable<ServiceOption>> GetServiceOptions();
        public Task<IEnumerable<Municipality>> GetMunicipalities();
        public Task<IEnumerable<MunicipalCatagory>> GetMunicipalCategories();
        public Task<IEnumerable<Province>> GetProvinces();
        public Task<IEnumerable<MunicipalityDemographics>> GetMunicipalityDemographics();

        public Task<IEnumerable<IntervieweeDetails>> GetAllCapturers();
    }
}
