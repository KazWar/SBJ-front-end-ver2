using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.CodeTypeTables.Dtos
{
    public class GetAllMessageTypesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }

		public string SourceFilter { get; set; }

		public int IsActiveFilter { get; set; }


		 public string MessageVersionFilter { get; set; }

		 
    }
}