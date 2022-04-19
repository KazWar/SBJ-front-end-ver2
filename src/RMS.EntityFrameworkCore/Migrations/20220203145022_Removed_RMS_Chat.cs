using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RMS.Migrations
{
    public partial class Removed_RMS_Chat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RmsChatMessage");

            migrationBuilder.DropTable(
                name: "ChatConversation");

            migrationBuilder.DropTable(
                name: "ChatEvent");

            migrationBuilder.AlterColumn<long>(
                name: "CountryId",
                table: "Registration",
                nullable: false,
                defaultValueSql: "(0)",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldDefaultValueSql: "(CONVERT([bigint],(0)))");

            migrationBuilder.AlterColumn<long>(
                name: "CampaignId",
                table: "Registration",
                nullable: false,
                defaultValueSql: "(0)",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldDefaultValueSql: "(CONVERT([bigint],(0)))");

            migrationBuilder.AlterColumn<long>(
                name: "ProductHandlingId",
                table: "HandlingLine",
                nullable: false,
                defaultValueSql: "(0)",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldDefaultValueSql: "(CONVERT([bigint],(0)))");

            migrationBuilder.AlterColumn<long>(
                name: "FormLocaleId",
                table: "FormBlock",
                nullable: false,
                defaultValueSql: "(0)",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldDefaultValueSql: "(CONVERT([bigint],(0)))");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Campaign",
                nullable: false,
                defaultValueSql: "(0)",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValueSql: "(CONVERT([bit],(0)))");

            migrationBuilder.CreateTable(
                name: "UniqueCodeByCampaign",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Used = table.Column<bool>(nullable: false),
                    CampaignId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UniqueCodeByCampaign", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UniqueCodeByCampaign_Campaign_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaign",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UniqueCodeByCampaign_CampaignId",
                table: "UniqueCodeByCampaign",
                column: "CampaignId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UniqueCodeByCampaign");

            migrationBuilder.AlterColumn<long>(
                name: "CountryId",
                table: "Registration",
                type: "bigint",
                nullable: false,
                defaultValueSql: "(CONVERT([bigint],(0)))",
                oldClrType: typeof(long),
                oldDefaultValueSql: "(0)");

            migrationBuilder.AlterColumn<long>(
                name: "CampaignId",
                table: "Registration",
                type: "bigint",
                nullable: false,
                defaultValueSql: "(CONVERT([bigint],(0)))",
                oldClrType: typeof(long),
                oldDefaultValueSql: "(0)");

            migrationBuilder.AlterColumn<long>(
                name: "ProductHandlingId",
                table: "HandlingLine",
                type: "bigint",
                nullable: false,
                defaultValueSql: "(CONVERT([bigint],(0)))",
                oldClrType: typeof(long),
                oldDefaultValueSql: "(0)");

            migrationBuilder.AlterColumn<long>(
                name: "FormLocaleId",
                table: "FormBlock",
                type: "bigint",
                nullable: false,
                defaultValueSql: "(CONVERT([bigint],(0)))",
                oldClrType: typeof(long),
                oldDefaultValueSql: "(0)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Campaign",
                type: "bit",
                nullable: false,
                defaultValueSql: "(CONVERT([bit],(0)))",
                oldClrType: typeof(bool),
                oldDefaultValueSql: "(0)");

            migrationBuilder.CreateTable(
                name: "ChatConversation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    RegistrationId = table.Column<long>(type: "bigint", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatConversation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatConversation_Registration_RegistrationId",
                        column: x => x.RegistrationId,
                        principalTable: "Registration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChatEvent",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatEvent", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RmsChatMessage",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChatConversationId = table.Column<long>(type: "bigint", nullable: false),
                    ChatEventId = table.Column<long>(type: "bigint", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GroupName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    SharedRmsChatMessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Side = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RmsChatMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RmsChatMessage_ChatConversation_ChatConversationId",
                        column: x => x.ChatConversationId,
                        principalTable: "ChatConversation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RmsChatMessage_ChatEvent_ChatEventId",
                        column: x => x.ChatEventId,
                        principalTable: "ChatEvent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatConversation_RegistrationId",
                table: "ChatConversation",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatConversation_TenantId",
                table: "ChatConversation",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatEvent_TenantId",
                table: "ChatEvent",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_RmsChatMessage_ChatConversationId",
                table: "RmsChatMessage",
                column: "ChatConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_RmsChatMessage_ChatEventId",
                table: "RmsChatMessage",
                column: "ChatEventId");

            migrationBuilder.CreateIndex(
                name: "IX_RmsChatMessage_TenantId",
                table: "RmsChatMessage",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_RmsChatMessage_UserId",
                table: "RmsChatMessage",
                column: "UserId");
        }
    }
}
