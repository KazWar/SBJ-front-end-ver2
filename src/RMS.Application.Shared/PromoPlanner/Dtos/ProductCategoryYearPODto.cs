using Abp.Application.Services.Dto;

namespace RMS.PromoPlanner.Dtos
{
    public class ProductCategoryYearPoDto : EntityDto<long>
    {
		public int Year { get; set; }

		public string PoNumberHandling { get; set; }

		public string PoNumberCashback { get; set; }

		public long ProductCategoryId { get; set; }		 
    }
}