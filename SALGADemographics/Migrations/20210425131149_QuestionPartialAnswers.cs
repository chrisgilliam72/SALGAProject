using Microsoft.EntityFrameworkCore.Migrations;

namespace SALGADBLib.Migrations
{
    public partial class QuestionPartialAnswers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Level1Partial",
                table: "QuestionnaireQuestions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Level2Partial",
                table: "QuestionnaireQuestions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Level3Partial",
                table: "QuestionnaireQuestions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Level4Partial",
                table: "QuestionnaireQuestions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Level1Partial",
                table: "QuestionnaireQuestions");

            migrationBuilder.DropColumn(
                name: "Level2Partial",
                table: "QuestionnaireQuestions");

            migrationBuilder.DropColumn(
                name: "Level3Partial",
                table: "QuestionnaireQuestions");

            migrationBuilder.DropColumn(
                name: "Level4Partial",
                table: "QuestionnaireQuestions");
        }
    }
}
