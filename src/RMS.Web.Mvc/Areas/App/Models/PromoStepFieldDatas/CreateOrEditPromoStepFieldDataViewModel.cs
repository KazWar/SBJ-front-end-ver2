using RMS.PromoPlanner.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.PromoStepFieldDatas
{
    public class CreateOrEditPromoStepFieldDataModalViewModel
    {
       public CreateOrEditPromoStepFieldDataDto PromoStepFieldData { get; set; }

	   		public string PromoStepFieldDescription { get; set;}

		public string PromoStepDataDescription { get; set;}


       
	   public bool IsEditMode => PromoStepFieldData.Id.HasValue;
    }
}