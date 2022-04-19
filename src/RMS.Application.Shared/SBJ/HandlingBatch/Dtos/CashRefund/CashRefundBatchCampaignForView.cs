using System.Collections.Generic;

namespace RMS.SBJ.HandlingBatch.Dtos.CashRefund
{
    public sealed class CashRefundBatchCampaignForView
    {
        public string CampaignName { get; set; }

        public IEnumerable<CashRefundBatchRegistrationForView> Registrations { get; set; }
    }
}
