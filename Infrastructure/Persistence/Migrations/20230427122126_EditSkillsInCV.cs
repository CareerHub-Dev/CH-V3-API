using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EditSkillsInCV : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExperienceHighlights",
                table: "CVs");

            migrationBuilder.DropColumn(
                name: "SkillsAndTechnologies",
                table: "CVs");

            migrationBuilder.AddColumn<int>(
                name: "ExperienceLevel",
                table: "CVs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<List<string>>(
                name: "HardSkills",
                table: "CVs",
                type: "text[]",
                nullable: false);

            migrationBuilder.AddColumn<List<string>>(
                name: "SoftSkills",
                table: "CVs",
                type: "text[]",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExperienceLevel",
                table: "CVs");

            migrationBuilder.DropColumn(
                name: "HardSkills",
                table: "CVs");

            migrationBuilder.DropColumn(
                name: "SoftSkills",
                table: "CVs");

            migrationBuilder.AddColumn<string>(
                name: "ExperienceHighlights",
                table: "CVs",
                type: "character varying(1024)",
                maxLength: 1024,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SkillsAndTechnologies",
                table: "CVs",
                type: "character varying(1024)",
                maxLength: 1024,
                nullable: false,
                defaultValue: "");
        }
    }
}
