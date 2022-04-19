using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.HandlingBatch.Dtos
{
    public class HandlingBatchHistoryDto : EntityDto<long>
    {

        public long HandlingBatchId { get; set; }

        public long HandlingBatchStatusId { get; set; }

    }
}