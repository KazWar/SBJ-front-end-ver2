using Microsoft.EntityFrameworkCore.Migrations;

namespace RMS.Migrations
{
    public partial class Added_Campaign_Country : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CampaignCountry",
                columns: table => new 
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    CampaignId = table.Column<long>(nullable: false),
                    CountryId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignCountry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampaignCountry_Campaign_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaign",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CampaignCountry_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CampaignCountry_CampaignId",
                table: "CampaignCountry",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignCountry_CountryId",
                table: "CampaignCountry",
                column: "CountryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("CampaignCountry");
        }
    }
}
