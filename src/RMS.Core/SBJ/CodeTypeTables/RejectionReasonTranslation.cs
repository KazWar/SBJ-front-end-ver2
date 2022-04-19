using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace RMS.SBJ.CodeTypeTables
{
    [Table("RejectionReasonTranslation")]
    public class RejectionReasonTranslation : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string Description { get; set; }

        public virtual long LocaleId { get; set; }

        public virtual long RejectionReasonId { get; set; }

        [ForeignKey("LocaleId")]
        public virtual Locale LocaleFk { get; set; }

        [ForeignKey("RejectionReasonId")]
        public virtual RejectionReason RejectionReasonFk { get; set; }
    }
}
