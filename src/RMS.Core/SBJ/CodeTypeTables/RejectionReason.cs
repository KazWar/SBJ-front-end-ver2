using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace RMS.SBJ.CodeTypeTables
{
    [Table("RejectionReason")]
    public class RejectionReason : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string Description { get; set; }

        [Required]
        public virtual bool IsIncompleteReason { get; set; }

        public virtual ICollection<RejectionReasonTranslation> RejectionReasonTranslations { get; set; }
    }
}