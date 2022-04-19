using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace RMS.SBJ.HandlingBatch
{
    [Table("HandlingBatchLineHistory")]
    public class HandlingBatchLineHistory : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual DateTime DateCreated { get; set; }

        public virtual string Remarks { get; set; }

        public virtual long AbpUserId { get; set; }

        public virtual long HandlingBatchLineId { get; set; }

        [ForeignKey("HandlingBatchLineId")]
        public HandlingBatchLine HandlingBatchLineFk { get; set; }

        public virtual long HandlingBatchLineStatusId { get; set; }

        [ForeignKey("HandlingBatchLineStatusId")]
        public HandlingBatchLineStatus HandlingBatchLineStatusFk { get; set; }

    }
}