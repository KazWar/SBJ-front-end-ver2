using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace RMS.SBJ.CodeTypeTables
{
    [Table("MessageVariable")]
    [Audited]
    public class MessageVariable : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string Description { get; set; }

        [Required]
        public virtual string RmsTable { get; set; }

        [Required]
        public virtual string TableField { get; set; }
    }
}