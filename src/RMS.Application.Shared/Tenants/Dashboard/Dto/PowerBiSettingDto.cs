using System;
using System.Collections.Generic;
using System.Text;

namespace RMS.DashboardCustomization.Dto
{
    public class PowerBISettingsModel
    {
        public PowerBiSettings PromoPlannerDashBoard { get; set; }
        public PowerBiSettings RMSDashBoard { get; set; }
        public PowerBiSettings MakitaDashBoard { get; set; }

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
