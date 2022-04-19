namespace RMS.SBJ.HandlingBatch.Dtos.Premium
{
    public sealed class GetInformationForNewPremiumBatchInput
    {       
        public string CampaignBatchables { get; set; }

        public int WarehouseId { get; set; }

        public string OrderUserId { get; set; }

        public string Password { get; set; }
    }
}