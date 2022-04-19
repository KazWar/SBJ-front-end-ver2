using System.Collections.Generic;

namespace RMS.SBJ.HandlingBatch.Dtos.Premium
{
    public sealed class GetInformationForNewPremiumBatchOutput
    {
        public IEnumerable<CampaignInformationForNewPremiumBatch> CampaignInformation { get; set; }

        public IEnumerable<PremiumInformationForNewHandlingBatch> TotalPremiumInformation { get; set; }

        public int TotalApprovedRegistrationsCount { get; set; }

        public int TotalBatchableRegistrationsCount { get; set; }

        public string TotalBatchableRegistrations { get; set; }

        public bool AllIsDeliverable { get; set; }

        public int WarehouseId { get; set; }

        public string OrderUserId { get; set; }

        public string Password { get; set; }
    }
}
