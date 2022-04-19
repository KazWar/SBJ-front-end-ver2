namespace RMS.SBJ.HandlingBatch.Dtos.Premium
{
    public sealed class GetPremiumBatchForData
    {
        public long Id { get; set; }

        public int WarehouseId { get; set; }

        public string OrderUserId { get; set; }

        public string Password { get; set; }
    }
}
