using System.Collections.Generic;

namespace RMS.SBJ.HandlingBatch.Dtos.CashRefund
{
    public sealed class GetInformationForNewCashRefundBatchOutput
    {
        public IEnumerable<CampaignInformationForNewCashRefundBatch> CampaignInformation { get; set; }

        public int TotalApprovedRegistrationsCount { get; set; }

        public int TotalBatchableRegistrationsCount { get; set; }

        public string TotalBatchableRegistrations { get; set; }

        public decimal TotalRefundAmount { get; set; }
    }
}
