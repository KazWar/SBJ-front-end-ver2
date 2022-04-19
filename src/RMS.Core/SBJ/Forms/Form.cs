using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using RMS.SBJ.SystemTables;
using RMS.SBJ.CampaignProcesses;
using System.Collections.Generic;

namespace RMS.SBJ.Forms
{
    [Table("Form")]
    [Audited]
    public class Form : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string Version { get; set; }

        public virtual long SystemLevelId { get; set; }

        [ForeignKey("SystemLevelId")]
        public SystemLevel SystemLevelFk { get; set; }

        public virtual ICollection<CampaignForm> CampaignForms { get; set; }
        public virtual ICollection<FormLocale> FormLocales { get; set; }
    }
}