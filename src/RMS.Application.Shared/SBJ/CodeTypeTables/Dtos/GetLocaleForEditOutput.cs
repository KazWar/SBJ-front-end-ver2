using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.CodeTypeTables.Dtos
{
    public class GetLocaleForEditOutput
    {
		public CreateOrEditLocaleDto Locale { get; set; }

		public string CountryCountryCode { get; set;}


    }
}