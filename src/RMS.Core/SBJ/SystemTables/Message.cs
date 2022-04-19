using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using System.Collections.Generic;
using RMS.SBJ.CampaignProcesses;
using RMS.SBJ.CodeTypeTables;

namespace RMS.SBJ.SystemTables
{
    [Table("Message")]
    [Audited]
    public class Message : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(MessageConsts.MaxVersionLength, MinimumLength = MessageConsts.MinVersionLength)]
        public virtual string Version { get; set; }

        public virtual long SystemLevelId { get; set; }

        [ForeignKey("SystemLevelId")]
        public SystemLevel SystemLevelFk { get; set; }

        public virtual ICollection<CampaignMessage> CampaignMessages { get; set; }
        public virtual ICollection<MessageType> MessageTypes { get; set; }
    }
}