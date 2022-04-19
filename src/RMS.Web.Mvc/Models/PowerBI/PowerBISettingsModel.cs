using System;

namespace RMS.Web.Models.PowerBI
{
    public class PowerBISettingsModel
    {
        public PowerBiSettings PromoPlannerDashBoard { get; set; }
        public PowerBiSettings RMSDashBoard { get; set; }
        public PowerBiSettings MakitaDashBoard { get; set; }
        public PowerBiSettings WeberDashBoard { get; set; }
        public PowerBiSettings SageDashBoard { get; set; }
        public PowerBiSettings MakitaTestDashBoard { get; set; }
        public PowerBiSettings SanimedDashBoard { get; set; }
        public PowerBiSettings WhirlpoolDashBoard { get; set; }
        public PowerBiSettings BatDashBoard { get; set; }
        public PowerBiSettings CarocrocDashBoard { get; set; }
        public PowerBiSettings MakitaBEDashboard { get; set; }
        public PowerBiSettings PhilipsPHDashboard { get; set; }
        public PowerBiSettings PhilipsDADashboard { get; set; }
    }

    public class PowerBiSettings
    {
        public Guid ApplicationId { get; set; }
        public string ApplicationSecret { get; set; }
        public string TenantId { get; set; }
        public Guid DashboardId { get; set; }
        public Guid ReportId { get; set; }
        public Guid? WorkspaceId { get; set; }
        public string AuthorityUrl { get; set; }
        public string ResourceUrl { get; set; }
        public string ApiUrl { get; set; }
        public string EmbedUrlBase { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
