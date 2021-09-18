using Microsoft.EntityFrameworkCore.Migrations;

namespace SALGADBLib.Migrations
{
    public partial class ProvinceMunicipality : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProvincepkID",
                table: "Municipalities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Provinces",
                columns: table => new
                {
                    pkID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provinces", x => x.pkID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Municipalities_ProvincepkID",
                table: "Municipalities",
                column: "ProvincepkID");

            migrationBuilder.AddForeignKey(
                name: "FK_Municipalities_Provinces_ProvincepkID",
                table: "Municipalities",
                column: "ProvincepkID",
                principalTable: "Provinces",
                principalColumn: "pkID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Municipalities_Provinces_ProvincepkID",
                table: "Municipalities");

            migrationBuilder.DropTable(
                name: "Provinces");

            migrationBuilder.DropIndex(
                name: "IX_Municipalities_ProvincepkID",
                table: "Municipalities");

            migrationBuilder.DropColumn(
                name: "ProvincepkID",
                table: "Municipalities");
        }
    }
}
