using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using RMS.SBJ.PurchaseRegistrations;

namespace RMS.SBJ.HandlingBatches
{
    [Table("HandlingBatchLine")]
    public class HandlingBatchLine : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string CustomerCode { get; set; }

        public virtual string ExternalOrderId { get; set; }

        public virtual decimal? Amount { get; set; }

        public virtual bool Fixed { get; set; }

        public virtual long HandlingBatchId { get; set; }

        public virtual long PurchaseRegistrationId { get; set; }

        public virtual long HandlingBatchLineStatusId { get; set; }

        [ForeignKey("HandlingBatchId")]
        public HandlingBatch HandlingBatchFk { get; set; }

        [ForeignKey("PurchaseRegistrationId")]
        public PurchaseRegistration PurchaseRegistrationFk { get; set; }

        [ForeignKey("HandlingBatchLineStatusId")]
        public HandlingBatchLineStatus HandlingBatchLineStatusFk { get; set; }
    }
}
