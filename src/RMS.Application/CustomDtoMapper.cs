using RMS.SBJ.MakitaBaseModelSerial.Dtos;
using RMS.SBJ.MakitaBaseModelSerial;
using RMS.SBJ.ProductGifts.Dtos;
using RMS.SBJ.ProductGifts;
using RMS.SBJ.UniqueCodes.Dtos;
using RMS.SBJ.UniqueCodes;
using RMS.SBJ.HandlingBatch.Dtos;
using RMS.SBJ.HandlingBatch;
using RMS.SBJ.RegistrationHistory.Dtos;
using RMS.SBJ.MakitaSerialNumber.Dtos;
using RMS.SBJ.RegistrationJsonData.Dtos;
using RMS.SBJ.PurchaseRegistrationFormFields.Dtos;
using RMS.SBJ.PurchaseRegistrationFormFields;
using RMS.SBJ.PurchaseRegistrationFieldDatas.Dtos;
using RMS.SBJ.PurchaseRegistrationFieldDatas;
using RMS.SBJ.PurchaseRegistrationFields.Dtos;
using RMS.SBJ.PurchaseRegistrationFields;
using RMS.SBJ.RegistrationFormFieldDatas.Dtos;
using RMS.SBJ.RegistrationFormFieldDatas;
using RMS.SBJ.RegistrationFormFields.Dtos;
using RMS.SBJ.RegistrationFormFields;
using RMS.SBJ.PurchaseRegistrations.Dtos;
using RMS.SBJ.PurchaseRegistrations;
using RMS.SBJ.ActivationCodeRegistrations.Dtos;
using RMS.SBJ.ActivationCodeRegistrations;
using RMS.SBJ.Registrations.Dtos;
using RMS.SBJ.Registrations;
using RMS.SBJ.ActivationCodes.Dtos;
using RMS.SBJ.ActivationCodes;
using RMS.SBJ.CampaignRetailerLocations.Dtos;
using RMS.SBJ.CampaignRetailerLocations;
using RMS.SBJ.HandlingLineLocales.Dtos;
using RMS.SBJ.HandlingLineLocales;
using RMS.SBJ.HandlingLineLogics.Dtos;
using RMS.SBJ.HandlingLineLogics;
using RMS.SBJ.HandlingLineRetailers.Dtos;
using RMS.SBJ.HandlingLineRetailers;
using RMS.SBJ.HandlingLineProducts.Dtos;
using RMS.SBJ.HandlingLineProducts;
using RMS.SBJ.RetailerLocations.Dtos;
using RMS.SBJ.RetailerLocations;
using RMS.SBJ.HandlingLines.Dtos;
using RMS.SBJ.HandlingLines;
using RMS.SBJ.ProductHandlings.Dtos;
using RMS.SBJ.ProductHandlings;
using RMS.SBJ.Forms.Dtos;
using RMS.SBJ.Forms;
using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.DynamicEntityParameters;
using Abp.EntityHistory;
using Abp.Localization;
using Abp.Notifications;
using Abp.Organizations;
using Abp.UI.Inputs;
using Abp.Webhooks;
using AutoMapper;
using RMS.Auditing.Dto;
using RMS.Authorization.Accounts.Dto;
using RMS.Authorization.Delegation;
using RMS.Authorization.Permissions.Dto;
using RMS.Authorization.Roles;
using RMS.Authorization.Roles.Dto;
using RMS.Authorization.Users;
using RMS.Authorization.Users.Delegation.Dto;
using RMS.Authorization.Users.Dto;
using RMS.Authorization.Users.Importing.Dto;
using RMS.Authorization.Users.Profile.Dto;
using RMS.Chat;
using RMS.Chat.Dto;
using RMS.DynamicEntityParameters.Dto;
using RMS.Editions;
using RMS.Editions.Dto;
using RMS.Friendships;
using RMS.Friendships.Cache;
using RMS.Friendships.Dto;
using RMS.Localization.Dto;
using RMS.MultiTenancy;
using RMS.MultiTenancy.Dto;
using RMS.MultiTenancy.HostDashboard.Dto;
using RMS.MultiTenancy.Payments;
using RMS.MultiTenancy.Payments.Dto;
using RMS.Notifications.Dto;
using RMS.Organizations.Dto;
using RMS.Sessions.Dto;
using RMS.WebHooks.Dto;
using RMS.SBJ.Products.Dtos;
using RMS.SBJ.Products;
using RMS.SBJ.Retailers.Dtos;
using RMS.SBJ.Retailers;
using RMS.SBJ.Company.Dtos;
using RMS.SBJ.Company;
using RMS.SBJ.SystemTables.Dtos;
using RMS.SBJ.SystemTables;
using RMS.SBJ.CampaignProcesses.Dtos;
using RMS.SBJ.CampaignProcesses;
using RMS.SBJ.CodeTypeTables.Dtos;
using RMS.SBJ.CodeTypeTables;
using RMS.PromoPlanner.Dtos;
using RMS.PromoPlanner;
using RMS.SBJ.RegistrationFields;
using RMS.SBJ.Messaging.Dtos;
using RMS.SBJ.Messaging;
using RMS.SBJ.Report.EmployeePerformanceReports.Dtos;
using RMS.SBJ.Report.EmployeePerformanceReports;

