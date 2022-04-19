using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.ActivationCodes.Dtos
{
    public class GetActivationCodeForEditOutput
    {
		public CreateOrEditActivationCodeDto ActivationCode { get; set; }

		public string LocaleLanguageCode { get; set;}


    }
}