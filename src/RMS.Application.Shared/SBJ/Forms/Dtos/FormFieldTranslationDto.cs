
using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.Forms.Dtos
{
    public class FormFieldTranslationDto : EntityDto<long>
    {
		public string Label { get; set; }

		public string DefaultValue { get; set; }

		public string RegularExpression { get; set; }


		 public long FormFieldId { get; set; }

		 		 public long LocaleId { get; set; }

		 
    }
}