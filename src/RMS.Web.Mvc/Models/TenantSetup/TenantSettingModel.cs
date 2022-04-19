namespace RMS.Web.Models.TenantSetup
{
    public class TenantSettingModel
    {
        public int TenantId { get; set; }
        public string TenantName { get; set; }
        public string RMSBlobStorage { get; set; }
        public string RMSBlobContainer { get; set; }
        public int WarehouseId { get; set; }
        public string OrderUserId { get; set; }
        public string Password { get; set; }
        public string SepaInitiator { get; set; }
    }
}