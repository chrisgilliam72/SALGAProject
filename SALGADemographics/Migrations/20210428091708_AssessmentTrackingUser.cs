using Microsoft.EntityFrameworkCore.Migrations;

namespace SALGADBLib.Migrations
{
    public partial class AssessmentTrackingUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "AssessmentTrackings",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentTrackings_UserId",
                table: "AssessmentTrackings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssessmentTrackings_AspNetUsers_UserId",
                table: "AssessmentTrackings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssessmentTrackings_AspNetUsers_UserId",
                table: "AssessmentTrackings");

            migrationBuilder.DropIndex(
                name: "IX_AssessmentTrackings_UserId",
                table: "AssessmentTrackings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AssessmentTrackings");
        }
    }
}
