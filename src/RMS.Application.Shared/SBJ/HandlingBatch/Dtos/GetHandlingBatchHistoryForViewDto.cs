namespace RMS.SBJ.HandlingBatch.Dtos
{
    public class GetHandlingBatchHistoryForViewDto
    {
        public HandlingBatchHistoryDto HandlingBatchHistory { get; set; }

        public string HandlingBatchRemarks { get; set; }

        public string HandlingBatchStatusStatusDescription { get; set; }

    }
}