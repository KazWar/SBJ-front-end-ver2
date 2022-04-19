using RMS.SBJ.MakitaBaseModelSerial;
using RMS.SBJ.ProductGifts;
using RMS.SBJ.UniqueCodes;
using RMS.SBJ.HandlingBatch;
using RMS.SBJ.RegistrationHistory;
using RMS.SBJ.MakitaSerialNumber;
using RMS.SBJ.RegistrationJsonData;
using RMS.SBJ.PurchaseRegistrationFormFields;
using RMS.SBJ.PurchaseRegistrationFieldDatas;
using RMS.SBJ.PurchaseRegistrationFields;
using RMS.SBJ.PurchaseRegistrations;
using RMS.SBJ.ActivationCodeRegistrations;
using RMS.SBJ.Registrations;
using RMS.SBJ.ActivationCodes;
using RMS.SBJ.CampaignRetailerLocations;
using RMS.SBJ.HandlingLineLocales;
using RMS.SBJ.HandlingLineLogics;
using RMS.SBJ.HandlingLineRetailers;
using RMS.SBJ.HandlingLineProducts;
using RMS.SBJ.RetailerLocations;
using RMS.SBJ.HandlingLines;
using RMS.SBJ.ProductHandlings;
using RMS.SBJ.Forms;
using RMS.SBJ.Products;
using RMS.SBJ.Retailers;
using RMS.SBJ.Company;
using RMS.SBJ.SystemTables;
using RMS.SBJ.CampaignProcesses;
using RMS.SBJ.CodeTypeTables;
using Abp.IdentityServer4;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RMS.Authorization.Roles;
using RMS.Authorization.Users;
using RMS.Chat;
using RMS.Editions;
using RMS.Friendships;
using RMS.MultiTenancy;
using RMS.MultiTenancy.Accounting;
using RMS.MultiTenancy.Payments;
using RMS.Storage;
using RMS.Authorization.Delegation;
using RMS.SBJ.RegistrationFields;
using RMS.SBJ.RegistrationFormFieldDatas;
using RMS.PromoPlanner;
using RMS.SBJ.Report.EmployeePerformanceReports;

namespace RMS.EntityFrameworkCore
{
    public class RMSDbContext : AbpZeroDbContext<Tenant, Role, User, RMSDbContext>, IAbpPersistedGrantDbContext
    {
        public virtual DbSet<UniqueCodeByCampaign> UniqueCodeByCampaigns { get; set; }

        public virtual DbSet<SBJ.MakitaBaseModelSerial.MakitaBaseModelSerial> MakitaBaseModelSerials { get; set; }

        public virtual DbSet<ProductGift> ProductGifts { get; set; }

        public virtual DbSet<CampaignCountry> CampaignCountries { get; set; }

        public virtual DbSet<UniqueCode> UniqueCodes { get; set; }

        // TODO: Just for Makita, find a way to only generate this when the DbContext is that of Makita
        public virtual DbSet<MakitaSerialNumber> MakitaSerialNumbers { get; set; }

        // SBJ entities
        public virtual DbSet<ActivationCode> ActivationCodes { get; set; }
        public virtual DbSet<ActivationCodeRegistration> ActivationCodeRegistrations { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }

