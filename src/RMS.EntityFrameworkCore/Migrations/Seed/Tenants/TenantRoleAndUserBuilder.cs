using System;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Authorization.Users;
using Abp.Notifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RMS.Authorization.Roles;
using RMS.Authorization.Users;
using RMS.EntityFrameworkCore;
using RMS.Notifications;

namespace RMS.Migrations.Seed.Tenants
{
    public class TenantRoleAndUserBuilder
    {
        private readonly RMSDbContext _context;
        private readonly int _tenantId;

        public TenantRoleAndUserBuilder(RMSDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public async void Create()
        {
            await CreateRolesAndUsersAsync();
        }

        private async Task CreateRolesAndUsersAsync()
        {
            // Admin Role
            var adminRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Admin);
            if (adminRole == null)
            {
                adminRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.Admin, StaticRoleNames.Tenants.Admin) { IsStatic = true }).Entity;
                _context.SaveChanges();
            }

            // User Role
            var userRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.User);
            if (userRole == null)
            {
                _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.User, StaticRoleNames.Tenants.User) { IsStatic = true, IsDefault = true });
                _context.SaveChanges();
            }

            // Admin User
            var adminUser = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == AbpUserBase.AdminUserName);
            if (adminUser == null)
            {
                adminUser = User.CreateTenantAdminUser(_tenantId, "ict@servicebureau.nl");
                adminUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(adminUser, "123qwe");
                adminUser.IsEmailConfirmed = true;
                adminUser.ShouldChangePasswordOnNextLogin = false;
                adminUser.IsActive = true;

                _context.Users.Add(adminUser);
                _context.SaveChanges();

                // Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(_tenantId, adminUser.Id, adminRole.Id));
                _context.SaveChanges();

                // User account of Admin user
                if (_tenantId == 1)
                {
                    _context.UserAccounts.Add(new UserAccount
                    {
                        TenantId = _tenantId,
                        UserId = adminUser.Id,
                        UserName = AbpUserBase.AdminUserName,
                        EmailAddress = adminUser.EmailAddress
                    });
                    _context.SaveChanges();
                }

                // Notification Subscription
                _context.NotificationSubscriptions.Add(new NotificationSubscriptionInfo(SequentialGuidGenerator.Instance.Create(), _tenantId, adminUser.Id, AppNotificationNames.NewUserRegistered));
                _context.SaveChanges();

                if (_tenantId != 1) // Tenant ID 1 is the host database, so we want to avoid executing it for that
                {
                    await SetupDataForTenants();
                }
            }
        }

        private async Task SetupDataForTenants()
        {
            // Migrate any pending migrations, so that we can start seeding the completed database
            await _context.Database.MigrateAsync();

            //_context.Countries.AddRange(
            //    new SBJ.CodeTypeTables.Country { Id = 1, TenantId = _tenantId, CreationTime = DateTime.Now, CountryCode = "NL", Description = "Netherlands" },
            //    new SBJ.CodeTypeTables.Country { Id = 2, TenantId = _tenantId, CreationTime = DateTime.Now, CountryCode = "BE", Description = "Belgium" });

            //_context.Locales.AddRange(
            //    new SBJ.CodeTypeTables.Locale { Id = 1, TenantId = _tenantId, CountryId = 1, CreationTime = DateTime.Now, IsActive = true, IsDeleted = false, LanguageCode = "nl", Description = "nl_nl" },
            //    new SBJ.CodeTypeTables.Locale { Id = 2, TenantId = _tenantId, CountryId = 2, CreationTime = DateTime.Now, IsActive = true, IsDeleted = false, LanguageCode = "nl", Description = "be_nl" },
            //    new SBJ.CodeTypeTables.Locale { Id = 3, TenantId = _tenantId, CountryId = 2, CreationTime = DateTime.Now, IsActive = true, IsDeleted = false, LanguageCode = "be", Description = "be_fr" },
            //    new SBJ.CodeTypeTables.Locale { Id = 4, TenantId = _tenantId, CountryId = 1, CreationTime = DateTime.Now, IsActive = true, IsDeleted = false, LanguageCode = "en", Description = "global" });

            //_context.Addresses.Add(new SBJ.Company.Address { Id = 1, TenantId = _tenantId, CountryId = 1, CreationTime = DateTime.Now, IsDeleted = false, AddressLine1 = "Your Street", PostalCode = "0000 PC", City = "Your City" });

            //_context.SystemLevels.AddRange(
            //    new SBJ.SystemTables.SystemLevel { Id = 1, TenantId = _tenantId, CreationTime = DateTime.Now, IsDeleted = false, Description = "Company" },
            //    new SBJ.SystemTables.SystemLevel { Id = 2, TenantId = _tenantId, CreationTime = DateTime.Now, IsDeleted = false, Description = "Campaign" });

            //_context.Companies.Add(new SBJ.Company.Company { Id = 1, TenantId = _tenantId, AddressId = 1, CreationTime = DateTime.Now, IsDeleted = false, Name = "Test Company", EmailAddress = "youremailaddress@domain.com", PhoneNumber = "+31209999999", BicCashBack = "BBBBLLPPFFF", IbanCashBack = "NL00RABO1122334455" });

            //_context.Forms.Add(new SBJ.Forms.Form { Id = 1, TenantId = _tenantId, SystemLevelId = 1, CreationTime = DateTime.Now, IsDeleted = false, Version = "1" });

            //_context.FieldTypes.AddRange(
            //    new SBJ.Forms.FieldType { Id = 1, TenantId = _tenantId, CreationTime = DateTime.Now, IsDeleted = false, Description = "InputText" },
            //    new SBJ.Forms.FieldType { Id = 2, TenantId = _tenantId, CreationTime = DateTime.Now, IsDeleted = false, Description = "InputPassword" },
            //    new SBJ.Forms.FieldType { Id = 3, TenantId = _tenantId, CreationTime = DateTime.Now, IsDeleted = false, Description = "FileUploader" },
            //    new SBJ.Forms.FieldType { Id = 4, TenantId = _tenantId, CreationTime = DateTime.Now, IsDeleted = false, Description = "Rating" },
            //    new SBJ.Forms.FieldType { Id = 5, TenantId = _tenantId, CreationTime = DateTime.Now, IsDeleted = false, Description = "DropdownMenu" },
            //    new SBJ.Forms.FieldType { Id = 6, TenantId = _tenantId, CreationTime = DateTime.Now, IsDeleted = false, Description = "DatePicker" },
            //    new SBJ.Forms.FieldType { Id = 7, TenantId = _tenantId, CreationTime = DateTime.Now, IsDeleted = false, Description = "InputNumber" },
            //    new SBJ.Forms.FieldType { Id = 8, TenantId = _tenantId, CreationTime = DateTime.Now, IsDeleted = false, Description = "RadioButton" },
            //    new SBJ.Forms.FieldType { Id = 9, TenantId = _tenantId, CreationTime = DateTime.Now, IsDeleted = false, Description = "CheckBox" },
            //    new SBJ.Forms.FieldType { Id = 10, TenantId = _tenantId, CreationTime = DateTime.Now, IsDeleted = false, Description = "IbanChecker" },
            //    new SBJ.Forms.FieldType { Id = 11, TenantId = _tenantId, CreationTime = DateTime.Now, IsDeleted = false, Description = "Tile" },
            //    new SBJ.Forms.FieldType { Id = 12, TenantId = _tenantId, CreationTime = DateTime.Now, IsDeleted = false, Description = "Product" },
            //    new SBJ.Forms.FieldType { Id = 13, TenantId = _tenantId, CreationTime = DateTime.Now, IsDeleted = false, Description = "RetailerLocation" },
            //    new SBJ.Forms.FieldType { Id = 14, TenantId = _tenantId, CreationTime = DateTime.Now, IsDeleted = false, Description = "Country" },
            //    new SBJ.Forms.FieldType { Id = 15, TenantId = _tenantId, CreationTime = DateTime.Now, IsDeleted = false, Description = "TextArea" },
            //    new SBJ.Forms.FieldType { Id = 16, TenantId = _tenantId, CreationTime = DateTime.Now, IsDeleted = false, Description = "Remark" },
            //    new SBJ.Forms.FieldType { Id = 17, TenantId = _tenantId, CreationTime = DateTime.Now, IsDeleted = false, Description = "PageSeparator" },
            //    new SBJ.Forms.FieldType { Id = 18, TenantId = _tenantId, CreationTime = DateTime.Now, IsDeleted = false, Description = "RetailerRadioButton" });

            //_context.FormLocales.AddRange(
            //    new SBJ.Forms.FormLocale { Id = 1, TenantId = _tenantId, FormId = 1, LocaleId = 1, CreationTime = DateTime.Now, IsDeleted = false, Description = "nl_nl" },
            //    new SBJ.Forms.FormLocale { Id = 2, TenantId = _tenantId, FormId = 1, LocaleId = 3, CreationTime = DateTime.Now, IsDeleted = false, Description = "be_fr" });

            //_context.FormFields.AddRange(
            //    new SBJ.Forms.FormField { Id = 1, TenantId = _tenantId, FieldTypeId = 1, CreationTime = DateTime.Now, IsDeleted = false, Description = "First Name", Label = "First Name", MaxLength = 255, Required = true, ReadOnly = false, RegistrationField = "FirstName", FieldName = "FirstName" },
            //    new SBJ.Forms.FormField { Id = 2, TenantId = _tenantId, FieldTypeId = 1, CreationTime = DateTime.Now, IsDeleted = false, Description = "Last Name", Label = "Last Name", MaxLength = 255, Required = true, ReadOnly = false, RegistrationField = "LastName", FieldName = "LastName" },
            //    new SBJ.Forms.FormField { Id = 3, TenantId = _tenantId, FieldTypeId = 1, CreationTime = DateTime.Now, IsDeleted = false, Description = "Action Code", Label = "Action Code", MaxLength = 255, Required = false, ReadOnly = false, FieldName = "ActionCode" },
            //    new SBJ.Forms.FormField { Id = 4, TenantId = _tenantId, FieldTypeId = 1, CreationTime = DateTime.Now, IsDeleted = false, Description = "Company Name", Label = "Company Name", MaxLength = 255, Required = false, ReadOnly = false, RegistrationField = "CompanyName", FieldName = "CompanyName" },
            //    new SBJ.Forms.FormField { Id = 5, TenantId = _tenantId, FieldTypeId = 5, CreationTime = DateTime.Now, IsDeleted = false, Description = "Gender", Label = "Gender", MaxLength = 0, Required = true, ReadOnly = false, RegistrationField = "Gender", FieldName = "Gender" },
            //    new SBJ.Forms.FormField { Id = 6, TenantId = _tenantId, FieldTypeId = 1, CreationTime = DateTime.Now, IsDeleted = false, Description = "Legal Form", Label = "Legal Form", MaxLength = 255, Required = false, ReadOnly = false, FieldName = "LegalForm" },
            //    new SBJ.Forms.FormField { Id = 7, TenantId = _tenantId, FieldTypeId = 7, CreationTime = DateTime.Now, IsDeleted = false, Description = "Business Number", Label = "Business Number", MaxLength = 255, Required = false, ReadOnly = false, FieldName = "BusinessNumber" },
            //    new SBJ.Forms.FormField { Id = 8, TenantId = _tenantId, FieldTypeId = 1, CreationTime = DateTime.Now, IsDeleted = false, Description = "VAT Number", Label = "VAT Number", MaxLength = 255, Required = false, ReadOnly = false, FieldName = "VatNumber" },
            //    new SBJ.Forms.FormField { Id = 9, TenantId = _tenantId, FieldTypeId = 1, CreationTime = DateTime.Now, IsDeleted = false, Description = "ZIP Code", Label = "ZIP Code", MaxLength = 6, Required = false, ReadOnly = false, RegistrationField = "PostalCode", FieldName = "ZipCode" },
            //    new SBJ.Forms.FormField { Id = 10, TenantId = _tenantId, FieldTypeId = 1, CreationTime = DateTime.Now, IsDeleted = false, Description = "Street Name", Label = "Street Name", MaxLength = 255, Required = true, ReadOnly = false, RegistrationField = "Street", FieldName = "StreetName" },
            //    new SBJ.Forms.FormField { Id = 11, TenantId = _tenantId, FieldTypeId = 1, CreationTime = DateTime.Now, IsDeleted = false, Description = "House Number", Label = "House Number", MaxLength = 255, Required = true, ReadOnly = false, RegistrationField = "HouseNr", FieldName = "HouseNumber" },
            //    new SBJ.Forms.FormField { Id = 12, TenantId = _tenantId, FieldTypeId = 1, CreationTime = DateTime.Now, IsDeleted = false, Description = "Residence", Label = "Residence", MaxLength = 255, Required = true, ReadOnly = false, RegistrationField = "City", FieldName = "Residence" },
            //    new SBJ.Forms.FormField { Id = 13, TenantId = _tenantId, FieldTypeId = 5, CreationTime = DateTime.Now, IsDeleted = false, Description = "Country", Label = "Country", MaxLength = 255, Required = true, ReadOnly = false, FieldName = "Country" },
            //    new SBJ.Forms.FormField { Id = 14, TenantId = _tenantId, FieldTypeId = 1, CreationTime = DateTime.Now, IsDeleted = false, Description = "Email Address", Label = "Email Address", MaxLength = 255, Required = true, ReadOnly = false, RegistrationField = "EmailAddress", FieldName = "EmailAddress" },
            //    new SBJ.Forms.FormField { Id = 15, TenantId = _tenantId, FieldTypeId = 1, CreationTime = DateTime.Now, IsDeleted = false, Description = "Phone Number", Label = "Phone Number", MaxLength = 255, Required = false, ReadOnly = false, RegistrationField = "PhoneNumber", FieldName = "PhoneNumber" },
            //    new SBJ.Forms.FormField { Id = 16, TenantId = _tenantId, FieldTypeId = 1, CreationTime = DateTime.Now, IsDeleted = false, Description = "Model", Label = "Model", MaxLength = 255, Required = false, ReadOnly = false, RegistrationField = "Model", FieldName = "Model" },
            //    new SBJ.Forms.FormField { Id = 17, TenantId = _tenantId, FieldTypeId = 6, CreationTime = DateTime.Now, IsDeleted = false, Description = "Purchase Date", Label = "Purchase Date", MaxLength = 255, Required = false, ReadOnly = false, PurchaseRegistrationField = "PurchaseDate", FieldName = "PurchaseDate" },
            //    new SBJ.Forms.FormField { Id = 18, TenantId = _tenantId, FieldTypeId = 13, CreationTime = DateTime.Now, IsDeleted = false, Description = "Store Purchased", Label = "Store Purchased", MaxLength = 255, Required = false, ReadOnly = false, FieldName = "StorePurchased" },
            //    new SBJ.Forms.FormField { Id = 19, TenantId = _tenantId, FieldTypeId = 3, CreationTime = DateTime.Now, IsDeleted = false, Description = "Invoice Image Path", Label = "Invoice Image Path", MaxLength = 1000000000, Required = false, ReadOnly = false, PurchaseRegistrationField = "InvoiceImagePath", FieldName = "InvoiceImagePath" },
            //    new SBJ.Forms.FormField { Id = 20, TenantId = _tenantId, FieldTypeId = 3, CreationTime = DateTime.Now, IsDeleted = false, Description = "Serial Number Image", Label = "Serial Number Image", MaxLength = 1000000000, Required = false, ReadOnly = false, FieldName = "SerialImagePath" },
            //    new SBJ.Forms.FormField { Id = 21, TenantId = _tenantId, FieldTypeId = 1, CreationTime = DateTime.Now, IsDeleted = false, Description = "Serial Number", Label = "Serial Number", MaxLength = 255, Required = false, ReadOnly = false, FieldName = "SerialNumber" },
            //    new SBJ.Forms.FormField { Id = 22, TenantId = _tenantId, FieldTypeId = 1, CreationTime = DateTime.Now, IsDeleted = false, Description = "Remarks", Label = "Remarks", MaxLength = 1000000000, Required = false, ReadOnly = false, FieldName = "Remarks" },
            //    new SBJ.Forms.FormField { Id = 23, TenantId = _tenantId, FieldTypeId = 5, CreationTime = DateTime.Now, IsDeleted = false, Description = "Product & Premium", Label = "Product & Premium", MaxLength = 255, Required = false, ReadOnly = false, FieldName = "ProductPremium" },
            //    new SBJ.Forms.FormField { Id = 24, TenantId = _tenantId, FieldTypeId = 7, CreationTime = DateTime.Now, IsDeleted = false, Description = "Quantity", Label = "Quantity", MaxLength = 1000000000, Required = false, ReadOnly = false, FieldName = "Quantity" },
            //    new SBJ.Forms.FormField { Id = 25, TenantId = _tenantId, FieldTypeId = 9, CreationTime = DateTime.Now, IsDeleted = false, Description = "Policy", Label = "Policy", MaxLength = 1000000000, Required = false, ReadOnly = false, FieldName = "Policy" },
            //    new SBJ.Forms.FormField { Id = 26, TenantId = _tenantId, FieldTypeId = 10, CreationTime = DateTime.Now, IsDeleted = false, Description = "IBAN", Label = "IBAN", MaxLength = 255, Required = false, ReadOnly = false, FieldName = "IbanChecker" },
            //    new SBJ.Forms.FormField { Id = 27, TenantId = _tenantId, FieldTypeId = 1, CreationTime = DateTime.Now, IsDeleted = false, Description = "Store Not Available", Label = "Store Not Available", MaxLength = 255, Required = false, ReadOnly = false, FieldName = "StoreNotAvailible" },
            //    new SBJ.Forms.FormField { Id = 28, TenantId = _tenantId, FieldTypeId = 9, CreationTime = DateTime.Now, IsDeleted = false, Description = "Newsletter", Label = "Newsletter", MaxLength = 255, Required = false, ReadOnly = false, FieldName = "Newsletter" },
            //    new SBJ.Forms.FormField { Id = 29, TenantId = _tenantId, FieldTypeId = 17, CreationTime = DateTime.Now, IsDeleted = false, Description = "Page Separator", Label = "Page Separator", MaxLength = 0, Required = false, ReadOnly = false, FieldName = "PageSeparator" },
            //    new SBJ.Forms.FormField { Id = 30, TenantId = _tenantId, FieldTypeId = 18, CreationTime = DateTime.Now, IsDeleted = false, Description = "Store Picker", Label = "Store Picker", MaxLength = 255, Required = false, ReadOnly = false, FieldName = "StorePicker" },
            //    new SBJ.Forms.FormField { Id = 31, TenantId = _tenantId, FieldTypeId = 12, CreationTime = DateTime.Now, IsDeleted = false, Description = "Product", Label = "Product", MaxLength = 255, Required = false, ReadOnly = false, FieldName = "Product" },
            //    new SBJ.Forms.FormField { Id = 32, TenantId = _tenantId, FieldTypeId = 1, CreationTime = DateTime.Now, IsDeleted = false, Description = "Place Name Store", Label = "Place Name Store", MaxLength = 255, Required = false, ReadOnly = false, RegistrationField = "FirstName", FieldName = "PlaceNameStore" });

            //_context.FormBlocks.AddRange(
            //    new SBJ.Forms.FormBlock { Id = 1, TenantId = _tenantId, FormLocaleId = 1, CreationTime = DateTime.Now, IsDeleted = false, IsPurchaseRegistration = false, Description = "NAW", SortOrder = 1 },
            //    new SBJ.Forms.FormBlock { Id = 2, TenantId = _tenantId, FormLocaleId = 2, CreationTime = DateTime.Now, IsDeleted = false, IsPurchaseRegistration = false, Description = "NAW", SortOrder = 1 });

            //_context.FormBlockFields.AddRange(
            //    new SBJ.Forms.FormBlockField { Id = 1, TenantId = _tenantId, FormFieldId = 5, FormBlockId = 1, CreationTime = DateTime.Now, IsDeleted = false, SortOrder = 1 },
            //    new SBJ.Forms.FormBlockField { Id = 2, TenantId = _tenantId, FormFieldId = 1, FormBlockId = 1, CreationTime = DateTime.Now, IsDeleted = false, SortOrder = 2 },
            //    new SBJ.Forms.FormBlockField { Id = 3, TenantId = _tenantId, FormFieldId = 2, FormBlockId = 1, CreationTime = DateTime.Now, IsDeleted = false, SortOrder = 3 },
            //    new SBJ.Forms.FormBlockField { Id = 4, TenantId = _tenantId, FormFieldId = 10, FormBlockId = 1, CreationTime = DateTime.Now, IsDeleted = false, SortOrder = 4 },
            //    new SBJ.Forms.FormBlockField { Id = 5, TenantId = _tenantId, FormFieldId = 11, FormBlockId = 1, CreationTime = DateTime.Now, IsDeleted = false, SortOrder = 5 },
            //    new SBJ.Forms.FormBlockField { Id = 6, TenantId = _tenantId, FormFieldId = 12, FormBlockId = 1, CreationTime = DateTime.Now, IsDeleted = false, SortOrder = 6 },
            //    new SBJ.Forms.FormBlockField { Id = 7, TenantId = _tenantId, FormFieldId = 14, FormBlockId = 1, CreationTime = DateTime.Now, IsDeleted = false, SortOrder = 7 });

            //_context.FormFieldTranslations.AddRange(
            //    new SBJ.Forms.FormFieldTranslation { Id = 1, TenantId = _tenantId, FormFieldId = 1, LocaleId = 1, CreatorUserId = 1, CreationTime = DateTime.Now, IsDeleted = false, Label = "Voornaam" },
            //    new SBJ.Forms.FormFieldTranslation { Id = 2, TenantId = _tenantId, FormFieldId = 2, LocaleId = 1, CreatorUserId = 1, CreationTime = DateTime.Now, IsDeleted = false, Label = "Achternaam" },
            //    new SBJ.Forms.FormFieldTranslation { Id = 3, TenantId = _tenantId, FormFieldId = 11, LocaleId = 1, CreatorUserId = 1, CreationTime = DateTime.Now, IsDeleted = false, Label = "Huisnummer" },
            //    new SBJ.Forms.FormFieldTranslation { Id = 4, TenantId = _tenantId, FormFieldId = 9, LocaleId = 1, CreatorUserId = 1, CreationTime = DateTime.Now, IsDeleted = false, Label = "Postcode" },
            //    new SBJ.Forms.FormFieldTranslation { Id = 5, TenantId = _tenantId, FormFieldId = 12, LocaleId = 1, CreatorUserId = 1, CreationTime = DateTime.Now, IsDeleted = false, Label = "Woonplaats" },
            //    new SBJ.Forms.FormFieldTranslation { Id = 6, TenantId = _tenantId, FormFieldId = 14, LocaleId = 1, CreatorUserId = 1, CreationTime = DateTime.Now, IsDeleted = false, Label = "E-mailadres" },
            //    new SBJ.Forms.FormFieldTranslation { Id = 7, TenantId = _tenantId, FormFieldId = 15, LocaleId = 1, CreatorUserId = 1, CreationTime = DateTime.Now, IsDeleted = false, Label = "Telefoonnummer" },
            //    new SBJ.Forms.FormFieldTranslation { Id = 8, TenantId = _tenantId, FormFieldId = 4, LocaleId = 1, CreatorUserId = 1, CreationTime = DateTime.Now, IsDeleted = false, Label = "Bedrijfsnaam" },
            //    new SBJ.Forms.FormFieldTranslation { Id = 9, TenantId = _tenantId, FormFieldId = 6, LocaleId = 1, CreatorUserId = 1, CreationTime = DateTime.Now, IsDeleted = false, Label = "Bedrijfsvorm (B.V., V.O.F., ZZP)" },
            //    new SBJ.Forms.FormFieldTranslation { Id = 10, TenantId = _tenantId, FormFieldId = 7, LocaleId = 1, CreatorUserId = 1, CreationTime = DateTime.Now, IsDeleted = false, Label = "KvK-nummer" },
            //    new SBJ.Forms.FormFieldTranslation { Id = 11, TenantId = _tenantId, FormFieldId = 8, LocaleId = 1, CreatorUserId = 1, CreationTime = DateTime.Now, IsDeleted = false, Label = "BTW-nummer" },
            //    new SBJ.Forms.FormFieldTranslation { Id = 12, TenantId = _tenantId, FormFieldId = 17, LocaleId = 1, CreatorUserId = 1, CreationTime = DateTime.Now, IsDeleted = false, Label = "Aankoopdatum" },
            //    new SBJ.Forms.FormFieldTranslation { Id = 13, TenantId = _tenantId, FormFieldId = 18, LocaleId = 1, CreatorUserId = 1, CreationTime = DateTime.Now, IsDeleted = false, Label = "Winkel" },
            //    new SBJ.Forms.FormFieldTranslation { Id = 14, TenantId = _tenantId, FormFieldId = 27, LocaleId = 1, CreatorUserId = 1, CreationTime = DateTime.Now, IsDeleted = false, Label = "Ik heb mijn product aangekocht bij [...]" },
            //    new SBJ.Forms.FormFieldTranslation { Id = 15, TenantId = _tenantId, FormFieldId = 24, LocaleId = 1, CreatorUserId = 1, CreationTime = DateTime.Now, IsDeleted = false, Label = "Aantal" },
            //    new SBJ.Forms.FormFieldTranslation { Id = 16, TenantId = _tenantId, FormFieldId = 19, LocaleId = 1, CreatorUserId = 1, CreationTime = DateTime.Now, IsDeleted = false, Label = "Aankoopnota" },
            //    new SBJ.Forms.FormFieldTranslation { Id = 17, TenantId = _tenantId, FormFieldId = 26, LocaleId = 1, CreatorUserId = 1, CreationTime = DateTime.Now, IsDeleted = false, Label = "IBAN voor uitbetaling" },
            //    new SBJ.Forms.FormFieldTranslation { Id = 18, TenantId = _tenantId, FormFieldId = 28, LocaleId = 1, CreatorUserId = 1, CreationTime = DateTime.Now, IsDeleted = false, Label = string.Empty },
            //    new SBJ.Forms.FormFieldTranslation { Id = 19, TenantId = _tenantId, FormFieldId = 10, LocaleId = 1, CreatorUserId = 1, CreationTime = DateTime.Now, IsDeleted = false, Label = "Straat" },
            //    new SBJ.Forms.FormFieldTranslation { Id = 20, TenantId = _tenantId, FormFieldId = 29, LocaleId = 1, CreatorUserId = 1, CreationTime = DateTime.Now, IsDeleted = false, Label = "Pagina-onderbreker" },
            //    new SBJ.Forms.FormFieldTranslation { Id = 21, TenantId = _tenantId, FormFieldId = 30, LocaleId = 1, CreatorUserId = 1, CreationTime = DateTime.Now, IsDeleted = false, Label = "Winkel" },
            //    new SBJ.Forms.FormFieldTranslation { Id = 22, TenantId = _tenantId, FormFieldId = 31, LocaleId = 1, CreatorUserId = 1, CreationTime = DateTime.Now, IsDeleted = false, Label = "Product" },
            //    new SBJ.Forms.FormFieldTranslation { Id = 23, TenantId = _tenantId, FormFieldId = 32, LocaleId = 1, CreatorUserId = 1, CreationTime = DateTime.Now, IsDeleted = false, Label = "Plaatsnaam winkel" },
            //    new SBJ.Forms.FormFieldTranslation { Id = 24, TenantId = _tenantId, FormFieldId = 25, LocaleId = 1, CreatorUserId = 1, CreationTime = DateTime.Now, IsDeleted = false, Label = "Voorwaarden" },
            //    new SBJ.Forms.FormFieldTranslation { Id = 25, TenantId = _tenantId, FormFieldId = 5, LocaleId = 1, CreatorUserId = 1, CreationTime = DateTime.Now, IsDeleted = false, Label = "Geslacht" },
            //    new SBJ.Forms.FormFieldTranslation { Id = 26, TenantId = _tenantId, FormFieldId = 23, LocaleId = 1, CreatorUserId = 1, CreationTime = DateTime.Now, IsDeleted = false, Label = "Product & geschenk" });

            //_context.ValueLists.AddRange(
            //    new SBJ.Forms.ValueList { Id = 1, TenantId = _tenantId, CreationTime = DateTime.Now, IsDeleted = false, Description = "Gender" },
            //    new SBJ.Forms.ValueList { Id = 2, TenantId = _tenantId, CreationTime = DateTime.Now, IsDeleted = false, Description = "Newsletter" });

            //_context.ListValues.AddRange(
            //    new SBJ.Forms.ListValue { Id = 1, TenantId = _tenantId, ValueListId = 1, CreationTime = DateTime.Now, IsDeleted = false, KeyValue = "Man", Description = "Man", SortOrder = 1 },
            //    new SBJ.Forms.ListValue { Id = 2, TenantId = _tenantId, ValueListId = 1, CreationTime = DateTime.Now, IsDeleted = false, KeyValue = "Woman", Description = "Woman", SortOrder = 2 },
            //    new SBJ.Forms.ListValue { Id = 3, TenantId = _tenantId, ValueListId = 1, CreationTime = DateTime.Now, IsDeleted = false, KeyValue = "Other", Description = "Other", SortOrder = 3 },
            //    new SBJ.Forms.ListValue { Id = 4, TenantId = _tenantId, ValueListId = 2, CreationTime = DateTime.Now, IsDeleted = false, KeyValue = "True", Description = "Newsletter", SortOrder = 1 });

            //_context.ListValueTranslations.AddRange(
            //    new SBJ.Forms.ListValueTranslation { Id = 1, TenantId = _tenantId, ListValueId = 1, LocaleId = 1, CreationTime = DateTime.Now, IsDeleted = false, KeyValue = "Man", Description = "Man" },
            //    new SBJ.Forms.ListValueTranslation { Id = 1, TenantId = _tenantId, ListValueId = 2, LocaleId = 1, CreationTime = DateTime.Now, IsDeleted = false, KeyValue = "Woman", Description = "Vrouw" },
            //    new SBJ.Forms.ListValueTranslation { Id = 1, TenantId = _tenantId, ListValueId = 3, LocaleId = 1, CreationTime = DateTime.Now, IsDeleted = false, KeyValue = "Other", Description = "Anders" },
            //    new SBJ.Forms.ListValueTranslation { Id = 1, TenantId = _tenantId, ListValueId = 4, LocaleId = 1, CreationTime = DateTime.Now, IsDeleted = false, KeyValue = "True", Description = "Ik wil e-mails van [...] ontvangen over [...]" });

            //_context.FormFieldValueLists.Add(new SBJ.Forms.FormFieldValueList { Id = 1, TenantId = _tenantId, FormFieldId = 5, ValueListId = 1, CreationTime = DateTime.Now, IsDeleted = false });

            //_context.Campaigns.Add(new SBJ.CampaignProcesses.Campaign { Id = 1, TenantId = _tenantId, CreatorUserId = 1, CreationTime = DateTime.Now, IsDeleted = false, Name = "CampaignName", Description = "Campaign Description", CampaignCode = 999999, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), ExternalCode = "Test Campaign" });

            //_context.CampaignForms.Add(new SBJ.CampaignProcesses.CampaignForm { Id = 1, TenantId = _tenantId, CampaignId = 1, FormId = 1, CreatorUserId = 1, CreationTime = DateTime.Now, IsDeleted = false, IsActive = true });

            //_context.CampaignTranslations.AddRange(
            //    new SBJ.CampaignProcesses.CampaignTranslation { Id = 1, TenantId = _tenantId, CampaignId = 1, LocaleId = 1, Name = "CampaignName", Description = "Naam van de campagne" },
            //    new SBJ.CampaignProcesses.CampaignTranslation { Id = 2, TenantId = _tenantId, CampaignId = 1, LocaleId = 1, Name = "CampaignDescription", Description = "Omschrijving van de campagne" });

            //_context.RegistrationStatuses.AddRange(
            //    new SBJ.CodeTypeTables.RegistrationStatus { Id = 1, TenantId = _tenantId, CreatorUserId = 1, CreationTime = DateTime.Now, IsDeleted = false, IsActive = true, StatusCode = "100", Description = "Pending" },
            //    new SBJ.CodeTypeTables.RegistrationStatus { Id = 2, TenantId = _tenantId, CreatorUserId = 1, CreationTime = DateTime.Now, IsDeleted = false, IsActive = true, StatusCode = "110", Description = "Awaiting Invoice Check" },
            //    new SBJ.CodeTypeTables.RegistrationStatus { Id = 3, TenantId = _tenantId, CreatorUserId = 1, CreationTime = DateTime.Now, IsDeleted = false, IsActive = true, StatusCode = "200", Description = "Approved" },
            //    new SBJ.CodeTypeTables.RegistrationStatus { Id = 4, TenantId = _tenantId, CreatorUserId = 1, CreationTime = DateTime.Now, IsDeleted = false, IsActive = true, StatusCode = "300", Description = "In Progress" },
            //    new SBJ.CodeTypeTables.RegistrationStatus { Id = 5, TenantId = _tenantId, CreatorUserId = 1, CreationTime = DateTime.Now, IsDeleted = false, IsActive = true, StatusCode = "400", Description = "Sent" },
            //    new SBJ.CodeTypeTables.RegistrationStatus { Id = 6, TenantId = _tenantId, CreatorUserId = 1, CreationTime = DateTime.Now, IsDeleted = false, IsActive = true, StatusCode = "500", Description = "Incomplete" },
            //    new SBJ.CodeTypeTables.RegistrationStatus { Id = 7, TenantId = _tenantId, CreatorUserId = 1, CreationTime = DateTime.Now, IsDeleted = false, IsActive = true, StatusCode = "999", Description = "Rejected" });

            //_context.RejectionReasons.AddRange(
            //    new SBJ.CodeTypeTables.RejectionReason { Id = 1, TenantId = _tenantId, Description = "Double registration" },
            //    new SBJ.CodeTypeTables.RejectionReason { Id = 2, TenantId = _tenantId, Description = "Test registration" },
            //    new SBJ.CodeTypeTables.RejectionReason { Id = 3, TenantId = _tenantId, Description = "Brought back" },
            //    new SBJ.CodeTypeTables.RejectionReason { Id = 4, TenantId = _tenantId, Description = "Incorrect type number" },
            //    new SBJ.CodeTypeTables.RejectionReason { Id = 5, TenantId = _tenantId, Description = "No participating retailer" },
            //    new SBJ.CodeTypeTables.RejectionReason { Id = 6, TenantId = _tenantId, Description = "Product wasn't purchased during the campaign" },
            //    new SBJ.CodeTypeTables.RejectionReason { Id = 7, TenantId = _tenantId, Description = "Combi-sets are not eligible" },
            //    new SBJ.CodeTypeTables.RejectionReason { Id = 8, TenantId = _tenantId, Description = "Incorrect campaign" },
            //    new SBJ.CodeTypeTables.RejectionReason { Id = 9, TenantId = _tenantId, Description = "Dealers are excluded from the campaign" });

            //_context.RejectionReasonTranslations.AddRange(
            //    new SBJ.CodeTypeTables.RejectionReasonTranslation { Id = 1, TenantId = _tenantId, LocaleId = 1, RejectionReasonId = 1, Description = "Double registration" },
            //    new SBJ.CodeTypeTables.RejectionReasonTranslation { Id = 2, TenantId = _tenantId, LocaleId = 1, RejectionReasonId = 2, Description = "Test registration" },
            //    new SBJ.CodeTypeTables.RejectionReasonTranslation { Id = 3, TenantId = _tenantId, LocaleId = 1, RejectionReasonId = 3, Description = "Retour gebracht" },
            //    new SBJ.CodeTypeTables.RejectionReasonTranslation { Id = 4, TenantId = _tenantId, LocaleId = 1, RejectionReasonId = 4, Description = "Onjuist typenummer" },
            //    new SBJ.CodeTypeTables.RejectionReasonTranslation { Id = 5, TenantId = _tenantId, LocaleId = 1, RejectionReasonId = 5, Description = "Geen deelnemende winkel" },
            //    new SBJ.CodeTypeTables.RejectionReasonTranslation { Id = 6, TenantId = _tenantId, LocaleId = 1, RejectionReasonId = 6, Description = "Product is gekocht buiten de actieperiode" },
            //    new SBJ.CodeTypeTables.RejectionReasonTranslation { Id = 7, TenantId = _tenantId, LocaleId = 1, RejectionReasonId = 7, Description = "Combi-sets zijn uitgesloten van deelname" },
            //    new SBJ.CodeTypeTables.RejectionReasonTranslation { Id = 8, TenantId = _tenantId, LocaleId = 1, RejectionReasonId = 8, Description = "Verkeerde actie" },
            //    new SBJ.CodeTypeTables.RejectionReasonTranslation { Id = 9, TenantId = _tenantId, LocaleId = 1, RejectionReasonId = 9, Description = "Dealers zijn uitgesloten van de actie" },
            //    new SBJ.CodeTypeTables.RejectionReasonTranslation { Id = 10, TenantId = _tenantId, LocaleId = 2, RejectionReasonId = 1, Description = "Double registration" },
            //    new SBJ.CodeTypeTables.RejectionReasonTranslation { Id = 11, TenantId = _tenantId, LocaleId = 2, RejectionReasonId = 2, Description = "Test registration" },
            //    new SBJ.CodeTypeTables.RejectionReasonTranslation { Id = 12, TenantId = _tenantId, LocaleId = 2, RejectionReasonId = 3, Description = "Retour gebracht" },
            //    new SBJ.CodeTypeTables.RejectionReasonTranslation { Id = 13, TenantId = _tenantId, LocaleId = 2, RejectionReasonId = 4, Description = "Onjuist typenummer" },
            //    new SBJ.CodeTypeTables.RejectionReasonTranslation { Id = 14, TenantId = _tenantId, LocaleId = 2, RejectionReasonId = 5, Description = "Geen deelnemende winkel" },
            //    new SBJ.CodeTypeTables.RejectionReasonTranslation { Id = 15, TenantId = _tenantId, LocaleId = 2, RejectionReasonId = 6, Description = "Product is gekocht buiten de actieperiode" },
            //    new SBJ.CodeTypeTables.RejectionReasonTranslation { Id = 16, TenantId = _tenantId, LocaleId = 2, RejectionReasonId = 7, Description = "Combi-sets zijn uitgesloten van deelname" },
            //    new SBJ.CodeTypeTables.RejectionReasonTranslation { Id = 17, TenantId = _tenantId, LocaleId = 2, RejectionReasonId = 8, Description = "Verkeerde actie" },
            //    new SBJ.CodeTypeTables.RejectionReasonTranslation { Id = 18, TenantId = _tenantId, LocaleId = 2, RejectionReasonId = 9, Description = "Dealers zijn uitgesloten van de actie" },
            //    new SBJ.CodeTypeTables.RejectionReasonTranslation { Id = 19, TenantId = _tenantId, LocaleId = 3, RejectionReasonId = 1, Description = "Double inscription" },
            //    new SBJ.CodeTypeTables.RejectionReasonTranslation { Id = 20, TenantId = _tenantId, LocaleId = 3, RejectionReasonId = 2, Description = "Inscription aux tests" },
            //    new SBJ.CodeTypeTables.RejectionReasonTranslation { Id = 21, TenantId = _tenantId, LocaleId = 3, RejectionReasonId = 3, Description = "Revenu" },
            //    new SBJ.CodeTypeTables.RejectionReasonTranslation { Id = 22, TenantId = _tenantId, LocaleId = 3, RejectionReasonId = 4, Description = "Numéro de type incorrect" },
            //    new SBJ.CodeTypeTables.RejectionReasonTranslation { Id = 23, TenantId = _tenantId, LocaleId = 3, RejectionReasonId = 5, Description = "Aucun magasin participant" },
            //    new SBJ.CodeTypeTables.RejectionReasonTranslation { Id = 24, TenantId = _tenantId, LocaleId = 3, RejectionReasonId = 6, Description = "Le produit a été acheté en dehors de la période promotionnelle" },
            //    new SBJ.CodeTypeTables.RejectionReasonTranslation { Id = 25, TenantId = _tenantId, LocaleId = 3, RejectionReasonId = 7, Description = "Les ensembles combinés sont exclus de la participation" },
            //    new SBJ.CodeTypeTables.RejectionReasonTranslation { Id = 26, TenantId = _tenantId, LocaleId = 3, RejectionReasonId = 8, Description = "Mauvaise action" },
            //    new SBJ.CodeTypeTables.RejectionReasonTranslation { Id = 27, TenantId = _tenantId, LocaleId = 3, RejectionReasonId = 9, Description = "Les revendeurs sont exclus de la promotion" });

            //_context.HandlingBatchStatuses.AddRange(
            //    new SBJ.HandlingBatch.HandlingBatchStatus { Id = 1, TenantId = _tenantId, StatusCode = "100", StatusDescription = "Pending" },
            //    new SBJ.HandlingBatch.HandlingBatchStatus { Id = 2, TenantId = _tenantId, StatusCode = "200", StatusDescription = "In Progress" },
            //    new SBJ.HandlingBatch.HandlingBatchStatus { Id = 2, TenantId = _tenantId, StatusCode = "300", StatusDescription = "Finished" },
            //    new SBJ.HandlingBatch.HandlingBatchStatus { Id = 2, TenantId = _tenantId, StatusCode = "600", StatusDescription = "Failed" },
            //    new SBJ.HandlingBatch.HandlingBatchStatus { Id = 2, TenantId = _tenantId, StatusCode = "700", StatusDescription = "Blocked" },
            //    new SBJ.HandlingBatch.HandlingBatchStatus { Id = 2, TenantId = _tenantId, StatusCode = "900", StatusDescription = "Cancelled" });

            //_context.HandlingBatchLineStatuses.AddRange(
            //    new SBJ.HandlingBatch.HandlingBatchLineStatus { Id = 1, TenantId = _tenantId, StatusCode = "100", StatusDescription = "Pending" },
            //    new SBJ.HandlingBatch.HandlingBatchLineStatus { Id = 1, TenantId = _tenantId, StatusCode = "200", StatusDescription = "In Progress" },
            //    new SBJ.HandlingBatch.HandlingBatchLineStatus { Id = 1, TenantId = _tenantId, StatusCode = "210", StatusDescription = "Processed partially with failed orders" },
            //    new SBJ.HandlingBatch.HandlingBatchLineStatus { Id = 1, TenantId = _tenantId, StatusCode = "220", StatusDescription = "Processed partially with blocked lines" },
            //    new SBJ.HandlingBatch.HandlingBatchLineStatus { Id = 1, TenantId = _tenantId, StatusCode = "230", StatusDescription = "Processed partially with failed orders & blocked lines" },
            //    new SBJ.HandlingBatch.HandlingBatchLineStatus { Id = 1, TenantId = _tenantId, StatusCode = "240", StatusDescription = "Unprocessed because of failed orders" },
            //    new SBJ.HandlingBatch.HandlingBatchLineStatus { Id = 1, TenantId = _tenantId, StatusCode = "250", StatusDescription = "Unprocessed because of blocked lines" },
            //    new SBJ.HandlingBatch.HandlingBatchLineStatus { Id = 1, TenantId = _tenantId, StatusCode = "260", StatusDescription = "Unprocessed because of failed orders & blocked lines" },
            //    new SBJ.HandlingBatch.HandlingBatchLineStatus { Id = 1, TenantId = _tenantId, StatusCode = "300", StatusDescription = "Finished" },
            //    new SBJ.HandlingBatch.HandlingBatchLineStatus { Id = 1, TenantId = _tenantId, StatusCode = "900", StatusDescription = "Cancelled" });

            await _context.SaveChangesAsync();
        }
    }
}
