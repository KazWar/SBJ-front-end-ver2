
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.Forms.Dtos
{
    public class CreateOrEditFormFieldTranslationDto : EntityDto<long?>
    {

		public string Label { get; set; }
		
		
		public string DefaultValue { get; set; }
		
		
		public string RegularExpression { get; set; }
		
		
		 public long FormFieldId { get; set; }
		 
		 		 public long LocaleId { get; set; }
		 
		 
    }
}