namespace RMS
{
    internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditUniqueCodeByCampaignDto, UniqueCodeByCampaign>().ReverseMap();
            configuration.CreateMap<UniqueCodeByCampaignDto, UniqueCodeByCampaign>().ReverseMap();
            configuration.CreateMap<CreateOrEditMakitaBaseModelSerialDto, SBJ.MakitaBaseModelSerial.MakitaBaseModelSerial>().ReverseMap();
            configuration.CreateMap<MakitaBaseModelSerialDto, SBJ.MakitaBaseModelSerial.MakitaBaseModelSerial>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductGiftDto, ProductGift>().ReverseMap();
            configuration.CreateMap<ProductGiftDto, ProductGift>().ReverseMap();
            configuration.CreateMap<CreateOrEditCampaignCountryDto, CampaignCountry>().ReverseMap();
            configuration.CreateMap<CampaignCountryDto, CampaignCountry>().ReverseMap();
            configuration.CreateMap<CreateOrEditUniqueCodeDto, UniqueCode>().ReverseMap();
            configuration.CreateMap<UniqueCodeDto, UniqueCode>().ReverseMap();
            configuration.CreateMap<CreateOrEditCampaignTranslationDto, CampaignTranslation>().ReverseMap();
            configuration.CreateMap<CampaignTranslationDto, CampaignTranslation>().ReverseMap();
            configuration.CreateMap<CreateOrEditMessageHistoryDto, MessageHistory>().ReverseMap();
            configuration.CreateMap<MessageHistoryDto, MessageHistory>().ReverseMap();
            configuration.CreateMap<CreateOrEditHandlingBatchLineHistoryDto, HandlingBatchLineHistory>().ReverseMap();
            configuration.CreateMap<HandlingBatchLineHistoryDto, HandlingBatchLineHistory>().ReverseMap();
            configuration.CreateMap<CreateOrEditHandlingBatchHistoryDto, HandlingBatchHistory>().ReverseMap();
            configuration.CreateMap<HandlingBatchHistoryDto, HandlingBatchHistory>().ReverseMap();
            configuration.CreateMap<CreateOrEditHandlingBatchLineStatusDto, HandlingBatchLineStatus>().ReverseMap();
            configuration.CreateMap<HandlingBatchLineStatusDto, HandlingBatchLineStatus>().ReverseMap();
            configuration.CreateMap<CreateOrEditHandlingBatchStatusDto, HandlingBatchStatus>().ReverseMap();
            configuration.CreateMap<HandlingBatchStatusDto, HandlingBatchStatus>().ReverseMap();
            configuration.CreateMap<CreateOrEditHandlingBatchLineDto, HandlingBatchLine>().ReverseMap();
            configuration.CreateMap<HandlingBatchLineDto, HandlingBatchLine>().ReverseMap();
            configuration.CreateMap<CreateOrEditHandlingBatchDto, SBJ.HandlingBatch.HandlingBatch>().ReverseMap();
            configuration.CreateMap<HandlingBatchDto, SBJ.HandlingBatch.HandlingBatch>().ReverseMap();
            configuration.CreateMap<CreateOrEditRegistrationHistoryDto, SBJ.RegistrationHistory.RegistrationHistory>().ReverseMap();
            configuration.CreateMap<RegistrationHistoryDto, SBJ.RegistrationHistory.RegistrationHistory>().ReverseMap();
            configuration.CreateMap<CreateOrEditMakitaSerialNumberDto, SBJ.MakitaSerialNumber.MakitaSerialNumber>().ReverseMap();
            configuration.CreateMap<MakitaSerialNumberDto, SBJ.MakitaSerialNumber.MakitaSerialNumber>().ReverseMap();
            configuration.CreateMap<CreateOrEditRejectionReasonDto, RejectionReason>().ReverseMap();
            configuration.CreateMap<RejectionReasonDto, RejectionReason>().ReverseMap();
            configuration.CreateMap<CreateOrEditRegistrationJsonDataDto, SBJ.RegistrationJsonData.RegistrationJsonData>().ReverseMap();
            configuration.CreateMap<RegistrationJsonDataDto, SBJ.RegistrationJsonData.RegistrationJsonData>().ReverseMap();
            configuration.CreateMap<CreateOrEditPurchaseRegistrationFormFieldDto, PurchaseRegistrationFormField>().ReverseMap();
            configuration.CreateMap<PurchaseRegistrationFormFieldDto, PurchaseRegistrationFormField>().ReverseMap();
            configuration.CreateMap<CreateOrEditCampaignMessageDto, CampaignMessage>().ReverseMap();
            configuration.CreateMap<CampaignMessageDto, CampaignMessage>().ReverseMap();
            configuration.CreateMap<CreateOrEditPurchaseRegistrationFieldDataDto, PurchaseRegistrationFieldData>().ReverseMap();
            configuration.CreateMap<PurchaseRegistrationFieldDataDto, PurchaseRegistrationFieldData>().ReverseMap();
            configuration.CreateMap<CreateOrEditPurchaseRegistrationFieldDto, PurchaseRegistrationField>().ReverseMap();
            configuration.CreateMap<PurchaseRegistrationFieldDto, PurchaseRegistrationField>().ReverseMap();
            configuration.CreateMap<CreateOrEditRegistrationFormFieldDataDto, SBJ.RegistrationFormFieldDatas.RegistrationFieldData>().ReverseMap();
            configuration.CreateMap<RegistrationFormFieldDataDto, SBJ.RegistrationFormFieldDatas.RegistrationFieldData>().ReverseMap();
            configuration.CreateMap<CreateOrEditRegistrationFormFieldDto, RegistrationField>().ReverseMap();
            configuration.CreateMap<RegistrationFormFieldDto, RegistrationField>().ReverseMap();
            configuration.CreateMap<CreateOrEditPurchaseRegistrationDto, PurchaseRegistration>().ReverseMap();
            configuration.CreateMap<PurchaseRegistrationDto, PurchaseRegistration>().ReverseMap();
            configuration.CreateMap<CreateOrEditActivationCodeRegistrationDto, ActivationCodeRegistration>().ReverseMap();
            configuration.CreateMap<ActivationCodeRegistrationDto, ActivationCodeRegistration>().ReverseMap();
            configuration.CreateMap<CreateOrEditRegistrationDto, Registration>().ReverseMap();
            configuration.CreateMap<RegistrationDto, Registration>().ReverseMap();
            configuration.CreateMap<CreateOrEditActivationCodeDto, ActivationCode>().ReverseMap();
            configuration.CreateMap<ActivationCodeDto, ActivationCode>().ReverseMap();
            configuration.CreateMap<CreateOrEditCampaignRetailerLocationDto, CampaignRetailerLocation>().ReverseMap();
            configuration.CreateMap<CampaignRetailerLocationDto, CampaignRetailerLocation>().ReverseMap();
            configuration.CreateMap<CreateOrEditHandlingLineLocaleDto, HandlingLineLocale>().ReverseMap();
            configuration.CreateMap<HandlingLineLocaleDto, HandlingLineLocale>().ReverseMap();
            configuration.CreateMap<CreateOrEditHandlingLineLogicDto, HandlingLineLogic>().ReverseMap();
            configuration.CreateMap<HandlingLineLogicDto, HandlingLineLogic>().ReverseMap();
            configuration.CreateMap<CreateOrEditHandlingLineRetailerDto, HandlingLineRetailer>().ReverseMap();
            configuration.CreateMap<HandlingLineRetailerDto, HandlingLineRetailer>().ReverseMap();
            configuration.CreateMap<CreateOrEditHandlingLineProductDto, HandlingLineProduct>().ReverseMap();
            configuration.CreateMap<HandlingLineProductDto, HandlingLineProduct>().ReverseMap();
            configuration.CreateMap<CreateOrEditRetailerLocationDto, RetailerLocation>().ReverseMap();
            configuration.CreateMap<RetailerLocationDto, RetailerLocation>().ReverseMap();
            configuration.CreateMap<CreateOrEditHandlingLineDto, HandlingLine>().ReverseMap();
            configuration.CreateMap<HandlingLineDto, HandlingLine>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductHandlingDto, ProductHandling>().ReverseMap();
            configuration.CreateMap<ProductHandlingDto, ProductHandling>().ReverseMap();
            configuration.CreateMap<CreateOrEditCampaignFormDto, CampaignForm>().ReverseMap();
            configuration.CreateMap<CampaignFormDto, CampaignForm>().ReverseMap();
            configuration.CreateMap<CreateOrEditCampaignCampaignTypeDto, CampaignCampaignType>().ReverseMap();
            configuration.CreateMap<CampaignCampaignTypeDto, CampaignCampaignType>().ReverseMap();
            configuration.CreateMap<CreateOrEditCampaignDto, Campaign>().ReverseMap();
            configuration.CreateMap<CampaignDto, Campaign>().ReverseMap();
            configuration.CreateMap<CreateOrEditFormFieldTranslationDto, FormFieldTranslation>().ReverseMap();
            configuration.CreateMap<FormFieldTranslationDto, FormFieldTranslation>().ReverseMap();
            configuration.CreateMap<CreateOrEditFormFieldValueListDto, FormFieldValueList>().ReverseMap();
            configuration.CreateMap<FormFieldValueListDto, FormFieldValueList>().ReverseMap();
            configuration.CreateMap<CreateOrEditListValueTranslationDto, ListValueTranslation>().ReverseMap();
            configuration.CreateMap<ListValueTranslationDto, ListValueTranslation>().ReverseMap();
            configuration.CreateMap<CreateOrEditListValueDto, ListValue>().ReverseMap();
            configuration.CreateMap<ListValueDto, ListValue>().ReverseMap();
            configuration.CreateMap<CreateOrEditValueListDto, ValueList>().ReverseMap();
            configuration.CreateMap<ValueListDto, ValueList>().ReverseMap();
            configuration.CreateMap<CreateOrEditFormBlockFieldDto, FormBlockField>().ReverseMap();
            configuration.CreateMap<FormBlockFieldDto, FormBlockField>().ReverseMap();
            configuration.CreateMap<CreateOrEditFormFieldDto, FormField>().ReverseMap();
            configuration.CreateMap<FormFieldDto, FormField>().ReverseMap();
            configuration.CreateMap<CreateOrEditFieldTypeDto, FieldType>().ReverseMap();
            configuration.CreateMap<FieldTypeDto, FieldType>().ReverseMap();
            configuration.CreateMap<CreateOrEditFormLocaleDto, FormLocale>().ReverseMap();
            configuration.CreateMap<FormLocaleDto, FormLocale>().ReverseMap();
            configuration.CreateMap<CreateOrEditFormBlockDto, FormBlock>().ReverseMap();
            configuration.CreateMap<FormBlockDto, FormBlock>().ReverseMap();
            configuration.CreateMap<CreateOrEditFormDto, Form>().ReverseMap();
            configuration.CreateMap<FormDto, Form>().ReverseMap();
            configuration.CreateMap<CreateOrEditPromoStepFieldDataDto, PromoStepFieldData>().ReverseMap();
            configuration.CreateMap<PromoStepFieldDataDto, PromoStepFieldData>().ReverseMap();
            configuration.CreateMap<CreateOrEditPromoStepDataDto, PromoStepData>().ReverseMap();
            configuration.CreateMap<PromoStepDataDto, PromoStepData>().ReverseMap();
            configuration.CreateMap<CreateOrEditPromoStepFieldDto, PromoStepField>().ReverseMap();
            configuration.CreateMap<PromoStepFieldDto, PromoStepField>().ReverseMap();
            configuration.CreateMap<CreateOrEditPromoStepDto, PromoStep>().ReverseMap();
            configuration.CreateMap<PromoStepDto, PromoStep>().ReverseMap();
            configuration.CreateMap<CreateOrEditPromoProductDto, PromoProduct>().ReverseMap();
            configuration.CreateMap<PromoProductDto, PromoProduct>().ReverseMap();
            configuration.CreateMap<CreateOrEditPromoRetailerDto, PromoRetailer>().ReverseMap();
            configuration.CreateMap<PromoRetailerDto, PromoRetailer>().ReverseMap();
            configuration.CreateMap<CreateOrEditPromoCountryDto, PromoCountry>().ReverseMap();
            configuration.CreateMap<PromoCountryDto, PromoCountry>().ReverseMap();
            configuration.CreateMap<CreateOrEditPromoDto, Promo>().ReverseMap();
            configuration.CreateMap<PromoDto, Promo>().ReverseMap();
            configuration.CreateMap<CreateOrEditPromoScopeDto, PromoScope>().ReverseMap();
            configuration.CreateMap<PromoScopeDto, PromoScope>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductCategoryYearPoDto, ProductCategoryYearPo>().ReverseMap();
            configuration.CreateMap<ProductCategoryYearPoDto, ProductCategoryYearPo>().ReverseMap();
            //RMS
            configuration.CreateMap<CreateOrEditProductDto, Product>().ReverseMap();
            configuration.CreateMap<ProductDto, Product>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductCategoryDto, ProductCategory>().ReverseMap();
            configuration.CreateMap<ProductCategoryDto, ProductCategory>().ReverseMap();
            configuration.CreateMap<CreateOrEditRetailerDto, Retailer>().ReverseMap();
            configuration.CreateMap<RetailerDto, Retailer>().ReverseMap();
            configuration.CreateMap<CreateOrEditCampaignStepDto, CampaignStep>().ReverseMap();
            configuration.CreateMap<CampaignStepDto, CampaignStep>().ReverseMap();
            configuration.CreateMap<CreateOrEditCompanyDto, Company>().ReverseMap();
            configuration.CreateMap<CompanyDto, Company>().ReverseMap();
            configuration.CreateMap<CreateOrEditProjectManagerDto, ProjectManager>().ReverseMap();
            configuration.CreateMap<ProjectManagerDto, ProjectManager>().ReverseMap();
            configuration.CreateMap<CreateOrEditAddressDto, Address>().ReverseMap();
            configuration.CreateMap<AddressDto, Address>().ReverseMap();
            configuration.CreateMap<CreateOrEditMessageVariableDto, MessageVariable>().ReverseMap();
            configuration.CreateMap<MessageVariableDto, MessageVariable>().ReverseMap();
            configuration.CreateMap<CreateOrEditMessageContentTranslationDto, MessageContentTranslation>().ReverseMap();
            configuration.CreateMap<MessageContentTranslationDto, MessageContentTranslation>().ReverseMap();
            configuration.CreateMap<CreateOrEditMessageComponentContentDto, MessageComponentContent>().ReverseMap();
            configuration.CreateMap<MessageComponentContentDto, MessageComponentContent>().ReverseMap();
            configuration.CreateMap<CreateOrEditMessageComponentDto, MessageComponent>().ReverseMap();
            configuration.CreateMap<MessageComponentDto, MessageComponent>().ReverseMap();
            configuration.CreateMap<CreateOrEditMessageTypeDto, MessageType>().ReverseMap();
            configuration.CreateMap<MessageTypeDto, MessageType>().ReverseMap();
            configuration.CreateMap<CreateOrEditMessageDto, Message>().ReverseMap();
            configuration.CreateMap<MessageDto, Message>().ReverseMap();
            configuration.CreateMap<CreateOrEditSystemLevelDto, SystemLevel>().ReverseMap();
            configuration.CreateMap<SystemLevelDto, SystemLevel>().ReverseMap();
            configuration.CreateMap<CreateOrEditCampaignTypeEventRegistrationStatusDto, CampaignTypeEventRegistrationStatus>().ReverseMap();
            configuration.CreateMap<CampaignTypeEventRegistrationStatusDto, CampaignTypeEventRegistrationStatus>().ReverseMap();
            configuration.CreateMap<CreateOrEditCampaignTypeEventDto, CampaignTypeEvent>().ReverseMap();
            configuration.CreateMap<CampaignTypeEventDto, CampaignTypeEvent>().ReverseMap();
            configuration.CreateMap<CreateOrEditCampaignCategoryTranslationDto, CampaignCategoryTranslation>().ReverseMap();
            configuration.CreateMap<CampaignCategoryTranslationDto, CampaignCategoryTranslation>().ReverseMap();
            configuration.CreateMap<CreateOrEditCampaignCategoryDto, CampaignCategory>().ReverseMap();
            configuration.CreateMap<CampaignCategoryDto, CampaignCategory>().ReverseMap();
            configuration.CreateMap<CreateOrEditMessageComponentTypeDto, MessageComponentType>().ReverseMap();
            configuration.CreateMap<MessageComponentTypeDto, MessageComponentType>().ReverseMap();
            configuration.CreateMap<CreateOrEditRegistrationStatusDto, RegistrationStatus>().ReverseMap();
            configuration.CreateMap<RegistrationStatusDto, RegistrationStatus>().ReverseMap();
            configuration.CreateMap<CreateOrEditProcessEventDto, ProcessEvent>().ReverseMap();
            configuration.CreateMap<ProcessEventDto, ProcessEvent>().ReverseMap();
            configuration.CreateMap<CreateOrEditCampaignTypeDto, SBJ.CodeTypeTables.CampaignType>().ReverseMap();
            configuration.CreateMap<CampaignTypeDto, SBJ.CodeTypeTables.CampaignType>().ReverseMap();
            configuration.CreateMap<CreateOrEditLocaleDto, Locale>().ReverseMap();
            configuration.CreateMap<LocaleDto, Locale>().ReverseMap();
            configuration.CreateMap<CreateOrEditCountryDto, Country>().ReverseMap();
            configuration.CreateMap<CountryDto, Country>().ReverseMap();
            configuration.CreateMap<MessagesDto, Messages>().ReverseMap();
            //Inputs
            configuration.CreateMap<CheckboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<SingleLineStringInputType, FeatureInputTypeDto>();
            configuration.CreateMap<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<IInputType, FeatureInputTypeDto>()
                .Include<CheckboxInputType, FeatureInputTypeDto>()
                .Include<SingleLineStringInputType, FeatureInputTypeDto>()
                .Include<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<ILocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>()
                .Include<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<LocalizableComboboxItem, LocalizableComboboxItemDto>();
            configuration.CreateMap<ILocalizableComboboxItem, LocalizableComboboxItemDto>()
                .Include<LocalizableComboboxItem, LocalizableComboboxItemDto>();

