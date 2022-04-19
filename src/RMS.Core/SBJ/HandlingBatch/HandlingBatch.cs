using RMS.SBJ.CodeTypeTables;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace RMS.SBJ.HandlingBatch
{
    [Table("HandlingBatch")]
    public class HandlingBatch : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual DateTime DateCreated { get; set; }

        public virtual string Remarks { get; set; }

        public virtual long CampaignTypeId { get; set; }

        [ForeignKey("CampaignTypeId")]
        public CampaignType CampaignTypeFk { get; set; }

        public virtual long HandlingBatchStatusId { get; set; }

        [ForeignKey("HandlingBatchStatusId")]
        public HandlingBatchStatus HandlingBatchStatusFk { get; set; }

    }
}