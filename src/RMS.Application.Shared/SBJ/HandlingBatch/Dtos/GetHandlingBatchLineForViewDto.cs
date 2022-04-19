namespace RMS.SBJ.HandlingBatch.Dtos
{
    public class GetHandlingBatchLineForViewDto
    {
        public HandlingBatchLineDto HandlingBatchLine { get; set; }

        public string HandlingBatchRemarks { get; set; }

        public string PurchaseRegistrationInvoiceImagePath { get; set; }

        public string HandlingBatchLineStatusStatusDescription { get; set; }

    }
}