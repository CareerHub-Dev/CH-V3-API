using System;
using System.Collections.Generic;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false, computedColumnSql: "LOWER(TRIM(\"Email\"))", stored: true),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    VerificationToken = table.Column<string>(type: "text", nullable: true),
                    Verified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ResetToken = table.Column<string>(type: "text", nullable: true),
                    ResetTokenExpires = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PasswordReset = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobPositions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobPositions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    IsAccepted = table.Column<bool>(type: "boolean", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsSuperAdmin = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Admins_Accounts_Id",
                        column: x => x.Id,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Reason = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    Expires = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bans_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Logo = table.Column<string>(type: "text", nullable: true),
                    Banner = table.Column<string>(type: "text", nullable: true),
                    Motto = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    Links = table.Column<List<CompanyLink>>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Companies_Accounts_Id",
                        column: x => x.Id,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    Images = table.Column<List<string>>(type: "text[]", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AccoundId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    Expires = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByIp = table.Column<string>(type: "text", nullable: false),
                    Revoked = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RevokedByIp = table.Column<string>(type: "text", nullable: false),
                    ReplacedByToken = table.Column<string>(type: "text", nullable: false),
                    ReasonRevoked = table.Column<string>(type: "text", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    LastName = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false, computedColumnSql: "LOWER(TRIM(\"Email\"))", stored: true),
                    StudentGroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentLogs_StudentGroups_StudentGroupId",
                        column: x => x.StudentGroupId,
                        principalTable: "StudentGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    LastName = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Photo = table.Column<string>(type: "text", nullable: true),
                    Phone = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    StudentGroupId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_Accounts_Id",
                        column: x => x.Id,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Students_StudentGroups_StudentGroupId",
                        column: x => x.StudentGroupId,
                        principalTable: "StudentGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobOffers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Overview = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    Requirements = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    Responsibilities = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    Preferences = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    Image = table.Column<string>(type: "text", nullable: true),
                    JobType = table.Column<int>(type: "integer", nullable: false),
                    WorkFormat = table.Column<int>(type: "integer", nullable: false),
                    ExperienceLevel = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    JobPositionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobOffers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobOffers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobOffers_JobPositions_JobPositionId",
                        column: x => x.JobPositionId,
                        principalTable: "JobPositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyStudent",
                columns: table => new
                {
                    CompanySubscriptionsId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubscribedStudentsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyStudent", x => new { x.CompanySubscriptionsId, x.SubscribedStudentsId });
                    table.ForeignKey(
                        name: "FK_CompanyStudent_Companies_CompanySubscriptionsId",
                        column: x => x.CompanySubscriptionsId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyStudent_Students_SubscribedStudentsId",
                        column: x => x.SubscribedStudentsId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CVs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    JobPositionId = table.Column<Guid>(type: "uuid", nullable: false),
                    TemplateLanguage = table.Column<int>(type: "integer", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    Photo = table.Column<string>(type: "text", nullable: true),
                    Goals = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    SkillsAndTechnologies = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    ExperienceHighlights = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ForeignLanguages = table.Column<List<ForeignLanguage>>(type: "jsonb", nullable: false),
                    ProjectLinks = table.Column<List<CVProjectLink>>(type: "jsonb", nullable: false),
                    Educations = table.Column<List<Education>>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CVs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CVs_JobPositions_JobPositionId",
                        column: x => x.JobPositionId,
                        principalTable: "JobPositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CVs_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Experiences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CompanyName = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    JobType = table.Column<int>(type: "integer", nullable: false),
                    WorkFormat = table.Column<int>(type: "integer", nullable: false),
                    ExperienceLevel = table.Column<int>(type: "integer", nullable: false),
                    JobLocation = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experiences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Experiences_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostStudent",
                columns: table => new
                {
                    LikedPostsId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentsLikedId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostStudent", x => new { x.LikedPostsId, x.StudentsLikedId });
                    table.ForeignKey(
                        name: "FK_PostStudent_Posts_LikedPostsId",
                        column: x => x.LikedPostsId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostStudent_Students_StudentsLikedId",
                        column: x => x.StudentsLikedId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentSubscriptions",
                columns: table => new
                {
                    SubscriptionOwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubscriptionTargetId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentSubscriptions", x => new { x.SubscriptionOwnerId, x.SubscriptionTargetId });
                    table.ForeignKey(
                        name: "FK_StudentSubscriptions_Students_SubscriptionOwnerId",
                        column: x => x.SubscriptionOwnerId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentSubscriptions_Students_SubscriptionTargetId",
                        column: x => x.SubscriptionTargetId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobOfferStudent",
                columns: table => new
                {
                    JobOfferSubscriptionsId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubscribedStudentsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobOfferStudent", x => new { x.JobOfferSubscriptionsId, x.SubscribedStudentsId });
                    table.ForeignKey(
                        name: "FK_JobOfferStudent_JobOffers_JobOfferSubscriptionsId",
                        column: x => x.JobOfferSubscriptionsId,
                        principalTable: "JobOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobOfferStudent_Students_SubscribedStudentsId",
                        column: x => x.SubscribedStudentsId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobOfferTag",
                columns: table => new
                {
                    JobOffersId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobOfferTag", x => new { x.JobOffersId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_JobOfferTag_JobOffers_JobOffersId",
                        column: x => x.JobOffersId,
                        principalTable: "JobOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobOfferTag_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CVJobOffer",
                columns: table => new
                {
                    AppliedCVsId = table.Column<Guid>(type: "uuid", nullable: false),
                    TargetJobOffersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CVJobOffer", x => new { x.AppliedCVsId, x.TargetJobOffersId });
                    table.ForeignKey(
                        name: "FK_CVJobOffer_CVs_AppliedCVsId",
                        column: x => x.AppliedCVsId,
                        principalTable: "CVs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CVJobOffer_JobOffers_TargetJobOffersId",
                        column: x => x.TargetJobOffersId,
                        principalTable: "JobOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_NormalizedEmail",
                table: "Accounts",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Bans_AccountId",
                table: "Bans",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyStudent_SubscribedStudentsId",
                table: "CompanyStudent",
                column: "SubscribedStudentsId");

            migrationBuilder.CreateIndex(
                name: "IX_CVJobOffer_TargetJobOffersId",
                table: "CVJobOffer",
                column: "TargetJobOffersId");

            migrationBuilder.CreateIndex(
                name: "IX_CVs_JobPositionId",
                table: "CVs",
                column: "JobPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_CVs_StudentId",
                table: "CVs",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Experiences_StudentId",
                table: "Experiences",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_JobOffers_CompanyId",
                table: "JobOffers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_JobOffers_JobPositionId",
                table: "JobOffers",
                column: "JobPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_JobOfferStudent_SubscribedStudentsId",
                table: "JobOfferStudent",
                column: "SubscribedStudentsId");

            migrationBuilder.CreateIndex(
                name: "IX_JobOfferTag_TagsId",
                table: "JobOfferTag",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_AccountId",
                table: "Posts",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PostStudent_StudentsLikedId",
                table: "PostStudent",
                column: "StudentsLikedId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_AccountId",
                table: "RefreshTokens",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentLogs_NormalizedEmail",
                table: "StudentLogs",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_StudentLogs_StudentGroupId",
                table: "StudentLogs",
                column: "StudentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_StudentGroupId",
                table: "Students",
                column: "StudentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentSubscriptions_SubscriptionTargetId",
                table: "StudentSubscriptions",
                column: "SubscriptionTargetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Bans");

            migrationBuilder.DropTable(
                name: "CompanyStudent");

            migrationBuilder.DropTable(
                name: "CVJobOffer");

            migrationBuilder.DropTable(
                name: "Experiences");

            migrationBuilder.DropTable(
                name: "JobOfferStudent");

            migrationBuilder.DropTable(
                name: "JobOfferTag");

            migrationBuilder.DropTable(
                name: "PostStudent");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "StudentLogs");

            migrationBuilder.DropTable(
                name: "StudentSubscriptions");

            migrationBuilder.DropTable(
                name: "CVs");

            migrationBuilder.DropTable(
                name: "JobOffers");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "JobPositions");

            migrationBuilder.DropTable(
                name: "StudentGroups");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
