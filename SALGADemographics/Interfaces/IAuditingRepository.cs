using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SALGADBLib
{
    public interface IAuditingRepository
    {
        public Task<List<AuditEvent>> GetAuditEvents(DateTime dateStart, DateTime dateEnd);
        public Task<List<AuditEvent>> GetAuditEvents(DateTime dateStart, DateTime dateEnd, String municipalityName);

        public Task<AuditEvent> AddLoginEvent(IdentityUser User, String municipalityName);
        public Task<AuditEvent> AddRegistrationEvent(IdentityUser User, String municipalityName);
        public Task<AuditEvent> AddAssessmentModifiedEvent(IdentityUser User,String municipalityName, String sectionName);
        public Task<AuditEvent> AddAssessmentSubmittedEvent(IdentityUser User, String municipalityName);
        public Task<AuditEvent> AddAssessmentApprovedEvent(IdentityUser User, String municipalityName);
        public Task<AuditEvent> AddAssessmentRejectedEvent(IdentityUser User, String municipalityName);
        public Task<AuditEvent> AddCustomEvent(AuditEvent itemAuditTracking);
        public Task<AnswerEvidence> GetAnswerEvidence(int evidenceID);
    }
}
