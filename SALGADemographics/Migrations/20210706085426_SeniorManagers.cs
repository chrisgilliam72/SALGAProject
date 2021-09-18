using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SALGADBLib.Migrations
{
    public partial class SeniorManagers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SeniorManagers",
                columns: table => new
                {
                    pkID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Portfolio = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeniorManagers", x => x.pkID);
                });

            migrationBuilder.CreateTable(
                name: "SeniorManagerPositions",
                columns: table => new
                {
                    pkID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Filled = table.Column<bool>(type: "bit", nullable: false),
                    AppointmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MunicipalityDemographicspkID = table.Column<int>(type: "int", nullable: true),
                    SeniorManagerpkID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeniorManagerPositions", x => x.pkID);
                    table.ForeignKey(
                        name: "FK_SeniorManagerPositions_MunicipalityDemographics_MunicipalityDemographicspkID",
                        column: x => x.MunicipalityDemographicspkID,
                        principalTable: "MunicipalityDemographics",
                        principalColumn: "pkID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SeniorManagerPositions_SeniorManagers_SeniorManagerpkID",
                        column: x => x.SeniorManagerpkID,
                        principalTable: "SeniorManagers",
                        principalColumn: "pkID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SeniorManagerPositions_MunicipalityDemographicspkID",
                table: "SeniorManagerPositions",
                column: "MunicipalityDemographicspkID");

            migrationBuilder.CreateIndex(
                name: "IX_SeniorManagerPositions_SeniorManagerpkID",
                table: "SeniorManagerPositions",
                column: "SeniorManagerpkID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SeniorManagerPositions");

            migrationBuilder.DropTable(
                name: "SeniorManagers");
        }
    }
}
