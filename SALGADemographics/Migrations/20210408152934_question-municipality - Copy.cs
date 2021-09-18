using Microsoft.EntityFrameworkCore.Migrations;

namespace SALGADBLib.Migrations
{
    public partial class questionmunicipality : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.AddForeignKey(
                name: "FK_QuestionnaireQuestionAnswers_Municipalities_MunicipalitypkID",
                table: "QuestionnaireQuestionAnswers",
                column: "MunicipalitypkID",
                principalTable: "Municipalities",
                principalColumn: "pkID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HRMDFunctionDemographics_AspNetUsers_UserId",
                table: "HRMDFunctionDemographics");

            migrationBuilder.DropForeignKey(
                name: "FK_IntervieweeDetails_AspNetUsers_UserId",
                table: "IntervieweeDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_IntervieweeDetails_Municipalities_MunicipalitypkID",
                table: "IntervieweeDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Municipalities_MunicipalCatagories_MunicipalCatagorypkID",
                table: "Municipalities");

            migrationBuilder.DropForeignKey(
                name: "FK_MunicipalityDemographics_AspNetUsers_UserId",
                table: "MunicipalityDemographics");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionnaireQuestionAnswers_Municipalities_MunicipalitypkID",
                table: "QuestionnaireQuestionAnswers");

            migrationBuilder.DropTable(
                name: "MunicipalCatagories");

            migrationBuilder.DropIndex(
                name: "IX_QuestionnaireQuestionAnswers_MunicipalitypkID",
                table: "QuestionnaireQuestionAnswers");

            migrationBuilder.DropIndex(
                name: "IX_MunicipalityDemographics_UserId",
                table: "MunicipalityDemographics");

            migrationBuilder.DropIndex(
                name: "IX_Municipalities_MunicipalCatagorypkID",
                table: "Municipalities");

            migrationBuilder.DropIndex(
                name: "IX_IntervieweeDetails_MunicipalitypkID",
                table: "IntervieweeDetails");

            migrationBuilder.DropIndex(
                name: "IX_IntervieweeDetails_UserId",
                table: "IntervieweeDetails");

            migrationBuilder.DropIndex(
                name: "IX_HRMDFunctionDemographics_UserId",
                table: "HRMDFunctionDemographics");

            migrationBuilder.DropColumn(
                name: "Level1",
                table: "QuestionnaireQuestions");

            migrationBuilder.DropColumn(
                name: "Level2",
                table: "QuestionnaireQuestions");

            migrationBuilder.DropColumn(
                name: "Level3",
                table: "QuestionnaireQuestions");

            migrationBuilder.DropColumn(
                name: "Level4",
                table: "QuestionnaireQuestions");

            migrationBuilder.DropColumn(
                name: "MunicipalitypkID",
                table: "QuestionnaireQuestionAnswers");

            migrationBuilder.DropColumn(
                name: "OriginalEvidenceFileName",
                table: "QuestionnaireQuestionAnswers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "MunicipalityDemographics");

            migrationBuilder.DropColumn(
                name: "MunicipalCatagorypkID",
                table: "Municipalities");

            migrationBuilder.DropColumn(
                name: "MunicipalitypkID",
                table: "IntervieweeDetails");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "IntervieweeDetails");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "HRMDFunctionDemographics");

            migrationBuilder.AddColumn<int>(
                name: "ResponseTypepkID",
                table: "QuestionnaireQuestions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Municipality",
                table: "IntervieweeDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionnaireQuestions_ResponseTypepkID",
                table: "QuestionnaireQuestions",
                column: "ResponseTypepkID");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionnaireQuestions_QuestionnaireResponseTypes_ResponseTypepkID",
                table: "QuestionnaireQuestions",
                column: "ResponseTypepkID",
                principalTable: "QuestionnaireResponseTypes",
                principalColumn: "pkID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
