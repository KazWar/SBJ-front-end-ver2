using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.Forms.Dtos
{
    public class GetFormFieldTranslationForEditOutput
    {
		public CreateOrEditFormFieldTranslationDto FormFieldTranslation { get; set; }

		public string FormFieldDescription { get; set;}

		public string LocaleLanguageCode { get; set;}


    }
}