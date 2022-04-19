using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.HandlingLineLogics.Dtos
{
    public class GetAllHandlingLineLogicsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public decimal? MaxFirstHandlingLineIdFilter { get; set; }
		public decimal? MinFirstHandlingLineIdFilter { get; set; }

		public string OperatorFilter { get; set; }

		public decimal? MaxSecondHandlingLineIdFilter { get; set; }
		public decimal? MinSecondHandlingLineIdFilter { get; set; }



    }
}