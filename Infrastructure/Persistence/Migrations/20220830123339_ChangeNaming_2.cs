using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    public partial class ChangeNaming_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CompanyName",
                table: "Companies",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "CompanyMotto",
                table: "Companies",
                newName: "Motto");

            migrationBuilder.RenameColumn(
                name: "CompanyLogoId",
                table: "Companies",
                newName: "LogoId");

            migrationBuilder.RenameColumn(
                name: "CompanyDescription",
                table: "Companies",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "CompanyBannerId",
                table: "Companies",
                newName: "BannerId");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Students",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Companies",
                newName: "CompanyName");

            migrationBuilder.RenameColumn(
                name: "Motto",
                table: "Companies",
                newName: "CompanyMotto");

            migrationBuilder.RenameColumn(
                name: "LogoId",
                table: "Companies",
                newName: "CompanyLogoId");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Companies",
                newName: "CompanyDescription");

            migrationBuilder.RenameColumn(
                name: "BannerId",
                table: "Companies",
                newName: "CompanyBannerId");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Students",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);
        }
    }
}
