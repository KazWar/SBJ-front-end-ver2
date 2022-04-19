
using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class MessageContentTranslationDto : EntityDto<long>
    {
		public string Content { get; set; }


		 public long LocaleId { get; set; }

		 		 public long MessageComponentContentId { get; set; }

		 
    }
}