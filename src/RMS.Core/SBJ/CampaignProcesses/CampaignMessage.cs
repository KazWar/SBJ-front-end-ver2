using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using RMS.SBJ.SystemTables;

namespace RMS.SBJ.CampaignProcesses
{
    [Table("CampaignMessage")]
    [Audited]
    public class CampaignMessage : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual long CampaignId { get; set; }

        public virtual long MessageId { get; set; }

        [ForeignKey("CampaignId")]
        public Campaign CampaignFk { get; set; }

        [ForeignKey("MessageId")]
        public Message MessageFk { get; set; }
    }
}