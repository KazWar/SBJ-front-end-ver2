
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.CodeTypeTables.Dtos
{
    public class CreateOrEditLocaleDto : EntityDto<long?>
    {

		[Required]
		[RegularExpression(LocaleConsts.LanguageCodeRegex)]
		[StringLength(LocaleConsts.MaxLanguageCodeLength, MinimumLength = LocaleConsts.MinLanguageCodeLength)]
		public string LanguageCode { get; set; }
		
		
		[Required]
		public string Description { get; set; }
		
		
		public bool IsActive { get; set; }
		
		
		 public long CountryId { get; set; }
		 
		 
    }
}