using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SALGADBLib
{
    public class SQLAssessmentRepository : IAssessmentRepository
    {
        private SALGADbContext _dbContext;

        public async Task<SALGAConfig> GetConfig()
        {
            return await _dbContext.SALGAConfigs.FirstOrDefaultAsync();
        }

        public SQLAssessmentRepository(SALGADbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<QuestionnaireCategory>> GetPageInfo()
        {
            var dbPages = await _dbContext.QuestionnaireCategories.ToListAsync();
            return dbPages;
        }


        public async Task<IEnumerable<QuestionnaireQuestion>> GetQuestions()
        {
            var dbQuestions = await _dbContext.QuestionnaireQuestions.ToListAsync();
            return dbQuestions;
        }


        public async Task<IEnumerable<QuestionnaireResponseType>> GetResponseTypes()
        {
            var dbResponseTypes = await _dbContext.QuestionnaireResponseTypes.ToListAsync();
            return dbResponseTypes;
        }
        public async Task<IEnumerable<CustomReponseType>> GetCustomResponseTypes()
        {
            var dbResponseTypes = await _dbContext.CustomReponseTypes.ToListAsync();
            return dbResponseTypes;
        }
        public async Task<IEnumerable<QuestionAnswerNotes>> GetQuestionAnswerNotes()
        {
            var notes = await _dbContext.QuestionAnswerNotes.ToListAsync();
            return notes;
        }

        public async Task<IEnumerable<MunicipalityAssessmentConfig>> AddAssessmentConfigs(IEnumerable<MunicipalityAssessmentConfig> municipalityAssessmentConfigs)
        {
            var municpalityIDs = municipalityAssessmentConfigs.Select(x => x.Municipality.pkID).ToList();
            var deleteItems = await _dbContext.MunicipalityAssessmentConfigs.Where(x => municpalityIDs.Contains(x.Municipality.pkID)).ToArrayAsync();
            _dbContext.MunicipalityAssessmentConfigs.RemoveRange(deleteItems);
            await _dbContext.SaveChangesAsync();

            _dbContext.MunicipalityAssessmentConfigs.AddRange(municipalityAssessmentConfigs);
            await _dbContext.SaveChangesAsync();
            return municipalityAssessmentConfigs;
        }

        public async Task<MunicipalityAssessmentConfig> GetAssessmentConfig(Municipality municipality)
        {
            var municpalityConfig = await _dbContext.MunicipalityAssessmentConfigs.FirstOrDefaultAsync(x => x.Municipality == municipality);
            return municpalityConfig;
        }

        public async Task<IEnumerable<MunicipalityAssessmentConfig>> GetMunicipalityAssessmentConfigs()
        {
            var municpalityConfigs = await _dbContext.MunicipalityAssessmentConfigs.ToListAsync();
            return municpalityConfigs;
        }

        public async Task<IEnumerable<QuestionnaireQuestionAnswer>> AddQuestionAnswers(IEnumerable<QuestionnaireQuestionAnswer> questionnaireQuestionAnswers, AssessmentTracking assessmentTracking)
        {
            questionnaireQuestionAnswers.ToList().ForEach(x => x.Tracking = assessmentTracking);
            _dbContext.QuestionnaireQuestionAnswers.AddRange(questionnaireQuestionAnswers);
            await _dbContext.SaveChangesAsync();
            return questionnaireQuestionAnswers;
        }

        public async Task<IEnumerable<QuestionnaireQuestionAnswer>> GetQuestionnaireQuestionAnswers(int auditYear)
        {
            var allAnswers = await _dbContext.QuestionnaireQuestionAnswers.Include(x => x.Question) .Include(x => x.Tracking).Include(x => x.Municipality)
                                        .Include(x => x.Municipality.Province).Where(x => x.Tracking.AuditYear == auditYear).ToListAsync();
            return allAnswers;
        }
        public async Task<IEnumerable<QuestionnaireQuestionAnswer>> GetQuestionnaireQuestionAnswers(AssessmentTracking assessmentTracking)
        {
            var answers = await _dbContext.QuestionnaireQuestionAnswers.Include(x => x.Question).Include(x=>x.ResponseType)
                                .Include(x=>x.Municipality).Include(x=>x.Tracking).Include(x=>x.AnswerEvidence).
                                Where(x => x.Tracking == assessmentTracking).ToListAsync();
            return answers;
        }

        public async Task<IEnumerable<QuestionnaireQuestionAnswer>> GetQuestionnaireQuestionAnswers(IEnumerable<AssessmentTracking> assessmentTrackings)
        {
            var trackingIDS = assessmentTrackings.Select(x => x.pkID).ToList();

            var answers = await _dbContext.QuestionnaireQuestionAnswers.Include(x => x.Question).Include(x => x.ResponseType)
                    .Include(x => x.Municipality).Include(x => x.Tracking).Include(x => x.AnswerEvidence).
                    Where(x => trackingIDS.Contains(x.Tracking.pkID)).ToListAsync();

            return answers;
        }

        public async Task<IEnumerable<QuestionnaireQuestionAnswer>> DeleteQuestionannaireAnswers(AssessmentTracking assessmentTracking, int SectionNo)
        {
            var answers = await _dbContext.QuestionnaireQuestionAnswers.Include(x => x.Question).Include(x=>x.AnswerEvidence)
                                        .Where(x => x.Tracking == assessmentTracking && x.Question.PageNo == SectionNo).ToListAsync();
            var evidences = answers.Select(x => x.AnswerEvidence).Where(x=>x!=null).ToList();
            _dbContext.AnswerEvidences.RemoveRange(evidences);
            _dbContext.QuestionnaireQuestionAnswers.RemoveRange(answers);
            await _dbContext.SaveChangesAsync();
            return answers;
        }


        public async Task<IEnumerable<QuestionnaireQuestionAnswer>> SaveReviewedAnswers(IEnumerable<QuestionnaireQuestionAnswer> questionnaireQuestionAnswers)
        {
            var answerIDs = questionnaireQuestionAnswers.Select(x => x.pkID).ToList();
            var dbAnswers = _dbContext.QuestionnaireQuestionAnswers.Where(x => answerIDs.Contains(x.pkID)).ToList();
            foreach (var dbAnswer in dbAnswers)
            {
                var answer = questionnaireQuestionAnswers.FirstOrDefault(x => x.pkID == dbAnswer.pkID);
                dbAnswer.ApproverComments = answer.ApproverComments;
                dbAnswer.SALGAComments = answer.SALGAComments;
                dbAnswer.ResponseType = answer.ResponseType;
                dbAnswer.CustomResponse = answer.CustomResponse;
            }
            await _dbContext.SaveChangesAsync();
            return dbAnswers;
        }

        public async Task<IEnumerable<AssessmentTracking>> GetAssessmentTrackings(Municipality municipality)
        {
            var trackings = await(_dbContext.AssessmentTrackings.Include(x => x.Municipality).Include(x => x.Municipality.Province).
                Where(x => x.Municipality == municipality).ToListAsync());
            return trackings;
        }

        public async Task<IEnumerable<AssessmentTracking>> GetAssessmentTrackings(int auditYear)
        {
            var trackings =  await (_dbContext.AssessmentTrackings.Include(x=>x.Municipality).Include(x=>x.Municipality.Province).
                            Where(x => x.AuditYear == auditYear).ToListAsync());
            return trackings;

        }

        async Task<IEnumerable<AssessmentTracking>> IAssessmentRepository.GetAssessmentTrackings()
        {
            var trackings = await (_dbContext.AssessmentTrackings.Include(x => x.Municipality).Include(x => x.Municipality.Province).ToListAsync());
            return trackings;
        }



        public async Task<AssessmentTracking> CreateAssessmentTracking(IdentityUser user, Municipality municipality,int auditYear)
        {

            var assessmentTracking = new AssessmentTracking()
            {
                SubmittedDate = DateTime.Now,
                User = user,
                AuditYear = auditYear,
                Municipality = municipality
            };

            _dbContext.AssessmentTrackings.Add(assessmentTracking);
            await _dbContext.SaveChangesAsync();
            return assessmentTracking;
        }

        public async Task<AssessmentTracking> UpdateAssesmentTracking(AssessmentTracking assessmentTracking)
        {
            _dbContext.AssessmentTrackings.Add(assessmentTracking);
            _dbContext.Entry(assessmentTracking).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return assessmentTracking;
        }

        public async Task<AssessmentTracking> GetAssessmentTracking(Municipality municipality, int auditYear)
        {
            var trackingObj = await _dbContext.AssessmentTrackings.Include(x=>x.User).Include(x=>x.Municipality)
                                         .FirstOrDefaultAsync(x => x.Municipality == municipality && x.AuditYear == auditYear);
            return trackingObj;
        }

        public async Task<AssessmentTracking> GetAssessmentTracking(IdentityUser capturerUser, int auditYear)
        {
            var trackingObj = await _dbContext.AssessmentTrackings.Include(x=>x.User).Include(x=>x.Municipality)
                                     .FirstOrDefaultAsync(x => x.User == capturerUser && x.AuditYear==auditYear);
            return trackingObj;
        }

        public async Task<IEnumerable<AnswerEvidence>> FindEvidenceDocuments(String partialName, int municipalityID)
        {
            var answers = await _dbContext.AnswerEvidences.Where(x => x.OriginalFileName.Contains(partialName) && x.Municipality.pkID==municipalityID).Include(x => x.Municipality).ToListAsync();
            return answers;
        }

        public async Task<IEnumerable<AnswerEvidence>> FindEvidenceDocuments(String partialName)
        {
            var answers = await _dbContext.AnswerEvidences.Where(x=>x.OriginalFileName.Contains(partialName)).Include(x => x.Municipality).ToListAsync();
            return answers;
        }

        public async Task<AnswerEvidence> GetEvidence(int evidenceID)
        {
            var answerEvidence = await _dbContext.AnswerEvidences.Include(x => x.Municipality).FirstOrDefaultAsync(x => x.pkID == evidenceID);
            return answerEvidence;
        }

        public async Task<AnswerEvidence> DeleteEvidence(int evidenceID)
        {
            var answerEvidence = await _dbContext.AnswerEvidences.Include(x => x.Municipality).FirstOrDefaultAsync(x => x.pkID == evidenceID);
            _dbContext.Remove(answerEvidence);
            return answerEvidence;
        }

        public async Task<string> GetTechnologyInUse(Municipality municipality, int AuditYear)
        {
            var config =await  _dbContext.SALGAConfigs.FirstOrDefaultAsync();
            var technologyAnswer=  await _dbContext.QuestionnaireQuestionAnswers
                                    .Include(x => x.Tracking).Where(x => x.Municipality == municipality && x.Tracking.AuditYear == AuditYear && x.Question.QuestionNo==config.TechnologyQuestion).FirstOrDefaultAsync();
            if (technologyAnswer != null)
                return technologyAnswer.CustomResponse;
            return "";
        }


    }
}
