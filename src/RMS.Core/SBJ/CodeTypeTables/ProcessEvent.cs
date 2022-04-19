using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using System.Collections.Generic;
using RMS.SBJ.CampaignProcesses;

namespace RMS.SBJ.CodeTypeTables
{
    [Table("ProcessEvent")]
    [Audited]
    public class ProcessEvent : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string Name { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual ICollection<CampaignTypeEvent> CampaignTypeEvents { get; set; }
    }
}