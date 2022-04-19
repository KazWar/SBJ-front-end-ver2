
using Abp.Application.Services.Dto;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class MessageComponentContentDto : EntityDto<long>
    {
        public string Content { get; set; }

        public long MessageComponentId { get; set; }

        public long CampaignTypeEventRegistrationStatusId { get; set; }

        public string MessageType { get; set; }

        public string MessageComponentType { get; set; }
    }
}