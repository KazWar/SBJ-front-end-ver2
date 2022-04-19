using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RMS.Migrations
{
    public partial class Added_Product_Gift : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductGift",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    ProductCode = table.Column<string>(nullable: true),
                    GiftId = table.Column<long>(nullable: false),
                    GiftName = table.Column<string>(nullable: true),
                    TotalPoints = table.Column<int>(nullable: false),
                    CampaignId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductGift", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductGift_Campaign_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaign",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductGift_CampaignId",
                table: "ProductGift",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductGift_TenantId",
                table: "ProductGift",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductGift");
        }
    }
}
