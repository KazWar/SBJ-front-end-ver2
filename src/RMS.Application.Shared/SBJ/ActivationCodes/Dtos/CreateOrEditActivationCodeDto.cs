
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.ActivationCodes.Dtos
{
    public class CreateOrEditActivationCodeDto : EntityDto<long?>
    {

		[Required]
		public string Code { get; set; }
		
		
		public string Description { get; set; }
		
		
		 public long LocaleId { get; set; }
		 
		 
    }
}