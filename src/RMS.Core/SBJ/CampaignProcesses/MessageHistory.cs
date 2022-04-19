using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace RMS.SBJ.CampaignProcesses
{
    [Table("MessageHistory")]
    [Audited]
    public class MessageHistory : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual long RegistrationId { get; set; }

        public virtual long AbpUserId { get; set; }

        public virtual string Content { get; set; }

        public virtual DateTime TimeStamp { get; set; }

        public virtual string MessageName { get; set; }

        public virtual long MessageId { get; set; }

        public virtual string Subject { get; set; }

        public virtual string To { get; set; }
    }
}