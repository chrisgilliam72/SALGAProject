using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SALGADBLib.Migrations
{
    public partial class AssessmentTracking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TrackingpkID",
                table: "QuestionnaireQuestionAnswers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AssessmentTrackings",
                columns: table => new
                {
                    pkID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsSubmitted = table.Column<bool>(type: "bit", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    SubmittedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApproverId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentTrackings", x => x.pkID);
                    table.ForeignKey(
                        name: "FK_AssessmentTrackings_AspNetUsers_ApproverId",
                        column: x => x.ApproverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionnaireQuestionAnswers_TrackingpkID",
                table: "QuestionnaireQuestionAnswers",
                column: "TrackingpkID");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentTrackings_ApproverId",
                table: "AssessmentTrackings",
                column: "ApproverId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionnaireQuestionAnswers_AssessmentTrackings_TrackingpkID",
                table: "QuestionnaireQuestionAnswers",
                column: "TrackingpkID",
                principalTable: "AssessmentTrackings",
                principalColumn: "pkID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionnaireQuestionAnswers_AssessmentTrackings_TrackingpkID",
                table: "QuestionnaireQuestionAnswers");

            migrationBuilder.DropTable(
                name: "AssessmentTrackings");

            migrationBuilder.DropIndex(
                name: "IX_QuestionnaireQuestionAnswers_TrackingpkID",
                table: "QuestionnaireQuestionAnswers");

            migrationBuilder.DropColumn(
                name: "TrackingpkID",
                table: "QuestionnaireQuestionAnswers");
        }
    }
}
