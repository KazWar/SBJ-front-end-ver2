using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using RMS.SBJ.Products;

namespace RMS.PromoPlanner
{
    [Table("PromoProduct")]
    public class PromoProduct : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual long PromoId { get; set; }

        public virtual long ProductId { get; set; }

        [ForeignKey("PromoId")]
        public Promo PromoFk { get; set; }

        [ForeignKey("ProductId")]
        public Product ProductFk { get; set; }
    }
}