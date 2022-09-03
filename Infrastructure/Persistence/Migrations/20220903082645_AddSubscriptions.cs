using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    public partial class AddSubscriptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_CompanyStudent_SubscribedStudentsId",
                table: "CompanyStudent",
                column: "SubscribedStudentsId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentSubscriptions_SubscriptionTargetId",
                table: "StudentSubscriptions",
                column: "SubscriptionTargetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyStudent");

            migrationBuilder.DropTable(
                name: "StudentSubscriptions");
        }
    }
}
