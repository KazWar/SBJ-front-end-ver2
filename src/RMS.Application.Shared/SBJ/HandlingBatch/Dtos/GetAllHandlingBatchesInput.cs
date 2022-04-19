using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.HandlingBatch.Dtos
{
    public class GetAllHandlingBatchesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public DateTime? MaxDateCreatedFilter { get; set; }
        public DateTime? MinDateCreatedFilter { get; set; }

        public string RemarksFilter { get; set; }

        public string CampaignTypeFilter { get; set; }

        public string HandlingBatchStatusFilter { get; set; }

    }
}