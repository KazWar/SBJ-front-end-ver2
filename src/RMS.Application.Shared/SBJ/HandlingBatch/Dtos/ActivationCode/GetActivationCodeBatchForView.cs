using System;
using System.Collections.Generic;

namespace RMS.SBJ.HandlingBatch.Dtos.ActivationCode
{
    public sealed class GetActivationCodeBatchForView
    {
        public long Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public int RegistrationsCount { get; set; }

        public int ActivationCodeCount { get; set; }

        public string StatusCode { get; set; }

        public string StatusDescription { get; set; }

        public string Remarks { get; set; }

        public IEnumerable<ActivationCodeBatchCampaignForView> Campaigns { get; set; }
    }
}
