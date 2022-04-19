using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.CodeTypeTables.Dtos
{
    public class GetAllMessageComponentTypesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }



    }
}