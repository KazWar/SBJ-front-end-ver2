using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.HandlingBatch.Dtos
{
    public class HandlingBatchLineHistoryDto : EntityDto<long>
    {

        public long HandlingBatchLineId { get; set; }

        public long HandlingBatchLineStatusId { get; set; }

    }
}