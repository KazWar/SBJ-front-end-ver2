using System.Collections.Generic;

namespace RMS.SBJ.HandlingBatch.Dtos.ActivationCode
{
    public sealed class ActivationCodeStore
    {
        public long CampaignId { get; set; }

        public List<ActivationCodeLocalStore> ActivationCodeLocalStore { get; set; }
    }
}
