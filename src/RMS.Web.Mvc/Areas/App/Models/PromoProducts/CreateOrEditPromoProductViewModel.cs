using RMS.PromoPlanner.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.PromoProducts
{
    public class CreateOrEditPromoProductModalViewModel
    {
       public CreateOrEditPromoProductDto PromoProduct { get; set; }

	   		public string PromoPromocode { get; set;}

		public string ProductCtn { get; set;}


       
	   public bool IsEditMode => PromoProduct.Id.HasValue;
    }
}