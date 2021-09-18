using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SALGADBLib.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobTitles",
                columns: table => new
                {
                    pkID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobTitles", x => x.pkID);
                });

            migrationBuilder.CreateTable(
                name: "Municipalities",
                columns: table => new
                {
                    pkID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Municipalities", x => x.pkID);
                });

            migrationBuilder.CreateTable(
                name: "MunicipalityDemographics",
                columns: table => new
                {
                    pkID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoEmployees = table.Column<int>(type: "int", nullable: false),
                    NoPerm54A56 = table.Column<int>(type: "int", nullable: false),
                    NoFixedTerm54A56 = table.Column<int>(type: "int", nullable: false),
                    NoPermNon54A56 = table.Column<int>(type: "int", nullable: false),
                    NoFixedTermNon54A56 = table.Column<int>(type: "int", nullable: false),
                    NoOther = table.Column<int>(type: "int", nullable: false),
                    TotalMonthlyPayroll = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MunicipalityDemographics", x => x.pkID);
                });

            migrationBuilder.CreateTable(
                name: "IntervieweeDetails",
                columns: table => new
                {
                    pkID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Municipality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LineManager = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YearsInPosition = table.Column<int>(type: "int", nullable: false),
                    JobTitlepkID = table.Column<int>(type: "int", nullable: true),
                    InterviewDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CellNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntervieweeDetails", x => x.pkID);
                    table.ForeignKey(
                        name: "FK_IntervieweeDetails_JobTitles_JobTitlepkID",
                        column: x => x.JobTitlepkID,
                        principalTable: "JobTitles",
                        principalColumn: "pkID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IntervieweeDetails_JobTitlepkID",
                table: "IntervieweeDetails",
                column: "JobTitlepkID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IntervieweeDetails");

            migrationBuilder.DropTable(
                name: "Municipalities");

            migrationBuilder.DropTable(
                name: "MunicipalityDemographics");

            migrationBuilder.DropTable(
                name: "JobTitles");
        }
    }
}
