using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.CodeTypeTables.Dtos
{
    public class GetAllCountriesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string CountryCodeFilter { get; set; }

		public string DescriptionFilter { get; set; }



    }
}