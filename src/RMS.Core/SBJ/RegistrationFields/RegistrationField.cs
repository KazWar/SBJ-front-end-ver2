using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using RMS.SBJ.Forms;

namespace RMS.SBJ.RegistrationFields
{
    [Table("RegistrationField")]
    public class RegistrationField : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string Description { get; set; }

        public virtual long FormFieldId { get; set; }

        [ForeignKey("FormFieldId")]
        public FormField FormFieldFk { get; set; }
    }
}