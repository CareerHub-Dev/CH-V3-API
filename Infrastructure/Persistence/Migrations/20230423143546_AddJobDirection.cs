using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddJobDirection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "JobDirectionId",
                table: "JobPositions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "JobDirections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    RecomendedTemplateLanguage = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobDirections", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobPositions_JobDirectionId",
                table: "JobPositions",
                column: "JobDirectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobPositions_JobDirections_JobDirectionId",
                table: "JobPositions",
                column: "JobDirectionId",
                principalTable: "JobDirections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobPositions_JobDirections_JobDirectionId",
                table: "JobPositions");

            migrationBuilder.DropTable(
                name: "JobDirections");

            migrationBuilder.DropIndex(
                name: "IX_JobPositions_JobDirectionId",
                table: "JobPositions");

            migrationBuilder.DropColumn(
                name: "JobDirectionId",
                table: "JobPositions");
        }
    }
}
