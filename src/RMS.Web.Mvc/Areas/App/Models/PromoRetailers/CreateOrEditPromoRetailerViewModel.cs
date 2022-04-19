using RMS.PromoPlanner.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.PromoRetailers
{
    public class CreateOrEditPromoRetailerModalViewModel
    {
       public CreateOrEditPromoRetailerDto PromoRetailer { get; set; }

	   		public string PromoPromocode { get; set;}

		public string RetailerCode { get; set;}


       
	   public bool IsEditMode => PromoRetailer.Id.HasValue;
    }
}