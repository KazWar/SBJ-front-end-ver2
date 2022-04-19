using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.Forms.Dtos
{
    public class GetAllValueListsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string DescriptionFilter { get; set; }

		public string ListValueApiCallFilter { get; set; }



    }
}