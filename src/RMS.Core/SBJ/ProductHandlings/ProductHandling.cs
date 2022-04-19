using RMS.SBJ.CampaignProcesses;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using System.Collections.Generic;
using RMS.SBJ.HandlingLines;

namespace RMS.SBJ.ProductHandlings
{
    [Table("ProductHandling")]
    public class ProductHandling : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string Description { get; set; }

        public virtual long CampaignId { get; set; }

        [ForeignKey("CampaignId")]
        public Campaign CampaignFk { get; set; }

        public virtual ICollection<HandlingLine> HandlingLines { get; set; }
    }
}