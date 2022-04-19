using System.Collections.Generic;

namespace RMS.SBJ.HandlingBatch.Dtos.Premium
{
    public sealed class PremiumBatchCampaignForView
    {
        public string CampaignName { get; set; }

        public IEnumerable<PremiumBatchRegistrationForView> Registrations { get; set; }
    }
}
