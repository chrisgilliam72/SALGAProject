using Microsoft.EntityFrameworkCore.Migrations;

namespace SALGADBLib.Migrations
{
    public partial class questionmunicipality2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MunicipalitypkID",
                table: "QuestionnaireQuestionAnswers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuestionnaireQuestionAnswers_MunicipalitypkID",
                table: "QuestionnaireQuestionAnswers",
                column: "MunicipalitypkID");

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
                name: "FK_QuestionnaireQuestionAnswers_Municipalities_MunicipalitypkID",
                table: "QuestionnaireQuestionAnswers");

            migrationBuilder.DropIndex(
                name: "IX_QuestionnaireQuestionAnswers_MunicipalitypkID",
                table: "QuestionnaireQuestionAnswers");

            migrationBuilder.DropColumn(
                name: "MunicipalitypkID",
                table: "QuestionnaireQuestionAnswers");
        }
    }
}
