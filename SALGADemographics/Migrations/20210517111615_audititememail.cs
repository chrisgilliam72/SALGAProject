using Microsoft.EntityFrameworkCore.Migrations;

namespace SALGADBLib.Migrations
{
    public partial class audititememail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemAuditTrackings_AspNetUsers_AccessedById",
                table: "ItemAuditTrackings");

            migrationBuilder.DropIndex(
                name: "IX_ItemAuditTrackings_AccessedById",
                table: "ItemAuditTrackings");

            migrationBuilder.DropColumn(
                name: "AccessedById",
                table: "ItemAuditTrackings");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "ItemAuditTrackings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserIDString",
                table: "ItemAuditTrackings",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "ItemAuditTrackings");

            migrationBuilder.DropColumn(
                name: "UserIDString",
                table: "ItemAuditTrackings");

            migrationBuilder.AddColumn<string>(
                name: "AccessedById",
                table: "ItemAuditTrackings",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemAuditTrackings_AccessedById",
                table: "ItemAuditTrackings",
                column: "AccessedById");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemAuditTrackings_AspNetUsers_AccessedById",
                table: "ItemAuditTrackings",
                column: "AccessedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
