using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SALGADBLib
{
    public class SQLAuditingRepository : IAuditingRepository
    {
        private SALGADbContext _dbContext;

        public SQLAuditingRepository(SALGADbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<List<AuditEvent>> GetAuditEvents(DateTime dateStart, DateTime dateEnd)
        {
            var lstAuditEvents = await _dbContext.AuditEvents.Where(x => x.Date >= dateStart && x.Date <= dateEnd).ToListAsync();
            return lstAuditEvents;
        }

        public async Task<List<AuditEvent>> GetAuditEvents(DateTime dateStart, DateTime dateEnd, String municipalityName)
        {
            var lstAuditEvents = await _dbContext.AuditEvents.Where(x => x.Date >= dateStart && x.Date <= dateEnd && x.ItemName==municipalityName).ToListAsync();
            return lstAuditEvents;
        }


        public async Task<AuditEvent> AddLoginEvent(IdentityUser User, String municipalityName)
        {
            return await AddUserEvent(User, "Login", municipalityName, null);
        }
        public async Task<AuditEvent> AddRegistrationEvent(IdentityUser User, String municipalityName)
        {
            return await AddUserEvent(User, "Registered", municipalityName, null);
        }
        public async Task<AuditEvent> AddAssessmentModifiedEvent(IdentityUser User, String municipalityName,String sectionName)
        {
            return await AddUserEvent(User, "Assessment Modified", municipalityName, sectionName);
        }

        public async Task<AuditEvent> AddAssessmentSubmittedEvent(IdentityUser User, String municipalityName)
        {
            return await AddUserEvent(User, "Assessment Submitted", municipalityName, null);
        }
        public async Task<AuditEvent> AddAssessmentRejectedEvent(IdentityUser User, String municipalityName)
        {
            return await AddUserEvent(User, "Assessment Rejected", municipalityName, null);
        }

        
        public async Task<AuditEvent> AddAssessmentApprovedEvent(IdentityUser User, String municipalityName)
        {
            return await AddUserEvent(User, "Assessment Approved", municipalityName, null);
        }


        public async Task<AuditEvent> AddCustomEvent(AuditEvent auditItem)
        {
            _dbContext.AuditEvents.Add(auditItem);
            await _dbContext.SaveChangesAsync();
            return auditItem;
        }

        public async Task<AnswerEvidence> GetAnswerEvidence(int evidenceID)
        {
            return await _dbContext.AnswerEvidences.FirstOrDefaultAsync(x => x.pkID == evidenceID);
        }

        private async Task<AuditEvent> AddUserEvent(IdentityUser User, String eventType, String itemName, String itemPath )
        {
            var auditItem = new AuditEvent()
            {
                Date = DateTime.Now,
                EventType = eventType,
                UserEmail = User.Email,
                UserIDString = User.Id,
                ItemName = itemName,
                ItemPath=itemPath
            };

            _dbContext.AuditEvents.Add(auditItem);
            await _dbContext.SaveChangesAsync();

            return auditItem;
        }


    }
}
