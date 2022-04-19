using Abp.Application.Services.Dto;
using RMS.SBJ.CampaignProcesses.Dtos;
using RMS.SBJ.CodeTypeTables.Dtos;
using System.Collections.Generic;

namespace RMS.Web.Areas.App.Models.CampaignTypeEvents
{
    public class CampaignTypeEventsViewModel
    {
		public string FilterText { get; set; }
        public PagedResultDto<GetCampaignTypeForViewDto> CampaignTypes { get; set; }
        public PagedResultDto<GetProcessEventForViewDto> ProcessEvents { get; set; }
        public PagedResultDto<GetRegistrationStatusForViewDto> RegistrationStatuses { get; set; }
        public PagedResultDto<GetCampaignTypeEventForViewDto> CampaignTypeEvents { get; set; }

        public IEnumerable<GetCampaignTypeEventForViewDto> SelectedProcessEvents { get; set; }
        public IEnumerable<GetProcessEventForViewDto> AvailableProcessEvents { get; set; }
        public IEnumerable<GetCampaignTypeEventRegistrationStatusForViewDto> SelectedRegistrationStatus { get; set; }
        public IEnumerable<GetRegistrationStatusForViewDto> AvailableRegistrationStatuses { get; set; }
    }
}