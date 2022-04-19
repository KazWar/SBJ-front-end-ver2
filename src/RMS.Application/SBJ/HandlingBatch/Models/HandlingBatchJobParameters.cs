namespace RMS.SBJ.HandlingBatch.Models
{
    public class HandlingBatchJobParameters
    {
        public int? TenantId { get; set; }
        public long AbpUserId { get; set; }

        public long? HandlingBatchId { get; set; }
        public int? WarehouseId { get; set; }
        public string OrderUserId { get; set; }
        public string Password { get; set; }
        public string SepaInitiator { get; set; }
    }
}
