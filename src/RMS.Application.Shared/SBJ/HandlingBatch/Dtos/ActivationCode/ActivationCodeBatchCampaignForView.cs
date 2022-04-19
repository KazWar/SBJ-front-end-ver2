using System.Collections.Generic;

namespace RMS.SBJ.HandlingBatch.Dtos.ActivationCode
{
    public sealed class ActivationCodeBatchCampaignForView
    {
        public string CampaignName { get; set; }

        public IEnumerable<ActivationCodeBatchRegistrationForView> Registrations { get; set; }
    }
}
