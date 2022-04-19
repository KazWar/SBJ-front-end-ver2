using RMS.SBJ.CampaignProcesses.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.CampaignTranslations
{
    public class CreateOrEditCampaignTranslationModalViewModel
    {
        public CreateOrEditCampaignTranslationDto CampaignTranslation { get; set; }

        public string CampaignName { get; set; }

        public string LocaleDescription { get; set; }

        public bool IsEditMode => CampaignTranslation.Id.HasValue;
    }
}