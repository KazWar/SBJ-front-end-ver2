using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Auditing;
using RMS.SBJ.HandlingLines;
using RMS.SBJ.Products;

namespace RMS.SBJ.HandlingLineProducts
{
    [Table("HandlingLineProduct")]
    [Audited]
    public class HandlingLineProduct : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public bool HiddenOnFrontend { get; set; }

        public virtual long HandlingLineId { get; set; }

        public virtual long ProductId { get; set; }

        [ForeignKey("HandlingLineId")]
        public HandlingLine HandlingLineFk { get; set; }

        [ForeignKey("ProductId")]
        public Product ProductFk { get; set; }
    }
}