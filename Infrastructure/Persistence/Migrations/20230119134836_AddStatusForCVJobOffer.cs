using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusForCVJobOffer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CVJobOffer");

            migrationBuilder.CreateTable(
                name: "CVJobOffers",
                columns: table => new
                {
                    CVId = table.Column<Guid>(type: "uuid", nullable: false),
                    JobOfferId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CVJobOffers", x => new { x.CVId, x.JobOfferId });
                    table.ForeignKey(
                        name: "FK_CVJobOffers_CVs_CVId",
                        column: x => x.CVId,
                        principalTable: "CVs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CVJobOffers_JobOffers_JobOfferId",
                        column: x => x.JobOfferId,
                        principalTable: "JobOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CVJobOffers_JobOfferId",
                table: "CVJobOffers",
                column: "JobOfferId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CVJobOffers");

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
                name: "IX_CVJobOffer_TargetJobOffersId",
                table: "CVJobOffer",
                column: "TargetJobOffersId");
        }
    }
}
