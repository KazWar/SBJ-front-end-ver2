using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using RMS.SBJ.Retailers;

namespace RMS.PromoPlanner
{
    [Table("PromoRetailer")]
    public class PromoRetailer : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual long PromoId { get; set; }

        public virtual long RetailerId { get; set; }

        [ForeignKey("PromoId")]
        public Promo PromoFk { get; set; }

        [ForeignKey("RetailerId")]
        public Retailer RetailerFk { get; set; }
    }
}