using System.Collections.Generic;

namespace RMS.SBJ.HandlingBatch.Dtos.Premium
{
    public sealed class PremiumBatchRegistrationForView
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

        public string PremiumDescription { get; set; }

        public string OrderId { get; set; }

        public string OrderStatus { get; set; }

        public IEnumerable<PremiumBatchPremiumForView> Premiums { get; set; }
    }
}
