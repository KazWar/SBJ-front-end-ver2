using RMS.SBJ.CampaignProcesses.Dtos;
using RMS.SBJ.CodeTypeTables.Dtos;
using RMS.Web.Areas.App.Models.CampaignCampaignTypes;
using System.Collections.Generic;

namespace RMS.Web.Areas.App.Models.Campaigns
{
    public class CreateOrEditCampaignViewModel
    {
        public CreateOrEditCampaignDto Campaign { get; set; }

        public CreateOrEditCampaignDto DuplicateFrom { get; set; }

        public IEnumerable<CampaignCampaignTypeMultiSelectModel> CampaignTypeViewModelList { get; set; }

        public IEnumerable<GetLocaleForViewDto> SelectableLocales { get; set; }

        public bool IsEditMode => Campaign.Id.HasValue;
    }
}