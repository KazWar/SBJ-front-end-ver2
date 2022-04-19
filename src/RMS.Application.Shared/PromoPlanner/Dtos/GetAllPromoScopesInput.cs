using Abp.Application.Services.Dto;
using System;

namespace RMS.PromoPlanner.Dtos
{
    public class GetAllPromoScopesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string DescriptionFilter { get; set; }



    }
}