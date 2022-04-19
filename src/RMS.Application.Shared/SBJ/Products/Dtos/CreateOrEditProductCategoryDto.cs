
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.Products.Dtos
{
    public class CreateOrEditProductCategoryDto : EntityDto<long?>
    {

		public string Code { get; set; }
		
		
		public string Description { get; set; }
		
		
		public string POHandling { get; set; }
		
		
		public string POCashback { get; set; }
		
		
		public string Color { get; set; }
		
		

    }
}