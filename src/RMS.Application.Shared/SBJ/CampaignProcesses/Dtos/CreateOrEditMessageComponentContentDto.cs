using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class CreateOrEditMessageComponentContentDto : EntityDto<long?>
    {
        [Required]
        public string Content { get; set; }
        public long MessageComponentId { get; set; }
        public long CampaignTypeEventRegistrationStatusId { get; set; }
    }
}