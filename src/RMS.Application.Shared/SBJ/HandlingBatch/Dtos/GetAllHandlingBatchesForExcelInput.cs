using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.HandlingBatch.Dtos
{
    public class GetAllHandlingBatchesForExcelInput
    {
        public string Filter { get; set; }

        public DateTime? MaxDateCreatedFilter { get; set; }
        public DateTime? MinDateCreatedFilter { get; set; }

        public string RemarksFilter { get; set; }

        public string CampaignTypeNameFilter { get; set; }

        public string HandlingBatchStatusStatusDescriptionFilter { get; set; }

    }
}