
namespace RMS.PromoPlanner.Dtos
{
    public class CustomProductForView
    {
        public long Id { get; set; }

        public long ProductId { get; set; }
        
        public string CtnCode { get; set; }

        public string EanCode { get; set; }

        public string Description { get; set; }
    }
}
