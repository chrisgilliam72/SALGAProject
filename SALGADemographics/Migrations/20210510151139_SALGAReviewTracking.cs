using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SALGADBLib.Migrations
{
    public partial class SALGAReviewTracking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SALGAApproved",
                table: "AssessmentTrackings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SALGARejected",
                table: "AssessmentTrackings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "SALGAReviewDate",
                table: "AssessmentTrackings",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SALGAApproved",
                table: "AssessmentTrackings");

            migrationBuilder.DropColumn(
                name: "SALGARejected",
                table: "AssessmentTrackings");

            migrationBuilder.DropColumn(
                name: "SALGAReviewDate",
                table: "AssessmentTrackings");
        }
    }
}
