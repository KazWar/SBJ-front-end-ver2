using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using RMS.SBJ.CodeTypeTables;

namespace RMS.SBJ.CampaignProcesses
{
    [Table("CampaignCampaignType")]
    [Audited]
    public class CampaignCampaignType : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string Description { get; set; }

        public virtual long CampaignId { get; set; }

        public virtual long CampaignTypeId { get; set; }

        [ForeignKey("CampaignId")]
        public Campaign CampaignFk { get; set; }

        [ForeignKey("CampaignTypeId")]
        public CampaignType CampaignTypeFk { get; set; }
    }
}