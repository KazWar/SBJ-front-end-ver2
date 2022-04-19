using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using RMS.SBJ.CodeTypeTables;
using RMS.SBJ.Registrations;

namespace RMS.SBJ.RegistrationHistory
{
    [Table("RegistrationHistory")]
    public class RegistrationHistory : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual DateTime DateCreated { get; set; }

        public virtual string Remarks { get; set; }

        public virtual long AbpUserId { get; set; }

        public virtual long RegistrationId { get; set; }

        public virtual long RegistrationStatusId { get; set; }

        [ForeignKey("RegistrationId")]
        public Registration RegistrationFk { get; set; }

        [ForeignKey("RegistrationStatusId")]
        public RegistrationStatus RegistrationStatusFk { get; set; }
    }
}