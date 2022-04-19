using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using RMS.SBJ.CampaignProcesses;
using RMS.SBJ.RetailerLocations;

namespace RMS.SBJ.CampaignRetailerLocations
{
    [Table("CampaignRetailerLocation")]
    public class CampaignRetailerLocation : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual long CampaignId { get; set; }

        public virtual long RetailerLocationId { get; set; }

        [ForeignKey("CampaignId")]
        public Campaign CampaignFk { get; set; }

        [ForeignKey("RetailerLocationId")]
        public RetailerLocation RetailerLocationFk { get; set; }
    }
}