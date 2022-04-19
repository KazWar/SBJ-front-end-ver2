using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RMS.SBJ.HandlingBatches
{
    [Table("HandlingBatchStatus")]
    public class HandlingBatchStatus : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string StatusCode { get; set; }

        [Required]
        public virtual string StatusDescription { get; set; }
    }
}
