using RMS.PromoPlanner.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.PromoScopes
{
    public class CreateOrEditPromoScopeModalViewModel
    {
       public CreateOrEditPromoScopeDto PromoScope { get; set; }

	   
       
	   public bool IsEditMode => PromoScope.Id.HasValue;
    }
}