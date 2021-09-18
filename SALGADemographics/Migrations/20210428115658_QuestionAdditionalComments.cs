using Microsoft.EntityFrameworkCore.Migrations;

namespace SALGADBLib.Migrations
{
    public partial class QuestionAdditionalComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApproverComments",
                table: "QuestionnaireQuestionAnswers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SALGAComments",
                table: "QuestionnaireQuestionAnswers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApproverComments",
                table: "QuestionnaireQuestionAnswers");

            migrationBuilder.DropColumn(
                name: "SALGAComments",
                table: "QuestionnaireQuestionAnswers");
        }
    }
}
