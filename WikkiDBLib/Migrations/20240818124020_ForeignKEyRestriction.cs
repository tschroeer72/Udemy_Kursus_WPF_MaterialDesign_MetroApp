using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WikkiDBLib.Migrations
{
    public partial class ForeignKEyRestriction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Person_Stadt_StadtID",
                table: "Person");

            migrationBuilder.AddForeignKey(
                name: "FK_Person_Stadt_StadtID",
                table: "Person",
                column: "StadtID",
                principalTable: "Stadt",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Person_Stadt_StadtID",
                table: "Person");

            migrationBuilder.AddForeignKey(
                name: "FK_Person_Stadt_StadtID",
                table: "Person",
                column: "StadtID",
                principalTable: "Stadt",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
