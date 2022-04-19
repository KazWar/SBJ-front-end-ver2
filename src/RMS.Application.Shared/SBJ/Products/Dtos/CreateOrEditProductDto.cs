
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.Products.Dtos
{
    public class CreateOrEditProductDto : EntityDto<long?>
    {

		public string ProductCode { get; set; }
		
		
		public string Description { get; set; }
		
		
		public string Ean { get; set; }
		
		
		 public long? ProductCategoryId { get; set; }
		 
		 
    }
}