        public virtual DbSet<Campaign> Campaigns { get; set; }
        public virtual DbSet<CampaignCampaignType> CampaignCampaignTypes { get; set; }
        public virtual DbSet<CampaignCategory> CampaignCategories { get; set; }
        public virtual DbSet<CampaignCategoryTranslation> CampaignCategoryTranslations { get; set; }
        public virtual DbSet<CampaignForm> CampaignForms { get; set; }
        public virtual DbSet<CampaignMessage> CampaignMessages { get; set; }
        public virtual DbSet<CampaignRetailerLocation> CampaignRetailerLocations { get; set; }
        public virtual DbSet<CampaignTranslation> CampaignTranslations { get; set; }
        public virtual DbSet<CampaignType> CampaignTypes { get; set; }
        public virtual DbSet<CampaignTypeEvent> CampaignTypeEvents { get; set; }
        public virtual DbSet<CampaignTypeEventRegistrationStatus> CampaignTypeEventRegistrationStatuses { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Country> Countries { get; set; }

        public virtual DbSet<FieldType> FieldTypes { get; set; }
        public virtual DbSet<Form> Forms { get; set; }
        public virtual DbSet<FormBlock> FormBlocks { get; set; }
        public virtual DbSet<FormBlockField> FormBlockFields { get; set; }
        public virtual DbSet<FormField> FormFields { get; set; }
        public virtual DbSet<FormFieldTranslation> FormFieldTranslations { get; set; }
        public virtual DbSet<FormFieldValueList> FormFieldValueLists { get; set; }
        public virtual DbSet<FormLocale> FormLocales { get; set; }

        public virtual DbSet<HandlingLine> HandlingLines { get; set; }
        public virtual DbSet<HandlingLineLocale> HandlingLineLocales { get; set; }
        public virtual DbSet<HandlingLineLogic> HandlingLineLogics { get; set; }
        public virtual DbSet<HandlingLineProduct> HandlingLineProducts { get; set; }
        public virtual DbSet<HandlingLineRetailer> HandlingLineRetailers { get; set; }

        public virtual DbSet<ListValue> ListValues { get; set; }
        public virtual DbSet<ListValueTranslation> ListValueTranslations { get; set; }
        public virtual DbSet<Locale> Locales { get; set; }

        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<MessageComponent> MessageComponents { get; set; }
        public virtual DbSet<MessageComponentContent> MessageComponentContents { get; set; }
        public virtual DbSet<MessageComponentType> MessageComponentTypes { get; set; }
        public virtual DbSet<MessageContentTranslation> MessageContentTranslations { get; set; }

        /* Added new one */
        public virtual DbSet<MessageHistory> MessageHistories { get; set; }
        /* End added new one */
        public virtual DbSet<MessageType> MessageTypes { get; set; }
        public virtual DbSet<MessageVariable> MessageVariables { get; set; }

        public virtual DbSet<ProcessEvent> ProcessEvents { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<ProductCategoryYearPo> ProductCategoryYearPos { get; set; }
        public virtual DbSet<ProductHandling> ProductHandlings { get; set; }
        public virtual DbSet<ProjectManager> ProjectManagers { get; set; }
        public virtual DbSet<PurchaseRegistration> PurchaseRegistrations { get; set; }
        public virtual DbSet<PurchaseRegistrationField> PurchaseRegistrationFields { get; set; }
        public virtual DbSet<PurchaseRegistrationFieldData> PurchaseRegistrationFieldData { get; set; }
        public virtual DbSet<PurchaseRegistrationFormField> PurchaseRegistrationFormFields { get; set; }

        public virtual DbSet<Registration> Registrations { get; set; }
        public virtual DbSet<RegistrationField> RegistrationFields { get; set; }
        public virtual DbSet<RegistrationFieldData> RegistrationFieldData { get; set; }
        public virtual DbSet<RegistrationHistory> RegistrationHistories { get; set; }
        public virtual DbSet<RegistrationJsonData> RegistrationJsonData { get; set; }
        public virtual DbSet<RegistrationStatus> RegistrationStatuses { get; set; }
        public virtual DbSet<RejectionReason> RejectionReasons { get; set; }
        public virtual DbSet<RejectionReasonTranslation> RejectionReasonTranslations { get; set; }
        public virtual DbSet<Retailer> Retailers { get; set; }
        public virtual DbSet<RetailerLocation> RetailerLocations { get; set; }

        public virtual DbSet<HandlingBatchLineHistory> HandlingBatchLineHistories { get; set; }
        public virtual DbSet<HandlingBatchHistory> HandlingBatchHistories { get; set; }
        public virtual DbSet<HandlingBatchLineStatus> HandlingBatchLineStatuses { get; set; }
        public virtual DbSet<HandlingBatchStatus> HandlingBatchStatuses { get; set; }
        public virtual DbSet<HandlingBatchLine> HandlingBatchLines { get; set; }
        public virtual DbSet<HandlingBatch> HandlingBatches { get; set; }

        // Stored Procedure related entities
        public virtual DbSet<StoredProcedureExistsCheck> StoredProcedureExistsCheck { get; set; }
        public virtual DbSet<EmployeePerformanceReport> EmployeePerformanceReport { get; set; }
        // End Stored Procedure related entities

        public virtual DbSet<SystemLevel> SystemLevels { get; set; }

        public virtual DbSet<ValueList> ValueLists { get; set; }

        // System Generated
        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }
        public virtual DbSet<ChatMessage> ChatMessages { get; set; }
        public virtual DbSet<Friendship> Friendships { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<PersistedGrantEntity> PersistedGrants { get; set; }
        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }
        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }
        public virtual DbSet<SubscriptionPaymentExtensionData> SubscriptionPaymentExtensionData { get; set; }
        public virtual DbSet<UserDelegation> UserDelegations { get; set; }

