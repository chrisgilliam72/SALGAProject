using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SALGADBLib.Migrations
{
    public partial class MunicipalityConfigs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MunicipalityAssessmentConfigs",
                columns: table => new
                {
                    pkID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MunicipalitypkID = table.Column<int>(type: "int", nullable: true),
                    CurrentYear = table.Column<int>(type: "int", nullable: false),
                    CurrentQuestionSet = table.Column<int>(type: "int", nullable: false),
                    OpenDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CloseDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MunicipalityAssessmentConfigs", x => x.pkID);
                    table.ForeignKey(
                        name: "FK_MunicipalityAssessmentConfigs_Municipalities_MunicipalitypkID",
                        column: x => x.MunicipalitypkID,
                        principalTable: "Municipalities",
                        principalColumn: "pkID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MunicipalityAssessmentConfigs_MunicipalitypkID",
                table: "MunicipalityAssessmentConfigs",
                column: "MunicipalitypkID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MunicipalityAssessmentConfigs");
        }
    }
}
