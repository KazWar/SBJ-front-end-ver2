using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RMS.Migrations
{
    public partial class Added_Handling_Batches : Migration
    {
        protected override void Up([NotNull] MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HandlingBatchStatus",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    StatusCode = table.Column<string>(nullable: false),
                    StatusDescription = table.Column<string>(nullable: false)
                },
                constraints: table => 
                {
                    table.PrimaryKey("PK_HandlingBatchStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HandlingBatchLineStatus",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    StatusCode = table.Column<string>(nullable: false),
                    StatusDescription = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HandlingBatchLineStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HandlingBatch",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    CampaignTypeId = table.Column<long>(nullable: false),
                    HandlingBatchStatusId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HandlingBatch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HandlingBatch_CampaignType_CampaignTypeId",
                        column: x => x.CampaignTypeId,
                        principalTable: "CampaignType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HandlingBatch_HandlingBatchStatus_HandlingBatchStatusId",
                        column: x => x.HandlingBatchStatusId,
                        principalTable: "HandlingBatchStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HandlingBatchHistory",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    AbpUserId = table.Column<long>(nullable: false),
                    HandlingBatchId = table.Column<long>(nullable: false),
                    HandlingBatchStatusId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HandlingBatchHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HandlingBatchHistory_HandlingBatch_HandlingBatchId",
                        column: x => x.HandlingBatchId,
                        principalTable: "HandlingBatch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HandlingBatchHistory_HandlingBatchStatus_HandlingBatchStatusId",
                        column: x => x.HandlingBatchStatusId,
                        principalTable: "HandlingBatchStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HandlingBatchLine",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    ExternalOrderId = table.Column<string>(nullable: true),
                    CustomerCode = table.Column<string>(nullable: true),
                    Quantity = table.Column<int>(nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    ActivationCodeId = table.Column<long?>(nullable: true),
                    HandlingBatchId = table.Column<long>(nullable: false),
                    PurchaseRegistrationId = table.Column<long>(nullable: false),
                    HandlingBatchLineStatusId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HandlingBatchLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HandlingBatchLine_ActivationCode_ActivationCodeId",
                        column: x => x.ActivationCodeId,
                        principalTable: "ActivationCode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HandlingBatchLine_HandlingBatch_HandlingBatchId",
                        column: x => x.HandlingBatchId,
                        principalTable: "HandlingBatch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HandlingBatchLine_PurchaseRegistration_PurchaseRegistrationId",
                        column: x => x.PurchaseRegistrationId,
                        principalTable: "PurchaseRegistration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HandlingBatchLine_HandlingBatchLineStatus_HandlingBatchLineStatusId",
                        column: x => x.HandlingBatchLineStatusId,
                        principalTable: "HandlingBatchLineStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HandlingBatchLineHistory",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    AbpUserId = table.Column<long>(nullable: false),
                    HandlingBatchLineId = table.Column<long>(nullable: false),
                    HandlingBatchLineStatusId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HandlingBatchLineHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HandlingBatchLineHistory_HandlingBatchLine_HandlingBatchLineId",
                        column: x => x.HandlingBatchLineId,
                        principalTable: "HandlingBatchLine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HandlingBatchLineHistory_HandlingBatchLineStatus_HandlingBatchLineStatusId",
                        column: x => x.HandlingBatchLineStatusId,
                        principalTable: "HandlingBatchLineStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UniqueCode",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nchar(10)", nullable: false),
                    Used = table.Column<bool>(nullable: false)
                });

            migrationBuilder.CreateIndex(
                name: "IX_HandlingBatchStatus_TenantId",
                table: "HandlingBatchStatus",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_HandlingBatchLineStatus_TenantId",
                table: "HandlingBatchLineStatus",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_HandlingBatch_TenantId",
                table: "HandlingBatch",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_HandlingBatch_CampaignTypeId",
                table: "HandlingBatch",
                column: "CampaignTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_HandlingBatch_HandlingBatchStatusId",
                table: "HandlingBatch",
                column: "HandlingBatchStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_HandlingBatchHistory_TenantId",
                table: "HandlingBatchHistory",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_HandlingBatchHistory_HandlingBatchId",
                table: "HandlingBatchHistory",
                column: "HandlingBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_HandlingBatchHistory_HandlingBatchStatusId",
                table: "HandlingBatchHistory",
                column: "HandlingBatchStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_HandlingBatchLine_TenantId",
                table: "HandlingBatchLine",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_HandlingBatchLine_ActivationCodeId",
                table: "HandlingBatchLine",
                column: "ActivationCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_HandlingBatchLine_HandlingBatchId",
                table: "HandlingBatchLine",
                column: "HandlingBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_HandlingBatchLine_PurchaseRegistrationId",
                table: "HandlingBatchLine",
                column: "PurchaseRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_HandlingBatchLine_HandlingBatchLineStatusId",
                table: "HandlingBatchLine",
                column: "HandlingBatchLineStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_HandlingBatchLineHistory_TenantId",
                table: "HandlingBatchLineHistory",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_HandlingBatchLineHistory_HandlingBatchLineId",
                table: "HandlingBatchLineHistory",
                column: "HandlingBatchLineId");

            migrationBuilder.CreateIndex(
                name: "IX_HandlingBatchLineHistory_HandlingBatchLineStatusId",
                table: "HandlingBatchLineHistory",
                column: "HandlingBatchLineStatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("HandlingBatchStatus");
            migrationBuilder.DropTable("HandlingBatchLineStatus");
            migrationBuilder.DropTable("HandlingBatch");
            migrationBuilder.DropTable("HandlingBatchHistory");
            migrationBuilder.DropTable("HandlingBatchLine");
            migrationBuilder.DropTable("HandlingBatchLineHistory");
            migrationBuilder.DropTable("UniqueCode");
        }
    }
}
