using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace SALGADBLib
{
    public class SALGADbContext : IdentityDbContext
    {
        public DbSet<IntervieweeDetails> IntervieweeDetails { get; set; }
        public DbSet<MunicipalityDemographics> MunicipalityDemographics { get; set; }
        public DbSet<JobTitle> JobTitles { get; set; }
        public DbSet<Municipality> Municipalities { get; set; }
        public DbSet<HRMDFunctionDemographics> HRMDFunctionDemographics { get; set; }
        public DbSet<ServiceOption> ServiceOptions { get; set; }
        public DbSet<OtherInstitution> OtherInstitutions { get; set; }
        public DbSet<QuestionnaireCategory> QuestionnaireCategories { get; set; }
        public DbSet<QuestionnaireQuestion> QuestionnaireQuestions { get; set; }
        public DbSet<QuestionnaireResponseType> QuestionnaireResponseTypes { get; set; }
        public DbSet<QuestionnaireQuestionAnswer> QuestionnaireQuestionAnswers { get; set; }
        public DbSet<MunicipalCatagory> MunicipalCatagories { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<AssessmentTracking> AssessmentTrackings { get; set; }

        public DbSet<QuestionAnswerNotes> QuestionAnswerNotes { get; set; }

        public DbSet<CustomReponseType> CustomReponseTypes { get; set; }
        
        public DbSet<AuditEvent>  AuditEvents { get; set; }

        public DbSet<AnswerEvidence> AnswerEvidences { get; set; }

        public DbSet<QuestionSet> QuestionSets { get; set; }

        public DbSet<SALGAConfig> SALGAConfigs { get; set; }
        public DbSet<SeniorManager> SeniorManagers { get; set; }
        public DbSet<SeniorManagerPosition> SeniorManagerPositions { get; set; }

        public DbSet<DasboardLevelRole> DasboardLevelRoles { get; set; }

        public DbSet<MunicipalityAssessmentConfig> MunicipalityAssessmentConfigs { get; set; }

        public DbSet<DasboardProvinceAccess> DasboardProvinceAccesses { get; set; }
        public DbSet<MunicipalityOrganogram> MunicipalityOrganograms { get; set; }

        public SALGADbContext(DbContextOptions<SALGADbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DasboardProvinceAccess>(entity =>
            {
                entity.HasKey(e => e.pkID);
                entity.HasOne(e => e.Province);
                entity.HasOne(e => e.Role);
            });

            modelBuilder.Entity<DasboardLevelRole>(entity =>
            {
                entity.HasKey(e => e.pkID);

            });

            modelBuilder.Entity<QuestionSet>(entity =>
            {
                entity.HasKey(e => e.pkID);

            });

            modelBuilder.Entity<MunicipalityAssessmentConfig>(entity =>
            {
                entity.HasKey(e => e.pkID);
                entity.HasOne(e => e.Municipality);
            });

            modelBuilder.Entity<SALGAConfig>(entity =>
            {
                entity.HasKey(e => e.pkID);
            });

            modelBuilder.Entity<Province>(entity =>
            {
                entity.HasKey(e => e.pkID);

            });

            modelBuilder.Entity<AnswerEvidence>(entity =>
            {
                entity.HasKey(e => e.pkID);
                entity.HasOne(e => e.Municipality);

            });

            modelBuilder.Entity<AuditEvent>(entity =>
            {
                entity.HasKey(e => e.pkID);

            });


            modelBuilder.Entity<QuestionAnswerNotes>(entity =>
            {
                entity.HasKey(e => e.pkID);

            });

            modelBuilder.Entity<CustomReponseType>(entity =>
            {
                entity.HasKey(e => e.pkID);

            });

            modelBuilder.Entity<MunicipalityOrganogram>(entity =>
            {
                entity.HasKey(e => e.pkID);

            });

            modelBuilder.Entity<AssessmentTracking>(entity =>
            {
                entity.HasKey(e => e.pkID);
                entity.HasOne(e => e.Approver);
                entity.HasOne(e => e.User);
                entity.HasOne(e => e.Municipality);
            });

            modelBuilder.Entity<MunicipalityDemographics>(entity =>
            {
                entity.HasKey(e => e.pkID);
                entity.HasOne(e => e.Capturer);
                entity.HasOne(e => e.Approver);
                entity.HasOne(e => e.Municipality);
            });

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IntervieweeDetails>(entity =>
            {
                entity.HasKey(e => e.pkID);
                entity.HasOne(x => x.User);
                entity.HasOne(x => x.Municipality);
                entity.HasOne(x => x.JobTitle);
            });

            modelBuilder.Entity<QuestionnaireQuestionAnswer>(entity =>
            {
                entity.HasKey(e => e.pkID);
                entity.HasOne(e => e.ResponseType);
                entity.HasOne(e => e.Question);
                entity.HasOne(e => e.User);
                entity.HasOne(e => e.Tracking);
                entity.HasOne(e => e.Municipality);

            });

            modelBuilder.Entity<QuestionnaireResponseType>(entity =>
            {
                entity.HasKey(e => e.pkID);

            });


            modelBuilder.Entity<MunicipalCatagory>(entity =>
            {
                entity.HasKey(e => e.pkID);

            });

            modelBuilder.Entity<HRMDFunctionDemographics>(entity =>
            {
                entity.HasKey(e => e.pkID);
                entity.HasOne(e => e.Municipality);
                entity.HasOne(e => e.MunicipalityOrganogram);
            });


            modelBuilder.Entity<JobTitle>(entity =>
            {
                entity.HasKey(e => e.pkID);

            });

            modelBuilder.Entity<Municipality>(entity =>
            {
                entity.HasKey(e => e.pkID);
                entity.HasOne(e => e.MunicipalCatagory);
                entity.HasOne(e => e.Province);
                entity.HasOne(e => e.District);
            });

            modelBuilder.Entity<ServiceOption>(entity =>
            {
                entity.HasKey(e => e.pkID);

            });

            modelBuilder.Entity<OtherInstitution>(entity =>
            {
                entity.HasKey(e => e.pkID);

            });

            modelBuilder.Entity<QuestionnaireCategory>(entity =>
            {
                entity.HasKey(e => e.pkID);

            });

            modelBuilder.Entity<QuestionnaireQuestion>(entity =>
            {
                entity.HasKey(e => e.pkID);
                entity.HasOne(e => e.QuestionSet);
            });


            modelBuilder.Entity<SeniorManager>(entity =>
            {
                entity.HasKey(e => e.pkID);

            });

            modelBuilder.Entity<SeniorManagerPosition>(entity =>
            {
                entity.HasKey(e => e.pkID);
                entity.HasOne(e => e.MunicipalityDemographics);
                entity.HasOne(e => e.SeniorManager);
            });
        }
    }
}
