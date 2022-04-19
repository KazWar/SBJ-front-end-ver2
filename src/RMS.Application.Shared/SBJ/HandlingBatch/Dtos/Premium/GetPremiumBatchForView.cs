using System;
using System.Collections.Generic;

namespace RMS.SBJ.HandlingBatch.Dtos.Premium
{
    public sealed class GetPremiumBatchForView
    {
        public long Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public int RegistrationsCount { get; set; }

        public int OrdersCount { get; set; }

        public string StatusDescription { get; set; }

        public string Remarks { get; set; }

        public IEnumerable<PremiumBatchCampaignForView> Campaigns { get; set; }

        public int WarehouseId { get; set; }

        public string OrderUserId { get; set; }

        public string Password { get; set; }
    }
}
