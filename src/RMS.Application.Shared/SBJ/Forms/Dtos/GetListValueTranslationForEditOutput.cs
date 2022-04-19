using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.Forms.Dtos
{
    public class GetListValueTranslationForEditOutput
    {
		public CreateOrEditListValueTranslationDto ListValueTranslation { get; set; }

		public string ListValueKeyValue { get; set;}

		public string LocaleLanguageCode { get; set;}


    }
}