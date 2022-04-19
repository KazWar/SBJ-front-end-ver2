namespace RMS.SBJ.HandlingBatch.Dtos.ActivationCode
{
    public sealed class ActivationCodeBatchRegistrationForView
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Street { get; set; }

        public string Postal { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string Email { get; set; }

        public string CampaignName { get; set; }

        public string StatusDescription { get; set; }

        public string ActivationCode { get; set; }

        public string MessageStatus { get; set; }
    }
}
