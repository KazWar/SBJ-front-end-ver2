using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using RMS.SBJ.ActivationCodes;
using RMS.SBJ.Forms;
using RMS.SBJ.HandlingLineLocales;
using RMS.SBJ.CampaignProcesses;

namespace RMS.SBJ.CodeTypeTables
{
    [Table("Locale")]
    [Audited]
    public class Locale : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [RegularExpression(LocaleConsts.LanguageCodeRegex)]
        [StringLength(LocaleConsts.MaxLanguageCodeLength, MinimumLength = LocaleConsts.MinLanguageCodeLength)]
        public virtual string LanguageCode { get; set; }

        [Required]
        public virtual string Description { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual long CountryId { get; set; }

        [ForeignKey("CountryId")]
        public Country CountryFk { get; set; }

        public virtual ICollection<ActivationCode> ActivationCodes { get; set; }
        public virtual ICollection<CampaignCategoryTranslation> CampaignCategoryTranslations { get; set; }
        public virtual ICollection<FormFieldTranslation> FormFieldTranslations { get; set; }
        public virtual ICollection<FormLocale> FormLocales { get; set; }
        public virtual ICollection<HandlingLineLocale> HandlingLineLocales { get; set; }
        public virtual ICollection<ListValueTranslation> ListValueTranslations { get; set; }
        public virtual ICollection<MessageContentTranslation> MessageContentTranslations { get; set; }
        public virtual ICollection<RejectionReasonTranslation> RejectionReasonTranslations { get; set; }
    }
}