using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.HandlingBatch.Dtos
{
    public class CreateOrEditHandlingBatchHistoryDto : EntityDto<long?>
    {

        public long HandlingBatchId { get; set; }

        public long HandlingBatchStatusId { get; set; }

    }
}