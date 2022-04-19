using System.Collections.Generic;


namespace RMS.Web.DashboardCustomization
{
    public class DashboardViewConfiguration
    {
        public Dictionary<string, WidgetViewDefinition> WidgetViewDefinitions { get; } = new Dictionary<string, WidgetViewDefinition>();

        public Dictionary<string, WidgetFilterViewDefinition> WidgetFilterViewDefinitions { get; } = new Dictionary<string, WidgetFilterViewDefinition>();

        public DashboardViewConfiguration()
        {
            var jsAndCssFileRoot = "/Areas/App/Views/CustomizableDashboard/Widgets/";
            var viewFileRoot = "~/Areas/App/Views/Shared/Components/CustomizableDashboard/Widgets/";

            #region FilterViewDefinitions

            WidgetFilterViewDefinitions.Add(RMSDashboardCustomizationConsts.Filters.FilterDateRangePicker,
                new WidgetFilterViewDefinition(
                    RMSDashboardCustomizationConsts.Filters.FilterDateRangePicker,
                    viewFileRoot + "DateRangeFilter.cshtml",
                    jsAndCssFileRoot + "DateRangeFilter/DateRangeFilter.min.js",
                    jsAndCssFileRoot + "DateRangeFilter/DateRangeFilter.min.css")
            );

            WidgetFilterViewDefinitions.Add(RMSDashboardCustomizationConsts.Filters.HelloWorldFilter,
                new WidgetFilterViewDefinition(
                    RMSDashboardCustomizationConsts.Filters.HelloWorldFilter,
                    viewFileRoot + "FilterHelloWorld.cshtml",
                    jsAndCssFileRoot + "FilterHelloWorld/FilterHelloWorld.min.js",
                    jsAndCssFileRoot + "FilterHelloWorld/FilterHelloWorld.min.css")
              );

            //add your filters iew definitions here
            #endregion

            #region WidgetViewDefinitions

            #region TenantWidgets

            WidgetViewDefinitions.Add(RMSDashboardCustomizationConsts.Widgets.Tenant.DailySales,
                new WidgetViewDefinition(
                    RMSDashboardCustomizationConsts.Widgets.Tenant.DailySales,
                    viewFileRoot + "DailySales.cshtml",
                    jsAndCssFileRoot + "DailySales/DailySales.min.js",
                    jsAndCssFileRoot + "DailySales/DailySales.min.css"));

            WidgetViewDefinitions.Add(RMSDashboardCustomizationConsts.Widgets.Tenant.GeneralStats,
                new WidgetViewDefinition(
                    RMSDashboardCustomizationConsts.Widgets.Tenant.GeneralStats,
                    viewFileRoot + "GeneralStats.cshtml",
                    jsAndCssFileRoot + "GeneralStats/GeneralStats.min.js",
                    jsAndCssFileRoot + "GeneralStats/GeneralStats.min.css"));

            WidgetViewDefinitions.Add(RMSDashboardCustomizationConsts.Widgets.Tenant.ProfitShare,
                new WidgetViewDefinition(
                    RMSDashboardCustomizationConsts.Widgets.Tenant.ProfitShare,
                    viewFileRoot + "ProfitShare.cshtml",
                    jsAndCssFileRoot + "ProfitShare/ProfitShare.min.js",
                    jsAndCssFileRoot + "ProfitShare/ProfitShare.min.css"));
  
            WidgetViewDefinitions.Add(RMSDashboardCustomizationConsts.Widgets.Tenant.MemberActivity,
                new WidgetViewDefinition(
                    RMSDashboardCustomizationConsts.Widgets.Tenant.MemberActivity,
                    viewFileRoot + "MemberActivity.cshtml",
                    jsAndCssFileRoot + "MemberActivity/MemberActivity.min.js",
                    jsAndCssFileRoot + "MemberActivity/MemberActivity.min.css"));

            WidgetViewDefinitions.Add(RMSDashboardCustomizationConsts.Widgets.Tenant.RegionalStats,
                new WidgetViewDefinition(
                    RMSDashboardCustomizationConsts.Widgets.Tenant.RegionalStats,
                    viewFileRoot + "RegionalStats.cshtml",
                    jsAndCssFileRoot + "RegionalStats/RegionalStats.min.js",
                    jsAndCssFileRoot + "RegionalStats/RegionalStats.min.css",
                    12,
                    10));

            WidgetViewDefinitions.Add(RMSDashboardCustomizationConsts.Widgets.Tenant.SalesSummary,
                new WidgetViewDefinition(
                    RMSDashboardCustomizationConsts.Widgets.Tenant.SalesSummary,
                    viewFileRoot + "SalesSummary.cshtml",
                    jsAndCssFileRoot + "SalesSummary/SalesSummary.min.js",
                    jsAndCssFileRoot + "SalesSummary/SalesSummary.min.css",
                    6,
                    10));

            WidgetViewDefinitions.Add(RMSDashboardCustomizationConsts.Widgets.Tenant.TopStats,
                new WidgetViewDefinition(
                    RMSDashboardCustomizationConsts.Widgets.Tenant.TopStats,
                    viewFileRoot + "TopStats.cshtml",
                    jsAndCssFileRoot + "TopStats/TopStats.min.js",
                    jsAndCssFileRoot + "TopStats/TopStats.min.css",
                    12,
                    10));



            //add your tenant side widget definitions here
            WidgetViewDefinitions.Add(RMSDashboardCustomizationConsts.Widgets.Tenant.HelloWorld,
            new WidgetViewDefinition(
                RMSDashboardCustomizationConsts.Widgets.Tenant.HelloWorld,
                viewFileRoot + "WidgetHelloWorld.cshtml",
                jsAndCssFileRoot + "HelloWorld/HelloWorld.min.js",
                jsAndCssFileRoot + "HelloWorld/HelloWorld.min.css",
                defaultWidth: 6,
                defaultHeight: 4));

            WidgetViewDefinitions.Add(RMSDashboardCustomizationConsts.Widgets.Tenant.RegistrationsByStatus,
            new WidgetViewDefinition(
                RMSDashboardCustomizationConsts.Widgets.Tenant.RegistrationsByStatus,
                viewFileRoot + "RegistrationsByStatus.cshtml",
                jsAndCssFileRoot + "RegistrationsByStatus/RegistrationsByStatus.min.js",
                jsAndCssFileRoot + "RegistrationsByStatus/RegistrationsByStatus.min.css",
                defaultWidth: 6,
                defaultHeight: 12));

            WidgetViewDefinitions.Add(RMSDashboardCustomizationConsts.Widgets.Tenant.ProductRegistrations,
            new WidgetViewDefinition(
                RMSDashboardCustomizationConsts.Widgets.Tenant.ProductRegistrations,
                viewFileRoot + "ProductRegistrations.cshtml",
                jsAndCssFileRoot + "ProductRegistrations/ProductRegistrations.min.js",
                jsAndCssFileRoot + "ProductRegistrations/ProductRegistrations.min.css",
                defaultWidth: 4,
                defaultHeight: 12));

            WidgetViewDefinitions.Add(RMSDashboardCustomizationConsts.Widgets.Tenant.RejectedRegistrations,
            new WidgetViewDefinition(
                RMSDashboardCustomizationConsts.Widgets.Tenant.RejectedRegistrations,
                viewFileRoot + "RejectedRegistrations.cshtml",
                jsAndCssFileRoot + "RejectedRegistrations/RejectedRegistrations.min.js",
                jsAndCssFileRoot + "RejectedRegistrations/RejectedRegistrations.min.css",
                defaultWidth: 3,
                defaultHeight: 12));

            WidgetViewDefinitions.Add(RMSDashboardCustomizationConsts.Widgets.Tenant.RegistrationsByCampaignAndCountry,
            new WidgetViewDefinition(
                RMSDashboardCustomizationConsts.Widgets.Tenant.RegistrationsByCampaignAndCountry,
                viewFileRoot + "RegistrationsByCampaignAndCountry.cshtml",
                jsAndCssFileRoot + "RegistrationsByCampaignAndCountry/RegistrationsByCampaignAndCountry.min.js",
                jsAndCssFileRoot + "RegistrationsByCampaignAndCountry/RegistrationsByCampaignAndCountry.min.css",
                defaultWidth: 6,
                defaultHeight: 12));

            WidgetViewDefinitions.Add(RMSDashboardCustomizationConsts.Widgets.Tenant.Campaigns,
            new WidgetViewDefinition(
                RMSDashboardCustomizationConsts.Widgets.Tenant.Campaigns,
                viewFileRoot + "Campaigns.cshtml",
                jsAndCssFileRoot + "Campaigns/Campaigns.min.js",
                jsAndCssFileRoot + "Campaigns/Campaigns.min.css",
                defaultWidth: 7,
                defaultHeight: 9));

            WidgetViewDefinitions.Add(RMSDashboardCustomizationConsts.Widgets.Tenant.RegistrationsByCampaign,
            new WidgetViewDefinition(
                RMSDashboardCustomizationConsts.Widgets.Tenant.RegistrationsByCampaign,
                viewFileRoot + "RegistrationsByCampaign.cshtml",
                jsAndCssFileRoot + "RegistrationsByCampaign/RegistrationsByCampaign.min.js",
                jsAndCssFileRoot + "RegistrationsByCampaign/RegistrationsByCampaign.min.css",
                defaultWidth: 6,
                defaultHeight: 12));

            WidgetViewDefinitions.Add(RMSDashboardCustomizationConsts.Widgets.Tenant.ToSendByCampaign,
            new WidgetViewDefinition(
                RMSDashboardCustomizationConsts.Widgets.Tenant.ToSendByCampaign,
                viewFileRoot + "ToSendByCampaign.cshtml",
                jsAndCssFileRoot + "ToSendByCampaign/ToSendByCampaign.min.js",
                jsAndCssFileRoot + "ToSendByCampaign/ToSendByCampaign.min.css",
                defaultWidth: 6,
                defaultHeight: 12));

            WidgetViewDefinitions.Add(RMSDashboardCustomizationConsts.Widgets.Tenant.ProductRegistrationsByCampaign,
            new WidgetViewDefinition(
                RMSDashboardCustomizationConsts.Widgets.Tenant.ProductRegistrationsByCampaign,
                viewFileRoot + "ProductRegistrationsByCampaign.cshtml",
                jsAndCssFileRoot + "ProductRegistrationsByCampaign/ProductRegistrationsByCampaign.min.js",
                jsAndCssFileRoot + "ProductRegistrationsByCampaign/ProductRegistrationsByCampaign.min.css",
                defaultWidth: 5,
                defaultHeight: 12));


            #endregion

            #region HostWidgets

            WidgetViewDefinitions.Add(RMSDashboardCustomizationConsts.Widgets.Host.IncomeStatistics,
                new WidgetViewDefinition(
                    RMSDashboardCustomizationConsts.Widgets.Host.IncomeStatistics,
                    viewFileRoot + "IncomeStatistics.cshtml",
                    jsAndCssFileRoot + "IncomeStatistics/IncomeStatistics.min.js",
                    jsAndCssFileRoot + "IncomeStatistics/IncomeStatistics.min.css"));

            WidgetViewDefinitions.Add(RMSDashboardCustomizationConsts.Widgets.Host.TopStats,
                new WidgetViewDefinition(
                    RMSDashboardCustomizationConsts.Widgets.Host.TopStats,
                    viewFileRoot + "HostTopStats.cshtml",
                    jsAndCssFileRoot + "HostTopStats/HostTopStats.min.js",
                    jsAndCssFileRoot + "HostTopStats/HostTopStats.min.css"));

            WidgetViewDefinitions.Add(RMSDashboardCustomizationConsts.Widgets.Host.EditionStatistics,
                new WidgetViewDefinition(
                    RMSDashboardCustomizationConsts.Widgets.Host.EditionStatistics,
                    viewFileRoot + "EditionStatistics.cshtml",
                    jsAndCssFileRoot + "EditionStatistics/EditionStatistics.min.js",
                    jsAndCssFileRoot + "EditionStatistics/EditionStatistics.min.css"));

            WidgetViewDefinitions.Add(RMSDashboardCustomizationConsts.Widgets.Host.SubscriptionExpiringTenants,
                new WidgetViewDefinition(
                    RMSDashboardCustomizationConsts.Widgets.Host.SubscriptionExpiringTenants,
                    viewFileRoot + "SubscriptionExpiringTenants.cshtml",
                    jsAndCssFileRoot + "SubscriptionExpiringTenants/SubscriptionExpiringTenants.min.js",
                    jsAndCssFileRoot + "SubscriptionExpiringTenants/SubscriptionExpiringTenants.min.css",
                    6,
                    10));

            WidgetViewDefinitions.Add(RMSDashboardCustomizationConsts.Widgets.Host.RecentTenants,
                new WidgetViewDefinition(
                    RMSDashboardCustomizationConsts.Widgets.Host.RecentTenants,
                    viewFileRoot + "RecentTenants.cshtml",
                    jsAndCssFileRoot + "RecentTenants/RecentTenants.min.js",
                    jsAndCssFileRoot + "RecentTenants/RecentTenants.min.css"));

            //add your host side widgets definitions here
            #endregion

            #endregion
        }
    }
}
