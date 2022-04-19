using Abp.Application.Services.Dto;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RMS.SBJ.CampaignProcesses.Dtos;
using RMS.SBJ.CodeTypeTables.Dtos;
using System;
using System.Collections.Generic;

namespace RMS.Web.Areas.App.Models.MessageComponentContents
{
    public class CreateOrEditMessageComponentContentModalEntityViewModel
    {
        public CreateOrEditMessageComponentContentDto MessageComponentContent { get; set; }
        public string MessageComponentIsActive { get; set; }
        public string CampaignTypeEventRegistrationStatusSortOrder { get; set; }
        public GetMessageComponentForViewDto[] MessageComponents { get; set; }
        public IReadOnlyCollection<MessagingViewModel> Messaging { get; set; }
        public PagedResultDto<GetMessageTypeForViewDto> MessageTypes { get; set; }
        public List<GetMessageContentTranslationForViewDto> MessageComponentContentTranslation { get; set; }
        public long LocaleId { get; set; }
        public List<GetMessageComponentContentForViewDto> MessageComponentContentsPerCampaignTypeEventRegistrationStatusId { get; set; }
    }
}