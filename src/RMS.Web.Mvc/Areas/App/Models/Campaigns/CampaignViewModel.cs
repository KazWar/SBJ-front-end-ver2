using Abp.Application.Services.Dto;
using RMS.SBJ.CampaignProcesses.Dtos;
using RMS.SBJ.CodeTypeTables.Dtos;
using RMS.SBJ.Forms.Dtos;
using RMS.Web.Areas.App.Models.MessageComponentContents;
using System.Collections.Generic;

namespace RMS.Web.Areas.App.Models.Campaigns
{
    public class CampaignViewModel : GetCampaignForViewDto
    {
        public List<GetCampaignCampaignTypeForViewDto> CampaignCampaignType { get; set; }
        public List<GetFormLocaleForViewDto> CampaignFormLocales { get; set; }
        public List<GetLocaleForViewDto> Locales { get; set; }  //Added to currently display locales for campaign messages
        public bool Editable { get; set; }

        #region Added for Campaign messaging - Message components
        public PagedResultDto<GetCampaignTypeEventForViewDto> CampaignTypeEvents { get; set; }
        public PagedResultDto<GetCampaignTypeEventRegistrationStatusForViewDto> CampaignTypeEventRegistrationStatuses { get; set; }
        public IReadOnlyCollection<MessagingViewModel> MessagingViewModel { get; set; }
        public PagedResultDto<GetMessageTypeForViewDto> MessageTypes { get; set; }
        public PagedResultDto<GetMessageComponentForViewDto> MessageComponents { get; set; }
        public PagedResultDto<GetMessageComponentContentForViewDto> MessageComponentContents { get; set; }
        public PagedResultDto<GetMessageContentTranslationForViewDto> MessageContentTranslations { get; set; }
        public PagedResultDto<GetMessageVariableForViewDto> MessageVariables { get; set; }
        #endregion
    }
}