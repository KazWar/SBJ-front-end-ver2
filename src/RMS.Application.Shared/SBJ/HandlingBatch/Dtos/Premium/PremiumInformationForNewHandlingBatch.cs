namespace RMS.SBJ.HandlingBatch.Dtos.Premium
{
    public sealed class PremiumInformationForNewHandlingBatch
    {
        public string CustomerCode { get; set; }

        public int QuantityToDeliver { get; set; }

        public int QuantityInStock { get; set; }

        public int? QuantityCalculation { get; set; }
    }
}
