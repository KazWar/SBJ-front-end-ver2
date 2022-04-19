using RMS.SBJ.CampaignProcesses.Dtos;
using RMS.Web.Areas.App.Models.CampaignCampaignTypes;
using RMS.Web.Areas.App.Models.CampaignForms;
using RMS.Web.Areas.App.Models.CampaignMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMS.Web.Areas.App.Models.Campaigns
{
    public class CreateCampaignViewModel
    {
        public CreateOrEditCampaignDto Campaign { get; set; }

        public List<CampaignCampaignTypeMultiSelectModel> SelectedCampaignCampaignType { get; set; }

        public List<CampaignCampaignTypeMultiSelectModel> UnselectedCampaignCampaignType { get; set; }

        public GetCampaignFormForViewDto CampaignFormViewModel { get; set; }

        public CampaignMessageViewModel CampaignMessageViewModel { get; set; }
    }
}
