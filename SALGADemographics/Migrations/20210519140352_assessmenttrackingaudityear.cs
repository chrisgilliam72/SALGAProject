using Microsoft.EntityFrameworkCore.Migrations;

namespace SALGADBLib.Migrations
{
    public partial class assessmenttrackingaudityear : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuditYear",
                table: "AuditEvents");

            migrationBuilder.AddColumn<int>(
                name: "AuditYear",
                table: "AssessmentTrackings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuditYear",
                table: "AssessmentTrackings");

            migrationBuilder.AddColumn<int>(
                name: "AuditYear",
                table: "AuditEvents",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
