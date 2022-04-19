using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.HandlingLineLocales.Dtos
{
    public class GetHandlingLineLocaleForEditOutput
    {
		public CreateOrEditHandlingLineLocaleDto HandlingLineLocale { get; set; }

		public string HandlingLineCustomerCode { get; set;}

		public string LocaleLanguageCode { get; set;}


    }
}