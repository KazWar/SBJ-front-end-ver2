using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using System.Collections.Generic;

namespace RMS.SBJ.CampaignProcesses
{
    [Table("MessageComponentContent")]
    [Audited]
    public class MessageComponentContent : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string Content { get; set; }

        public virtual long MessageComponentId { get; set; }

        public virtual long CampaignTypeEventRegistrationStatusId { get; set; }

        [ForeignKey("MessageComponentId")]
        public MessageComponent MessageComponentFk { get; set; }

        [ForeignKey("CampaignTypeEventRegistrationStatusId")]
        public CampaignTypeEventRegistrationStatus CampaignTypeEventRegistrationStatusFk { get; set; }

        public virtual ICollection<MessageContentTranslation> MessageContentTranslations { get; set; }
    }
}