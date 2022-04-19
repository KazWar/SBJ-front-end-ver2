using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using RMS.SBJ.CampaignRetailerLocations;
using RMS.SBJ.PurchaseRegistrations;
using RMS.SBJ.Retailers;

namespace RMS.SBJ.RetailerLocations
{
    [Table("RetailerLocation")]
    public class RetailerLocation : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string Name { get; set; }

        public virtual string Address { get; set; }

        public virtual string PostalCode { get; set; }

        public virtual string City { get; set; }

        public virtual string ExternalCode { get; set; }

        public virtual long RetailerId { get; set; }

        public virtual string ExternalId { get; set; }

        [ForeignKey("RetailerId")]
        public Retailer RetailerFk { get; set; }

        public virtual ICollection<CampaignRetailerLocation> CampaignRetailerLocations { get; set; }

        public virtual ICollection<PurchaseRegistration> PurchaseRegistrations { get; set; }
    }
}