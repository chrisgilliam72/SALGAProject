using Microsoft.EntityFrameworkCore.Migrations;

namespace SALGADBLib.Migrations
{
    public partial class CustomAnswerTypesScoring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomResponse",
                table: "QuestionnaireQuestionAnswers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Level1",
                table: "CustomReponseTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Level2",
                table: "CustomReponseTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Level3",
                table: "CustomReponseTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Level4",
                table: "CustomReponseTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomResponse",
                table: "QuestionnaireQuestionAnswers");

            migrationBuilder.DropColumn(
                name: "Level1",
                table: "CustomReponseTypes");

            migrationBuilder.DropColumn(
                name: "Level2",
                table: "CustomReponseTypes");

            migrationBuilder.DropColumn(
                name: "Level3",
                table: "CustomReponseTypes");

            migrationBuilder.DropColumn(
                name: "Level4",
                table: "CustomReponseTypes");
        }
    }
}
