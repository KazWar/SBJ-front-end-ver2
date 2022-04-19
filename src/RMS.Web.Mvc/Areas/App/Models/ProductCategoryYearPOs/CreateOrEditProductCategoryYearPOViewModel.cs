using RMS.PromoPlanner.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.ProductCategoryYearPOs
{
    public class CreateOrEditProductCategoryYearPOModalViewModel
    {
       public CreateOrEditProductCategoryYearPoDto ProductCategoryYearPO { get; set; }

	   		public string ProductCategoryCode { get; set;}


       
	   public bool IsEditMode => ProductCategoryYearPO.Id.HasValue;
    }
}