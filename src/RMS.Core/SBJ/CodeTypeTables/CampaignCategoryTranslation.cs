using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace RMS.SBJ.CodeTypeTables
{
    [Table("CampaignCategoryTranslation")]
    [Audited]
    public class CampaignCategoryTranslation : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string Name { get; set; }

        public virtual long LocaleId { get; set; }

        public virtual long CampaignCategoryId { get; set; }

        [ForeignKey("CampaignCategoryId")]
        public CampaignCategory CampaignCategoryFk { get; set; }

        [ForeignKey("LocaleId")]
        public Locale LocaleFk { get; set; }
    }
}