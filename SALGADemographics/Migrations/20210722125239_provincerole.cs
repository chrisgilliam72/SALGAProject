using Microsoft.EntityFrameworkCore.Migrations;

namespace SALGADBLib.Migrations
{
    public partial class provincerole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DasboardProvinceAccesses",
                columns: table => new
                {
                    pkID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RolepkID = table.Column<int>(type: "int", nullable: true),
                    ProvincepkID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DasboardProvinceAccesses", x => x.pkID);
                    table.ForeignKey(
                        name: "FK_DasboardProvinceAccesses_DasboardLevelRoles_RolepkID",
                        column: x => x.RolepkID,
                        principalTable: "DasboardLevelRoles",
                        principalColumn: "pkID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DasboardProvinceAccesses_Provinces_ProvincepkID",
                        column: x => x.ProvincepkID,
                        principalTable: "Provinces",
                        principalColumn: "pkID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DasboardProvinceAccesses_ProvincepkID",
                table: "DasboardProvinceAccesses",
                column: "ProvincepkID");

            migrationBuilder.CreateIndex(
                name: "IX_DasboardProvinceAccesses_RolepkID",
                table: "DasboardProvinceAccesses",
                column: "RolepkID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DasboardProvinceAccesses");
        }
    }
}
