using Abp.Application.Services.Dto;

namespace RMS.SBJ.Registrations.Dtos
{
    public sealed class GetRegistrationListForViewDto
    {
        public RegistrationDto Registration { get; set; }
        public string CampaignDescription { get; set; }
    }
}
