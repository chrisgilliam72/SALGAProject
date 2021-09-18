using Microsoft.EntityFrameworkCore.Migrations;

namespace SALGADBLib.Migrations
{
    public partial class QuestionnaireResponseTypeRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
   
            migrationBuilder.AlterColumn<string>(
                name: "ResponseType",
                table: "QuestionnaireResponseTypes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ResponseTypepkID",
                table: "QuestionnaireQuestions",
                type: "int",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionnaireQuestions_QuestionnaireResponseTypes_ResponseTypepkID",
                table: "QuestionnaireQuestions");

            migrationBuilder.DropIndex(
                name: "IX_QuestionnaireQuestions_ResponseTypepkID",
                table: "QuestionnaireQuestions");

            migrationBuilder.DropColumn(
                name: "ResponseTypepkID",
                table: "QuestionnaireQuestions");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "JobTitles",
                newName: "Description");

            migrationBuilder.AlterColumn<int>(
                name: "ResponseType",
                table: "QuestionnaireResponseTypes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
