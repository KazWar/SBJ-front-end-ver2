using RMS.SBJ.CampaignProcesses;
using RMS.SBJ.CodeTypeTables;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace RMS.SBJ.CampaignProcesses
{
    [Table("CampaignTranslation")]
    public class CampaignTranslation : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string Name { get; set; }

        [Required]
        public virtual string Description { get; set; }

        public virtual long CampaignId { get; set; }

        [ForeignKey("CampaignId")]
        public Campaign CampaignFk { get; set; }

        public virtual long LocaleId { get; set; }

        [ForeignKey("LocaleId")]
        public Locale LocaleFk { get; set; }

    }
}