using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using RMS.SBJ.ActivationCodes;
using RMS.SBJ.Registrations;

namespace RMS.SBJ.ActivationCodeRegistrations
{
    [Table("ActivationCodeRegistration")]
    public class ActivationCodeRegistration : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual long ActivationCodeId { get; set; }

        public virtual long? RegistrationId { get; set; }

        [ForeignKey("ActivationCodeId")]
        public ActivationCode ActivationCodeFk { get; set; }

        [ForeignKey("RegistrationId")]
        public Registration RegistrationFk { get; set; }
    }
}