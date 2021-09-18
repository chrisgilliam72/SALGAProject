using Microsoft.EntityFrameworkCore.Migrations;

namespace SALGADBLib.Migrations
{
    public partial class responsetypevisibility : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "VisibleToApprover",
                table: "QuestionnaireResponseTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "VisibleToUser",
                table: "QuestionnaireResponseTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VisibleToApprover",
                table: "QuestionnaireResponseTypes");

            migrationBuilder.DropColumn(
                name: "VisibleToUser",
                table: "QuestionnaireResponseTypes");
        }
    }
}
