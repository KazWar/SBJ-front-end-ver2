namespace RMS.PromoPlanner.Dtos
{
    public sealed class PromoStepForViewDto
    {
        public long? Id { get; set; }
        public long PromoId { get; set; }
        public long PromoStepId { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public short Sequence { get; set; }
        public CustomPromoStepFieldForView[] PromoStepFields { get; set; }
    }
}
