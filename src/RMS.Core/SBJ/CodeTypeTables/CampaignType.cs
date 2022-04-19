using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using System.Collections.Generic;
using RMS.SBJ.CampaignProcesses;
using RMS.SBJ.HandlingLines;

namespace RMS.SBJ.CodeTypeTables
{
    [Table("CampaignType")]
    [Audited]
    public class CampaignType : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string Name { get; set; }

        [Required]
        public virtual string Code { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual ICollection<CampaignCampaignType> CampaignCampaignTypes { get; set; }
        public virtual ICollection<CampaignTypeEvent> CampaignTypeEvents { get; set; }
        public virtual ICollection<HandlingLine> HandlingLines { get; set; }
    }
}