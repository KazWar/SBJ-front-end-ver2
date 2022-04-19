using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using RMS.SBJ.PurchaseRegistrations;
using RMS.SBJ.PurchaseRegistrationFields;

namespace RMS.SBJ.PurchaseRegistrationFieldDatas
{
    [Table("PurchaseRegistrationFieldData")]
    public class PurchaseRegistrationFieldData : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string Value { get; set; }

        public virtual long PurchaseRegistrationId { get; set; }

        public virtual long PurchaseRegistrationFieldId { get; set; }

        [ForeignKey("PurchaseRegistrationId")]
        public PurchaseRegistration PurchaseRegistrationFk { get; set; }

        [ForeignKey("PurchaseRegistrationFieldId")]
        public PurchaseRegistrationField PurchaseRegistrationFieldFk { get; set; }
    }
}