using RMS.SBJ.CampaignProcesses.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.CampaignCountries
{
    public class CreateOrEditCampaignCountryModalViewModel
    {
        public CreateOrEditCampaignCountryDto CampaignCountry { get; set; }

        public string CampaignName { get; set; }

        public string CountryDescription { get; set; }

        public bool IsEditMode => CampaignCountry.Id.HasValue;
    }
}