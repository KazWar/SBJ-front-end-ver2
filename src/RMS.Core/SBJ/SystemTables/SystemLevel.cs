using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using System.Collections.Generic;
using RMS.SBJ.Forms;

namespace RMS.SBJ.SystemTables
{
    [Table("SystemLevel")]
    [Audited]
    public class SystemLevel : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string Description { get; set; }

        public virtual ICollection<Form> Forms { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}