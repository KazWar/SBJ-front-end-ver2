using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.CodeTypeTables.Dtos
{
    public class GetAllLocalesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string LanguageCodeFilter { get; set; }

		public string DescriptionFilter { get; set; }

		public int IsActiveFilter { get; set; }


		 public string CountryCountryCodeFilter { get; set; }

		 
    }
}