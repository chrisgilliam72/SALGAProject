using Microsoft.EntityFrameworkCore.Migrations;

namespace SALGADBLib.Migrations
{
    public partial class AnswerEvidence : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EvidencePath",
                table: "QuestionnaireQuestionAnswers");

            migrationBuilder.DropColumn(
                name: "OriginalEvidenceFileName",
                table: "QuestionnaireQuestionAnswers");

            migrationBuilder.AddColumn<int>(
                name: "AnswerEvidencepkID",
                table: "QuestionnaireQuestionAnswers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AnswerEvidences",
                columns: table => new
                {
                    pkID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OriginalFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlobName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContainerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MunicipalitypkID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerEvidences", x => x.pkID);
                    table.ForeignKey(
                        name: "FK_AnswerEvidences_Municipalities_MunicipalitypkID",
                        column: x => x.MunicipalitypkID,
                        principalTable: "Municipalities",
                        principalColumn: "pkID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionnaireQuestionAnswers_AnswerEvidencepkID",
                table: "QuestionnaireQuestionAnswers",
                column: "AnswerEvidencepkID");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerEvidences_MunicipalitypkID",
                table: "AnswerEvidences",
                column: "MunicipalitypkID");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionnaireQuestionAnswers_AnswerEvidences_AnswerEvidencepkID",
                table: "QuestionnaireQuestionAnswers",
                column: "AnswerEvidencepkID",
                principalTable: "AnswerEvidences",
                principalColumn: "pkID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionnaireQuestionAnswers_AnswerEvidences_AnswerEvidencepkID",
                table: "QuestionnaireQuestionAnswers");

            migrationBuilder.DropTable(
                name: "AnswerEvidences");

            migrationBuilder.DropIndex(
                name: "IX_QuestionnaireQuestionAnswers_AnswerEvidencepkID",
                table: "QuestionnaireQuestionAnswers");

            migrationBuilder.DropColumn(
                name: "AnswerEvidencepkID",
                table: "QuestionnaireQuestionAnswers");

            migrationBuilder.AddColumn<string>(
                name: "EvidencePath",
                table: "QuestionnaireQuestionAnswers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginalEvidenceFileName",
                table: "QuestionnaireQuestionAnswers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
