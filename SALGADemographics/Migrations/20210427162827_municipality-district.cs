using Microsoft.EntityFrameworkCore.Migrations;

namespace SALGADBLib.Migrations
{
    public partial class municipalitydistrict : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DistrictpkID",
                table: "Municipalities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Municipalities_DistrictpkID",
                table: "Municipalities",
                column: "DistrictpkID");

            migrationBuilder.AddForeignKey(
                name: "FK_Municipalities_Municipalities_DistrictpkID",
                table: "Municipalities",
                column: "DistrictpkID",
                principalTable: "Municipalities",
                principalColumn: "pkID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Municipalities_Municipalities_DistrictpkID",
                table: "Municipalities");

            migrationBuilder.DropIndex(
                name: "IX_Municipalities_DistrictpkID",
                table: "Municipalities");

            migrationBuilder.DropColumn(
                name: "DistrictpkID",
                table: "Municipalities");
        }
    }
}
