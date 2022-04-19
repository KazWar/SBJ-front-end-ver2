using System;
using System.Collections.Generic;

namespace RMS.SBJ.Report.CostReports.Dtos
{

    public class MonthlyTotal
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int NewRegistrations { get; set; }
        public int CompleteRegistrations { get; set; }
        public int ExtraHandlingRegistrations { get; set; }
        public int PaymentBatches { get; set; }
        public int PaymentsSent{ get; set; }
        public int ActivationCodeBatches { get; set; }
        public int ActivationCodesSent { get; set; }
        public int PremiumBatches { get; set; }
        public int PremiumsSent { get; set; }
    }
    public class CostReportDto
    {
        public string CampaignCode { get; set; }
        public string CampaignName { get; set; }
        public DateTime CampaignStart { get; set; }
        public DateTime CampaignEnd { get; set; }
        public List<MonthlyTotal> MonthlyTotals { get; set; }
    }
}
