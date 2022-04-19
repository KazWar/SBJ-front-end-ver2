using System;

namespace RMS.SBJ.Report.GeneralReports.Dtos
{
    public class GetAllGeneralReportForExcelInput
    {
        public long CampaignFilter { get; set; }

        public DateTime StartDateFilter { get; set; }

        public DateTime EndDateFilter { get; set; }
    }
}
