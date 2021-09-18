using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SALGADBLib
{
    public class SQLDemographicsRepository : IDemographicsRepository
    {
        private SALGADbContext _dbContext;

        public SQLDemographicsRepository(SALGADbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<UserRole>> GetAllUserRoles()
        {
            var userRoles = new List<UserRole>();

            var roles = await _dbContext.Roles.ToListAsync();
            var dbUserRoles= await _dbContext.UserRoles.ToListAsync();
            foreach(var dbUserRole in dbUserRoles)
            {
                var userRole = new UserRole();
                userRole.UserID = dbUserRole.UserId;
                userRole.RoleName = roles.FirstOrDefault(x => x.Id == dbUserRole.RoleId).Name;
                userRoles.Add(userRole);
            }

            return userRoles;
        }

        public async Task<IEnumerable<SeniorManager>> GetSeniorManagers()
        {
            var managers= await _dbContext.SeniorManagers.OrderBy(x=>x.DisplayOrder).ToListAsync();
            return managers;
        }

        public async Task<IEnumerable<SeniorManagerPosition>> GetSeniorManagerPositions(MunicipalityDemographics municipalityDemographics)
        {
            var seniorPositions = await  _dbContext.SeniorManagerPositions.Include(x=>x.SeniorManager).Where(x => x.MunicipalityDemographics == municipalityDemographics).ToListAsync();
            return seniorPositions;
        }

        public async Task<IEnumerable<SeniorManagerPosition>> UpdateSeniorManagerPositions(IEnumerable<SeniorManagerPosition> listPositions)
        {

            foreach (var position in listPositions)
            {
                _dbContext.SeniorManagerPositions.Add(position);
                if (position.pkID>0)
                    _dbContext.Entry(position).State = EntityState.Modified;
            }
            await _dbContext.SaveChangesAsync();
            return listPositions;
        }

        public async Task<IEnumerable<IntervieweeDetails>> GetAllPersonnelData()
        {
  
            var lstIntervieweeDtls = await _dbContext.IntervieweeDetails.Include(x=>x.Municipality).Include(x=>x.User).ToListAsync();
            return lstIntervieweeDtls;
        }

        public async Task<MunicipalityDemographics> GetDemographics(IdentityUser user)
        {
           var dbDemographics = await  _dbContext.MunicipalityDemographics.Include(x=>x.Capturer).Include(x=>x.Approver).
                                                                            Include(x=>x.Municipality).FirstOrDefaultAsync(x => x.Capturer == user || x.Approver==user);
           return dbDemographics;
        }

        public async Task<MunicipalityDemographics> GetDemographics(Municipality municipality)
        {
            var dbDemographics = await _dbContext.MunicipalityDemographics
                                        .Include(x => x.Capturer).Include(x => x.Approver).FirstOrDefaultAsync(x => x.Municipality == municipality);
            return dbDemographics;
        }

        public async Task<Municipality> GetMunicipality(int pkMunicipalityID)
        {
            var dbMunicipality = await _dbContext.Municipalities.Include(x=>x.MunicipalCatagory).
                                                Include(x=>x.District).FirstOrDefaultAsync(x => x.pkID==pkMunicipalityID);
            return dbMunicipality;
        }

        public async Task<IntervieweeDetails> GetIntervieweeDetails(IdentityUser user)
        {
            var dbDemographics = await _dbContext.IntervieweeDetails.Include(x => x.JobTitle).Include(x => x.Municipality).
                                        Include(x => x.Municipality.MunicipalCatagory).FirstOrDefaultAsync(x => x.User == user);

            return dbDemographics;
        }

        public async Task<IntervieweeDetails> GetIntervieweeDetails(int pkIntervieweeID)
        {
            var dbDemographics = await _dbContext.IntervieweeDetails.Include(x => x.JobTitle).Include(x => x.Municipality).
                            Include(x => x.Municipality.MunicipalCatagory).FirstOrDefaultAsync(x=>x.pkID== pkIntervieweeID);

            return dbDemographics;
        }



        public async Task<List<IntervieweeDetails>> GetIntervieweeDetails(Municipality municipality)
        {
            var dbDemographics = await _dbContext.IntervieweeDetails.Include(x=>x.User).Include(x => x.JobTitle).Include(x => x.Municipality).
                                        Include(x => x.Municipality.MunicipalCatagory).Where(x => x.Municipality == municipality).ToListAsync();

            return dbDemographics;
        }

        public async Task<HRMDFunctionDemographics> GetHRMDDemographics(IdentityUser user)
        {
            var dbHRMDeomographics =await  _dbContext.HRMDFunctionDemographics.Include(x=>x.MunicipalityOrganogram).FirstOrDefaultAsync(x => x.User == user);
            return dbHRMDeomographics;
        }

        public async Task<HRMDFunctionDemographics> GetHRMDDemographics(Municipality municipality)
        {
            var dbHRMDeomographics = await _dbContext.HRMDFunctionDemographics.Include(x => x.MunicipalityOrganogram).FirstOrDefaultAsync(x => x.Municipality == municipality);
            return dbHRMDeomographics;
        }


        public async Task<IntervieweeDetails> SaveInterviewDetails(IntervieweeDetails intervieweeDetails)
        {
            _dbContext.IntervieweeDetails.Add(intervieweeDetails);
            if (intervieweeDetails.pkID > 0)
                _dbContext.Entry(intervieweeDetails).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return intervieweeDetails;
        }

        public async  Task<MunicipalityDemographics> SaveMunicipalityDemographics(MunicipalityDemographics municipalityDemographics)
        {
            _dbContext.MunicipalityDemographics.Add(municipalityDemographics);
            if (municipalityDemographics.pkID >0)
                _dbContext.Entry(municipalityDemographics).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return municipalityDemographics;
        }

        public async Task<HRMDFunctionDemographics> SaveHRMDDemographics(HRMDFunctionDemographics hrmdFunctionDemographics)
        {
            _dbContext.HRMDFunctionDemographics.Add(hrmdFunctionDemographics);
            if (hrmdFunctionDemographics.pkID>0)
                _dbContext.Entry(hrmdFunctionDemographics).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return hrmdFunctionDemographics;
        }

        public async Task<IEnumerable<JobTitle>> GetJobTitles()
        {
            var jobTitleLst = await _dbContext.JobTitles.ToListAsync();
            return jobTitleLst;
        }

        public async Task<IEnumerable<ServiceOption>> GetServiceOptions()
        {
            var serviceOptionsLst = await _dbContext.ServiceOptions.ToListAsync();
            return serviceOptionsLst;
        }

        public async Task<IEnumerable<Municipality>> GetMunicipalities()
        {
            var municipalityLst = await _dbContext.Municipalities.Include(x=>x.Province).Include(x=>x.MunicipalCatagory).ToArrayAsync();
            return municipalityLst;
        }

        public async Task<IEnumerable<MunicipalCatagory>> GetMunicipalCategories()
        {
            var municipalityLst = await _dbContext.MunicipalCatagories.ToListAsync();
            return municipalityLst;
        }

        public async Task<IEnumerable<Province>> GetProvinces()
        {
            var provinceLst = await _dbContext.Provinces.ToListAsync();
            return provinceLst;
        }

        public async Task<IEnumerable<MunicipalityDemographics>> GetMunicipalityDemographics()
        {
            var demographicsList = await _dbContext.MunicipalityDemographics.ToListAsync();
            return demographicsList;
        }

        public async Task<IEnumerable<IntervieweeDetails>> GetAllCapturers()
        {
           var detailsLst =await _dbContext.IntervieweeDetails.Include(x=>x.Municipality).Where(x => x.Active).ToListAsync();
           return detailsLst;

        }
    }
}
