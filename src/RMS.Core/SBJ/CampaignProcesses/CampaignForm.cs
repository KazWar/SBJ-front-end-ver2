using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using RMS.SBJ.Forms;

namespace RMS.SBJ.CampaignProcesses
{
    [Table("CampaignForm")]
    [Audited]
    public class CampaignForm : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual long CampaignId { get; set; }

        public virtual long FormId { get; set; }

        [ForeignKey("CampaignId")]
        public Campaign CampaignFk { get; set; }

        [ForeignKey("FormId")]
        public Form FormFk { get; set; }
    }
}