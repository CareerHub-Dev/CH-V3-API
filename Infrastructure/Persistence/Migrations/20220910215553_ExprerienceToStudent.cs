using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    public partial class ExprerienceToStudent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Experiences_CVs_CVId",
                table: "Experiences");

            migrationBuilder.RenameColumn(
                name: "CVId",
                table: "Experiences",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Experiences_CVId",
                table: "Experiences",
                newName: "IX_Experiences_StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Experiences_Students_StudentId",
                table: "Experiences",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Experiences_Students_StudentId",
                table: "Experiences");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "Experiences",
                newName: "CVId");

            migrationBuilder.RenameIndex(
                name: "IX_Experiences_StudentId",
                table: "Experiences",
                newName: "IX_Experiences_CVId");

            migrationBuilder.AddForeignKey(
                name: "FK_Experiences_CVs_CVId",
                table: "Experiences",
                column: "CVId",
                principalTable: "CVs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
