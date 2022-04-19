using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.Forms.Dtos
{
    public class GetAllListValuesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string KeyValueFilter { get; set; }

		public string DescriptionFilter { get; set; }

		public int? MaxSortOrderFilter { get; set; }
		public int? MinSortOrderFilter { get; set; }


		 public string ValueListDescriptionFilter { get; set; }

		 
    }
}