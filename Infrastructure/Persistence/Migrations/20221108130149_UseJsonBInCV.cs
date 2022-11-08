using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    public partial class UseJsonBInCV : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CVProjectLinks");

            migrationBuilder.DropTable(
                name: "Educations");

            migrationBuilder.DropTable(
                name: "ForeignLanguages");

            migrationBuilder.AddColumn<List<Education>>(
                name: "Educations",
                table: "CVs",
                type: "jsonb",
                nullable: false);

            migrationBuilder.AddColumn<List<ForeignLanguage>>(
                name: "ForeignLanguages",
                table: "CVs",
                type: "jsonb",
                nullable: false);

            migrationBuilder.AddColumn<List<CVProjectLink>>(
                name: "ProjectLinks",
                table: "CVs",
                type: "jsonb",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Educations",
                table: "CVs");

            migrationBuilder.DropColumn(
                name: "ForeignLanguages",
                table: "CVs");

            migrationBuilder.DropColumn(
                name: "ProjectLinks",
                table: "CVs");

            migrationBuilder.CreateTable(
                name: "CVProjectLinks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CVId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CVProjectLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CVProjectLinks_CVs_CVId",
                        column: x => x.CVId,
                        principalTable: "CVs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Educations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CVId = table.Column<Guid>(type: "uuid", nullable: false),
                    City = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Country = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Degree = table.Column<Degree>(type: "degree", nullable: false),
                    End = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Specialty = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Start = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    University = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Educations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Educations_CVs_CVId",
                        column: x => x.CVId,
                        principalTable: "CVs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ForeignLanguages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CVId = table.Column<Guid>(type: "uuid", nullable: false),
                    LanguageLevel = table.Column<LanguageLevel>(type: "language_level", nullable: false),
                    Name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForeignLanguages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForeignLanguages_CVs_CVId",
                        column: x => x.CVId,
                        principalTable: "CVs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CVProjectLinks_CVId",
                table: "CVProjectLinks",
                column: "CVId");

            migrationBuilder.CreateIndex(
                name: "IX_Educations_CVId",
                table: "Educations",
                column: "CVId");

            migrationBuilder.CreateIndex(
                name: "IX_ForeignLanguages_CVId",
                table: "ForeignLanguages",
                column: "CVId");
        }
    }
}
