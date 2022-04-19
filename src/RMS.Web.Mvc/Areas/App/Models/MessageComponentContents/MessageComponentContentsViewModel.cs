using Abp.Application.Services.Dto;
using RMS.Migrations;
using RMS.SBJ.CampaignProcesses.Dtos;
using RMS.SBJ.CodeTypeTables.Dtos;
using System.Collections.Generic;
using System.Net.Sockets;

namespace RMS.Web.Areas.App.Models.MessageComponentContents
{
    public class MessageComponentContentsViewModel
    {
        public string FilterText { get; set; }
        public PagedResultDto<GetCampaignTypeEventForViewDto> CampaignTypeEvents { get; set; }
        public PagedResultDto<GetCampaignTypeEventRegistrationStatusForViewDto> CampaignTypeEventRegistrationStatuses { get; set; }
        public IReadOnlyCollection<MessagingViewModel> MessagingViewModel { get; set; }
        public PagedResultDto<GetMessageTypeForViewDto> MessageTypes { get; set; }
        public PagedResultDto<GetMessageComponentForViewDto> MessageComponents { get; set; }
        public List<GetMessageComponentTypeForViewDto> MessageComponentTypes { get; set; }
        public PagedResultDto<GetMessageComponentContentForViewDto> MessageComponentContents { get; set; }
        public PagedResultDto<GetMessageVariableForViewDto> MessageVariables { get; set; }
        public long? MessageTypeId { get; set; }
        public long? CampaignTypeEventRegistrationStatusId { get; set; }
        public List<GetLocaleForViewDto> Locales { get; set; }
        public List<GetMessageContentTranslationForViewDto> MessageContentTranslations { get; set; }
        public long LocaleId { get; set; }
    }
}