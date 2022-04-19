using Abp.Application.Services.Dto;
using System;

namespace RMS.PromoPlanner.Dtos
{
    public class GetAllPromoRetailersInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }


		 public string PromoPromocodeFilter { get; set; }

		 		 public string RetailerCodeFilter { get; set; }

		 
    }
}