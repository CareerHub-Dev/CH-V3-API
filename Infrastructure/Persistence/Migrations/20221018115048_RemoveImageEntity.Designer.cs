﻿// <auto-generated />
using System;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20221018115048_RemoveImageEntity")]
    partial class RemoveImageEntity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CompanyStudent", b =>
                {
                    b.Property<Guid>("CompanySubscriptionsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SubscribedStudentsId")
                        .HasColumnType("uuid");

                    b.HasKey("CompanySubscriptionsId", "SubscribedStudentsId");

                    b.HasIndex("SubscribedStudentsId");

                    b.ToTable("CompanyStudent");
                });

            modelBuilder.Entity("CVJobOffer", b =>
                {
                    b.Property<Guid>("AppliedCVsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TargetJobOffersId")
                        .HasColumnType("uuid");

                    b.HasKey("AppliedCVsId", "TargetJobOffersId");

                    b.HasIndex("TargetJobOffersId");

                    b.ToTable("CVJobOffer");
                });

            modelBuilder.Entity("Domain.Entities.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("ActivationStatus")
                        .HasColumnType("integer");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedEmail")
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasComputedColumnSql("LOWER(TRIM(\"Email\"))", true);

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("PasswordReset")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ResetToken")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ResetTokenExpires")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("VerificationToken")
                        .HasColumnType("text");

                    b.Property<DateTime?>("Verified")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Domain.Entities.CompanyLink", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("Uri")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("CompanyLinks");
                });

            modelBuilder.Entity("Domain.Entities.CV", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ExperienceHighlights")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Goals")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)");

                    b.Property<Guid>("JobPositionId")
                        .HasColumnType("uuid");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Photo")
                        .HasColumnType("text");

                    b.Property<string>("SkillsAndTechnologies")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)");

                    b.Property<Guid>("StudentId")
                        .HasColumnType("uuid");

                    b.Property<int>("TemplateLanguage")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.HasKey("Id");

                    b.HasIndex("JobPositionId");

                    b.HasIndex("StudentId");

                    b.ToTable("CVs");
                });

            modelBuilder.Entity("Domain.Entities.CVProjectLink", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CVId")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CVId");

                    b.ToTable("CVProjectLinks");
                });

            modelBuilder.Entity("Domain.Entities.Education", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CVId")
                        .HasColumnType("uuid");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<int>("Degree")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("End")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Specialty")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime>("Start")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("University")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.HasKey("Id");

                    b.HasIndex("CVId");

                    b.ToTable("Educations");
                });

            modelBuilder.Entity("Domain.Entities.Experience", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ExperienceLevel")
                        .HasColumnType("integer");

                    b.Property<string>("JobLocation")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<int>("JobType")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("StudentId")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<int>("WorkFormat")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("StudentId");

                    b.ToTable("Experiences");
                });

            modelBuilder.Entity("Domain.Entities.ForeignLanguage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CVId")
                        .HasColumnType("uuid");

                    b.Property<int>("LanguageLevel")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.HasKey("Id");

                    b.HasIndex("CVId");

                    b.ToTable("ForeignLanguages");
                });

            modelBuilder.Entity("Domain.Entities.JobOffer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ExperienceLevel")
                        .HasColumnType("integer");

                    b.Property<string>("Image")
                        .HasColumnType("text");

                    b.Property<Guid>("JobPositionId")
                        .HasColumnType("uuid");

                    b.Property<int>("JobType")
                        .HasColumnType("integer");

                    b.Property<string>("Overview")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)");

                    b.Property<string>("Preferences")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)");

                    b.Property<string>("Requirements")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)");

                    b.Property<string>("Responsibilities")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<int>("WorkFormat")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("JobPositionId");

                    b.ToTable("JobOffers");
                });

            modelBuilder.Entity("Domain.Entities.JobPosition", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("LastModifiedBy")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.HasKey("Id");

                    b.ToTable("JobPositions");
                });

            modelBuilder.Entity("Domain.Entities.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedByIp")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ReasonRevoked")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ReplacedByToken")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("Revoked")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("RevokedByIp")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("Domain.Entities.StudentGroup", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("LastModifiedBy")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.HasKey("Id");

                    b.ToTable("StudentGroups");
                });

            modelBuilder.Entity("Domain.Entities.StudentLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("LastModifiedBy")
                        .HasColumnType("uuid");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.Property<string>("NormalizedEmail")
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasComputedColumnSql("LOWER(TRIM(\"Email\"))", true);

                    b.Property<Guid>("StudentGroupId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail");

                    b.HasIndex("StudentGroupId");

                    b.ToTable("StudentLogs");
                });

            modelBuilder.Entity("Domain.Entities.StudentSubscription", b =>
                {
                    b.Property<Guid>("SubscriptionOwnerId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SubscriptionTargetId")
                        .HasColumnType("uuid");

                    b.HasKey("SubscriptionOwnerId", "SubscriptionTargetId");

                    b.HasIndex("SubscriptionTargetId");

                    b.ToTable("StudentSubscriptions");
                });

            modelBuilder.Entity("Domain.Entities.Tag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsAccepted")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("LastModifiedBy")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("JobOfferStudent", b =>
                {
                    b.Property<Guid>("JobOfferSubscriptionsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SubscribedStudentsId")
                        .HasColumnType("uuid");

                    b.HasKey("JobOfferSubscriptionsId", "SubscribedStudentsId");

                    b.HasIndex("SubscribedStudentsId");

                    b.ToTable("JobOfferStudent");
                });

            modelBuilder.Entity("JobOfferTag", b =>
                {
                    b.Property<Guid>("JobOffersId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TagsId")
                        .HasColumnType("uuid");

                    b.HasKey("JobOffersId", "TagsId");

                    b.HasIndex("TagsId");

                    b.ToTable("JobOfferTag");
                });

            modelBuilder.Entity("Domain.Entities.Admin", b =>
                {
                    b.HasBaseType("Domain.Entities.Account");

                    b.Property<bool>("IsSuperAdmin")
                        .HasColumnType("boolean");

                    b.ToTable("Admins", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Company", b =>
                {
                    b.HasBaseType("Domain.Entities.Account");

                    b.Property<string>("Banner")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)");

                    b.Property<string>("Logo")
                        .HasColumnType("text");

                    b.Property<string>("Motto")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.ToTable("Companies", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Student", b =>
                {
                    b.HasBaseType("Domain.Entities.Account");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.Property<string>("Phone")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.Property<string>("Photo")
                        .HasColumnType("text");

                    b.Property<Guid>("StudentGroupId")
                        .HasColumnType("uuid");

                    b.HasIndex("StudentGroupId");

                    b.ToTable("Students", (string)null);
                });

            modelBuilder.Entity("CompanyStudent", b =>
                {
                    b.HasOne("Domain.Entities.Company", null)
                        .WithMany()
                        .HasForeignKey("CompanySubscriptionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Student", null)
                        .WithMany()
                        .HasForeignKey("SubscribedStudentsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CVJobOffer", b =>
                {
                    b.HasOne("Domain.Entities.CV", null)
                        .WithMany()
                        .HasForeignKey("AppliedCVsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.JobOffer", null)
                        .WithMany()
                        .HasForeignKey("TargetJobOffersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.CompanyLink", b =>
                {
                    b.HasOne("Domain.Entities.Company", "Company")
                        .WithMany("Links")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Domain.Entities.CV", b =>
                {
                    b.HasOne("Domain.Entities.JobPosition", "JobPosition")
                        .WithMany("CVs")
                        .HasForeignKey("JobPositionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Student", "Student")
                        .WithMany("CVs")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("JobPosition");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Domain.Entities.CVProjectLink", b =>
                {
                    b.HasOne("Domain.Entities.CV", "CV")
                        .WithMany("ProjectLinks")
                        .HasForeignKey("CVId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CV");
                });

            modelBuilder.Entity("Domain.Entities.Education", b =>
                {
                    b.HasOne("Domain.Entities.CV", "CV")
                        .WithMany("Educations")
                        .HasForeignKey("CVId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CV");
                });

            modelBuilder.Entity("Domain.Entities.Experience", b =>
                {
                    b.HasOne("Domain.Entities.Student", "Student")
                        .WithMany("Experiences")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Domain.Entities.ForeignLanguage", b =>
                {
                    b.HasOne("Domain.Entities.CV", "CV")
                        .WithMany("ForeignLanguages")
                        .HasForeignKey("CVId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CV");
                });

            modelBuilder.Entity("Domain.Entities.JobOffer", b =>
                {
                    b.HasOne("Domain.Entities.Company", "Company")
                        .WithMany("JobOffers")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.JobPosition", "JobPosition")
                        .WithMany("JobOffers")
                        .HasForeignKey("JobPositionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");

                    b.Navigation("JobPosition");
                });

            modelBuilder.Entity("Domain.Entities.RefreshToken", b =>
                {
                    b.HasOne("Domain.Entities.Account", "Account")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("Domain.Entities.StudentLog", b =>
                {
                    b.HasOne("Domain.Entities.StudentGroup", "StudentGroup")
                        .WithMany()
                        .HasForeignKey("StudentGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("StudentGroup");
                });

            modelBuilder.Entity("Domain.Entities.StudentSubscription", b =>
                {
                    b.HasOne("Domain.Entities.Student", "SubscriptionOwner")
                        .WithMany("StudentSubscriptions")
                        .HasForeignKey("SubscriptionOwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Student", "SubscriptionTarget")
                        .WithMany("StudentsSubscribed")
                        .HasForeignKey("SubscriptionTargetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SubscriptionOwner");

                    b.Navigation("SubscriptionTarget");
                });

            modelBuilder.Entity("JobOfferStudent", b =>
                {
                    b.HasOne("Domain.Entities.JobOffer", null)
                        .WithMany()
                        .HasForeignKey("JobOfferSubscriptionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Student", null)
                        .WithMany()
                        .HasForeignKey("SubscribedStudentsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("JobOfferTag", b =>
                {
                    b.HasOne("Domain.Entities.JobOffer", null)
                        .WithMany()
                        .HasForeignKey("JobOffersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.Admin", b =>
                {
                    b.HasOne("Domain.Entities.Account", null)
                        .WithOne()
                        .HasForeignKey("Domain.Entities.Admin", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.Company", b =>
                {
                    b.HasOne("Domain.Entities.Account", null)
                        .WithOne()
                        .HasForeignKey("Domain.Entities.Company", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.Student", b =>
                {
                    b.HasOne("Domain.Entities.Account", null)
                        .WithOne()
                        .HasForeignKey("Domain.Entities.Student", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.StudentGroup", "StudentGroup")
                        .WithMany()
                        .HasForeignKey("StudentGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("StudentGroup");
                });

            modelBuilder.Entity("Domain.Entities.Account", b =>
                {
                    b.Navigation("RefreshTokens");
                });

            modelBuilder.Entity("Domain.Entities.CV", b =>
                {
                    b.Navigation("Educations");

                    b.Navigation("ForeignLanguages");

                    b.Navigation("ProjectLinks");
                });

            modelBuilder.Entity("Domain.Entities.JobPosition", b =>
                {
                    b.Navigation("CVs");

                    b.Navigation("JobOffers");
                });

            modelBuilder.Entity("Domain.Entities.Company", b =>
                {
                    b.Navigation("JobOffers");

                    b.Navigation("Links");
                });

            modelBuilder.Entity("Domain.Entities.Student", b =>
                {
                    b.Navigation("CVs");

                    b.Navigation("Experiences");

                    b.Navigation("StudentSubscriptions");

                    b.Navigation("StudentsSubscribed");
                });
#pragma warning restore 612, 618
        }
    }
}
