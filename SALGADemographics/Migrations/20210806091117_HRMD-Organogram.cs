using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SALGADBLib.Migrations
{
    public partial class HRMDOrganogram : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MunicipalityDemographics_Organograms_OrganogrampkID",
                table: "MunicipalityDemographics");

            migrationBuilder.DropTable(
                name: "Organograms");

            migrationBuilder.DropIndex(
                name: "IX_MunicipalityDemographics_OrganogrampkID",
                table: "MunicipalityDemographics");

            migrationBuilder.DropColumn(
                name: "OrganogrampkID",
                table: "MunicipalityDemographics");

            migrationBuilder.DropColumn(
                name: "Organogram",
                table: "HRMDFunctionDemographics");

            migrationBuilder.DropColumn(
                name: "OrganogramFileFormat",
                table: "HRMDFunctionDemographics");


            migrationBuilder.AddColumn<int>(
                name: "MunicipalityOrganogrampkID",
                table: "HRMDFunctionDemographics",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MunicipalityOrganograms",
                columns: table => new
                {
                    pkID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContainerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlobName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalFileName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MunicipalityOrganograms", x => x.pkID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HRMDFunctionDemographics_MunicipalityOrganogrampkID",
                table: "HRMDFunctionDemographics",
                column: "MunicipalityOrganogrampkID");

            migrationBuilder.AddForeignKey(
                name: "FK_HRMDFunctionDemographics_MunicipalityOrganograms_MunicipalityOrganogrampkID",
                table: "HRMDFunctionDemographics",
                column: "MunicipalityOrganogrampkID",
                principalTable: "MunicipalityOrganograms",
                principalColumn: "pkID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HRMDFunctionDemographics_MunicipalityOrganograms_MunicipalityOrganogrampkID",
                table: "HRMDFunctionDemographics");

            migrationBuilder.DropTable(
                name: "MunicipalityOrganograms");

            migrationBuilder.DropIndex(
                name: "IX_HRMDFunctionDemographics_MunicipalityOrganogrampkID",
                table: "HRMDFunctionDemographics");

            migrationBuilder.DropColumn(
                name: "MunicipalityOrganogrampkID",
                table: "HRMDFunctionDemographics");


            migrationBuilder.AddColumn<int>(
                name: "OrganogrampkID",
                table: "MunicipalityDemographics",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Organogram",
                table: "HRMDFunctionDemographics",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganogramFileFormat",
                table: "HRMDFunctionDemographics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Organograms",
                columns: table => new
                {
                    pkID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    COO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityManager = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DirectorHR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExecutiveMayor = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organograms", x => x.pkID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MunicipalityDemographics_OrganogrampkID",
                table: "MunicipalityDemographics",
                column: "OrganogrampkID");

            migrationBuilder.AddForeignKey(
                name: "FK_MunicipalityDemographics_Organograms_OrganogrampkID",
                table: "MunicipalityDemographics",
                column: "OrganogrampkID",
                principalTable: "Organograms",
                principalColumn: "pkID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