            //Chat
            configuration.CreateMap<ChatMessage, ChatMessageDto>();
            configuration.CreateMap<ChatMessage, ChatMessageExportDto>();

            //Feature
            configuration.CreateMap<FlatFeatureSelectDto, Feature>().ReverseMap();
            configuration.CreateMap<Feature, FlatFeatureDto>();

            //Role
            configuration.CreateMap<RoleEditDto, Role>().ReverseMap();
            configuration.CreateMap<Role, RoleListDto>();
            configuration.CreateMap<UserRole, UserListRoleDto>();

            //Edition
            configuration.CreateMap<EditionEditDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<EditionCreateDto, SubscribableEdition>();
            configuration.CreateMap<EditionSelectDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<Edition, EditionInfoDto>().Include<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<SubscribableEdition, EditionListDto>();
            configuration.CreateMap<Edition, EditionEditDto>();
            configuration.CreateMap<Edition, SubscribableEdition>();
            configuration.CreateMap<Edition, EditionSelectDto>();

            //Payment
            configuration.CreateMap<SubscriptionPaymentDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPaymentListDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPayment, SubscriptionPaymentInfoDto>();

            //Permission
            configuration.CreateMap<Permission, FlatPermissionDto>();
            configuration.CreateMap<Permission, FlatPermissionWithLevelDto>();

