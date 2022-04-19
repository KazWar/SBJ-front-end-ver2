
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class CreateOrEditMessageContentTranslationDto : EntityDto<long?>
    {

		[Required]
		public string Content { get; set; }
		
		
		 public long LocaleId { get; set; }
		 
		 		 public long MessageComponentContentId { get; set; }
		 
		 
    }
}