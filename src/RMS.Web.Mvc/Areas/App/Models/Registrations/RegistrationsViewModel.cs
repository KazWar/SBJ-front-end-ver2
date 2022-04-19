using RMS.SBJ.CodeTypeTables.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using RMS.SBJ.CampaignProcesses.Dtos;

namespace RMS.Web.Areas.App.Models.Registrations
{
    public class RegistrationsViewModel
    {
		public string FilterText { get; set; }
		public PagedResultDto<GetRegistrationStatusForViewDto> RegistrationStatuses { get; set; }
    }
}