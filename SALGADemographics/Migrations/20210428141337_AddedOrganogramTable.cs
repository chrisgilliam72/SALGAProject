using Microsoft.EntityFrameworkCore.Migrations;

namespace SALGADBLib.Migrations
{
    public partial class AddedOrganogramTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Organogram",
                table: "MunicipalityDemographics",
                newName: "OrganogramImage");

            migrationBuilder.AddColumn<int>(
                name: "OrganogrampkID",
                table: "MunicipalityDemographics",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Organograms",
                columns: table => new
                {
                    pkID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExecutiveMayor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityManager = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    COO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DirectorHR = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organograms", x => x.pkID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MunicipalityDemographics_OrganogrampkID",
                table: "MunicipalityDemographics",
                column: "OrganogrampkID");

            migrationBuilder.AddForeignKey(
                name: "FK_MunicipalityDemographics_Organograms_OrganogrampkID",
                table: "MunicipalityDemographics",
                column: "OrganogrampkID",
                principalTable: "Organograms",
                principalColumn: "pkID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MunicipalityDemographics_Organograms_OrganogrampkID",
                table: "MunicipalityDemographics");

            migrationBuilder.DropTable(
                name: "Organograms");

            migrationBuilder.DropIndex(
                name: "IX_MunicipalityDemographics_OrganogrampkID",
                table: "MunicipalityDemographics");

            migrationBuilder.DropColumn(
                name: "OrganogrampkID",
                table: "MunicipalityDemographics");

            migrationBuilder.RenameColumn(
                name: "OrganogramImage",
                table: "MunicipalityDemographics",
                newName: "Organogram");
        }
    }
}
