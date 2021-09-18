using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunctionsSharedModelLib
{
    public class SALGADBContext : IdentityDbContext
    {


        public DbSet<MunicipalityAssessmentConfig> MunicipalityAssessmentConfigs { get; set; }

        public DbSet<AssessmentTracking> AssessmentTrackings { get; set; }

        public DbSet<Municipality> Municipalities { get; set; }

        public SALGADBContext(DbContextOptions<SALGADBContext> options)
           : base(options)
        {

 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<AssessmentTracking>(entity =>
            {
                entity.HasKey(e => e.pkID);
                entity.HasOne(e => e.Approver);
                entity.HasOne(e => e.User);
                entity.HasOne(e => e.Municipality);
            });


            modelBuilder.Entity<Municipality>(entity =>
            {
                entity.HasKey(e => e.pkID);
                entity.HasOne(e => e.District);
            });

            modelBuilder.Entity<MunicipalityAssessmentConfig>(entity =>
            {
                entity.HasKey(e => e.pkID);
                entity.HasOne(e => e.Municipality);
            });

        }
    }
}
