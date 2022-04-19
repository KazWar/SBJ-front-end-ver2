using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using RMS.SBJ.Forms;
using RMS.SBJ.PurchaseRegistrationFieldDatas;

namespace RMS.SBJ.PurchaseRegistrationFields
{
    [Table("PurchaseRegistrationField")]
    public class PurchaseRegistrationField : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string Description { get; set; }

        public virtual long FormFieldId { get; set; }

        [ForeignKey("FormFieldId")]
        public FormField FormFieldFk { get; set; }

        public virtual ICollection<PurchaseRegistrationFieldData> PurchaseRegistrationFieldData { get; set; }
    }
}