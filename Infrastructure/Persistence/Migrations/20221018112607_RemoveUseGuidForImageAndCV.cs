using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    public partial class RemoveUseGuidForImageAndCV : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "JobOffers");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "CVs");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "JobOffers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "CVs",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "JobOffers");

            migrationBuilder.DropColumn(
                name: "Photo",
                table: "CVs");

            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "JobOffers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PhotoId",
                table: "CVs",
                type: "uuid",
                nullable: true);
        }
    }
}
