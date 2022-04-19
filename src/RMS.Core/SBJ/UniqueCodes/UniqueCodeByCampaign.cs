using RMS.SBJ.CampaignProcesses;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace RMS.SBJ.UniqueCodes
{
    [Table("UniqueCodeByCampaign")]
    public class UniqueCodeByCampaign : Entity<string>
    {

        public virtual bool Used { get; set; }

        public virtual long CampaignId { get; set; }

        [ForeignKey("CampaignId")]
        public Campaign CampaignFk { get; set; }

    }
}