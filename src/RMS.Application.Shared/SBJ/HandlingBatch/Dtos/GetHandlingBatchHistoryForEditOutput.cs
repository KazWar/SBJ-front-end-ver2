using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.HandlingBatch.Dtos
{
    public class GetHandlingBatchHistoryForEditOutput
    {
        public CreateOrEditHandlingBatchHistoryDto HandlingBatchHistory { get; set; }

        public string HandlingBatchRemarks { get; set; }

        public string HandlingBatchStatusStatusDescription { get; set; }

    }
}