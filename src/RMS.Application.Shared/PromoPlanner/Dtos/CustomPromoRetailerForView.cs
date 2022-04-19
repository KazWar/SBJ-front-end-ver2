
namespace RMS.PromoPlanner.Dtos
{
    public class CustomPromoRetailerForView
    {
        public long Id { get; set; }
        public long RetailerId { get; set; }

        public string RetailerCode { get; set; }

        public string RetailerName { get; set; }

        public string RetailerCountry { get; set; }
    }
}
