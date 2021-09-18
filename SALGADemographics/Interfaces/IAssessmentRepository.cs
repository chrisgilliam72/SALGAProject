using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SALGADBLib
{
    public interface IAssessmentRepository
    {

        public Task<IEnumerable<MunicipalityAssessmentConfig>> AddAssessmentConfigs(IEnumerable<MunicipalityAssessmentConfig> municipalityAssessmentConfigs);
        public Task<IEnumerable<MunicipalityAssessmentConfig>> GetMunicipalityAssessmentConfigs();
        public Task<MunicipalityAssessmentConfig> GetAssessmentConfig(Municipality municipality);
        public Task<String> GetTechnologyInUse(Municipality municipality, int AuditYear);
        public Task<IEnumerable<QuestionnaireCategory>> GetPageInfo();
        public Task<IEnumerable<QuestionnaireQuestion>> GetQuestions();
        public Task<IEnumerable<QuestionnaireResponseType>> GetResponseTypes();
        public  Task<IEnumerable<CustomReponseType>> GetCustomResponseTypes();
        public Task<IEnumerable<QuestionAnswerNotes>> GetQuestionAnswerNotes();
        public Task<IEnumerable<QuestionnaireQuestionAnswer>> GetQuestionnaireQuestionAnswers(int auditYear);
        public Task<IEnumerable<QuestionnaireQuestionAnswer>> GetQuestionnaireQuestionAnswers(AssessmentTracking assessmentTracking);
        public Task<IEnumerable<QuestionnaireQuestionAnswer>> GetQuestionnaireQuestionAnswers(IEnumerable<AssessmentTracking> assessmentTrackings);
        public Task<IEnumerable<QuestionnaireQuestionAnswer>> SaveReviewedAnswers(IEnumerable<QuestionnaireQuestionAnswer> questionnaireQuestionAnswers);
        public Task<IEnumerable<QuestionnaireQuestionAnswer>> AddQuestionAnswers(IEnumerable<QuestionnaireQuestionAnswer> questionnaireQuestionAnswers, AssessmentTracking assessmentTracking);
        public Task<IEnumerable<QuestionnaireQuestionAnswer>> DeleteQuestionannaireAnswers(AssessmentTracking assessmentTracking, int SectionNo);

        public Task<IEnumerable<AssessmentTracking>> GetAssessmentTrackings(int auditYear);
        public Task<IEnumerable<AssessmentTracking>> GetAssessmentTrackings();
        public Task<IEnumerable<AssessmentTracking>> GetAssessmentTrackings(Municipality municipality);
        public Task<AssessmentTracking> CreateAssessmentTracking(IdentityUser user, Municipality municipality, int auditYear);

        public Task<AssessmentTracking> GetAssessmentTracking(Municipality municipality, int auditYear);
        public Task<AssessmentTracking> GetAssessmentTracking(IdentityUser capturerUser,int auditYear);

        public Task<AssessmentTracking> UpdateAssesmentTracking(AssessmentTracking assessmentTracking);
        public Task<IEnumerable<AnswerEvidence>> FindEvidenceDocuments(String partialName, int municipalityID);

        public Task<IEnumerable<AnswerEvidence>> FindEvidenceDocuments(String partialName);
        public Task<AnswerEvidence> GetEvidence(int evidenceID);
        public Task<AnswerEvidence> DeleteEvidence(int evidenceID);

    }
}
