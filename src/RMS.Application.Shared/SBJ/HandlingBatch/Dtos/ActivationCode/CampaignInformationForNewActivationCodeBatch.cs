using System.Collections.Generic;

namespace RMS.SBJ.HandlingBatch.Dtos.ActivationCode
{
    public sealed class CampaignInformationForNewActivationCodeBatch
    {
        public long CampaignId { get; set; }

        public string CampaignName { get; set; }

        public int ApprovedRegistrationsCount { get; set; }

        public int BatchableRegistrationsCount { get; set; }

        public IEnumerable<ActivationCodeInformationForNewHandlingBatch> ActivationCodeInformation { get; set; }

        public IEnumerable<string> Remarks { get; set; }
    }
}
