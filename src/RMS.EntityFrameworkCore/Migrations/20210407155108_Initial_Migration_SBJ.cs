using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RMS.Migrations
{
    public partial class Initial_Migration_SBJ : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Campaign",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false, defaultValueSql: "('0001-01-01T00:00:00.0000000')"),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "(CONVERT([bit],(0)))"),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false, defaultValueSql: "('0001-01-01T00:00:00.0000000')"),
                    EndDate = table.Column<DateTime>(nullable: false, defaultValueSql: "('0001-01-01T00:00:00.0000000')"),
                    CampaignCode = table.Column<int>(nullable: true),
                    ExternalCode = table.Column<string>(nullable: true),
                    ThumbnailImagePath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaign", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CampaignCategory",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    SortOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CampaignType",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Code = table.Column<string>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChatEvent",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatEvent", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    CountryCode = table.Column<string>(maxLength: 2, nullable: false),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FieldType",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HandlingLineLogic",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    FirstHandlingLineId = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Operator = table.Column<string>(nullable: false),
                    SecondHandlingLineId = table.Column<decimal>(type: "decimal(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HandlingLineLogic", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MakitaSerialNumber",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    ProductCode = table.Column<string>(nullable: true),
                    SerialNumber = table.Column<string>(nullable: true),
                    RetailerExternalCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MakitaSerialNumber", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MessageComponentType",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageComponentType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MessageHistory",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    RegistrationId = table.Column<long>(nullable: false),
                    AbpUserId = table.Column<long>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    MessageName = table.Column<string>(nullable: true),
                    MessageId = table.Column<long>(nullable: false),
                    Subject = table.Column<string>(nullable: true),
                    To = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MessageVariable",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: false),
                    RmsTable = table.Column<string>(nullable: false),
                    TableField = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageVariable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProcessEvent",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessEvent", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategory",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    PoHandling = table.Column<string>(nullable: true),
                    PoCashBack = table.Column<string>(nullable: true),
                    Color = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationStatus",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    StatusCode = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RejectionReason",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RejectionReason", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemLevel",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemLevel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ValueList",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ListValueApiCall = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValueList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductHandling",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CampaignId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductHandling", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductHandling_Campaign_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaign",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CampaignCampaignType",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CampaignId = table.Column<long>(nullable: false),
                    CampaignTypeId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignCampaignType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampaignCampaignType_Campaign_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaign",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CampaignCampaignType_CampaignType_CampaignTypeId",
                        column: x => x.CampaignTypeId,
                        principalTable: "CampaignType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    AddressLine1 = table.Column<string>(nullable: false),
                    AddressLine2 = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(maxLength: 10, nullable: false),
                    City = table.Column<string>(nullable: false),
                    CountryId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Address_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Locale",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    LanguageCode = table.Column<string>(maxLength: 2, nullable: false),
                    Description = table.Column<string>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CountryId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locale", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Locale_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CampaignTranslation",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    CampaignId = table.Column<long>(nullable: false),
                    LocaleId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampaignTranslation_Campaign_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaign",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CampaignTranslation_Locale_LocaleId",
                        column: x => x.LocaleId,
                        principalTable: "Locale",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Retailer",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    CountryId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Retailer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Retailer_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FormField",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Label = table.Column<string>(nullable: true),
                    DefaultValue = table.Column<string>(nullable: true),
                    MaxLength = table.Column<int>(nullable: false),
                    Required = table.Column<bool>(nullable: false),
                    ReadOnly = table.Column<bool>(nullable: false),
                    InputMask = table.Column<string>(nullable: true),
                    RegularExpression = table.Column<string>(nullable: true),
                    ValidationApiCall = table.Column<string>(nullable: true),
                    RegistrationField = table.Column<string>(nullable: true),
                    PurchaseRegistrationField = table.Column<string>(nullable: true),
                    FieldName = table.Column<string>(nullable: false),
                    FieldTypeId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormField", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormField_FieldType_FieldTypeId",
                        column: x => x.FieldTypeId,
                        principalTable: "FieldType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CampaignTypeEvent",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    SortOrder = table.Column<int>(nullable: false),
                    CampaignTypeId = table.Column<long>(nullable: false),
                    ProcessEventId = table.Column<long>(nullable: false),
                    CampaignTypeFkId = table.Column<long>(nullable: true),
                    ProcessEventFkId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignTypeEvent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampaignTypeEvent_CampaignType_CampaignTypeFkId",
                        column: x => x.CampaignTypeFkId,
                        principalTable: "CampaignType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CampaignTypeEvent_CampaignType_CampaignTypeId",
                        column: x => x.CampaignTypeId,
                        principalTable: "CampaignType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CampaignTypeEvent_ProcessEvent_ProcessEventFkId",
                        column: x => x.ProcessEventFkId,
                        principalTable: "ProcessEvent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CampaignTypeEvent_ProcessEvent_ProcessEventId",
                        column: x => x.ProcessEventId,
                        principalTable: "ProcessEvent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    ProductCode = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Ean = table.Column<string>(nullable: true),
                    ProductCategoryId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_ProductCategory_ProductCategoryId",
                        column: x => x.ProductCategoryId,
                        principalTable: "ProductCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategoryYearPo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    Year = table.Column<int>(nullable: false),
                    PoNumberHandling = table.Column<string>(nullable: true),
                    PoNumberCashback = table.Column<string>(nullable: true),
                    ProductCategoryId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategoryYearPo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCategoryYearPo_ProductCategory_ProductCategoryId",
                        column: x => x.ProductCategoryId,
                        principalTable: "ProductCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Form",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Version = table.Column<string>(nullable: true),
                    SystemLevelId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Form", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Form_SystemLevel_SystemLevelId",
                        column: x => x.SystemLevelId,
                        principalTable: "SystemLevel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Version = table.Column<string>(maxLength: 20, nullable: false),
                    SystemLevelId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Message_SystemLevel_SystemLevelId",
                        column: x => x.SystemLevelId,
                        principalTable: "SystemLevel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ListValue",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    KeyValue = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    SortOrder = table.Column<int>(nullable: false),
                    ValueListId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListValue_ValueList_ValueListId",
                        column: x => x.ValueListId,
                        principalTable: "ValueList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HandlingLine",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    MinimumPurchaseAmount = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    MaximumPurchaseAmount = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    CustomerCode = table.Column<string>(nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    Fixed = table.Column<bool>(nullable: false),
                    Percentage = table.Column<bool>(nullable: false),
                    ActivationCode = table.Column<bool>(nullable: false),
                    Quantity = table.Column<int>(nullable: true),
                    PremiumDescription = table.Column<string>(nullable: true),
                    CampaignTypeId = table.Column<long>(nullable: false),
                    ProductHandlingId = table.Column<long>(nullable: false, defaultValueSql: "(CONVERT([bigint],(0)))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HandlingLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HandlingLine_CampaignType_CampaignTypeId",
                        column: x => x.CampaignTypeId,
                        principalTable: "CampaignType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HandlingLine_ProductHandling_ProductHandlingId",
                        column: x => x.ProductHandlingId,
                        principalTable: "ProductHandling",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 1000, nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    EmailAddress = table.Column<string>(nullable: true),
                    BicCashBack = table.Column<string>(nullable: true),
                    IbanCashBack = table.Column<string>(nullable: true),
                    AddressId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Company_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectManager",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 50, nullable: true),
                    EmailAddress = table.Column<string>(nullable: true),
                    AddressId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectManager", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectManager_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivationCode",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    CampaignId = table.Column<long>(nullable: false),
                    LocaleId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivationCode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivationCode_Campaign_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaign",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActivationCode_Locale_LocaleId",
                        column: x => x.LocaleId,
                        principalTable: "Locale",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CampaignCategoryTranslation",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    LocaleId = table.Column<long>(nullable: false),
                    CampaignCategoryId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignCategoryTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampaignCategoryTranslation_CampaignCategory_CampaignCategoryId",
                        column: x => x.CampaignCategoryId,
                        principalTable: "CampaignCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CampaignCategoryTranslation_Locale_LocaleId",
                        column: x => x.LocaleId,
                        principalTable: "Locale",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RejectionReasonTranslation",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: false),
                    LocaleId = table.Column<long>(nullable: false),
                    RejectionReasonId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RejectionReasonTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RejectionReasonTranslation_Locale",
                        column: x => x.LocaleId,
                        principalTable: "Locale",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RejectionReasonTranslation_RejectionReason",
                        column: x => x.RejectionReasonId,
                        principalTable: "RejectionReason",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RetailerLocation",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    ExternalCode = table.Column<string>(nullable: true),
                    RetailerId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RetailerLocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RetailerLocation_Retailer_RetailerId",
                        column: x => x.RetailerId,
                        principalTable: "Retailer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FormFieldTranslation",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Label = table.Column<string>(nullable: true),
                    DefaultValue = table.Column<string>(nullable: true),
                    RegularExpression = table.Column<string>(nullable: true),
                    FormFieldId = table.Column<long>(nullable: false),
                    LocaleId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormFieldTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormFieldTranslation_FormField_FormFieldId",
                        column: x => x.FormFieldId,
                        principalTable: "FormField",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormFieldTranslation_Locale_LocaleId",
                        column: x => x.LocaleId,
                        principalTable: "Locale",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FormFieldValueList",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    PossibleListValues = table.Column<string>(nullable: true),
                    FormFieldId = table.Column<long>(nullable: false),
                    ValueListId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormFieldValueList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormFieldValueList_FormField_FormFieldId",
                        column: x => x.FormFieldId,
                        principalTable: "FormField",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormFieldValueList_ValueList_ValueListId",
                        column: x => x.ValueListId,
                        principalTable: "ValueList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseRegistrationField",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    FormFieldId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseRegistrationField", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseRegistrationField_FormField_FormFieldId",
                        column: x => x.FormFieldId,
                        principalTable: "FormField",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseRegistrationFormField",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    FormFieldId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseRegistrationFormField", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseRegistrationFormField_FormField_FormFieldId",
                        column: x => x.FormFieldId,
                        principalTable: "FormField",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationField",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    FormFieldId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationField", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrationFormField_FormField_FormFieldId",
                        column: x => x.FormFieldId,
                        principalTable: "FormField",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CampaignTypeEventRegistrationStatus",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    SortOrder = table.Column<int>(nullable: false),
                    CampaignTypeEventId = table.Column<long>(nullable: false),
                    RegistrationStatusId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignTypeEventRegistrationStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampaignTypeEventRegistrationStatus_CampaignTypeEvent_CampaignTypeEventId",
                        column: x => x.CampaignTypeEventId,
                        principalTable: "CampaignTypeEvent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CampaignTypeEventRegistrationStatus_RegistrationStatus_RegistrationStatusId",
                        column: x => x.RegistrationStatusId,
                        principalTable: "RegistrationStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CampaignForm",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CampaignId = table.Column<long>(nullable: false),
                    FormId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignForm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampaignForm_Campaign_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaign",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CampaignForm_Form_FormId",
                        column: x => x.FormId,
                        principalTable: "Form",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FormLocale",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    FormId = table.Column<long>(nullable: false),
                    LocaleId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormLocale", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormLocale_Form_FormId",
                        column: x => x.FormId,
                        principalTable: "Form",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormLocale_Locale_LocaleId",
                        column: x => x.LocaleId,
                        principalTable: "Locale",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CampaignMessage",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CampaignId = table.Column<long>(nullable: false),
                    MessageId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampaignMessage_Campaign_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaign",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CampaignMessage_Message_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Message",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MessageType",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Source = table.Column<string>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    MessageId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageType_Message_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Message",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ListValueTranslation",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    KeyValue = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ListValueId = table.Column<long>(nullable: false),
                    LocaleId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListValueTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListValueTranslation_ListValue_ListValueId",
                        column: x => x.ListValueId,
                        principalTable: "ListValue",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ListValueTranslation_Locale_LocaleId",
                        column: x => x.LocaleId,
                        principalTable: "Locale",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HandlingLineLocale",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    HandlingLineId = table.Column<long>(nullable: false),
                    LocaleId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HandlingLineLocale", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HandlingLineLocale_HandlingLine_HandlingLineId",
                        column: x => x.HandlingLineId,
                        principalTable: "HandlingLine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HandlingLineLocale_Locale_LocaleId",
                        column: x => x.LocaleId,
                        principalTable: "Locale",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HandlingLineProduct",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    HandlingLineId = table.Column<long>(nullable: false),
                    ProductId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HandlingLineProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HandlingLineProduct_HandlingLine_HandlingLineId",
                        column: x => x.HandlingLineId,
                        principalTable: "HandlingLine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HandlingLineProduct_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HandlingLineRetailer",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    HandlingLineId = table.Column<long>(nullable: false),
                    RetailerId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HandlingLineRetailer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HandlingLineRetailer_HandlingLine_HandlingLineId",
                        column: x => x.HandlingLineId,
                        principalTable: "HandlingLine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HandlingLineRetailer_Retailer_RetailerId",
                        column: x => x.RetailerId,
                        principalTable: "Retailer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CampaignRetailerLocation",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    CampaignId = table.Column<long>(nullable: false),
                    RetailerLocationId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignRetailerLocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampaignRetailerLocation_Campaign_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaign",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CampaignRetailerLocation_RetailerLocation_RetailerLocationId",
                        column: x => x.RetailerLocationId,
                        principalTable: "RetailerLocation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Registration",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    Street = table.Column<string>(nullable: true),
                    HouseNr = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    EmailAddress = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    CompanyName = table.Column<string>(nullable: true),
                    Bic = table.Column<string>(nullable: true),
                    Iban = table.Column<string>(nullable: true),
                    IncompleteFields = table.Column<string>(nullable: true),
                    RejectedFields = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    CampaignId = table.Column<long>(nullable: false, defaultValueSql: "(CONVERT([bigint],(0)))"),
                    CampaignFormId = table.Column<long>(nullable: false),
                    CountryId = table.Column<long>(nullable: false, defaultValueSql: "(CONVERT([bigint],(0)))"),
                    LocaleId = table.Column<long>(nullable: false),
                    RegistrationStatusId = table.Column<long>(nullable: false),
                    RejectionReasonId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Registration_CampaignForm_CampaignFormId",
                        column: x => x.CampaignFormId,
                        principalTable: "CampaignForm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Registration_Locale_LocaleId",
                        column: x => x.LocaleId,
                        principalTable: "Locale",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Registration_RegistrationStatus_RegistrationStatusId",
                        column: x => x.RegistrationStatusId,
                        principalTable: "RegistrationStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Registration_RejectionReason_RejectionReasonId",
                        column: x => x.RejectionReasonId,
                        principalTable: "RejectionReason",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FormBlock",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsPurchaseRegistration = table.Column<bool>(nullable: false, defaultValueSql: "(CONVERT([bit],(0)))"),
                    SortOrder = table.Column<int>(nullable: false),
                    FormLocaleId = table.Column<long>(nullable: false, defaultValueSql: "(CONVERT([bigint],(0)))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormBlock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormBlock_FormLocale_FormLocaleId",
                        column: x => x.FormLocaleId,
                        principalTable: "FormLocale",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MessageComponent",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    MessageTypeId = table.Column<long>(nullable: false),
                    MessageComponentTypeId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageComponent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageComponent_MessageComponentType_MessageComponentTypeId",
                        column: x => x.MessageComponentTypeId,
                        principalTable: "MessageComponentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MessageComponent_MessageType_MessageTypeId",
                        column: x => x.MessageTypeId,
                        principalTable: "MessageType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivationCodeRegistration",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    ActivationCodeId = table.Column<long>(nullable: false),
                    RegistrationId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivationCodeRegistration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivationCodeRegistration_ActivationCode_ActivationCodeId",
                        column: x => x.ActivationCodeId,
                        principalTable: "ActivationCode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivationCodeRegistration_Registration_RegistrationId",
                        column: x => x.RegistrationId,
                        principalTable: "Registration",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ChatConversation",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: true),
                    RegistrationId = table.Column<long>(nullable: false)
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
                name: "PurchaseRegistration",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    PurchaseDate = table.Column<DateTime>(nullable: false),
                    InvoiceImage = table.Column<string>(nullable: true),
                    InvoiceImagePath = table.Column<string>(nullable: true),
                    HandlingLineId = table.Column<long>(nullable: false),
                    ProductId = table.Column<long>(nullable: false),
                    RegistrationId = table.Column<long>(nullable: false),
                    RetailerLocationId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseRegistration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseRegistration_HandlingLine_HandlingLineId",
                        column: x => x.HandlingLineId,
                        principalTable: "HandlingLine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseRegistration_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseRegistration_Registration_RegistrationId",
                        column: x => x.RegistrationId,
                        principalTable: "Registration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PurchaseRegistration_RetailerLocation_RetailerLocationId",
                        column: x => x.RetailerLocationId,
                        principalTable: "RetailerLocation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationFieldData",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    RegistrationFieldId = table.Column<long>(nullable: false),
                    RegistrationId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationFieldData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrationFieldData_RegistrationField_RegistrationFieldId",
                        column: x => x.RegistrationFieldId,
                        principalTable: "RegistrationField",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegistrationFieldData_Registration_RegistrationId",
                        column: x => x.RegistrationId,
                        principalTable: "Registration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationHistory",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    Remarks = table.Column<string>(unicode: false, nullable: true),
                    AbpUserId = table.Column<long>(nullable: false),
                    RegistrationId = table.Column<long>(nullable: false),
                    RegistrationStatusId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrationHistory_Registration",
                        column: x => x.RegistrationId,
                        principalTable: "Registration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegistrationHistory_RegistrationStatus",
                        column: x => x.RegistrationStatusId,
                        principalTable: "RegistrationStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationJsonData",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    Data = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    RegistrationId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationJsonData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrationJsonData_Registration_RegistrationId",
                        column: x => x.RegistrationId,
                        principalTable: "Registration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FormBlockField",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    SortOrder = table.Column<int>(nullable: false),
                    FormFieldId = table.Column<long>(nullable: true),
                    FormBlockId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormBlockField", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormBlockField_FormBlock_FormBlockId",
                        column: x => x.FormBlockId,
                        principalTable: "FormBlock",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormBlockField_FormField_FormFieldId",
                        column: x => x.FormFieldId,
                        principalTable: "FormField",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MessageComponentContent",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Content = table.Column<string>(nullable: false),
                    MessageComponentId = table.Column<long>(nullable: false),
                    CampaignTypeEventRegistrationStatusId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageComponentContent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageComponentContent_CampaignTypeEventRegistrationStatus_CampaignTypeEventRegistrationStatusId",
                        column: x => x.CampaignTypeEventRegistrationStatusId,
                        principalTable: "CampaignTypeEventRegistrationStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MessageComponentContent_MessageComponent_MessageComponentId",
                        column: x => x.MessageComponentId,
                        principalTable: "MessageComponent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RmsChatMessage",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Side = table.Column<int>(nullable: false),
                    SharedRMSChatMessageId = table.Column<Guid>(nullable: false),
                    GroupName = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    ChatEventId = table.Column<long>(nullable: false),
                    ChatConversationId = table.Column<long>(nullable: false)
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

            migrationBuilder.CreateTable(
                name: "PurchaseRegistrationFieldData",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    PurchaseRegistrationId = table.Column<long>(nullable: false),
                    PurchaseRegistrationFieldId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseRegistrationFieldData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseRegistrationFieldData_PurchaseRegistrationField_PurchaseRegistrationFieldId",
                        column: x => x.PurchaseRegistrationFieldId,
                        principalTable: "PurchaseRegistrationField",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseRegistrationFieldData_PurchaseRegistration_PurchaseRegistrationId",
                        column: x => x.PurchaseRegistrationId,
                        principalTable: "PurchaseRegistration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MessageContentTranslation",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Content = table.Column<string>(nullable: false),
                    LocaleId = table.Column<long>(nullable: false),
                    MessageComponentContentId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageContentTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageContentTranslation_Locale_LocaleId",
                        column: x => x.LocaleId,
                        principalTable: "Locale",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MessageContentTranslation_MessageComponentContent_MessageComponentContentId",
                        column: x => x.MessageComponentContentId,
                        principalTable: "MessageComponentContent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivationCode_LocaleId",
                table: "ActivationCode",
                column: "LocaleId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivationCode_TenantId",
                table: "ActivationCode",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivationCodeRegistration_ActivationCodeId",
                table: "ActivationCodeRegistration",
                column: "ActivationCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivationCodeRegistration_RegistrationId",
                table: "ActivationCodeRegistration",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivationCodeRegistration_TenantId",
                table: "ActivationCodeRegistration",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Address_CountryId",
                table: "Address",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Address_TenantId",
                table: "Address",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Campaign_TenantId",
                table: "Campaign",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignCampaignType_CampaignId",
                table: "CampaignCampaignType",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignCampaignType_CampaignTypeId",
                table: "CampaignCampaignType",
                column: "CampaignTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignCampaignType_TenantId",
                table: "CampaignCampaignType",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignCategory_TenantId",
                table: "CampaignCategory",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignCategoryTranslation_CampaignCategoryId",
                table: "CampaignCategoryTranslation",
                column: "CampaignCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignCategoryTranslation_LocaleId",
                table: "CampaignCategoryTranslation",
                column: "LocaleId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignCategoryTranslation_TenantId",
                table: "CampaignCategoryTranslation",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignForm_CampaignId",
                table: "CampaignForm",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignForm_FormId",
                table: "CampaignForm",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignForm_TenantId",
                table: "CampaignForm",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignMessage_CampaignId",
                table: "CampaignMessage",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignMessage_MessageId",
                table: "CampaignMessage",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignRetailerLocation_CampaignId",
                table: "CampaignRetailerLocation",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignRetailerLocation_RetailerLocationId",
                table: "CampaignRetailerLocation",
                column: "RetailerLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignRetailerLocation_TenantId",
                table: "CampaignRetailerLocation",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignType_TenantId",
                table: "CampaignType",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignTypeEvent_CampaignTypeFkId",
                table: "CampaignTypeEvent",
                column: "CampaignTypeFkId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignTypeEvent_CampaignTypeId",
                table: "CampaignTypeEvent",
                column: "CampaignTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignTypeEvent_ProcessEventFkId",
                table: "CampaignTypeEvent",
                column: "ProcessEventFkId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignTypeEvent_ProcessEventId",
                table: "CampaignTypeEvent",
                column: "ProcessEventId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignTypeEvent_TenantId",
                table: "CampaignTypeEvent",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignTypeEventRegistrationStatus_CampaignTypeEventId",
                table: "CampaignTypeEventRegistrationStatus",
                column: "CampaignTypeEventId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignTypeEventRegistrationStatus_RegistrationStatusId",
                table: "CampaignTypeEventRegistrationStatus",
                column: "RegistrationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignTypeEventRegistrationStatus_TenantId",
                table: "CampaignTypeEventRegistrationStatus",
                column: "TenantId");

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
                name: "IX_Company_AddressId",
                table: "Company",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Company_TenantId",
                table: "Company",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Country_TenantId",
                table: "Country",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldType_TenantId",
                table: "FieldType",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Form_SystemLevelId",
                table: "Form",
                column: "SystemLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Form_TenantId",
                table: "Form",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_FormBlock_FormLocaleId",
                table: "FormBlock",
                column: "FormLocaleId");

            migrationBuilder.CreateIndex(
                name: "IX_FormBlock_TenantId",
                table: "FormBlock",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_FormBlockField_FormBlockId",
                table: "FormBlockField",
                column: "FormBlockId");

            migrationBuilder.CreateIndex(
                name: "IX_FormBlockField_FormFieldId",
                table: "FormBlockField",
                column: "FormFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_FormBlockField_TenantId",
                table: "FormBlockField",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_FormField_FieldTypeId",
                table: "FormField",
                column: "FieldTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FormField_TenantId",
                table: "FormField",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_FormFieldTranslation_FormFieldId",
                table: "FormFieldTranslation",
                column: "FormFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_FormFieldTranslation_LocaleId",
                table: "FormFieldTranslation",
                column: "LocaleId");

            migrationBuilder.CreateIndex(
                name: "IX_FormFieldTranslation_TenantId",
                table: "FormFieldTranslation",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_FormFieldValueList_FormFieldId",
                table: "FormFieldValueList",
                column: "FormFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_FormFieldValueList_TenantId",
                table: "FormFieldValueList",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_FormFieldValueList_ValueListId",
                table: "FormFieldValueList",
                column: "ValueListId");

            migrationBuilder.CreateIndex(
                name: "IX_FormLocale_FormId",
                table: "FormLocale",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_FormLocale_LocaleId",
                table: "FormLocale",
                column: "LocaleId");

            migrationBuilder.CreateIndex(
                name: "IX_FormLocale_TenantId",
                table: "FormLocale",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_HandlingLine_CampaignTypeId",
                table: "HandlingLine",
                column: "CampaignTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_HandlingLine_ProductHandlingId",
                table: "HandlingLine",
                column: "ProductHandlingId");

            migrationBuilder.CreateIndex(
                name: "IX_HandlingLine_TenantId",
                table: "HandlingLine",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_HandlingLineLocale_HandlingLineId",
                table: "HandlingLineLocale",
                column: "HandlingLineId");

            migrationBuilder.CreateIndex(
                name: "IX_HandlingLineLocale_LocaleId",
                table: "HandlingLineLocale",
                column: "LocaleId");

            migrationBuilder.CreateIndex(
                name: "IX_HandlingLineLocale_TenantId",
                table: "HandlingLineLocale",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_HandlingLineLogic_TenantId",
                table: "HandlingLineLogic",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_HandlingLineProduct_HandlingLineId",
                table: "HandlingLineProduct",
                column: "HandlingLineId");

            migrationBuilder.CreateIndex(
                name: "IX_HandlingLineProduct_ProductId",
                table: "HandlingLineProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_HandlingLineProduct_TenantId",
                table: "HandlingLineProduct",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_HandlingLineRetailer_HandlingLineId",
                table: "HandlingLineRetailer",
                column: "HandlingLineId");

            migrationBuilder.CreateIndex(
                name: "IX_HandlingLineRetailer_RetailerId",
                table: "HandlingLineRetailer",
                column: "RetailerId");

            migrationBuilder.CreateIndex(
                name: "IX_HandlingLineRetailer_TenantId",
                table: "HandlingLineRetailer",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ListValue_TenantId",
                table: "ListValue",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ListValue_ValueListId",
                table: "ListValue",
                column: "ValueListId");

            migrationBuilder.CreateIndex(
                name: "IX_ListValueTranslation_ListValueId",
                table: "ListValueTranslation",
                column: "ListValueId");

            migrationBuilder.CreateIndex(
                name: "IX_ListValueTranslation_LocaleId",
                table: "ListValueTranslation",
                column: "LocaleId");

            migrationBuilder.CreateIndex(
                name: "IX_ListValueTranslation_TenantId",
                table: "ListValueTranslation",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Locale_CountryId",
                table: "Locale",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Locale_TenantId",
                table: "Locale",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_SystemLevelId",
                table: "Message",
                column: "SystemLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_TenantId",
                table: "Message",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageComponent_MessageComponentTypeId",
                table: "MessageComponent",
                column: "MessageComponentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageComponent_MessageTypeId",
                table: "MessageComponent",
                column: "MessageTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageComponent_TenantId",
                table: "MessageComponent",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageComponentContent_CampaignTypeEventRegistrationStatusId",
                table: "MessageComponentContent",
                column: "CampaignTypeEventRegistrationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageComponentContent_MessageComponentId",
                table: "MessageComponentContent",
                column: "MessageComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageComponentContent_TenantId",
                table: "MessageComponentContent",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageComponentType_TenantId",
                table: "MessageComponentType",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageContentTranslation_LocaleId",
                table: "MessageContentTranslation",
                column: "LocaleId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageContentTranslation_MessageComponentContentId",
                table: "MessageContentTranslation",
                column: "MessageComponentContentId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageContentTranslation_TenantId",
                table: "MessageContentTranslation",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageType_MessageId",
                table: "MessageType",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageType_TenantId",
                table: "MessageType",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageVariable_TenantId",
                table: "MessageVariable",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessEvent_TenantId",
                table: "ProcessEvent",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductCategoryId",
                table: "Product",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_TenantId",
                table: "Product",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategory_TenantId",
                table: "ProductCategory",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategoryYearPo_ProductCategoryId",
                table: "ProductCategoryYearPo",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductHandling_CampaignId",
                table: "ProductHandling",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductHandling_TenantId",
                table: "ProductHandling",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectManager_AddressId",
                table: "ProjectManager",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectManager_TenantId",
                table: "ProjectManager",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRegistration_HandlingLineId",
                table: "PurchaseRegistration",
                column: "HandlingLineId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRegistration_ProductId",
                table: "PurchaseRegistration",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRegistration_RegistrationId",
                table: "PurchaseRegistration",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRegistration_RetailerLocationId",
                table: "PurchaseRegistration",
                column: "RetailerLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRegistration_TenantId",
                table: "PurchaseRegistration",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRegistrationField_FormFieldId",
                table: "PurchaseRegistrationField",
                column: "FormFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRegistrationField_TenantId",
                table: "PurchaseRegistrationField",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRegistrationFieldData_PurchaseRegistrationFieldId",
                table: "PurchaseRegistrationFieldData",
                column: "PurchaseRegistrationFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRegistrationFieldData_PurchaseRegistrationId",
                table: "PurchaseRegistrationFieldData",
                column: "PurchaseRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRegistrationFieldData_TenantId",
                table: "PurchaseRegistrationFieldData",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRegistrationFormField_FormFieldId",
                table: "PurchaseRegistrationFormField",
                column: "FormFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_Registration_CampaignFormId",
                table: "Registration",
                column: "CampaignFormId");

            migrationBuilder.CreateIndex(
                name: "IX_Registration_LocaleId",
                table: "Registration",
                column: "LocaleId");

            migrationBuilder.CreateIndex(
                name: "IX_Registration_RegistrationStatusId",
                table: "Registration",
                column: "RegistrationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Registration_RejectionReasonId",
                table: "Registration",
                column: "RejectionReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Registration_TenantId",
                table: "Registration",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationField_FormFieldId",
                table: "RegistrationField",
                column: "FormFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationField_TenantId",
                table: "RegistrationField",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationFieldData_RegistrationFieldId",
                table: "RegistrationFieldData",
                column: "RegistrationFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationFieldData_RegistrationId",
                table: "RegistrationFieldData",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationFieldData_TenantId",
                table: "RegistrationFieldData",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationHistory_AbpUserId",
                table: "RegistrationHistory",
                column: "AbpUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationHistory_RegistrationId",
                table: "RegistrationHistory",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationHistory_RegistrationStatusId",
                table: "RegistrationHistory",
                column: "RegistrationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationJsonData_RegistrationId",
                table: "RegistrationJsonData",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationStatus_TenantId",
                table: "RegistrationStatus",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_RejectionReasonTranslation_LocaleId",
                table: "RejectionReasonTranslation",
                column: "LocaleId");

            migrationBuilder.CreateIndex(
                name: "IX_RejectionReasonTranslation_RejectionReasonId",
                table: "RejectionReasonTranslation",
                column: "RejectionReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Retailer_CountryId",
                table: "Retailer",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Retailer_TenantId",
                table: "Retailer",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_RetailerLocation_RetailerId",
                table: "RetailerLocation",
                column: "RetailerId");

            migrationBuilder.CreateIndex(
                name: "IX_RetailerLocation_TenantId",
                table: "RetailerLocation",
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

            migrationBuilder.CreateIndex(
                name: "IX_SystemLevel_TenantId",
                table: "SystemLevel",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ValueList_TenantId",
                table: "ValueList",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivationCodeRegistration");

            migrationBuilder.DropTable(
                name: "CampaignCampaignType");

            migrationBuilder.DropTable(
                name: "CampaignCategoryTranslation");

            migrationBuilder.DropTable(
                name: "CampaignMessage");

            migrationBuilder.DropTable(
                name: "CampaignRetailerLocation");

            migrationBuilder.DropTable(
                name: "CampaignTranslation");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "FormBlockField");

            migrationBuilder.DropTable(
                name: "FormFieldTranslation");

            migrationBuilder.DropTable(
                name: "FormFieldValueList");

            migrationBuilder.DropTable(
                name: "HandlingLineLocale");

            migrationBuilder.DropTable(
                name: "HandlingLineLogic");

            migrationBuilder.DropTable(
                name: "HandlingLineProduct");

            migrationBuilder.DropTable(
                name: "HandlingLineRetailer");

            migrationBuilder.DropTable(
                name: "ListValueTranslation");

            migrationBuilder.DropTable(
                name: "MakitaSerialNumber");

            migrationBuilder.DropTable(
                name: "MessageContentTranslation");

            migrationBuilder.DropTable(
                name: "MessageHistory");

            migrationBuilder.DropTable(
                name: "MessageVariable");

            migrationBuilder.DropTable(
                name: "ProductCategoryYearPo");

            migrationBuilder.DropTable(
                name: "ProjectManager");

            migrationBuilder.DropTable(
                name: "PurchaseRegistrationFieldData");

            migrationBuilder.DropTable(
                name: "PurchaseRegistrationFormField");

            migrationBuilder.DropTable(
                name: "RegistrationFieldData");

            migrationBuilder.DropTable(
                name: "RegistrationHistory");

            migrationBuilder.DropTable(
                name: "RegistrationJsonData");

            migrationBuilder.DropTable(
                name: "RejectionReasonTranslation");

            migrationBuilder.DropTable(
                name: "RmsChatMessage");

            migrationBuilder.DropTable(
                name: "ActivationCode");

            migrationBuilder.DropTable(
                name: "AbpEditions");

            migrationBuilder.DropTable(
                name: "CampaignCategory");

            migrationBuilder.DropTable(
                name: "FormBlock");

            migrationBuilder.DropTable(
                name: "ListValue");

            migrationBuilder.DropTable(
                name: "MessageComponentContent");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "PurchaseRegistrationField");

            migrationBuilder.DropTable(
                name: "PurchaseRegistration");

            migrationBuilder.DropTable(
                name: "RegistrationField");

            migrationBuilder.DropTable(
                name: "ChatConversation");

            migrationBuilder.DropTable(
                name: "ChatEvent");

            migrationBuilder.DropTable(
                name: "AbpDynamicParameters");

            migrationBuilder.DropTable(
                name: "AbpEntityChangeSets");

            migrationBuilder.DropTable(
                name: "AbpUsers");

            migrationBuilder.DropTable(
                name: "FormLocale");

            migrationBuilder.DropTable(
                name: "ValueList");

            migrationBuilder.DropTable(
                name: "CampaignTypeEventRegistrationStatus");

            migrationBuilder.DropTable(
                name: "MessageComponent");

            migrationBuilder.DropTable(
                name: "HandlingLine");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "RetailerLocation");

            migrationBuilder.DropTable(
                name: "FormField");

            migrationBuilder.DropTable(
                name: "Registration");

            migrationBuilder.DropTable(
                name: "CampaignTypeEvent");

            migrationBuilder.DropTable(
                name: "MessageComponentType");

            migrationBuilder.DropTable(
                name: "MessageType");

            migrationBuilder.DropTable(
                name: "ProductHandling");

            migrationBuilder.DropTable(
                name: "ProductCategory");

            migrationBuilder.DropTable(
                name: "Retailer");

            migrationBuilder.DropTable(
                name: "FieldType");

            migrationBuilder.DropTable(
                name: "CampaignForm");

            migrationBuilder.DropTable(
                name: "Locale");

            migrationBuilder.DropTable(
                name: "RegistrationStatus");

            migrationBuilder.DropTable(
                name: "RejectionReason");

            migrationBuilder.DropTable(
                name: "CampaignType");

            migrationBuilder.DropTable(
                name: "ProcessEvent");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "Campaign");

            migrationBuilder.DropTable(
                name: "Form");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropTable(
                name: "SystemLevel");
        }
    }
}
