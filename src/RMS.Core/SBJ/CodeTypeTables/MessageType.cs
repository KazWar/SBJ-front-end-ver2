using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using RMS.SBJ.SystemTables;
using System.Collections.Generic;
using RMS.SBJ.CampaignProcesses;

namespace RMS.SBJ.CodeTypeTables
{
    [Table("MessageType")]
    [Audited]
    public class MessageType : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string Name { get; set; }

        [Required]
        public virtual string Source { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual long MessageId { get; set; }

        [ForeignKey("MessageId")]
        public Message MessageFk { get; set; }

        public virtual ICollection<MessageComponent> MessageComponents { get; set; }
    }
}