using Microsoft.EntityFrameworkCore.Migrations;

namespace SALGADBLib.Migrations
{
    public partial class DemographicsMunicipality : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MunicipalitypkID",
                table: "MunicipalityDemographics",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MunicipalitypkID",
                table: "HRMDFunctionDemographics",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MunicipalityDemographics_MunicipalitypkID",
                table: "MunicipalityDemographics",
                column: "MunicipalitypkID");

            migrationBuilder.CreateIndex(
                name: "IX_HRMDFunctionDemographics_MunicipalitypkID",
                table: "HRMDFunctionDemographics",
                column: "MunicipalitypkID");

            migrationBuilder.AddForeignKey(
                name: "FK_HRMDFunctionDemographics_Municipalities_MunicipalitypkID",
                table: "HRMDFunctionDemographics",
                column: "MunicipalitypkID",
                principalTable: "Municipalities",
                principalColumn: "pkID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MunicipalityDemographics_Municipalities_MunicipalitypkID",
                table: "MunicipalityDemographics",
                column: "MunicipalitypkID",
                principalTable: "Municipalities",
                principalColumn: "pkID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HRMDFunctionDemographics_Municipalities_MunicipalitypkID",
                table: "HRMDFunctionDemographics");

            migrationBuilder.DropForeignKey(
                name: "FK_MunicipalityDemographics_Municipalities_MunicipalitypkID",
                table: "MunicipalityDemographics");

            migrationBuilder.DropIndex(
                name: "IX_MunicipalityDemographics_MunicipalitypkID",
                table: "MunicipalityDemographics");

            migrationBuilder.DropIndex(
                name: "IX_HRMDFunctionDemographics_MunicipalitypkID",
                table: "HRMDFunctionDemographics");

            migrationBuilder.DropColumn(
                name: "MunicipalitypkID",
                table: "MunicipalityDemographics");

            migrationBuilder.DropColumn(
                name: "MunicipalitypkID",
                table: "HRMDFunctionDemographics");
        }
    }
}
