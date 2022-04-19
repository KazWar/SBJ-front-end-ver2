using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using System.Collections.Generic;
using RMS.SBJ.HandlingLineProducts;
using RMS.SBJ.PurchaseRegistrations;

namespace RMS.SBJ.Products
{
    [Table("Product")]
    [Audited]
    public class Product : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string ProductCode { get; set; }

        public virtual string Description { get; set; }

        public virtual string Ean { get; set; }

        public virtual long? ProductCategoryId { get; set; }

        [ForeignKey("ProductCategoryId")]
        public ProductCategory ProductCategoryFk { get; set; }

        public virtual ICollection<HandlingLineProduct> HandlingLineProducts { get; set; }
        public virtual ICollection<PurchaseRegistration> PurchaseRegistrations { get; set; }
    }
}