using RMS.PromoPlanner.Dtos;

namespace RMS.Web.Areas.App.Models.Promos
{
    public class CreateOrEditPromoModalViewModel
    {
        public CreateOrEditPromoDto Promo { get; set; }

        public string PromoScopeDescription { get; set; }

        public string CampaignTypeName { get; set; }

        public string ProductCategoryDescription { get; set; }

        public bool IsEditMode => Promo.Id.HasValue;
    }
}