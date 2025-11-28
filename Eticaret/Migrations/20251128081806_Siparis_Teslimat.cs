using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eticaret.Migrations
{
    /// <inheritdoc />
    public partial class Siparis_Teslimat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeslimatSecenegiId",
                table: "Siparisler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Siparisler_TeslimatSecenegiId",
                table: "Siparisler",
                column: "TeslimatSecenegiId");

            migrationBuilder.AddForeignKey(
                name: "FK_Siparisler_TeslimatSecenekleri_TeslimatSecenegiId",
                table: "Siparisler",
                column: "TeslimatSecenegiId",
                principalTable: "TeslimatSecenekleri",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Siparisler_TeslimatSecenekleri_TeslimatSecenegiId",
                table: "Siparisler");

            migrationBuilder.DropIndex(
                name: "IX_Siparisler_TeslimatSecenegiId",
                table: "Siparisler");

            migrationBuilder.DropColumn(
                name: "TeslimatSecenegiId",
                table: "Siparisler");
        }
    }
}
