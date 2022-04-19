using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using RMS.SBJ.ActivationCodeRegistrations;
using RMS.SBJ.CampaignProcesses;
using RMS.SBJ.CodeTypeTables;

namespace RMS.SBJ.ActivationCodes
{
    [Table("ActivationCode")]
    public class ActivationCode : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string Code { get; set; }

        public virtual string Description { get; set; }

        public virtual long LocaleId { get; set; }

        [ForeignKey("LocaleId")]
        public Locale LocaleFk { get; set; }

        public virtual long CampaignId { get; set; }

        [ForeignKey("CampaignId")]
        public Campaign CampaignFk { get; set; }

        public virtual ICollection<ActivationCodeRegistration> ActivationCodeRegistrations { get; set; }
    }
}