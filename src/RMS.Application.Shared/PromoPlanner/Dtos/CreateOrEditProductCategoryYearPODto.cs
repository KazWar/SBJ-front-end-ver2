
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.PromoPlanner.Dtos
{
    public class CreateOrEditProductCategoryYearPoDto : EntityDto<long?>
    {

		public int Year { get; set; }
		
		
		public string PONumberHandling { get; set; }
		
		
		public string PONumberCashback { get; set; }
		
		
		 public long ProductCategoryId { get; set; }
		 
		 
    }
}