using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.HandlingBatch.Dtos
{
    public class CreateOrEditHandlingBatchLineHistoryDto : EntityDto<long?>
    {

        public long HandlingBatchLineId { get; set; }

        public long HandlingBatchLineStatusId { get; set; }

    }
}