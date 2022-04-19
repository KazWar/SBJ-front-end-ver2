using RMS.SBJ.Products;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace RMS.PromoPlanner
{
    [Table("ProductCategoryYearPo")]
    public class ProductCategoryYearPo : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual int Year { get; set; }

        public virtual string PoNumberHandling { get; set; }

        public virtual string PoNumberCashback { get; set; }

        public virtual long ProductCategoryId { get; set; }

        [ForeignKey("ProductCategoryId")]
        public ProductCategory ProductCategoryFk { get; set; }
    }
}