            //Language
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageListDto>();
            configuration.CreateMap<NotificationDefinition, NotificationSubscriptionWithDisplayNameDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>()
                .ForMember(ldto => ldto.IsEnabled, options => options.MapFrom(l => !l.IsDisabled));

            //Tenant
            configuration.CreateMap<Tenant, RecentTenant>();
            configuration.CreateMap<Tenant, TenantLoginInfoDto>();
            configuration.CreateMap<Tenant, TenantListDto>();
            configuration.CreateMap<TenantEditDto, Tenant>().ReverseMap();
            configuration.CreateMap<CurrentTenantInfoDto, Tenant>().ReverseMap();

            //User
            configuration.CreateMap<User, UserEditDto>()
                .ForMember(dto => dto.Password, options => options.Ignore())
                .ReverseMap()
                .ForMember(user => user.Password, options => options.Ignore());
            configuration.CreateMap<User, UserLoginInfoDto>();
            configuration.CreateMap<User, UserListDto>();
            configuration.CreateMap<User, ChatUserDto>();
            configuration.CreateMap<User, OrganizationUnitUserListDto>();
            configuration.CreateMap<Role, OrganizationUnitRoleListDto>();
            configuration.CreateMap<CurrentUserProfileEditDto, User>().ReverseMap();
            configuration.CreateMap<UserLoginAttemptDto, UserLoginAttempt>().ReverseMap();
            configuration.CreateMap<ImportUserDto, User>();

