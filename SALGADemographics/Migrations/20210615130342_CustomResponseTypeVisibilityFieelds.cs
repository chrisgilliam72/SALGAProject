using Microsoft.EntityFrameworkCore.Migrations;

namespace SALGADBLib.Migrations
{
    public partial class CustomResponseTypeVisibilityFieelds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_municipalityAssessmentConfigs_Municipalities_MunicipalitypkID",
                table: "municipalityAssessmentConfigs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_municipalityAssessmentConfigs",
                table: "municipalityAssessmentConfigs");

            migrationBuilder.RenameTable(
                name: "municipalityAssessmentConfigs",
                newName: "MunicipalityAssessmentConfigs");

            migrationBuilder.RenameIndex(
                name: "IX_municipalityAssessmentConfigs_MunicipalitypkID",
                table: "MunicipalityAssessmentConfigs",
                newName: "IX_MunicipalityAssessmentConfigs_MunicipalitypkID");

            migrationBuilder.AlterColumn<string>(
                name: "YearsInPosition",
                table: "IntervieweeDetails",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "VisibleToApprover",
                table: "CustomReponseTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "VisibleToUser",
                table: "CustomReponseTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MunicipalityAssessmentConfigs",
                table: "MunicipalityAssessmentConfigs",
                column: "pkID");

            migrationBuilder.AddForeignKey(
                name: "FK_MunicipalityAssessmentConfigs_Municipalities_MunicipalitypkID",
                table: "MunicipalityAssessmentConfigs",
                column: "MunicipalitypkID",
                principalTable: "Municipalities",
                principalColumn: "pkID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MunicipalityAssessmentConfigs_Municipalities_MunicipalitypkID",
                table: "MunicipalityAssessmentConfigs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MunicipalityAssessmentConfigs",
                table: "MunicipalityAssessmentConfigs");

            migrationBuilder.DropColumn(
                name: "VisibleToApprover",
                table: "CustomReponseTypes");

            migrationBuilder.DropColumn(
                name: "VisibleToUser",
                table: "CustomReponseTypes");

            migrationBuilder.RenameTable(
                name: "MunicipalityAssessmentConfigs",
                newName: "municipalityAssessmentConfigs");

            migrationBuilder.RenameIndex(
                name: "IX_MunicipalityAssessmentConfigs_MunicipalitypkID",
                table: "municipalityAssessmentConfigs",
                newName: "IX_municipalityAssessmentConfigs_MunicipalitypkID");

            migrationBuilder.AlterColumn<int>(
                name: "YearsInPosition",
                table: "IntervieweeDetails",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_municipalityAssessmentConfigs",
                table: "municipalityAssessmentConfigs",
                column: "pkID");

            migrationBuilder.AddForeignKey(
                name: "FK_municipalityAssessmentConfigs_Municipalities_MunicipalitypkID",
                table: "municipalityAssessmentConfigs",
                column: "MunicipalitypkID",
                principalTable: "Municipalities",
                principalColumn: "pkID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
