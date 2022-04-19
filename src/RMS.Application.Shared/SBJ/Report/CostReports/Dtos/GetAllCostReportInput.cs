using System;

namespace RMS.SBJ.Report.CostReports.Dtos
{
    public class GetAllCostReportInput
    {
        public long CampaignId { get; set; }
        public DateTime StartDateFilter { get; set; }

        public DateTime EndDateFilter { get; set; }

    }
}
