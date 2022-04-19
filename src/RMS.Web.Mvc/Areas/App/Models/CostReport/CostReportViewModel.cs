using System;

namespace RMS.Web.Areas.App.Models.CostReport
{
    public class CostReportViewModel
    {
        public String CampaignCode { get; set; }
        public String CampaignName { get; set; }
        public DateTime CampaignStart { get; set; }
        public DateTime CampaignEnd { get; set; }
    }
}
