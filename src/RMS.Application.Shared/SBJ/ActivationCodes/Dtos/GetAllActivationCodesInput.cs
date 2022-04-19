using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.ActivationCodes.Dtos
{
    public class GetAllActivationCodesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string CodeFilter { get; set; }

		public string DescriptionFilter { get; set; }


		 public string LocaleLanguageCodeFilter { get; set; }

		 
    }
}