using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using System.Collections.Generic;
using RMS.SBJ.CampaignProcesses;

namespace RMS.SBJ.CodeTypeTables
{
    [Table("MessageComponentType")]
    [Audited]
    public class MessageComponentType : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string Name { get; set; }

        public virtual ICollection<MessageComponent> MessageComponents { get; set; }
    }
}