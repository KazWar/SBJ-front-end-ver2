using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using RMS.SBJ.RegistrationFields;
using RMS.SBJ.Registrations;

namespace RMS.SBJ.RegistrationFormFieldDatas
{
    [Table("RegistrationFieldData")]
    public class RegistrationFieldData : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string Value { get; set; }

        public virtual long RegistrationFieldId { get; set; }

        [ForeignKey("RegistrationFieldId")]
        public RegistrationField RegistrationFieldFk { get; set; }

        public virtual long RegistrationId { get; set; }

        [ForeignKey("RegistrationId")]
        public Registration RegistrationFk { get; set; }
    }
}