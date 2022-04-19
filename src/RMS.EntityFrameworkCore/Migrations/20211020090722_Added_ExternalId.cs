using Microsoft.EntityFrameworkCore.Migrations;

namespace RMS.Migrations
{
    public partial class Added_ExternalId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "RetailerLocation",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "Campaign",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "RetailerLocation");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "Campaign");
        }
    }
}
