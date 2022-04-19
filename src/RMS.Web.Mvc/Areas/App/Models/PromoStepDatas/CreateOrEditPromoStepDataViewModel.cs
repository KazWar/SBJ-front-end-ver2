using RMS.PromoPlanner.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.PromoStepDatas
{
    public class CreateOrEditPromoStepDataModalViewModel
    {
       public CreateOrEditPromoStepDataDto PromoStepData { get; set; }

	   		public string PromoStepDescription { get; set;}

		public string PromoPromocode { get; set;}


       
	   public bool IsEditMode => PromoStepData.Id.HasValue;
    }
}