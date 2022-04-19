
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.SystemTables.Dtos
{
    public class CreateOrEditMessageDto : EntityDto<long?>
    {

		[Required]
		[StringLength(MessageConsts.MaxVersionLength, MinimumLength = MessageConsts.MinVersionLength)]
		public string Version { get; set; }
		
		
		 public long SystemLevelId { get; set; }
		 
		 
    }
}