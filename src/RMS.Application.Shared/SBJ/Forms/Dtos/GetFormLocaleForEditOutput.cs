using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.Forms.Dtos
{
    public class GetFormLocaleForEditOutput
    {
		public CreateOrEditFormLocaleDto FormLocale { get; set; }

		public string FormVersion { get; set;}

		public string LocaleDescription { get; set;}


    }
}