using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using RMS.SBJ.CodeTypeTables;
using System.Collections.Generic;

namespace RMS.SBJ.CampaignProcesses
{
    [Table("CampaignTypeEvent")]
    [Audited]
    public class CampaignTypeEvent : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual int SortOrder { get; set; }

        public virtual long CampaignTypeId { get; set; }

        public virtual long ProcessEventId { get; set; }

        [ForeignKey("CampaignTypeId")]
        public CampaignType CampaignTypeFk { get; set; }

        [ForeignKey("ProcessEventId")]
        public ProcessEvent ProcessEventFk { get; set; }

        public virtual ICollection<CampaignTypeEventRegistrationStatus> CampaignTypeEventRegistrationStatuses { get; set; }
    }
}