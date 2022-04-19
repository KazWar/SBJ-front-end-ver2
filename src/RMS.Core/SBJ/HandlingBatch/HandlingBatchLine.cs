using RMS.SBJ.PurchaseRegistrations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using RMS.SBJ.ActivationCodes;

namespace RMS.SBJ.HandlingBatch
{
    [Table("HandlingBatchLine")]
    public class HandlingBatchLine : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string ExternalOrderId { get; set; }

        public virtual string CustomerCode { get; set; }

        public virtual int? Quantity { get; set; }

        public virtual decimal? Amount { get; set; }

        public virtual long? ActivationCodeId { get; set; }

        [ForeignKey("ActivationCodeId")]
        public ActivationCode ActivationCodeFk { get; set; }

        public virtual long HandlingBatchId { get; set; }

        [ForeignKey("HandlingBatchId")]
        public HandlingBatch HandlingBatchFk { get; set; }

        public virtual long PurchaseRegistrationId { get; set; }

        [ForeignKey("PurchaseRegistrationId")]
        public PurchaseRegistration PurchaseRegistrationFk { get; set; }

        public virtual long HandlingBatchLineStatusId { get; set; }

        [ForeignKey("HandlingBatchLineStatusId")]
        public HandlingBatchLineStatus HandlingBatchLineStatusFk { get; set; }

    }
}