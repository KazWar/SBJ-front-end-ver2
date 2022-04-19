namespace RMS.SBJ.HandlingBatch.Dtos.ActivationCode
{
    public sealed class ActivationCodeInformationForNewHandlingBatch
    {
        public string Locale { get; set; }

        public int ActivationCodesToDeliver { get; set; }

        public int ActivationCodesInStore { get; set; }
    }
}
