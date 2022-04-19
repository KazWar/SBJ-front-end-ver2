using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.HandlingBatch.Dtos
{
    public class GetAllHandlingBatchHistoriesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string HandlingBatchRemarksFilter { get; set; }

        public string HandlingBatchStatusStatusDescriptionFilter { get; set; }

    }
}