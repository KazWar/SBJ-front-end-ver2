using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class CreateOrEditMessageHistoryDto : EntityDto<long?>
    {

        public long RegistrationId { get; set; }

        public long AbpUserId { get; set; }

        public string Content { get; set; }

        public DateTime TimeStamp { get; set; }

        public string MessageName { get; set; }

        public long MessageId { get; set; }

        public string Subject { get; set; }

        public string To { get; set; }

    }
}