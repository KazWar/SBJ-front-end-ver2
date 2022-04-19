namespace RMS.SBJ.HandlingBatch.Dtos
{
    public class GetHandlingBatchForViewDto
    {
        public HandlingBatchDto HandlingBatch { get; set; }

        public string CampaignTypeName { get; set; }

        public string HandlingBatchStatusStatusDescription { get; set; }

    }
}