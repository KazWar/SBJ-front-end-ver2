using RMS.SBJ.CodeTypeTables;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using System.Collections.Generic;

namespace RMS.SBJ.CampaignProcesses
{
    [Table("CampaignTypeEventRegistrationStatus")]
    [Audited]
    public class CampaignTypeEventRegistrationStatus : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual int SortOrder { get; set; }

        public virtual long CampaignTypeEventId { get; set; }

        public virtual long RegistrationStatusId { get; set; }

        [ForeignKey("CampaignTypeEventId")]
        public CampaignTypeEvent CampaignTypeEventFk { get; set; }

        [ForeignKey("RegistrationStatusId")]
        public RegistrationStatus RegistrationStatusFk { get; set; }

        public virtual ICollection<MessageComponentContent> MessageComponentContents { get; set; }
    }
}