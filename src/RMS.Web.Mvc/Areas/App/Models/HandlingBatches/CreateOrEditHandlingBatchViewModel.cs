using RMS.SBJ.HandlingBatch.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.HandlingBatches
{
    public class CreateOrEditHandlingBatchViewModel
    {
        public CreateOrEditHandlingBatchDto HandlingBatch { get; set; }

        public string CampaignTypeName { get; set; }

        public string HandlingBatchStatusStatusDescription { get; set; }

        public bool IsEditMode => HandlingBatch.Id.HasValue;
    }
}