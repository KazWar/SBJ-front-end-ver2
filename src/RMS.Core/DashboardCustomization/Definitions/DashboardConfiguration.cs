using System.Collections.Generic;
using System.Linq;
using Abp.MultiTenancy;
using RMS.Authorization;

namespace RMS.DashboardCustomization.Definitions
{
    public class DashboardConfiguration
    {
        public List<DashboardDefinition> DashboardDefinitions { get; } = new List<DashboardDefinition>();

        public List<WidgetDefinition> WidgetDefinitions { get; } = new List<WidgetDefinition>();

        public List<WidgetFilterDefinition> WidgetFilterDefinitions { get; } = new List<WidgetFilterDefinition>();

        public DashboardConfiguration()
        {
            #region FilterDefinitions

            // These are global filter which all widgets can use
            var dateRangeFilter = new WidgetFilterDefinition(
                RMSDashboardCustomizationConsts.Filters.FilterDateRangePicker,
                "FilterDateRangePicker"
            );

            WidgetFilterDefinitions.Add(dateRangeFilter);

            var helloWorldFilter = new WidgetFilterDefinition(
            RMSDashboardCustomizationConsts.Filters.HelloWorldFilter, "FilterHelloWorld");

            WidgetFilterDefinitions.Add(helloWorldFilter);
            // Add your filters here

            #endregion

            #region WidgetDefinitions

            // Define Widgets

            #region TenantWidgets

            var tenantWidgetsDefaultPermission = new List<string>
            {
                AppPermissions.Pages_Tenant_Dashboard
            };

            var dailySales = new WidgetDefinition(
                RMSDashboardCustomizationConsts.Widgets.Tenant.DailySales,
                "WidgetDailySales",
                side: MultiTenancySides.Tenant,
                usedWidgetFilters: new List<string> { dateRangeFilter.Id },
                permissions: tenantWidgetsDefaultPermission
            );

            var generalStats = new WidgetDefinition(
                RMSDashboardCustomizationConsts.Widgets.Tenant.GeneralStats,
                "WidgetGeneralStats",
                side: MultiTenancySides.Tenant,
                permissions: tenantWidgetsDefaultPermission.Concat(new List<string>{ AppPermissions.Pages_Administration_AuditLogs }).ToList());

            var profitShare = new WidgetDefinition(
                RMSDashboardCustomizationConsts.Widgets.Tenant.ProfitShare,
                "WidgetProfitShare",
                side: MultiTenancySides.Tenant,
                permissions: tenantWidgetsDefaultPermission);

            var memberActivity = new WidgetDefinition(
                RMSDashboardCustomizationConsts.Widgets.Tenant.MemberActivity,
                "WidgetMemberActivity",
                side: MultiTenancySides.Tenant,
                permissions: tenantWidgetsDefaultPermission);

            var regionalStats = new WidgetDefinition(
                RMSDashboardCustomizationConsts.Widgets.Tenant.RegionalStats,
                "WidgetRegionalStats",
                side: MultiTenancySides.Tenant,
                permissions: tenantWidgetsDefaultPermission);

            var salesSummary = new WidgetDefinition(
                RMSDashboardCustomizationConsts.Widgets.Tenant.SalesSummary,
                "WidgetSalesSummary",
                usedWidgetFilters: new List<string>() { dateRangeFilter.Id },
                side: MultiTenancySides.Tenant,
                permissions: tenantWidgetsDefaultPermission);

            var topStats = new WidgetDefinition(
                RMSDashboardCustomizationConsts.Widgets.Tenant.TopStats,
                "WidgetTopStats",
                side: MultiTenancySides.Tenant,
                permissions: tenantWidgetsDefaultPermission);

            WidgetDefinitions.Add(generalStats);
            WidgetDefinitions.Add(dailySales);
            WidgetDefinitions.Add(profitShare);
            WidgetDefinitions.Add(memberActivity);
            WidgetDefinitions.Add(regionalStats);
            WidgetDefinitions.Add(topStats);
            WidgetDefinitions.Add(salesSummary);
            // Add your tenant side widgets here

            var helloWorld = new WidgetDefinition(
                id: RMSDashboardCustomizationConsts.Widgets.Tenant.HelloWorld,
                name: "WidgetRecentTenants",//localized string key
                side: MultiTenancySides.Tenant,
                usedWidgetFilters: new List<string>() { helloWorldFilter.Id },// you can use any filter you need
                permissions: tenantWidgetsDefaultPermission);
            WidgetDefinitions.Add(helloWorld);

            var registrationsByStatus = new WidgetDefinition(
                id: RMSDashboardCustomizationConsts.Widgets.Tenant.RegistrationsByStatus,
                name: "WidgetRegistrationsByStatus",//localized string key
                side: MultiTenancySides.Tenant,
                usedWidgetFilters: new List<string>() { },// you can use any filter you need
                permissions: tenantWidgetsDefaultPermission);
            WidgetDefinitions.Add(registrationsByStatus);

            var productRegistration = new WidgetDefinition(
                id: RMSDashboardCustomizationConsts.Widgets.Tenant.ProductRegistrations,
                name: "WidgetProductRegistration",//localized string key
                side: MultiTenancySides.Tenant,
                usedWidgetFilters: new List<string>() { },// you can use any filter you need
                permissions: tenantWidgetsDefaultPermission);
            WidgetDefinitions.Add(productRegistration);

            var rejectedRegistrations = new WidgetDefinition(
                id: RMSDashboardCustomizationConsts.Widgets.Tenant.RejectedRegistrations,
                name: "WidgetRejectedRegistrations",//localized string key
                side: MultiTenancySides.Tenant,
                usedWidgetFilters: new List<string>() { },// you can use any filter you need
                permissions: tenantWidgetsDefaultPermission);
            WidgetDefinitions.Add(rejectedRegistrations);

            var registrationsByCampaignAndCountry = new WidgetDefinition(
                id: RMSDashboardCustomizationConsts.Widgets.Tenant.RegistrationsByCampaignAndCountry,
                name: "WidgetRegistrationsByCampaignAndCountry",//localized string key
                side: MultiTenancySides.Tenant,
                usedWidgetFilters: new List<string>() { },// you can use any filter you need
                permissions: tenantWidgetsDefaultPermission);
            WidgetDefinitions.Add(registrationsByCampaignAndCountry);

            var campaigns = new WidgetDefinition(
                id: RMSDashboardCustomizationConsts.Widgets.Tenant.Campaigns,
                name: "WidgetCampaigns",//localized string key
                side: MultiTenancySides.Tenant,
                usedWidgetFilters: new List<string>() { },// you can use any filter you need
                permissions: tenantWidgetsDefaultPermission);
            WidgetDefinitions.Add(campaigns);

            var registrationsByCampaign = new WidgetDefinition(
                id: RMSDashboardCustomizationConsts.Widgets.Tenant.RegistrationsByCampaign,
                name: "WidgetRegistrationsByCampaign",//localized string key
                side: MultiTenancySides.Tenant,
                usedWidgetFilters: new List<string>() { },// you can use any filter you need
                permissions: tenantWidgetsDefaultPermission);
            WidgetDefinitions.Add(registrationsByCampaign);

            var toSendByCampaign = new WidgetDefinition(
                id: RMSDashboardCustomizationConsts.Widgets.Tenant.ToSendByCampaign,
                name: "WidgetToSendByCampaign",//localized string key
                side: MultiTenancySides.Tenant,
                usedWidgetFilters: new List<string>() { },// you can use any filter you need
                permissions: tenantWidgetsDefaultPermission);
            WidgetDefinitions.Add(toSendByCampaign);

            var producRegistrationsByCampaign = new WidgetDefinition(
                id: RMSDashboardCustomizationConsts.Widgets.Tenant.ProductRegistrationsByCampaign,
                name: "WidgetProductRegistraionsByCampaign",//localized string key
                side: MultiTenancySides.Tenant,
                usedWidgetFilters: new List<string>() { },// you can use any filter you need
                permissions: tenantWidgetsDefaultPermission);
            WidgetDefinitions.Add(producRegistrationsByCampaign);

            #endregion

            #region HostWidgets

            var hostWidgetsDefaultPermission = new List<string>
            {
                AppPermissions.Pages_Administration_Host_Dashboard
            };

            var incomeStatistics = new WidgetDefinition(
                RMSDashboardCustomizationConsts.Widgets.Host.IncomeStatistics,
                "WidgetIncomeStatistics",
                side: MultiTenancySides.Host,
                permissions: hostWidgetsDefaultPermission);

            var hostTopStats = new WidgetDefinition(
                RMSDashboardCustomizationConsts.Widgets.Host.TopStats,
                "WidgetTopStats",
                side: MultiTenancySides.Host,
                permissions: hostWidgetsDefaultPermission);

            var editionStatistics = new WidgetDefinition(
                RMSDashboardCustomizationConsts.Widgets.Host.EditionStatistics,
                "WidgetEditionStatistics",
                side: MultiTenancySides.Host,
                permissions: hostWidgetsDefaultPermission);

            var subscriptionExpiringTenants = new WidgetDefinition(
                RMSDashboardCustomizationConsts.Widgets.Host.SubscriptionExpiringTenants,
                "WidgetSubscriptionExpiringTenants",
                side: MultiTenancySides.Host,
                permissions: hostWidgetsDefaultPermission);

            var recentTenants = new WidgetDefinition(
                RMSDashboardCustomizationConsts.Widgets.Host.RecentTenants,
                "WidgetRecentTenants",
                side: MultiTenancySides.Host,
                usedWidgetFilters: new List<string>() { dateRangeFilter.Id },
                permissions: hostWidgetsDefaultPermission);

            WidgetDefinitions.Add(incomeStatistics);
            WidgetDefinitions.Add(hostTopStats);
            WidgetDefinitions.Add(editionStatistics);
            WidgetDefinitions.Add(subscriptionExpiringTenants);
            WidgetDefinitions.Add(recentTenants);

            // Add your host side widgets here

            #endregion

            #endregion

            #region

            // Create dashboard
            var defaultTenantDashboard = new DashboardDefinition(
                RMSDashboardCustomizationConsts.DashboardNames.DefaultTenantDashboard,
                new List<string>
                {
                    //generalStats.Id, 
                    //dailySales.Id, 
                    //profitShare.Id, 
                    //memberActivity.Id, 
                    //regionalStats.Id, 
                    //topStats.Id, 
                    //salesSummary.Id, 
                    //helloWorld.Id, 
                    registrationsByStatus.Id,
                    productRegistration.Id,
                    rejectedRegistrations.Id,
                    registrationsByCampaignAndCountry.Id,
                    campaigns.Id,
                    registrationsByCampaign.Id,
                    toSendByCampaign.Id,
                    producRegistrationsByCampaign.Id
                });

            DashboardDefinitions.Add(defaultTenantDashboard);

            var defaultHostDashboard = new DashboardDefinition(
                RMSDashboardCustomizationConsts.DashboardNames.DefaultHostDashboard,
                new List<string>
                {
                    incomeStatistics.Id,
                    hostTopStats.Id,
                    editionStatistics.Id,
                    subscriptionExpiringTenants.Id,
                    recentTenants.Id
                });

            DashboardDefinitions.Add(defaultHostDashboard);

            // Add your dashboard definiton here

            #endregion

        }

    }
}
