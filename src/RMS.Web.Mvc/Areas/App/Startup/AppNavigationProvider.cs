using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Localization;
using RMS.Authorization;
using RMS.Authorization.Users;
using RMS.SBJ.CodeTypeTables;
using Abp.Domain.Repositories;

namespace RMS.Web.Areas.App.Startup
{
    public class AppNavigationProvider : NavigationProvider
    {
        public const string MenuName = "App";

        private readonly ICampaignTypesAppService _campaignTypesAppService;
        private readonly IRepository<CampaignType, long> _campaignTypeRepository;
        private readonly UserManager _userManager;

        public AppNavigationProvider(ICampaignTypesAppService campaignTypesAppService, IRepository<CampaignType, long> campaignTypeRepository, UserManager userManager)
        {
            this._campaignTypesAppService = campaignTypesAppService;
            this._userManager = userManager;
            this._campaignTypeRepository = campaignTypeRepository;
        }

        public override void SetNavigation(INavigationProviderContext context)
        {
            var menu = context.Manager.Menus[MenuName] = new MenuDefinition(MenuName, new FixedLocalizableString("Main Menu"));

            menu
                .AddItem(new MenuItemDefinition(
                    AppPageNames.Host.Dashboard,
                    L("Dashboard"),
                    url: "App/HostDashboard",
                    icon: "flaticon-line-graph",
                    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Host_Dashboard)
                ))
                //.AddItem(new MenuItemDefinition(
                //        AppPageNames.Common.CampaignCountries,
                //        L("CampaignCountries"),
                //        url: "App/CampaignCountries",
                //        icon: "flaticon-more",
                //        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_CampaignCountries)
                //    )
                //)
                //.AddItem(new MenuItemDefinition(
                //        AppPageNames.Common.CampaignTranslations,
                //        L("CampaignTranslations"),
                //        url: "App/CampaignTranslations",
                //        icon: "flaticon-more",
                //        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_CampaignTranslations)
                //    )
                //)
                //.AddItem(new MenuItemDefinition(
                //        AppPageNames.Common.RegistrationHistories,
                //        L("RegistrationHistories"),
                //        url: "App/RegistrationHistories",
                //        icon: "flaticon-more",
                //        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_RegistrationHistories_ViewMenu)
                //    )
                //)
                //.AddItem(new MenuItemDefinition(
                //        AppPageNames.Common.MakitaSerialNumbers,
                //        L("MakitaSerialNumbers"),
                //        url: "App/MakitaSerialNumbers",
                //        icon: "flaticon-more",
                //        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_MakitaSerialNumbers_ViewMenu)
                //    )
                //)
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.MakitaCampaigns,
                        L("MakitaCampaigns"),
                        url: "App/MakitaCampaigns",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_MakitaCampaigns)
                    )
                )
                //.AddItem(new MenuItemDefinition(
                //        AppPageNames.Common.RejectionReasons,
                //        L("RejectionReasons"),
                //        url: "App/RejectionReasons",
                //        icon: "flaticon-more",
                //        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_RejectionReasons_ViewMenu)
                //    )
                //)
                //.AddItem(new MenuItemDefinition(
                //        AppPageNames.Common.RegistrationJsonDatas,
                //        L("RegistrationJsonData"),
                //        url: "App/RegistrationJsonData",
                //        icon: "flaticon-more",
                //        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_RegistrationJsonDatas_ViewMenu)
                //    )
                //)
                //.AddItem(new MenuItemDefinition(
                //        AppPageNames.Common.PurchaseRegistrationFormFields,
                //        L("PurchaseRegistrationFormFields"),
                //        url: "App/PurchaseRegistrationFormFields",
                //        icon: "flaticon-more",
                //        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_PurchaseRegistrationFormFields_ViewMenu)
                //    )
                //)
                //.AddItem(new MenuItemDefinition(
                //        AppPageNames.Common.PurchaseRegistrations,
                //        L("PurchaseRegistrations"),
                //        url: "App/PurchaseRegistrations",
                //        icon: "flaticon-more",
                //        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_PurchaseRegistrations_ViewMenu)
                //    )
                //)
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.Registrations,
                        L("Registrations"),
                        url: "App/Registrations",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Registrations_ViewMenu)
                    )
                )
                //.AddItem(new MenuItemDefinition(
                //        AppPageNames.Common.RetailerLocations,
                //        L("RetailerLocations"),
                //        url: "App/RetailerLocations",
                //        icon: "flaticon-more",
                //        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_RetailerLocations_ViewMenu)
                //    )
                //)
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.HandlingBatches,
                        L("HandlingBatches"),
                        url: "App/HandlingBatches",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_HandlingBatches)
                    )
                )
                .AddItem(new MenuItemDefinition(
                    AppPageNames.Host.Tenants,
                    L("Tenants"),
                    url: "App/Tenants",
                    icon: "flaticon-list-3",
                    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Tenants)
                ))
                //.AddItem(new MenuItemDefinition(
                //    AppPageNames.Host.Editions,
                //    L("Editions"),
                //    url: "App/Editions",
                //    icon: "flaticon-app",
                //    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Editions)
                //))
                //.AddItem(new MenuItemDefinition(
                //    AppPageNames.Tenant.Dashboard,
                //    L("Dashboard"),
                //    url: "App/Dashboard",
                //    icon: "flaticon-line-graph",
                //    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Tenant_Dashboard)
                //))
                .AddItem(new MenuItemDefinition(
                    AppPageNames.Tenant.Dashboard,
                    L("Dashboard"),
                    url: "App/Dashboard",
                    icon: "flaticon-line-graph",
                    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Tenant_Dashboard)
                ))
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.Companies,
                        L("Company"),
                        icon: "flaticon-interface-8"
                    ).AddItem(new MenuItemDefinition(
                        AppPageNames.Common.Companies,
                        L("Companies"),
                        url: "App/Companies",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Companies_ViewMenu)
                    )).AddItem(new MenuItemDefinition(
                        AppPageNames.Common.ProjectManagers,
                        L("ProjectManagers"),
                        url: "App/ProjectManagers",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_ProjectManagers_ViewMenu)
                    )).AddItem(new MenuItemDefinition(
                        AppPageNames.Common.Addresses,
                        L("Addresses"),
                        url: "App/Addresses",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Addresses_ViewMenu)
                    ))
                //.AddItem(new MenuItemDefinition(
                //    AppPageNames.Common.CompanyForms,
                //    L("Forms"),
                //    url: "App/Forms?category=CompanyForms",
                //    icon: "flaticon-more",
                //    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Forms_ViewMenu)
                //))
                //.AddItem(new MenuItemDefinition(
                //    AppPageNames.Common.MessageComponentContents,
                //    L("MessageComponentContents"),
                //    url: "App/MessageComponentContents",
                //    icon: "flaticon-more",
                //    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_MessageComponentContents_ViewMenu)
                //))
                )
                .AddItem(new MenuItemDefinition(
                        AppPageNames.SBJ.Campaigns,
                        L("Campaigns"),
                        icon: "flaticon-interface-8",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_CampaignTypeEvents_ViewMenu)
                    ).AddItem(new MenuItemDefinition(
                        AppPageNames.Common.Campaigns,
                        L("Campaigns"),
                        url: "App/Campaigns",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Campaigns_ViewMenu)
                    ))
                //.AddItem(new MenuItemDefinition(
                //    AppPageNames.Common.CampaignTypeEvents,
                //    L("CampaignTypeEvents"),
                //    url: "App/CampaignTypeEvents",
                //    icon: "flaticon-confetti",
                //    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_CampaignTypeEvents_ViewMenu)
                //))
                )
                //.AddItem(new MenuItemDefinition(
                //        AppPageNames.Common.Retailers,
                //        L("Retailers"),
                //        url: "App/Retailers",
                //        icon: "flaticon-more",
                //        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Retailers_ViewMenu)
                //    ))
                //.AddItem(new MenuItemDefinition(
                //        AppPageNames.Common.Products,
                //        L("Product"),
                //        icon: "flaticon-interface-8",
                //        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Products_ViewMenu)
                //        ).AddItem(new MenuItemDefinition(
                //            AppPageNames.Common.Products,
                //            L("Products"),
                //            url: "App/Products",
                //            icon: "flaticon-more",
                //            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Products_ViewMenu)
                //    )).AddItem(new MenuItemDefinition(
                //        AppPageNames.Common.ProductCategories,
                //        L("ProductCategories"),
                //        url: "App/ProductCategories",
                //        icon: "flaticon-more",
                //        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_ProductCategories_ViewMenu)
                //    ))
                //)
                .AddItem(new MenuItemDefinition(
                        AppPageNames.SBJ.Configuration,
                        L("Configuration"),
                        icon: "flaticon-interface-8",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration)
                    )
                    //.AddItem(new MenuItemDefinition(
                    //    AppPageNames.Common.SystemLevels,
                    //    L("SystemLevels"),
                    //    url: "App/SystemLevels",
                    //    icon: "flaticon-list",
                    //    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_SystemLevels_ViewMenu)
                    //))
                    .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.Countries,
                        L("Countries"),
                        url: "App/Countries",
                        icon: "flaticon-earth-globe",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Countries_ViewMenu)
                    )).AddItem(new MenuItemDefinition(
                        AppPageNames.Common.Locales,
                        L("Locales"),
                        url: "App/Locales",
                        icon: "flaticon2-map",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Locales_ViewMenu)
                    )).AddItem(new MenuItemDefinition(
                        AppPageNames.Common.CampaignCategories,
                        L("CampaignCategories"),
                        url: "App/CampaignCategories",
                        icon: "flaticon-list",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_CampaignCategories_ViewMenu)
                    )).AddItem(new MenuItemDefinition(
                        AppPageNames.Common.CampaignCategoryTranslations,
                        L("CampaignCategoryTranslations"),
                        url: "App/CampaignCategoryTranslations",
                        icon: "flaticon-globe",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_CampaignCategoryTranslations_ViewMenu)
                    ))
                //.AddItem(new MenuItemDefinition(
                //    AppPageNames.Common.CampaignTypes,
                //    L("CampaignTypes"),
                //    url: "App/CampaignTypes",
                //    icon: "flaticon-gift",
                //    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_CampaignTypes_ViewMenu)
                //)).AddItem(new MenuItemDefinition(
                //    AppPageNames.Common.ProcessEvents,
                //    L("ProcessEvents"),
                //    url: "App/ProcessEvents",
                //    icon: "flaticon-rotate",
                //    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_ProcessEvents_ViewMenu)
                //)).AddItem(new MenuItemDefinition(
                //    AppPageNames.Common.RegistrationStatuses,
                //    L("RegistrationStatuses"),
                //    url: "App/RegistrationStatuses",
                //    icon: "flaticon-computer",
                //    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_RegistrationStatuses_ViewMenu)
                //)).AddItem(new MenuItemDefinition(
                //    AppPageNames.Common.Messages,
                //    L("Messages"),
                //    url: "App/Messages",
                //    icon: "flaticon-envelope",
                //    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Messages_ViewMenu)
                //))
                //.AddItem(new MenuItemDefinition(
                //    AppPageNames.Common.MessageTypes,
                //    L("MessageTypes"),
                //    url: "App/MessageTypes",
                //    icon: "flaticon-envelope",
                //    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_MessageTypes_ViewMenu)
                //))
                //.AddItem(new MenuItemDefinition(
                //    AppPageNames.Common.MessageComponents,
                //    L("MessageComponents"),
                //    url: "App/MessageComponents",
                //    icon: "flaticon-more",
                //    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_MessageComponents_ViewMenu)
                //))
                //.AddItem(new MenuItemDefinition(
                //    AppPageNames.Common.MessageComponentTypes,
                //    L("MessageComponentTypes"),
                //    url: "App/MessageComponentTypes",
                //    icon: "flaticon-envelope",
                //    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_MessageComponentTypes_ViewMenu)
                //))
                //.AddItem(new MenuItemDefinition(
                //    AppPageNames.Common.MessageVariables,
                //    L("MessageVariables"),
                //    url: "App/MessageVariables",
                //    icon: "flaticon-envelope",
                //    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_MessageVariables_ViewMenu)
                //))
                //.AddItem(new MenuItemDefinition(
                //        AppPageNames.Common.Forms,
                //        L("Forms"),
                //        url: "App/Forms",
                //        icon: "flaticon-more",
                //        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Forms_ViewMenu)
                //))
                //.AddItem(new MenuItemDefinition(
                //    AppPageNames.Common.FormFields,
                //    L("FormFields"),
                //    url: "App/FormFields",
                //    icon: "flaticon-file-1",
                //    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_FormFields_ViewMenu)
                //))
                //.AddItem(new MenuItemDefinition(
                //    AppPageNames.Common.FieldTypes,
                //    L("FieldTypes"),
                //    url: "App/FieldTypes",
                //    icon: "flaticon2-file",
                //    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_FieldTypes_ViewMenu)
                //))
                )
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.Administration,
                        L("Administration"),
                        icon: "flaticon-interface-8",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_ViewMenu)
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.OrganizationUnits,
                            L("OrganizationUnits"),
                            url: "App/OrganizationUnits",
                            icon: "flaticon-map",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_OrganizationUnits)
                    )).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Roles,
                            L("Roles"),
                            url: "App/Roles",
                            icon: "flaticon-suitcase",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Roles)
                    )).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Users,
                            L("Users"),
                            url: "App/Users",
                            icon: "flaticon-users",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Users_ViewMenu)
                    )).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Languages,
                            L("Languages"),
                            url: "App/Languages",
                            icon: "flaticon-tabs",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Languages)
                    )).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.AuditLogs,
                            L("AuditLogs"),
                            url: "App/AuditLogs",
                            icon: "flaticon-folder-1",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_AuditLogs)
                    )).AddItem(new MenuItemDefinition(
                            AppPageNames.Host.Maintenance,
                            L("Maintenance"),
                            url: "App/Maintenance",
                            icon: "flaticon-lock",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Host_Maintenance)
                    )).AddItem(new MenuItemDefinition(
                            AppPageNames.Tenant.SubscriptionManagement,
                            L("Subscription"),
                            url: "App/SubscriptionManagement",
                            icon: "flaticon-refresh",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Tenant_SubscriptionManagement)
                    )).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.UiCustomization,
                            L("VisualSettings"),
                            url: "App/UiCustomization",
                            icon: "flaticon-medical",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_UiCustomization)
                    )).AddItem(new MenuItemDefinition(
                            AppPageNames.Host.Settings,
                            L("Settings"),
                            url: "App/HostSettings",
                            icon: "flaticon-settings",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Host_Settings)
                    )).AddItem(new MenuItemDefinition(
                            AppPageNames.Tenant.Settings,
                            L("Settings"),
                            url: "App/Settings",
                            icon: "flaticon-settings",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Tenant_Settings)
                    )))
                //Reports
                .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Reports,
                            L("Reports"),
                            icon: "flaticon-interface-8",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Reports)
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.EmployeePerformanceReport,
                            L("EmployeePerformanceReport"),
                            url: "App/EmployeePerformanceReport",
                            icon: "flaticon-file-1",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_EmployeePerformanceReport)
                    )).AddItem(new MenuItemDefinition(
                        AppPageNames.Common.GeneralReports,
                        L("GeneralReport"),
                        url: "App/GeneralReport",
                        icon: "flaticon-file-1",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_GeneralReport)
                    )).AddItem(new MenuItemDefinition(
                        AppPageNames.Common.CostReport,
                        L("CostReport"),
                        url: "App/CostReport",
                        icon: "flaticon-file-1",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_CostReport)
                        ))
                 )
                ////PromoPlanner
                //.AddItem(new MenuItemDefinition(
                //            AppPageNames.SBJ.PromoPlanner,
                //            L("PromoPlanner"),
                //            icon: "flaticon-interface-8",
                //            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_PromoPlanner_ViewMenu)
                //        ).AddItem(new MenuItemDefinition(
                //            AppPageNames.Common.PromoDashboard,
                //            L("Dashboard"),
                //            url: "App/PromoDashboard",
                //            icon: "flaticon-analytics",
                //            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_PromoDashboard_ViewMenu)
                //        )).AddItem(new MenuItemDefinition(
                //            AppPageNames.Common.PromoCalendar,
                //            L("Calendar"),
                //            url: "App/PromoCalendar",
                //            icon: "flaticon-event-calendar-symbol",
                //            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_PromoCalendar_ViewMenu)
                //        )).AddItem(new MenuItemDefinition(
                //            AppPageNames.Common.Promos,
                //            L("Promos"),
                //            url: "App/Promos",
                //            icon: "flaticon-edit-1",
                //            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Promos_ViewMenu)
                //        ))

                ////PromoConfig
                //.AddItem(new MenuItemDefinition(
                //            AppPageNames.SBJ.PromoConfig,
                //            L("PromoConfig"),
                //            icon: "flaticon-interface-8",
                //            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_PromoConfig_ViewMenu)
                //        ).AddItem(new MenuItemDefinition(
                //            AppPageNames.Common.ProductCategoryYearPOs,
                //            L("ProductCategoryYearPos"),
                //            url: "App/ProductCategoryYearPos",
                //            icon: "flaticon-coins",
                //            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_ProductCategoryYearPOs_ViewMenu)
                //        )).AddItem(new MenuItemDefinition(
                //            AppPageNames.Common.PromoScopes,
                //            L("PromoScopes"),
                //            url: "App/PromoScopes",
                //            icon: "flaticon-search",
                //            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_PromoScopes_ViewMenu)
                //        ))

                //  ) // PromoConfig
                //.AddItem(new MenuItemDefinition(
                //    AppPageNames.Host.Dashboard,
                //    L("Dashboard"),
                //    url: "App/HostDashboard",
                //    icon: "flaticon-line-graph",
                //    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Host_Dashboard)
                //)))
                // Webhooks
                .AddItem(new MenuItemDefinition(
                    AppPageNames.Common.WebhookSubscriptions,
                    L("WebhookSubscriptions"),
                    url: "App/WebhookSubscription",
                    icon: "flaticon-interface-8",
                    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_WebhookSubscription)
                ));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, RMSConsts.LocalizationSourceName);
        }
    }
}