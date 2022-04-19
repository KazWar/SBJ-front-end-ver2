using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Auditing;
using System.Collections.Generic;

namespace RMS.SBJ.Products
{
    [Table("ProductCategory")]
    [Audited]
    public class ProductCategory : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string Code { get; set; }

        public virtual string Description { get; set; }

        public virtual string PoHandling { get; set; }

        public virtual string PoCashBack { get; set; }

        public virtual string Color { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}