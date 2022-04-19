using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using RMS.SBJ.CodeTypeTables;
using System.Collections.Generic;

namespace RMS.SBJ.CampaignProcesses
{
    [Table("MessageComponent")]
    [Audited]
    public class MessageComponent : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual long MessageTypeId { get; set; }

        public virtual long MessageComponentTypeId { get; set; }

        [ForeignKey("MessageTypeId")]
        public MessageType MessageTypeFk { get; set; }

        [ForeignKey("MessageComponentTypeId")]
        public MessageComponentType MessageComponentTypeFk { get; set; }

        public virtual ICollection<MessageComponentContent> MessageComponentContents { get; set; }
    }
}