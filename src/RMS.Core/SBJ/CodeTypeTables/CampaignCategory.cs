using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using System.Collections.Generic;

namespace RMS.SBJ.CodeTypeTables
{
    [Table("CampaignCategory")]
    [Audited]
    public class CampaignCategory : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string Name { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual int SortOrder { get; set; }

        public virtual ICollection<CampaignCategoryTranslation> CampaignCategoryTranslations { get; set; }
    }
}