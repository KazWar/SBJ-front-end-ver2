using RMS.SBJ.CodeTypeTables;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace RMS.SBJ.CampaignProcesses
{
    [Table("CampaignCountry")]
    public class CampaignCountry : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual long CampaignId { get; set; }

        [ForeignKey("CampaignId")]
        public Campaign CampaignFk { get; set; }

        public virtual long CountryId { get; set; }

        [ForeignKey("CountryId")]
        public Country CountryFk { get; set; }
    }
}