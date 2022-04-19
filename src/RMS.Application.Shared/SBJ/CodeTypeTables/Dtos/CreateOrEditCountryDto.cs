
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.CodeTypeTables.Dtos
{
    public class CreateOrEditCountryDto : EntityDto<long?>
    {

		[Required]
		[RegularExpression(CountryConsts.CountryCodeRegex)]
		[StringLength(CountryConsts.MaxCountryCodeLength, MinimumLength = CountryConsts.MinCountryCodeLength)]
		public string CountryCode { get; set; }
		
		
		[Required]
		public string Description { get; set; }
		
		

    }
}