using System.Collections.Generic;

namespace RMS.SBJ.HandlingBatch.Dtos.Premium
{
    public sealed class CampaignInformationForNewPremiumBatch
    {
        public long CampaignId { get; set; }

        public string CampaignName { get; set; }

        public int ApprovedRegistrationsCount { get; set; }

        public int BatchableRegistrationsCount { get; set; }

        public IEnumerable<PremiumInformationForNewHandlingBatch> PremiumInformation { get; set; }
    }
}
