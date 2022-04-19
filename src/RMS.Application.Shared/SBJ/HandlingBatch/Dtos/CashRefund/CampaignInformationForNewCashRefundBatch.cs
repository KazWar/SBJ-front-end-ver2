namespace RMS.SBJ.HandlingBatch.Dtos.CashRefund
{
    public sealed class CampaignInformationForNewCashRefundBatch
    {
        public long CampaignId { get; set; }

        public string CampaignName { get; set; }

        public int ApprovedRegistrationsCount { get; set; }

        public int BatchableRegistrationsCount { get; set; }

        public decimal RefundAmount { get; set; }
    }
}
