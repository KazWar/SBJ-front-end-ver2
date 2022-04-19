using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace RMS.SBJ.HandlingBatch
{
    [Table("HandlingBatchLineStatus")]
    public class HandlingBatchLineStatus : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string StatusCode { get; set; }

        [Required]
        public virtual string StatusDescription { get; set; }

    }
}