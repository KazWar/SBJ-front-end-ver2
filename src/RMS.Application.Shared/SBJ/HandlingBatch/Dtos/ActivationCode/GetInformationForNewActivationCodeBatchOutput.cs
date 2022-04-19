using System.Collections.Generic;

namespace RMS.SBJ.HandlingBatch.Dtos.ActivationCode
{
    public sealed class GetInformationForNewActivationCodeBatchOutput
    {
        public IEnumerable<CampaignInformationForNewActivationCodeBatch> CampaignInformation { get; set; }

        public IEnumerable<ActivationCodeInformationForNewHandlingBatch> TotalActivationCodeInformation { get; set; }

        public int TotalApprovedRegistrationsCount { get; set; }

        public int TotalBatchableRegistrationsCount { get; set; }

        public string TotalBatchableRegistrations { get; set; }

        public bool AllIsDeliverable { get; set; }
    }
}
