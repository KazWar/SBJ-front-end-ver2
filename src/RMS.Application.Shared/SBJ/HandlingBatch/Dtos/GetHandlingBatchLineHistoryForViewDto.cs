namespace RMS.SBJ.HandlingBatch.Dtos
{
    public class GetHandlingBatchLineHistoryForViewDto
    {
        public HandlingBatchLineHistoryDto HandlingBatchLineHistory { get; set; }

        public string HandlingBatchLineCustomerCode { get; set; }

        public string HandlingBatchLineStatusStatusDescription { get; set; }

    }
}