using RMS.PromoPlanner.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.PromoSteps
{
    public class CreateOrEditPromoStepModalViewModel
    {
       public CreateOrEditPromoStepDto PromoStep { get; set; }

	   
       
	   public bool IsEditMode => PromoStep.Id.HasValue;
    }
}