            //AuditLog
            configuration.CreateMap<AuditLog, AuditLogListDto>();
            configuration.CreateMap<EntityChange, EntityChangeListDto>();
            configuration.CreateMap<EntityPropertyChange, EntityPropertyChangeDto>();

            //Friendship
            configuration.CreateMap<Friendship, FriendDto>();
            configuration.CreateMap<FriendCacheItem, FriendDto>();

            //OrganizationUnit
            configuration.CreateMap<OrganizationUnit, OrganizationUnitDto>();

            //Webhooks
            configuration.CreateMap<WebhookSubscription, GetAllSubscriptionsOutput>();
            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOutput>()
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.WebhookName,
                    options => options.MapFrom(l => l.WebhookEvent.WebhookName))
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.Data,
                    options => options.MapFrom(l => l.WebhookEvent.Data));

            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOfWebhookEventOutput>();

            configuration.CreateMap<DynamicParameter, DynamicParameterDto>().ReverseMap();
            configuration.CreateMap<DynamicParameterValue, DynamicParameterValueDto>().ReverseMap();
            configuration.CreateMap<EntityDynamicParameter, EntityDynamicParameterDto>()
                .ForMember(dto => dto.DynamicParameterName,
                    options => options.MapFrom(entity => entity.DynamicParameter.ParameterName));
            configuration.CreateMap<EntityDynamicParameterDto, EntityDynamicParameter>();

            configuration.CreateMap<EntityDynamicParameterValue, EntityDynamicParameterValueDto>().ReverseMap();
            //User Delegations
            configuration.CreateMap<CreateUserDelegationDto, UserDelegation>();

            /* ADD YOUR OWN CUSTOM AUTOMAPPER MAPPINGS HERE */
            configuration.CreateMap<GetFormAndProductHandelingDto, GetFormLayoutAndDataDto>();
            configuration.CreateMap<EmployeePerformanceReport, EmployeePerformanceReportDto>();
        }
    }
}