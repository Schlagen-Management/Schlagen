using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Schlagen.Data.EntityClasses;

namespace Schlagen.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<EmploymentLocation> EmploymentLocations { get; set; }
        public DbSet<EmploymentType> EmploymentTypes { get; set; }
        public DbSet<JobApplicant> JobApplicants { get; set; }
        public DbSet<JobRequisition> JobRequisitions { get; set; }
        public DbSet<JobType> JobTypes { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //
            // EMployment Locations
            //
            builder.Entity<EmploymentLocation>().HasKey(el => el.EmploymentLocationId);
            builder.Entity<EmploymentLocation>().ToTable("EmploymentLocations", "5thFloor");
            builder.Entity<EmploymentLocation>().Property(el => el.Name)
                .IsRequired().HasMaxLength(100);
            builder.Entity<EmploymentLocation>().Property(el => el.StreetAddress)
                .HasMaxLength(100);
            builder.Entity<EmploymentLocation>().Property(el => el.City)
                .HasMaxLength(100);
            builder.Entity<EmploymentLocation>().Property(el => el.State)
                .HasMaxLength(100);
            builder.Entity<EmploymentLocation>().Property(el => el.PostalCode)
                .HasMaxLength(25);
            builder.Entity<EmploymentLocation>().Property(el => el.Latitude)
                .HasColumnType("Decimal(9,6)");
            builder.Entity<EmploymentLocation>().Property(el => el.Longitude)
                .HasColumnType("Decimal(9,6)");
            builder.Entity<EmploymentLocation>().HasData(
                new EmploymentLocation() { EmploymentLocationId = 1, Name = "Orlando" });

            //
            // Employment Type
            //
            builder.Entity<EmploymentType>().HasKey(et => et.EmploymentTypeId);
            builder.Entity<EmploymentType>().ToTable("EmploymentTypes", "5thFloor");
            builder.Entity<EmploymentType>().Property(et => et.Name)
                .IsRequired().HasMaxLength(100);
            builder.Entity<EmploymentType>().HasData(
                new EmploymentType() { EmploymentTypeId = 1, Name = "Consultant" },
                new EmploymentType() { EmploymentTypeId = 2, Name = "Employee" });

            //
            // Job Type
            //
            builder.Entity<JobType>().HasKey(jt => jt.JobTypeId);
            builder.Entity<JobType>().ToTable("JobTypes", "5thFloor");
            builder.Entity<JobType>().Property(et => et.Name)
                .IsRequired().HasMaxLength(100);
            builder.Entity<JobType>().HasData(
                new JobType() { JobTypeId = 1, Name = "Part Time" },
                new JobType() { JobTypeId = 2, Name = "Full Time" });

            //
            // Job Requisition
            //
            builder.Entity<JobRequisition>().HasKey(jr => jr.JobRequisitionId);
            builder.Entity<JobRequisition>().ToTable("JobRequisitions", "5thFloor");
            builder.Entity<JobRequisition>().Property(jr => jr.Description)
                .IsRequired().HasMaxLength(1000);
            builder.Entity<JobRequisition>().Property(jr => jr.Title)
                .IsRequired().HasMaxLength(200);
            builder.Entity<JobRequisition>().Property(jr => jr.DateToPost)
                .IsRequired();
            builder.Entity<JobRequisition>().HasOne(jr => jr.JobType)
                .WithMany()
                .HasForeignKey(jr => jr.JobTypeId);
            builder.Entity<JobRequisition>().HasOne(jr => jr.Location)
                .WithMany()
                .HasForeignKey(jr => jr.EmploymentLocationId);
            builder.Entity<JobRequisition>().HasOne(jr => jr.EmploymentType)
                .WithMany()
                .HasForeignKey(jr => jr.EmploymentTypeId);
            builder.Entity<JobRequisition>().HasMany(jr => jr.Applicants);

            //
            // Job Applicant
            //
            builder.Entity<JobApplicant>().HasKey(ja => ja.JobApplicantId);
            builder.Entity<JobApplicant>().ToTable("JobApplicants", "5thFloor");
            builder.Entity<JobApplicant>().Property(ja => ja.FirstName)
                .IsRequired().HasMaxLength(100);
            builder.Entity<JobApplicant>().Property(ja => ja.LastName)
                .IsRequired().HasMaxLength(100);
            builder.Entity<JobApplicant>().Property(ja => ja.PhoneNumber)
                .IsRequired().HasMaxLength(50);
            builder.Entity<JobApplicant>().Property(ja => ja.EmailAddress)
                .IsRequired().HasMaxLength(100);
            builder.Entity<JobApplicant>().Property(ja => ja.ResumeText)
                .IsRequired().HasMaxLength(20000);
            builder.Entity<JobApplicant>().HasOne(ja => ja.JobRequisition)
                .WithMany()
                .HasForeignKey(ja => ja.JobRequisitionId);
        }
    }
}
