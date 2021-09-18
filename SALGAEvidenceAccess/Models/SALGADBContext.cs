using Microsoft.EntityFrameworkCore;
using SALGADBLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace SALGAEvidenceAccess
{
    public class SALGADBContext :DbContext
    {

        public  DbSet<AnswerEvidence> AnswerEvidences { get; set; }
        public DbSet<AuditEvent> AuditEvents { get; set; }

        public DbSet<Municipality> Municipalities { get; set; }

        public SALGADBContext(DbContextOptions<SALGADBContext> options)
           : base(options)
        {

 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AuditEvent>(entity =>
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

            modelBuilder.Entity<MunicipalCatagory>(entity =>
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

        }
    }
}
