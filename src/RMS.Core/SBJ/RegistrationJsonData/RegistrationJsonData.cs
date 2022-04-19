using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Auditing;
using RMS.SBJ.Registrations;

namespace RMS.SBJ.RegistrationJsonData
{
    [Table("RegistrationJsonData")]
    [Audited]
    public class RegistrationJsonData : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string Data { get; set; }

        public virtual DateTime DateCreated { get; set; }

        public virtual long? RegistrationId { get; set; }

        [ForeignKey("RegistrationId")]
        public Registration RegistrationFk { get; set; }
    }
}