        public RMSDbContext(DbContextOptions<RMSDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<MakitaBaseModelSerial>(m =>
            {
                m.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<ProductGift>(p =>
                       {
                           p.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<CampaignCountry>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<UniqueCode>();
            modelBuilder.Entity<ActivationCode>(entity =>
                       {
                           entity.ToTable("ActivationCode");

                           entity.HasIndex(e => e.LocaleId);
                           entity.HasIndex(e => e.TenantId);

                           entity.Property(e => e.Code).IsRequired();

                           entity
                               .HasOne(e => e.LocaleFk)
                               .WithMany(p => p.ActivationCodes)
                               .HasForeignKey(d => d.LocaleId);
                       });

            modelBuilder.Entity<ActivationCodeRegistration>(entity =>
            {
                entity.ToTable("ActivationCodeRegistration");

                entity.HasIndex(e => e.ActivationCodeId);
                entity.HasIndex(e => e.RegistrationId);
                entity.HasIndex(e => e.TenantId);

                entity
                    .HasOne(d => d.ActivationCodeFk)
                    .WithMany(p => p.ActivationCodeRegistrations)
                    .HasForeignKey(d => d.ActivationCodeId);

                entity
                    .HasOne(d => d.RegistrationFk)
                    .WithMany(p => p.ActivationCodeRegistrations)
                    .HasForeignKey(d => d.RegistrationId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Address");

                entity.HasIndex(e => e.CountryId);

                entity.HasIndex(e => e.TenantId);

                entity.Property(e => e.AddressLine1).IsRequired();

                entity.Property(e => e.City).IsRequired();

                entity.Property(e => e.PostalCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.HasOne(d => d.CountryFk)
                    .WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.CountryId);
            });

            modelBuilder.Entity<Campaign>(entity =>
            {
                entity.ToTable("Campaign");

                entity.HasIndex(e => e.TenantId);

                entity.Property(e => e.CreationTime).HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");

                entity.Property(e => e.EndDate).HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");

                entity.Property(e => e.IsDeleted)
                    .IsRequired()
                    .HasDefaultValueSql("(0)");

                entity.Property(e => e.StartDate).HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");
            });

            modelBuilder.Entity<CampaignCampaignType>(entity =>
            {
                entity.ToTable("CampaignCampaignType");

                entity.HasIndex(e => e.CampaignId);

                entity.HasIndex(e => e.CampaignTypeId);

                entity.HasIndex(e => e.TenantId);

                entity.HasOne(d => d.CampaignFk)
                    .WithMany(p => p.CampaignCampaignTypes)
                    .HasForeignKey(d => d.CampaignId);

                entity.HasOne(d => d.CampaignTypeFk)
                    .WithMany(p => p.CampaignCampaignTypes)
                    .HasForeignKey(d => d.CampaignTypeId);
            });

            modelBuilder.Entity<CampaignCategory>(entity =>
            {
                entity.ToTable("CampaignCategory");

                entity.HasIndex(e => e.TenantId);

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<CampaignCategoryTranslation>(entity =>
            {
                entity.ToTable("CampaignCategoryTranslation");

                entity.HasIndex(e => e.CampaignCategoryId);

                entity.HasIndex(e => e.LocaleId);

                entity.HasIndex(e => e.TenantId);

                entity.Property(e => e.Name).IsRequired();

                entity.HasOne(d => d.CampaignCategoryFk)
                    .WithMany(p => p.CampaignCategoryTranslations)
                    .HasForeignKey(d => d.CampaignCategoryId);

                entity.HasOne(d => d.LocaleFk)
                    .WithMany(p => p.CampaignCategoryTranslations)
                    .HasForeignKey(d => d.LocaleId);
            });

            modelBuilder.Entity<CampaignForm>(entity =>
            {
                entity.ToTable("CampaignForm");

                entity.HasIndex(e => e.CampaignId);

                entity.HasIndex(e => e.FormId);

                entity.HasIndex(e => e.TenantId);

                entity.HasOne(d => d.CampaignFk)
                    .WithMany(p => p.CampaignForms)
                    .HasForeignKey(d => d.CampaignId);

                entity.HasOne(d => d.FormFk)
                    .WithMany(p => p.CampaignForms)
                    .HasForeignKey(d => d.FormId);
            });

            modelBuilder.Entity<CampaignMessage>(entity =>
            {
                entity.ToTable("CampaignMessage");

                entity.HasIndex(e => e.CampaignId);

                entity.HasIndex(e => e.MessageId);

                entity.HasOne(d => d.CampaignFk)
                    .WithMany(p => p.CampaignMessages)
                    .HasForeignKey(d => d.CampaignId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.MessageFk)
                    .WithMany(p => p.CampaignMessages)
                    .HasForeignKey(d => d.MessageId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<CampaignRetailerLocation>(entity =>
            {
                entity.ToTable("CampaignRetailerLocation");

                entity.HasIndex(e => e.CampaignId);

                entity.HasIndex(e => e.RetailerLocationId);

                entity.HasIndex(e => e.TenantId);

                entity.HasOne(d => d.CampaignFk)
                    .WithMany(p => p.CampaignRetailerLocations)
                    .HasForeignKey(d => d.CampaignId);

                entity.HasOne(d => d.RetailerLocationFk)
                    .WithMany(p => p.CampaignRetailerLocations)
                    .HasForeignKey(d => d.RetailerLocationId);
            });

            modelBuilder.Entity<CampaignTranslation>(entity =>
            {
                entity.ToTable("CampaignTranslation");

                entity.HasIndex(e => e.TenantId);

                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Description).IsRequired();
            });

            modelBuilder.Entity<CampaignType>(entity =>
            {
                entity.ToTable("CampaignType");

                entity.HasIndex(e => e.TenantId);

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<CampaignTypeEvent>(entity =>
            {
                entity.ToTable("CampaignTypeEvent");

                entity.HasIndex(e => e.CampaignTypeId);

                entity.HasIndex(e => e.ProcessEventId);

                entity.HasIndex(e => e.TenantId);

                entity.HasOne(d => d.CampaignTypeFk)
                    .WithMany(p => p.CampaignTypeEvents)
                    .HasForeignKey(d => d.CampaignTypeId);

                entity.HasOne(d => d.ProcessEventFk)
                    .WithMany(p => p.CampaignTypeEvents)
                    .HasForeignKey(d => d.ProcessEventId);
            });

            modelBuilder.Entity<CampaignTypeEventRegistrationStatus>(entity =>
            {
                entity.ToTable("CampaignTypeEventRegistrationStatus");

                entity.HasIndex(e => e.CampaignTypeEventId);

                entity.HasIndex(e => e.RegistrationStatusId);

                entity.HasIndex(e => e.TenantId);

                entity.HasOne(d => d.CampaignTypeEventFk)
                    .WithMany(p => p.CampaignTypeEventRegistrationStatuses)
                    .HasForeignKey(d => d.CampaignTypeEventId);

                entity.HasOne(d => d.RegistrationStatusFk)
                    .WithMany(p => p.CampaignTypeEventRegistrationStatuses)
                    .HasForeignKey(d => d.RegistrationStatusId);
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("Company");

                entity.HasIndex(e => e.AddressId);

                entity.HasIndex(e => e.TenantId);

                entity.Property(e => e.BicCashBack).HasColumnName("BicCashBack");

                entity.Property(e => e.IbanCashBack).HasColumnName("IbanCashBack");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.HasOne(d => d.AddressFk)
                    .WithMany(p => p.Companies)
                    .HasForeignKey(d => d.AddressId);
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("Country");

                entity.HasIndex(e => e.TenantId);

                entity.Property(e => e.CountryCode)
                    .IsRequired()
                    .HasMaxLength(2);

                entity.Property(e => e.Description).IsRequired();
            });

            modelBuilder.Entity<FieldType>(entity =>
            {
                entity.ToTable("FieldType");

                entity.HasIndex(e => e.TenantId);
            });

            modelBuilder.Entity<Form>(entity =>
            {
                entity.ToTable("Form");

                entity.HasIndex(e => e.SystemLevelId);

                entity.HasIndex(e => e.TenantId);

                entity.HasOne(d => d.SystemLevelFk)
                    .WithMany(p => p.Forms)
                    .HasForeignKey(d => d.SystemLevelId);
            });

            modelBuilder.Entity<FormBlock>(entity =>
            {
                entity.ToTable("FormBlock");

                entity.HasIndex(e => e.FormLocaleId);

                entity.HasIndex(e => e.TenantId);

                entity.Property(e => e.FormLocaleId).HasDefaultValueSql("(0)");

                entity.Property(e => e.IsPurchaseRegistration)
                    .IsRequired();

                entity.HasOne(d => d.FormLocaleFk)
                    .WithMany(p => p.FormBlocks)
                    .HasForeignKey(d => d.FormLocaleId);
            });

            modelBuilder.Entity<FormBlockField>(entity =>
            {
                entity.ToTable("FormBlockField");

                entity.HasIndex(e => e.FormBlockId);

                entity.HasIndex(e => e.FormFieldId);

                entity.HasIndex(e => e.TenantId);

                entity.HasOne(d => d.FormBlockFk)
                    .WithMany(p => p.FormBlockFields)
                    .HasForeignKey(d => d.FormBlockId);

                entity.HasOne(d => d.FormFieldFk)
                    .WithMany(p => p.FormBlockFields)
                    .HasForeignKey(d => d.FormFieldId);
            });

            modelBuilder.Entity<FormField>(entity =>
            {
                entity.ToTable("FormField");

                entity.HasIndex(e => e.FieldTypeId);

                entity.HasIndex(e => e.TenantId);

                entity.Property(e => e.FieldName).IsRequired();

                entity.HasOne(d => d.FieldTypeFk)
                    .WithMany(p => p.FormFields)
                    .HasForeignKey(d => d.FieldTypeId);
            });

            modelBuilder.Entity<FormFieldTranslation>(entity =>
            {
                entity.ToTable("FormFieldTranslation");

                entity.HasIndex(e => e.FormFieldId);

                entity.HasIndex(e => e.LocaleId);

                entity.HasIndex(e => e.TenantId);

                entity.HasOne(d => d.FormFieldFk)
                    .WithMany(p => p.FormFieldTranslations)
                    .HasForeignKey(d => d.FormFieldId);

                entity.HasOne(d => d.LocaleFk)
                    .WithMany(p => p.FormFieldTranslations)
                    .HasForeignKey(d => d.LocaleId);
            });

            modelBuilder.Entity<FormFieldValueList>(entity =>
            {
                entity.ToTable("FormFieldValueList");

                entity.HasIndex(e => e.FormFieldId);

                entity.HasIndex(e => e.TenantId);

                entity.HasIndex(e => e.ValueListId);

                entity.HasOne(d => d.FormFieldFk)
                    .WithMany(p => p.FormFieldValueLists)
                    .HasForeignKey(d => d.FormFieldId);

                entity.HasOne(d => d.ValueListFk)
                    .WithMany(p => p.FormFieldValueLists)
                    .HasForeignKey(d => d.ValueListId);
            });

            modelBuilder.Entity<FormLocale>(entity =>
            {
                entity.ToTable("FormLocale");

                entity.HasIndex(e => e.FormId);

                entity.HasIndex(e => e.LocaleId);

                entity.HasIndex(e => e.TenantId);

                entity.HasOne(d => d.FormFk)
                    .WithMany(p => p.FormLocales)
                    .HasForeignKey(d => d.FormId);

                entity.HasOne(d => d.LocaleFk)
                    .WithMany(p => p.FormLocales)
                    .HasForeignKey(d => d.LocaleId);
            });

            modelBuilder.Entity<HandlingLine>(entity =>
            {
                entity.ToTable("HandlingLine");

                entity.HasIndex(e => e.CampaignTypeId);

                entity.HasIndex(e => e.ProductHandlingId);

                entity.HasIndex(e => e.TenantId);

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.MaximumPurchaseAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.MinimumPurchaseAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ProductHandlingId).HasDefaultValueSql("(0)");

                entity.HasOne(d => d.CampaignTypeFk)
                    .WithMany(p => p.HandlingLines)
                    .HasForeignKey(d => d.CampaignTypeId);

                entity.HasOne(d => d.ProductHandlingFk)
                    .WithMany(p => p.HandlingLines)
                    .HasForeignKey(d => d.ProductHandlingId);
            });

            modelBuilder.Entity<HandlingLineLocale>(entity =>
            {
                entity.ToTable("HandlingLineLocale");

                entity.HasIndex(e => e.HandlingLineId);

                entity.HasIndex(e => e.LocaleId);

                entity.HasIndex(e => e.TenantId);

                entity.HasOne(d => d.HandlingLineFk)
                    .WithMany(p => p.HandlingLineLocales)
                    .HasForeignKey(d => d.HandlingLineId);

                entity.HasOne(d => d.LocaleFk)
                    .WithMany(p => p.HandlingLineLocales)
                    .HasForeignKey(d => d.LocaleId);
            });

            modelBuilder.Entity<HandlingLineLogic>(entity =>
            {
                entity.ToTable("HandlingLineLogic");

                entity.HasIndex(e => e.TenantId);

                entity.Property(e => e.FirstHandlingLineId).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Operator).IsRequired();

                entity.Property(e => e.SecondHandlingLineId).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<HandlingLineProduct>(entity =>
            {
                entity.ToTable("HandlingLineProduct");

                entity.HasIndex(e => e.HandlingLineId);

                entity.HasIndex(e => e.ProductId);

                entity.HasIndex(e => e.TenantId);

                entity.HasOne(d => d.HandlingLineFk)
                    .WithMany(p => p.HandlingLineProducts)
                    .HasForeignKey(d => d.HandlingLineId);

                entity.HasOne(d => d.ProductFk)
                    .WithMany(p => p.HandlingLineProducts)
                    .HasForeignKey(d => d.ProductId);
            });

            modelBuilder.Entity<HandlingLineRetailer>(entity =>
            {
                entity.ToTable("HandlingLineRetailer");

                entity.HasIndex(e => e.HandlingLineId);

                entity.HasIndex(e => e.RetailerId);

                entity.HasIndex(e => e.TenantId);

                entity.HasOne(d => d.HandlingLineFk)
                    .WithMany(p => p.HandlingLineRetailers)
                    .HasForeignKey(d => d.HandlingLineId);

                entity.HasOne(d => d.RetailerFk)
                    .WithMany(p => p.HandlingLineRetailers)
                    .HasForeignKey(d => d.RetailerId);
            });

            modelBuilder.Entity<ListValue>(entity =>
            {
                entity.ToTable("ListValue");

                entity.HasIndex(e => e.TenantId);

                entity.HasIndex(e => e.ValueListId);

                entity.HasOne(d => d.ValueListFk)
                    .WithMany(p => p.ListValues)
                    .HasForeignKey(d => d.ValueListId);
            });

            modelBuilder.Entity<ListValueTranslation>(entity =>
            {
                entity.ToTable("ListValueTranslation");

                entity.HasIndex(e => e.ListValueId);

                entity.HasIndex(e => e.LocaleId);

                entity.HasIndex(e => e.TenantId);

                entity.HasOne(d => d.ListValueFk)
                    .WithMany(p => p.ListValueTranslations)
                    .HasForeignKey(d => d.ListValueId);

                entity.HasOne(d => d.LocaleFk)
                    .WithMany(p => p.ListValueTranslations)
                    .HasForeignKey(d => d.LocaleId);
            });

            modelBuilder.Entity<Locale>(entity =>
            {
                entity.ToTable("Locale");

                entity.HasIndex(e => e.CountryId);

                entity.HasIndex(e => e.TenantId);

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.LanguageCode)
                    .IsRequired()
                    .HasMaxLength(2);

                entity.HasOne(d => d.CountryFk)
                    .WithMany(p => p.Locales)
                    .HasForeignKey(d => d.CountryId);
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("Message");

                entity.HasIndex(e => e.SystemLevelId);

                entity.HasIndex(e => e.TenantId);

                entity.Property(e => e.Version)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(d => d.SystemLevelFk)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.SystemLevelId);
            });

            modelBuilder.Entity<MessageComponent>(entity =>
            {
                entity.ToTable("MessageComponent");

                entity.HasIndex(e => e.MessageComponentTypeId);

                entity.HasIndex(e => e.MessageTypeId);

                entity.HasIndex(e => e.TenantId);

                entity.HasOne(d => d.MessageComponentTypeFk)
                    .WithMany(p => p.MessageComponents)
                    .HasForeignKey(d => d.MessageComponentTypeId);

                entity.HasOne(d => d.MessageTypeFk)
                    .WithMany(p => p.MessageComponents)
                    .HasForeignKey(d => d.MessageTypeId);
            });

            modelBuilder.Entity<MessageComponentContent>(entity =>
            {
                entity.ToTable("MessageComponentContent");

                entity.HasIndex(e => e.CampaignTypeEventRegistrationStatusId);

                entity.HasIndex(e => e.MessageComponentId);

                entity.HasIndex(e => e.TenantId);

                entity.Property(e => e.Content).IsRequired();

                entity.HasOne(d => d.CampaignTypeEventRegistrationStatusFk)
                    .WithMany(p => p.MessageComponentContents)
                    .HasForeignKey(d => d.CampaignTypeEventRegistrationStatusId);

                entity.HasOne(d => d.MessageComponentFk)
                    .WithMany(p => p.MessageComponentContents)
                    .HasForeignKey(d => d.MessageComponentId);
            });

            modelBuilder.Entity<MessageComponentType>(entity =>
            {
                entity.ToTable("MessageComponentType");

                entity.HasIndex(e => e.TenantId);

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<MessageContentTranslation>(entity =>
            {
                entity.ToTable("MessageContentTranslation");

                entity.HasIndex(e => e.LocaleId);

                entity.HasIndex(e => e.MessageComponentContentId);

                entity.HasIndex(e => e.TenantId);

                entity.Property(e => e.Content).IsRequired();

                entity.HasOne(d => d.LocaleFk)
                    .WithMany(p => p.MessageContentTranslations)
                    .HasForeignKey(d => d.LocaleId);

                entity.HasOne(d => d.MessageComponentContentFk)
                    .WithMany(p => p.MessageContentTranslations)
                    .HasForeignKey(d => d.MessageComponentContentId);
            });

            modelBuilder.Entity<MessageHistory>(entity =>
            {
                entity.ToTable("MessageHistory");
            });

            modelBuilder.Entity<MessageType>(entity =>
            {
                entity.ToTable("MessageType");

                entity.HasIndex(e => e.MessageId);

                entity.HasIndex(e => e.TenantId);

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Source).IsRequired();

                entity.HasOne(d => d.MessageFk)
                    .WithMany(p => p.MessageTypes)
                    .HasForeignKey(d => d.MessageId);
            });

            modelBuilder.Entity<MessageVariable>(entity =>
            {
                entity.ToTable("MessageVariable");

                entity.HasIndex(e => e.TenantId);

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.RmsTable).IsRequired();

                entity.Property(e => e.TableField).IsRequired();
            });

            modelBuilder.Entity<ProcessEvent>(entity =>
            {
                entity.ToTable("ProcessEvent");

                entity.HasIndex(e => e.TenantId);

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.HasIndex(e => e.ProductCategoryId);

                entity.HasIndex(e => e.TenantId);

                entity.HasOne(d => d.ProductCategoryFk)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ProductCategoryId);
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.ToTable("ProductCategory");

                entity.HasIndex(e => e.TenantId);

                entity.Property(e => e.PoCashBack).HasColumnName("PoCashBack");

                entity.Property(e => e.PoHandling).HasColumnName("PoHandling");
            });

            modelBuilder.Entity<ProductHandling>(entity =>
            {
                entity.ToTable("ProductHandling");

                entity.HasIndex(e => e.CampaignId);

                entity.HasIndex(e => e.TenantId);

                entity.HasOne(d => d.CampaignFk)
                    .WithMany(p => p.ProductHandlings)
                    .HasForeignKey(d => d.CampaignId);
            });

            modelBuilder.Entity<ProjectManager>(entity =>
            {
                entity.ToTable("ProjectManager");

                entity.HasIndex(e => e.AddressId);

                entity.HasIndex(e => e.TenantId);

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.PhoneNumber).HasMaxLength(50);

                entity.HasOne(d => d.AddressFk)
                    .WithMany(p => p.ProjectManagers)
                    .HasForeignKey(d => d.AddressId);
            });

            modelBuilder.Entity<PurchaseRegistration>(entity =>
            {
                entity.ToTable("PurchaseRegistration");

                entity.HasIndex(e => e.HandlingLineId);

                entity.HasIndex(e => e.ProductId);

                entity.HasIndex(e => e.RegistrationId);

                entity.HasIndex(e => e.RetailerLocationId);

                entity.HasIndex(e => e.TenantId);

                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.HandlingLineFk)
                    .WithMany(p => p.PurchaseRegistrations)
                    .HasForeignKey(d => d.HandlingLineId);

                entity.HasOne(d => d.ProductFk)
                    .WithMany(p => p.PurchaseRegistrations)
                    .HasForeignKey(d => d.ProductId);

                entity.HasOne(d => d.RegistrationFk)
                    .WithMany(p => p.PurchaseRegistrations)
                    .HasForeignKey(d => d.RegistrationId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(d => d.RetailerLocationFk)
                    .WithMany(p => p.PurchaseRegistrations)
                    .HasForeignKey(d => d.RetailerLocationId);
            });

            modelBuilder.Entity<PurchaseRegistrationField>(entity =>
            {
                entity.ToTable("PurchaseRegistrationField");

                entity.HasIndex(e => e.FormFieldId);

                entity.HasIndex(e => e.TenantId);

                entity.HasOne(d => d.FormFieldFk)
                    .WithMany(p => p.PurchaseRegistrationFields)
                    .HasForeignKey(d => d.FormFieldId);
            });

            modelBuilder.Entity<PurchaseRegistrationFieldData>(entity =>
            {
                entity.HasIndex(e => e.PurchaseRegistrationFieldId);

                entity.HasIndex(e => e.PurchaseRegistrationId);

                entity.HasIndex(e => e.TenantId);

                entity.HasOne(d => d.PurchaseRegistrationFieldFk)
                    .WithMany(p => p.PurchaseRegistrationFieldData)
                    .HasForeignKey(d => d.PurchaseRegistrationFieldId);

                entity.HasOne(d => d.PurchaseRegistrationFk)
                    .WithMany(p => p.PurchaseRegistrationFieldData)
                    .HasForeignKey(d => d.PurchaseRegistrationId);
            });

            modelBuilder.Entity<Registration>(entity =>
            {
                entity.ToTable("Registration");

                entity.HasIndex(e => e.RegistrationStatusId);

                entity.HasIndex(e => e.TenantId);

                entity.Property(e => e.CampaignId).HasDefaultValueSql("(0)");

                entity.Property(e => e.CountryId).HasDefaultValueSql("(0)");
            });

            modelBuilder.Entity<RegistrationField>(entity =>
            {
                entity.ToTable("RegistrationField");

                entity.HasIndex(e => e.FormFieldId);

                entity.HasIndex(e => e.TenantId);

                entity.HasOne(d => d.FormFieldFk)
                    .WithMany(p => p.RegistrationFields)
                    .HasForeignKey(d => d.FormFieldId)
                    .HasConstraintName("FK_RegistrationFormField_FormField_FormFieldId");
            });

            modelBuilder.Entity<RegistrationFieldData>(entity =>
            {
                entity.HasIndex(e => e.RegistrationFieldId);

                entity.HasIndex(e => e.RegistrationId);

                entity.HasIndex(e => e.TenantId);
            });

            modelBuilder.Entity<RegistrationHistory>(entity =>
            {
                entity.ToTable("RegistrationHistory");

                entity.HasIndex(e => e.AbpUserId);

                entity.HasIndex(e => e.RegistrationStatusId);

                entity.HasIndex(e => e.RegistrationId);

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.Remarks).IsUnicode(false);

                entity.HasOne(d => d.RegistrationFk)
                    .WithMany(p => p.RegistrationHistories)
                    .HasForeignKey(d => d.RegistrationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RegistrationHistory_Registration");

                entity.HasOne(d => d.RegistrationStatusFk)
                    .WithMany(p => p.RegistrationHistories)
                    .HasForeignKey(d => d.RegistrationStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RegistrationHistory_RegistrationStatus");
            });

            modelBuilder.Entity<RegistrationStatus>(entity =>
            {
                entity.ToTable("RegistrationStatus");

                entity.HasIndex(e => e.TenantId);

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.StatusCode).IsRequired();
            });

            modelBuilder.Entity<RejectionReason>(entity =>
            {
                entity.ToTable("RejectionReason");

                entity.Property(e => e.Description).IsRequired();
            });

            modelBuilder.Entity<RejectionReasonTranslation>(entity =>
            {
                entity.ToTable("RejectionReasonTranslation");

                entity.HasIndex(e => e.LocaleId);

                entity.HasIndex(e => e.RejectionReasonId);

                entity.Property(e => e.Description).IsRequired();

                entity.HasOne(d => d.LocaleFk)
                    .WithMany(p => p.RejectionReasonTranslations)
                    .HasForeignKey(d => d.LocaleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RejectionReasonTranslation_Locale");

                entity.HasOne(d => d.RejectionReasonFk)
                    .WithMany(p => p.RejectionReasonTranslations)
                    .HasForeignKey(d => d.RejectionReasonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RejectionReasonTranslation_RejectionReason");
            });

            modelBuilder.Entity<Retailer>(entity =>
            {
                entity.ToTable("Retailer");

                entity.HasIndex(e => e.CountryId);

                entity.HasIndex(e => e.TenantId);

                entity.HasOne(d => d.CountryFk)
                    .WithMany(p => p.Retailers)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<RetailerLocation>(entity =>
            {
                entity.ToTable("RetailerLocation");

                entity.HasIndex(e => e.RetailerId);

                entity.HasIndex(e => e.TenantId);

                entity.Property(e => e.Name).IsRequired();

                entity.HasOne(d => d.RetailerFk)
                    .WithMany(p => p.RetailerLocations)
                    .HasForeignKey(d => d.RetailerId);
            });

            modelBuilder.Entity<HandlingBatchLineHistory>(entity =>
            {
                entity.ToTable("HandlingBatchLineHistory");

                entity.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<HandlingBatchLine>(entity =>
            {
                entity.ToTable("HandlingBatchLine");

                entity.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<HandlingBatch>(entity =>
            {
                entity.ToTable("HandlingBatch");

                entity.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<HandlingBatchHistory>(entity =>
            {
                entity.ToTable("HandlingBatchHistory");

                entity.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<HandlingBatchLineStatus>(entity =>
            {
                entity.ToTable("HandlingBatchLineStatus");

                entity.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<HandlingBatchStatus>(entity =>
            {
                entity.ToTable("HandlingBatchStatus");

                entity.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<EmployeePerformanceReport>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<SystemLevel>(entity =>
            {
                entity.ToTable("SystemLevel");

                entity.HasIndex(e => e.TenantId);

                entity.Property(e => e.Description).IsRequired();
            });

            modelBuilder.Entity<ValueList>(entity =>
            {
                entity.ToTable("ValueList");

                entity.HasIndex(e => e.TenantId);
            });

            modelBuilder.Entity<BinaryObject>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
            });
            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.FriendUserId });
                b.HasIndex(e => new { e.FriendTenantId, e.UserId });
                b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
            });
            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.SubscriptionEndDateUtc });
                b.HasIndex(e => new { e.CreationTime });
            });
            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new { e.Status, e.CreationTime });
                b.HasIndex(e => new { PaymentId = e.ExternalPaymentId, e.Gateway });
            });

            modelBuilder.ConfigurePersistedGrantEntity();
        }
    }
}