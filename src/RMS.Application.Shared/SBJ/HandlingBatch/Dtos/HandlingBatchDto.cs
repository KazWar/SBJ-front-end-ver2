using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.HandlingBatch.Dtos
{
    public class HandlingBatchDto : EntityDto<long>
    {
        public DateTime DateCreated { get; set; }

        public string Remarks { get; set; }

        public long CampaignTypeId { get; set; }

        public string CampaignTypeCode { get; set; }

        public long HandlingBatchStatusId { get; set; }

    }
}