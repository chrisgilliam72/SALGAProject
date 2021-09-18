using Microsoft.EntityFrameworkCore.Migrations;

namespace SALGADBLib.Migrations
{
    public partial class assmentshistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.AddColumn<int>(
                name: "QuestionSetpkID",
                table: "QuestionnaireQuestions",
                type: "int",
                nullable: true);


            migrationBuilder.CreateTable(
                name: "QuestionSets",
                columns: table => new
                {
                    pkID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Version = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionSets", x => x.pkID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionnaireQuestions_QuestionSetpkID",
                table: "QuestionnaireQuestions",
                column: "QuestionSetpkID");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionnaireQuestions_QuestionSets_QuestionSetpkID",
                table: "QuestionnaireQuestions",
                column: "QuestionSetpkID",
                principalTable: "QuestionSets",
                principalColumn: "pkID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionnaireQuestions_QuestionSets_QuestionSetpkID",
                table: "QuestionnaireQuestions");

            migrationBuilder.DropTable(
                name: "QuestionSets");

            migrationBuilder.DropIndex(
                name: "IX_QuestionnaireQuestions_QuestionSetpkID",
                table: "QuestionnaireQuestions");


            migrationBuilder.DropColumn(
                name: "QuestionSetpkID",
                table: "QuestionnaireQuestions");

        }
    }
}
