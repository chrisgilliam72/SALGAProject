using Microsoft.EntityFrameworkCore.Migrations;

namespace SALGADBLib.Migrations
{
    public partial class TrackingMunicipality : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MunicipalitypkID",
                table: "AssessmentTrackings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentTrackings_MunicipalitypkID",
                table: "AssessmentTrackings",
                column: "MunicipalitypkID");

            migrationBuilder.AddForeignKey(
                name: "FK_AssessmentTrackings_Municipalities_MunicipalitypkID",
                table: "AssessmentTrackings",
                column: "MunicipalitypkID",
                principalTable: "Municipalities",
                principalColumn: "pkID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssessmentTrackings_Municipalities_MunicipalitypkID",
                table: "AssessmentTrackings");

            migrationBuilder.DropIndex(
                name: "IX_AssessmentTrackings_MunicipalitypkID",
                table: "AssessmentTrackings");

            migrationBuilder.DropColumn(
                name: "MunicipalitypkID",
                table: "AssessmentTrackings");
        }
    }
}
