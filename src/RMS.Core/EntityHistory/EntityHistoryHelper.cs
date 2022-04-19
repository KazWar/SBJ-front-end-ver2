using RMS.SBJ.RegistrationJsonData;
using RMS.SBJ.PurchaseRegistrationFormFields;
using RMS.SBJ.HandlingLineProducts;
using RMS.SBJ.Forms;
using RMS.SBJ.SystemTables;
using RMS.SBJ.Retailers;
using RMS.SBJ.Products;
using RMS.SBJ.Company;
using RMS.SBJ.CodeTypeTables;
using RMS.SBJ.CampaignProcesses;
using RMS.PromoPlanner;
using System;
using System.Linq;
using Abp.Organizations;
using RMS.Authorization.Roles;
using RMS.MultiTenancy;

namespace RMS.EntityHistory
{
    public static class EntityHistoryHelper
    {
        public const string EntityHistoryConfigurationName = "EntityHistory";

        public static readonly Type[] HostSideTrackedTypes =
        {
            typeof(Address),
            typeof(Campaign),
            typeof(CampaignCategory),
            typeof(CampaignCategoryTranslation),
            typeof(MessageHistory),
            typeof(RegistrationJsonData),
            typeof(PurchaseRegistrationFormField),
            typeof(CampaignMessage),
            typeof(HandlingLineProduct),
            typeof(CampaignForm),
            typeof(CampaignCampaignType),
            typeof(CampaignMessage),
            typeof(CampaignStep),
            typeof(CampaignType),
            typeof(CampaignTypeEvent),
            typeof(CampaignTypeEventRegistrationStatus),
            typeof(Company),
            typeof(Country),
            typeof(FieldType),
            typeof(Form),
            typeof(FormBlock),
            typeof(FormBlockField),
            typeof(FormField),
            typeof(FormFieldTranslation),
            typeof(FormFieldValueList),
            typeof(FormLocale),
            typeof(HandlingLineProduct),
            typeof(Locale),
            typeof(ListValue),
            typeof(ListValueTranslation),
            typeof(Message),
            typeof(MessageType),
            typeof(MessageComponent),
            typeof(MessageComponentContent),
            typeof(MessageContentTranslation),
            typeof(MessageComponentType),
            typeof(MessageVariable),
            typeof(OrganizationUnit),
            typeof(ProcessEvent),
            typeof(Product),
            typeof(ProductCategory),
            typeof(ProjectManager),
            typeof(Promo),
            typeof(PromoStep),
            typeof(PromoStepData),
            typeof(PurchaseRegistrationFormField),
            typeof(RegistrationJsonData),
            typeof(RegistrationStatus),
            typeof(Retailer),
            typeof(Role),    
            typeof(SystemLevel),
            typeof(Tenant),
            typeof(ValueList)
        };

        public static readonly Type[] TenantSideTrackedTypes =
        {
            typeof(MessageHistory),
            typeof(RegistrationJsonData),
            typeof(PurchaseRegistrationFormField),
            typeof(CampaignMessage),
            typeof(HandlingLineProduct),
            typeof(CampaignForm),
            typeof(CampaignCampaignType),
            typeof(CampaignMessage),
            typeof(CampaignStep),
            typeof(CampaignType),
            typeof(CampaignTypeEvent),
            typeof(CampaignTypeEventRegistrationStatus),
            typeof(Company),
            typeof(Country),
            typeof(FieldType),
            typeof(Form),
            typeof(FormBlock),
            typeof(FormBlockField),
            typeof(FormField),
            typeof(FormFieldTranslation),
            typeof(FormFieldValueList),
            typeof(FormLocale),
            typeof(HandlingLineProduct),
            typeof(Locale),
            typeof(ListValue),
            typeof(ListValueTranslation),
            typeof(Message),
            typeof(MessageType),
            typeof(MessageComponent),
            typeof(MessageComponentContent),
            typeof(MessageContentTranslation),
            typeof(MessageComponentType),
            typeof(MessageVariable),
            typeof(OrganizationUnit),
            typeof(ProcessEvent),
            typeof(Product),
            typeof(ProductCategory),
            typeof(ProjectManager),
            typeof(Promo),
            typeof(PromoStep),
            typeof(PromoStepData),
            typeof(PurchaseRegistrationFormField),
            typeof(RegistrationJsonData),
            typeof(RegistrationStatus),
            typeof(Retailer),
            typeof(Role),
            typeof(SystemLevel),
            typeof(Tenant),
            typeof(ValueList)
        };

        public static readonly Type[] TrackedTypes =
            HostSideTrackedTypes
                .Concat(TenantSideTrackedTypes)
                .GroupBy(type => type.FullName)
                .Select(types => types.First())
                .ToArray();
    }
}