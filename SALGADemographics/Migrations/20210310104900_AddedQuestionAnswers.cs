using Microsoft.EntityFrameworkCore.Migrations;

namespace SALGADBLib.Migrations
{
    public partial class AddedQuestionAnswers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuestionnaireQuestionAnswers",
                columns: table => new
                {
                    pkID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionpkID = table.Column<int>(type: "int", nullable: true),
                    ResponseTypepkID = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EvidencePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionnaireQuestionAnswers", x => x.pkID);
                    table.ForeignKey(
                        name: "FK_QuestionnaireQuestionAnswers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionnaireQuestionAnswers_QuestionnaireQuestions_QuestionpkID",
                        column: x => x.QuestionpkID,
                        principalTable: "QuestionnaireQuestions",
                        principalColumn: "pkID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionnaireQuestionAnswers_QuestionnaireResponseTypes_ResponseTypepkID",
                        column: x => x.ResponseTypepkID,
                        principalTable: "QuestionnaireResponseTypes",
                        principalColumn: "pkID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionnaireQuestionAnswers_QuestionpkID",
                table: "QuestionnaireQuestionAnswers",
                column: "QuestionpkID");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionnaireQuestionAnswers_ResponseTypepkID",
                table: "QuestionnaireQuestionAnswers",
                column: "ResponseTypepkID");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionnaireQuestionAnswers_UserId",
                table: "QuestionnaireQuestionAnswers",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionnaireQuestionAnswers");
        }
    }
}
