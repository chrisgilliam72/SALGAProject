using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SALGADBLib.Migrations
{
    public partial class AddHRMTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Organogram",
                table: "MunicipalityDemographics",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganogramFileFormat",
                table: "MunicipalityDemographics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HRMDFunctionDemographics",
                columns: table => new
                {
                    pkID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoPeople = table.Column<int>(type: "int", nullable: false),
                    CorporateService = table.Column<bool>(type: "bit", nullable: false),
                    Organogram = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    OrganogramFileFormat = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRMDFunctionDemographics", x => x.pkID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HRMDFunctionDemographics");

            migrationBuilder.DropColumn(
                name: "Organogram",
                table: "MunicipalityDemographics");

            migrationBuilder.DropColumn(
                name: "OrganogramFileFormat",
                table: "MunicipalityDemographics");
        }
    }
}
