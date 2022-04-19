using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace RMS.SBJ.HandlingBatch
{
    [Table("HandlingBatchHistory")]
    public class HandlingBatchHistory : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual DateTime DateCreated { get; set; }

        public virtual string Remarks { get; set; }

        public virtual long AbpUserId { get; set; }

        public virtual long HandlingBatchId { get; set; }

        [ForeignKey("HandlingBatchId")]
        public HandlingBatch HandlingBatchFk { get; set; }

        public virtual long HandlingBatchStatusId { get; set; }

        [ForeignKey("HandlingBatchStatusId")]
        public HandlingBatchStatus HandlingBatchStatusFk { get; set; }

    }
}