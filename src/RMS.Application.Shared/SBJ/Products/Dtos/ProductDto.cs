
using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.Products.Dtos
{
    public class ProductDto : EntityDto<long>
    {
		public string ProductCode { get; set; }

		public string Description { get; set; }

		public string Ean { get; set; }


		 public long? ProductCategoryId { get; set; }

		 
    }
}