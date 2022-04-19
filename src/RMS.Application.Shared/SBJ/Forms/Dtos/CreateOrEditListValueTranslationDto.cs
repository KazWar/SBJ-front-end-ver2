
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.Forms.Dtos
{
    public class CreateOrEditListValueTranslationDto : EntityDto<long?>
    {

		public string KeyValue { get; set; }
		
		
		public string Description { get; set; }
		
		
		 public long ListValueId { get; set; }
		 
		 		 public long LocaleId { get; set; }
		 
		 
    }
}