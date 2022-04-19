using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using System.Collections.Generic;
using RMS.SBJ.CampaignProcesses;

namespace RMS.SBJ.CodeTypeTables
{
    [Table("RegistrationStatus")]
    [Audited]
    public class RegistrationStatus : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string StatusCode { get; set; }

        [Required]
        public virtual string Description { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual ICollection<CampaignTypeEventRegistrationStatus> CampaignTypeEventRegistrationStatuses { get; set; }
        public virtual ICollection<RegistrationHistory.RegistrationHistory> RegistrationHistories { get; set; }
    }
}