using RMS.PromoPlanner.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.PromoStepFields
{
    public class CreateOrEditPromoStepFieldModalViewModel
    {
       public CreateOrEditPromoStepFieldDto PromoStepField { get; set; }

	   		public string PromoStepDescription { get; set;}


       
	   public bool IsEditMode => PromoStepField.Id.HasValue;
    }
}