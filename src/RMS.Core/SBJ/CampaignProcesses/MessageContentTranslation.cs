using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using RMS.SBJ.CodeTypeTables;

namespace RMS.SBJ.CampaignProcesses
{
    [Table("MessageContentTranslation")]
    [Audited]
    public class MessageContentTranslation : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string Content { get; set; }

        public virtual long LocaleId { get; set; }

        public virtual long MessageComponentContentId { get; set; }

        [ForeignKey("LocaleId")]
        public Locale LocaleFk { get; set; }

        [ForeignKey("MessageComponentContentId")]
        public MessageComponentContent MessageComponentContentFk { get; set; }
    }
}