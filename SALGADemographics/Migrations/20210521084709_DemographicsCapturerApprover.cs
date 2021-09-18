using Microsoft.EntityFrameworkCore.Migrations;

namespace SALGADBLib.Migrations
{
    public partial class DemographicsCapturerApprover : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "MunicipalityDemographics",
                newName: "CapturerId");

            migrationBuilder.CreateIndex(
                name: "IX_MunicipalityDemographics_CapturerId",
                table: "MunicipalityDemographics",
                column: "CapturerId");

            migrationBuilder.AddColumn<string>(
                name: "ApproverId",
                table: "MunicipalityDemographics",
                type: "nvarchar(450)",
                nullable: true);


            migrationBuilder.CreateIndex(
                name: "IX_MunicipalityDemographics_ApproverId",
                table: "MunicipalityDemographics",
                column: "ApproverId");

            migrationBuilder.AddForeignKey(
                name: "FK_MunicipalityDemographics_AspNetUsers_ApproverId",
                table: "MunicipalityDemographics",
                column: "ApproverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MunicipalityDemographics_AspNetUsers_CapturerId",
                table: "MunicipalityDemographics",
                column: "CapturerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MunicipalityDemographics_AspNetUsers_ApproverId",
                table: "MunicipalityDemographics");

            migrationBuilder.DropForeignKey(
                name: "FK_MunicipalityDemographics_AspNetUsers_CapturerId",
                table: "MunicipalityDemographics");

            migrationBuilder.DropIndex(
                name: "IX_MunicipalityDemographics_ApproverId",
                table: "MunicipalityDemographics");

            migrationBuilder.DropColumn(
                name: "ApproverId",
                table: "MunicipalityDemographics");

            migrationBuilder.DropColumn(
                name: "Onboarded",
                table: "IntervieweeDetails");

            migrationBuilder.RenameColumn(
                name: "CapturerId",
                table: "MunicipalityDemographics",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_MunicipalityDemographics_CapturerId",
                table: "MunicipalityDemographics",
                newName: "IX_MunicipalityDemographics_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MunicipalityDemographics_AspNetUsers_UserId",
                table: "MunicipalityDemographics",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
