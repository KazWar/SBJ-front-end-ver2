using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.Forms.Dtos
{
    public class GetAllFormBlocksInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string DescriptionFilter { get; set; }

		public int? IsPurchaseRegistrationFilter { get; set; }

		public int? MaxSortOrderFilter { get; set; }
		public int? MinSortOrderFilter { get; set; }


		 public string FormLocaleDescriptionFilter { get; set; }

		 
    }
}